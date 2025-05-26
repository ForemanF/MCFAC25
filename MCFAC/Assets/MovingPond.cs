using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPond : MonoBehaviour
{
    [SerializeField]
    BoxCollider2D region;

    [SerializeField]
    float speed = 0.2f;

    Vector3 target_pos = Vector3.zero;

    [SerializeField]
    float threshold = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        target_pos = GetValidSpawnLocation(region);
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - target_pos).magnitude < threshold) {
            target_pos = GetValidSpawnLocation(region);
        }

        Vector3 dir = (target_pos - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    Vector3 GetValidSpawnLocation(BoxCollider2D spawn_area) {
        Bounds bounds = spawn_area.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0);
    }
}
