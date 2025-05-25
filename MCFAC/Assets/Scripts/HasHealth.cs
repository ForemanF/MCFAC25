using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    [SerializeField]
    float health = 3.0f;

    [SerializeField]
    ExplosivePs damage_effect = ExplosivePs.Blood;

    [SerializeField]
    ExplosivePs death_effect = ExplosivePs.FireExplosion;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount) {
        health -= amount;

        EventBus.Publish(new ParticleExplosionEvent(transform.position, damage_effect, 5 * (int)amount));

        if(health <= 0) {
            StartCoroutine(StartDeathSequence());
        }
    }

    IEnumerator StartDeathSequence() {
        EventBus.Publish(new ParticleExplosionEvent(transform.position, death_effect));
        yield return null;
        Destroy(gameObject);
    }
}
