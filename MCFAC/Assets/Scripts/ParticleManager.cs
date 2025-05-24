using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem explosive_ps;
    
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
        explosive_ps.transform.position = e.position;
        explosive_ps.Emit(100);
    }
}
