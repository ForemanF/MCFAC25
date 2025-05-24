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

    public ParticleExplosionEvent(Vector3 _position) {
        position = _position;
    }
}
