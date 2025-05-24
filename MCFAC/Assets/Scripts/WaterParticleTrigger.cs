using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleTrigger : MonoBehaviour
{
    ParticleSystem ps;

    [SerializeField]
    float affect_radius = 0.05f;

    [SerializeField]
    Transform debug_obj;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleTrigger()
    {
        //Get all particles that entered a box collider
        List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
        //int enterCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enteredParticles);
        int enterCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

        //Get all fires
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        int num_hits = 0;
        foreach (ParticleSystem.Particle particle in enteredParticles)
        {
            debug_obj.transform.position = particle.position;
            for (int i = 0; i < plants.Length; i++)
            {
                Collider2D collider = plants[i].GetComponent<Collider2D>();
                //if (collider.bounds.Contains(particle.position))
                if (collider.OverlapPoint(particle.position))
                {
                    num_hits += 1;
                    //fires[i].GetComponent<Fire>().Damage();
                }

                float dist = (collider.ClosestPoint(particle.position) - (Vector2) particle.position).magnitude;
                if(dist < affect_radius) {
                    collider.gameObject.transform.localScale += Vector3.one * 0.01f;

                    Plant affected_plant = collider.gameObject.GetComponent<Plant>();
                    affected_plant.WaterPlant(1);
                }
                Debug.Log(dist);
                //if(collider.ClosestPoint(particle.position))

            }
        }
        Debug.Log(num_hits);
    }
}
