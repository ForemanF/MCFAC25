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

    int current_stage = 0;
    float amt_to_next_stage = 0;


    // Start is called before the first frame update
    void Start()
    {
        
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

        // Todo: add some visual when the plant gains water w/o upgrading

        if(amt_to_next_stage >= stages[current_stage].amount_to_upgrade) {
            // todo: add some explosion type effect when the upgrade happens to hide the 
            // sprite replacement
            current_stage += 1;
            sr.sprite = stages[current_stage].sprite;
            amt_to_next_stage = 0;
        }
    }
}
