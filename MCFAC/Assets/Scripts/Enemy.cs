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



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        has_health = GetComponent<HasHealth>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
