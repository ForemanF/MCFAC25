using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    [SerializeField]
    float shrink_amount = 0.1f;

    [SerializeField]
    float stretch_amount = 0.1f;

    [SerializeField]
    float time_to_stretch = 0.2f;

    Quaternion start_rot;

    [SerializeField]
    float rotate_amt = 5;

    [SerializeField]
    float rotate_time = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        start_rot = transform.rotation;
        StartCoroutine(WobbleLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WobbleLoop() {
        Vector3 shrunk_scale = transform.localScale;
        shrunk_scale.x += shrink_amount;
        shrunk_scale.y -= stretch_amount;

        Vector3 peak_scale = transform.localScale;
        peak_scale.x -= shrink_amount;
        peak_scale.y += stretch_amount;
        while(true) {

            yield return ScaleLerp(transform, transform.localScale, shrunk_scale, time_to_stretch);

            Quaternion target_rot = start_rot * Quaternion.Euler(0, 0, rotate_amt);
            StartCoroutine(RotationLerp(transform, transform.rotation, target_rot, rotate_time));
            rotate_amt *= -1;


            yield return ScaleLerp(transform, transform.localScale, peak_scale, time_to_stretch);
        }
    }

    IEnumerator RotationLerp(Transform tf, Quaternion start_rot, Quaternion end_rot, float time) { 
        float progress = 0;
        float start_time = Time.time;

        while(progress < 1) {
            progress = (Time.time - start_time) / time;
            Quaternion new_rot = Quaternion.Lerp(start_rot, end_rot, progress);
            tf.rotation = new_rot;

            yield return null;
        }

        tf.rotation = end_rot;
    }

    IEnumerator ScaleLerp(Transform tf, Vector3 start_scale, Vector3 end_scale, float time) {

        float progress = 0;
        float start_time = Time.time;

        while(progress < 1) {
            progress = (Time.time - start_time) / time;
            Vector3 new_scale = Vector3.Lerp(start_scale, end_scale, progress);
            tf.localScale = new_scale;

            yield return null;
        }

        tf.localScale = end_scale;
    }
}
