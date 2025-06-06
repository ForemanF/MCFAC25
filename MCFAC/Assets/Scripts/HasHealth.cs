using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    [SerializeField]
    float health = 3.0f;

    [SerializeField]
    float max_health = 3.0f;

    [SerializeField]
    ExplosivePs damage_effect = ExplosivePs.Blood;

    [SerializeField]
    ExplosivePs death_effect = ExplosivePs.FireExplosion;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void TakeDamage(float amount) {
        health -= amount;

        EventBus.Publish(new ParticleExplosionEvent(transform.position, damage_effect, 5 * (int)amount));

        EventBus.Publish(new HealthEvent(gameObject, health, max_health));

        if(health <= 0) {
            StartCoroutine(StartDeathSequence());
        }
    }

    public void Heal(float amount) {
        if(health == max_health) {
            return;
        }

        health = Mathf.Min(max_health, health + amount);
        EventBus.Publish(new HealthEvent(gameObject, health, max_health));

    }

    public float GetHealth() {
        return health;
    }

    public float GetMaxHealth() {
        return max_health;
    }

    IEnumerator StartDeathSequence() {
        EventBus.Publish(new ParticleExplosionEvent(transform.position, death_effect));

        if(gameObject.tag == "Player") {
            EventBus.Publish(new GameOverEvent(false));
        }

        yield return null;
        Destroy(gameObject);
    }

    public void IncreaseMaxHealth(float amount) {
        health += amount;
        max_health += amount;
        EventBus.Publish(new HealthEvent(gameObject, health, max_health));
    }
}
