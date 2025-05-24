using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAndStretch : MonoBehaviour
{
    [SerializeField]
    float shrink_amount = 0.1f;

    [SerializeField]
    float stretch_amount = 0.1f;

    [SerializeField]
    float speed = 1f;

    float base_shrink;

    // Start is called before the first frame update
    void Start()
    {
        base_shrink = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 new_scale = new Vector3(base_shrink, 1, 1);
        new_scale += new Vector3(Mathf.PingPong(Time.time * speed, shrink_amount), Mathf.PingPong(Time.time * speed, stretch_amount), 0);
        transform.localScale = new_scale;
    }
}
