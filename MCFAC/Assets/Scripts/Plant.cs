using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class StageData { public Sprite sprite; public int amount_to_upgrade = 1; }
public class Plant : MonoBehaviour
{
    [SerializeField]
    List<StageData> stages;

    [SerializeField]
    SpriteRenderer sr;

    [SerializeField]
    GameObject green_patch_pf;

    GameObject green_patch;

    [SerializeField]
    float patch_growth_rate = 0.04f;

    int current_stage = 0;
    float amt_to_next_stage = 0;


    // Start is called before the first frame update
    void Start()
    {
        green_patch = Instantiate(green_patch_pf, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WaterPlant(float amount) {
        amt_to_next_stage += amount;

        if(current_stage + 1 >= stages.Count) {
            Debug.Log("Can't upgrade any more");
            return;
        }

        green_patch.transform.localScale += Vector3.one * patch_growth_rate;

        if(amt_to_next_stage >= stages[current_stage].amount_to_upgrade) {
            current_stage += 1;
            sr.sprite = stages[current_stage].sprite;
            EventBus.Publish(new ParticleExplosionEvent(transform.position, ExplosivePs.PlantGrowth));
            amt_to_next_stage = 0;
        }
    }
}
