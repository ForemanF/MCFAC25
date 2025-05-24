using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float speed = 1f;

    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) {
            return;
        }
        Vector3 velocity = target.transform.position - transform.position;
        velocity = velocity.normalized;
        velocity = velocity * speed;
        rb2d.velocity = velocity;
    }
}
