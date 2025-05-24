using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCanAmountVisual : MonoBehaviour
{
    Subscription<WaterAmountEvent> water_amount_sub;

    // Start is called before the first frame update
    void Start()
    {
        water_amount_sub = EventBus.Subscribe<WaterAmountEvent>(_OnWaterAmountEvent);
    }

    void _OnWaterAmountEvent(WaterAmountEvent e) {
        float new_scale = e.current_water / e.max_water;

        transform.localScale = new Vector3(1, new_scale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
