using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCanManager : MonoBehaviour
{
    [SerializeField]
    Transform water_spawn_loc;

    [SerializeField]
    float radius = 0.1f;

    [SerializeField]
    Transform reference_tf;

    [SerializeField]
    float current_water = 10;

    [SerializeField]
    float max_water = 10;

    [SerializeField]
    float water_refill_speed = 2f;

    enum WaterState { 
        NotWatering,
        IsWatering
    }

    [SerializeField]
    float water_speed = 2f;

    [SerializeField]
    float water_forward_spread = 0.3f;
    
    [SerializeField]
    float water_side_spread = 0.2f;

    [SerializeField]
    float water_fire_rate = 0.2f;
    
    float time_elapsed = 0;

    [SerializeField]
    GameObject water_droplet_prefab;

    [SerializeField]
    float autorefill = 0;

    WaterState current_state;

    // Start is called before the first frame update
    void Start()
    {
        StopWatering();
        EventBus.Publish(new WaterAmountEvent(current_water, max_water));
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

        if(autorefill != 0) { 
            ChangeWaterValue(Time.deltaTime * autorefill);
        }

        HandleWaterState();

        UpdateWateringCanPos();
    }

    void HandleWaterState() { 
        if(current_state == WaterState.IsWatering) {
            float spawned_droplets = SpawnWaterDroplets();
            ChangeWaterValue(-spawned_droplets);
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
        current_state = WaterState.NotWatering;
    }

    void StartWatering() {
        current_state = WaterState.IsWatering;
    }

    int SpawnWaterDroplets() {
        time_elapsed += Time.deltaTime;

        if(time_elapsed > water_fire_rate) {
            GameObject new_droplet = Instantiate(water_droplet_prefab, water_spawn_loc.position, transform.rotation);
            Vector3 initial_velocity = transform.right * (water_speed + Random.Range(-water_forward_spread, water_forward_spread));
            Vector3 side_comp = transform.up * Random.Range(-water_side_spread, water_side_spread);
            initial_velocity += side_comp;

            new_droplet.GetComponent<Rigidbody2D>().velocity = initial_velocity;
            time_elapsed = 0;
            return 1;
        }

        return 0;
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

    public void UpgradeCapacity(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            current_water += (max_water * val - max_water);
            max_water *= val;
        }
        else {
            current_water += val;
            max_water += val;
        }
    }

    public void UpgradeFireRate(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            water_fire_rate *= val;
        }
        else { 
            water_fire_rate += val;
        }
    }

    public void UpgradeSideSpread(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            water_side_spread *= val;
        }
        else {
            water_side_spread += val;
        }
    }

    public void UpgradeRange(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            water_speed *= val;
        }
        else {
            water_speed += val;
        }
    }

    public void UpgradeRefillRate(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            water_refill_speed *= val;
        }
        else {
            water_refill_speed += val;
        }
    }

    public void UpgradeAutoRefill(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            autorefill *= val;
        }
        else {
            autorefill += val;
        }
    }
}
