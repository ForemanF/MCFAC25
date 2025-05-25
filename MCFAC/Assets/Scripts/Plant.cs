using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class StageData { public Sprite sprite; public int amount_to_upgrade = 1; }

public enum PlantType { 
    None,
    YellowFlower,
    BlueFlower,
    RedFlower,
    PurpleFlower,
    GreenCactus,
    MoneyTree
}

public class Plant : MonoBehaviour
{
    [SerializeField]
    List<StageData> stages;

    [SerializeField]
    int face_stage = -1;

    [SerializeField]
    SpriteRenderer sr;

    [SerializeField]
    GameObject green_patch_pf;

    GameObject green_patch;
    ProgressToSeedVisual ptsv;
    

    [SerializeField]
    float patch_growth_rate = 0.04f;

    int current_stage = 0;
    float amt_to_next_stage = 0;

    [SerializeField]
    float water_for_seed = 20;
    float amt_to_seed = 0;


    HasHealth has_health;

    [SerializeField]
    PlantType plant_type = PlantType.None;


    [SerializeField]
    GameObject proj_pf;

    [SerializeField]
    float shoot_range = 2f;

    [SerializeField]
    float fire_rate_s = 1f;

    [SerializeField]
    float fire_speed = 1f;

    float fire_time_elapsed = 0;

    Transform current_target = null;

    HasFace has_face;


    // Start is called before the first frame update
    void Start()
    {
        has_face = GetComponent<HasFace>();
        sr.sprite = stages[current_stage].sprite;
        has_health = GetComponent<HasHealth>();
        green_patch = Instantiate(green_patch_pf, transform.position, transform.rotation);
        ptsv = green_patch.GetComponent<ProgressToSeedVisual>();
    }

    // Update is called once per frame
    void Update()
    {
        if(current_stage + 1 == stages.Count) {
            current_target = GetTarget();

            fire_time_elapsed += Time.deltaTime;

            if(current_target == null) {
                return;
            }
            FireAtTarget();
        }
    }

    Transform GetTarget() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject target in targets) {
            float dist = (target.transform.position - transform.position).magnitude;
            if(dist < shoot_range) {
                return target.transform;
            }
        }

        return null;
    }

    void FireAtTarget() { 
        if(fire_time_elapsed > fire_rate_s) {
            Transform face_location = has_face.GetFaceLocation();
            GameObject new_droplet = Instantiate(proj_pf, face_location.position, transform.rotation);
            Vector3 initial_velocity = (current_target.transform.position - face_location.position).normalized * fire_speed; //  + Random.Range(-water_forward_spread, water_forward_spread));

            new_droplet.GetComponent<Rigidbody2D>().velocity = initial_velocity;
            fire_time_elapsed = 0;
        }
    }

    public void WaterPlant(float amount) {

        if(current_stage + 1 >= stages.Count) {
            amt_to_seed += amount;

            if(amt_to_seed > water_for_seed) {
                amt_to_seed = 0;
                Debug.Log("Give player a seed");
                EventBus.Publish(new AquireSeedEvent(plant_type));
            }

            ptsv.SetProgress(amt_to_seed / water_for_seed);

            return;
        }

        amt_to_next_stage += amount;
        green_patch.transform.localScale += Vector3.one * patch_growth_rate;

        if(amt_to_next_stage >= stages[current_stage].amount_to_upgrade) {
            current_stage += 1;
            sr.sprite = stages[current_stage].sprite;
            EventBus.Publish(new ParticleExplosionEvent(transform.position, ExplosivePs.PlantGrowth));
            amt_to_next_stage = 0;


            // this is a work around to get the face to show up
            if(current_stage == face_stage) {
                EventBus.Publish(new HealthEvent(gameObject, has_health.GetHealth(), has_health.GetMaxHealth(), true));
            }
        }
    }
}
