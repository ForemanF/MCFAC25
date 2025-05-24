using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateRotation : MonoBehaviour
{
    [SerializeField]
    float magnitude = 45;

    [SerializeField]
    float speed = 1;

    float base_rotation;

    // Start is called before the first frame update
    void Start()
    {
        base_rotation = transform.localRotation.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        float new_rot = Mathf.PingPong(Time.time * speed, magnitude * 2) - magnitude + base_rotation;
        transform.localRotation = Quaternion.Euler(0, 0, new_rot);
    }
}
