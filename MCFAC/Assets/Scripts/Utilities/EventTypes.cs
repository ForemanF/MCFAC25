using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterAmountEvent {
    public float current_water;
    public float max_water;

    public WaterAmountEvent(float _current_water, float _max_water) {
        current_water = _current_water;
        max_water = _max_water;
    }
}

public class ParticleExplosionEvent {
    public Vector3 position;
    public ExplosivePs explosive_ps;
    public int num_particles;

    public ParticleExplosionEvent(Vector3 _position, ExplosivePs _explosive_ps) {
        position = _position;
        explosive_ps = _explosive_ps;
        num_particles = 100;
    }

    public ParticleExplosionEvent(Vector3 _position, ExplosivePs _explosive_ps, int _num_particles) {
        position = _position;
        explosive_ps = _explosive_ps;
        num_particles = _num_particles;
    }
}

public class HealthEvent {
    public GameObject obj;
    public float health;
    public float max_health;

    public HealthEvent(GameObject _obj, float _health, float _max_health) {
        obj = _obj;
        health = _health;
        max_health = _max_health;
    }
}
