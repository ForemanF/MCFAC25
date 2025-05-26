using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float speed = 1f;

    [SerializeField]
    float acceleration = 1f;

    Rigidbody2D rb2d;

    HasHealth has_health;

    [SerializeField]
    float contact_damage = 1f;

    [SerializeField]
    float own_kb_amt = 1f;

    [SerializeField]
    float time_between_retarget = 0.5f;

    float time_retarget_elapsed = 0;

    List<string> target_tags;


    // Start is called before the first frame update
    void Start()
    {
        target_tags = new List<string>();
        target_tags.Add("Player");
        target_tags.Add("Plant");

        rb2d = GetComponent<Rigidbody2D>();
        has_health = GetComponent<HasHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        time_retarget_elapsed += Time.deltaTime;
        if(time_retarget_elapsed > time_between_retarget) {
            time_retarget_elapsed = 0;
            target = FindNearestTarget(target_tags, transform);
        }

        if(target == null) {
            return;
        }

        Vector3 desired_velocity = target.transform.position - transform.position;
        desired_velocity = desired_velocity.normalized;
        desired_velocity = desired_velocity * speed;

        Vector3 new_velocity = rb2d.velocity + (Vector2) desired_velocity * acceleration * Time.deltaTime;
        if(new_velocity.magnitude > speed && Vector3.Dot(new_velocity, desired_velocity) > 0) {
            new_velocity = desired_velocity;
        }

        rb2d.velocity = new_velocity;
    }

    public void Knockback(Vector3 knockback_amt, float damage) {
        rb2d.velocity = knockback_amt;

        if(damage > 0) {
            has_health.TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        DamageLogic(collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageLogic(collision);
    }

    void DamageLogic(Collider2D collider) { 
        if(collider.CompareTag("Player") || collider.CompareTag("Plant")) {
            HasHealth hit_health = collider.gameObject.GetComponent<HasHealth>();
            if(hit_health != null) {
                hit_health.TakeDamage(contact_damage);
            }

            float additional_kb = 0;
            if(collider.TryGetComponent<Plant>(out Plant plant)) { 
                if(plant.GetPlantType() == PlantType.GreenCactus) {
                    has_health.TakeDamage(plant.GetSpecialValue());
                    additional_kb = 1;
                }
            }

            // knockback this unit after it hits something
            Vector3 direction = transform.position - collider.gameObject.transform.position;
            direction = direction.normalized;
            Knockback(direction * (own_kb_amt + additional_kb), 0);

        }
    }

    Transform FindNearestTarget(List<string> tags, Transform tf) {

        float distance = Mathf.Infinity;
        Transform closest_tf = null;

        foreach(string tag in tags) {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            foreach(GameObject obj in objects) {
                float cur_dist = (obj.transform.position - tf.position).magnitude;

                if(cur_dist < distance) {
                    closest_tf = obj.transform;
                    distance = cur_dist;
                }
            }
        }

        return closest_tf;
    }

}
