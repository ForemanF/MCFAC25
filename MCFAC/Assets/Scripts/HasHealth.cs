using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    [SerializeField]
    float health = 3.0f;


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

        if(health <= 0) {
            StartCoroutine(StartDeathSequence());
        }
    }

    IEnumerator StartDeathSequence() {
        EventBus.Publish(new ParticleExplosionEvent(transform.position));
        yield return null;
        Destroy(gameObject);
    }
}
