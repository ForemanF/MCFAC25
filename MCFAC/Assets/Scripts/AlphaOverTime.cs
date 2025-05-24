using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaOverTime : MonoBehaviour
{
    [SerializeField]
    float life = 5;

    SpriteRenderer sr;

    float time_elapsed = 0;

    Color base_color;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        base_color = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        time_elapsed += Time.deltaTime;
        float new_alpha = Mathf.Lerp(1f, 0, time_elapsed / life);

        if(new_alpha == 0) {
            Destroy(gameObject);
        }

        Color new_color = base_color;
        new_color.a = new_alpha;
        sr.color = new_color;
    }
}
