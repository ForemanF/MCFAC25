using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    WaveManager wm;

    bool has_pressed_space = false;

    bool has_fired_watering_can = false;

    [SerializeField]
    GameObject has_planted_obj;

    float time_elapsed = 0;

    [SerializeField]
    GameObject has_fired;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time_elapsed += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space)) {
            has_pressed_space = true;
            has_planted_obj.SetActive(false);
        }

        if(has_pressed_space == false && time_elapsed > 5) {
            has_planted_obj.SetActive(true);
        }

        if(Input.GetMouseButtonDown(0)) {
            has_fired_watering_can = true;
        }

        if(has_fired_watering_can == false && time_elapsed > 15) {
            has_fired.SetActive(true);
        }
        
        if(has_pressed_space && has_fired_watering_can) {
            wm.HasGrownSeed();
            wm.HasPlantedSeed();
            has_planted_obj.SetActive(false);
            has_fired.SetActive(false);
            Destroy(this);
        }
    }
}
