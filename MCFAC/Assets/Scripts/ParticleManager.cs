using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExplosivePs { 
    FireExplosion,
    PlantGrowth,
    Blood,
    IndividualGrowth
}

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem explosive_ps;

    [SerializeField]
    ParticleSystem plant_growth_ps;

    [SerializeField]
    ParticleSystem blood_ps;

    [SerializeField]
    ParticleSystem individual_growth_ps;
    
    Subscription<ParticleExplosionEvent> explosive_ps_sub;



    // Start is called before the first frame update
    void Start()
    {
        explosive_ps_sub = EventBus.Subscribe<ParticleExplosionEvent>(_OnExplosivePsEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _OnExplosivePsEvent(ParticleExplosionEvent e) {
        ParticleSystem selected_ps = null;

        switch(e.explosive_ps) {
            case ExplosivePs.FireExplosion:
                selected_ps = explosive_ps;
                break;
            case ExplosivePs.PlantGrowth:
                selected_ps = plant_growth_ps;
                break;
            case ExplosivePs.Blood:
                selected_ps = blood_ps;
                break;
            case ExplosivePs.IndividualGrowth:
                selected_ps = individual_growth_ps;
                break;
            default:
                Debug.Log("Unimplemented Particle Effect");
                break;
        }

        if(selected_ps == null) {
            return;
        }

        selected_ps.transform.position = e.position;
        selected_ps.Emit(e.num_particles);
    }
}
