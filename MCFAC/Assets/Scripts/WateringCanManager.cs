using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCanManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem ps;

    [SerializeField]
    float radius = 0.1f;

    [SerializeField]
    Transform reference_tf;

    [SerializeField]
    float current_water = 10;

    [SerializeField]
    float max_water = 10;

    [SerializeField]
    float water_per_second = 1f;

    [SerializeField]
    float water_refill_speed = 2f;

    enum WaterState { 
        NotWatering,
        IsWatering
    }

    WaterState current_state;

    // Start is called before the first frame update
    void Start()
    {
        StopWatering();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartWatering();
        }
        if(Input.GetMouseButtonUp(0)) {
            StopWatering();
        }

        HandleWaterState();

        UpdateWateringCanPos();
    }

    void HandleWaterState() { 
        if(current_state == WaterState.IsWatering) {
            ChangeWaterValue(-Time.deltaTime * water_per_second);
        }
    }

    public void RefillWater() {
        ChangeWaterValue(Time.deltaTime * water_refill_speed);
    }

    void ChangeWaterValue(float amount) {
        current_water += amount;

        current_water = Mathf.Clamp(current_water, 0, max_water);

        if(current_water == 0) {
            StopWatering();
        }

        EventBus.Publish(new WaterAmountEvent(current_water, max_water));
    }

    void StopWatering() {
        ps.Stop();
        current_state = WaterState.NotWatering;
    }

    void StartWatering() {
        ps.Play();
        current_state = WaterState.IsWatering;
    }

    void UpdateWateringCanPos() {
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = mouse_pos - reference_tf.position;
        dir = dir.normalized;

        transform.localPosition = dir * radius;

        float deg_rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(0, 0, deg_rot);
        if(dir.x < 0) {
            rot *= Quaternion.Euler(180.0f, 0, 0);
        }
        transform.rotation = rot;
    }
}
