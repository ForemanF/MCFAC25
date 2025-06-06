using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropletManager : MonoBehaviour
{
    [SerializeField]
    float kb_strength = 1;

    [SerializeField]
    float damage = 1f;

    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

    [SerializeField]
    bool grows_plants = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rb2d.velocity.magnitude < 0.01f) {
            bc2d.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Enemy>().Knockback(rb2d.velocity.normalized * kb_strength, damage);
            bc2d.enabled = false;
        }

        else if(grows_plants && collision.CompareTag("Plant")) {
            collision.gameObject.GetComponent<Plant>().WaterPlant(1); 
            EventBus.Publish(new ParticleExplosionEvent(transform.position, ExplosivePs.IndividualGrowth, 5));
        }
    }
}
