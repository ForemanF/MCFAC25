using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterUIMeter : MonoBehaviour
{
    [SerializeField]
    Slider water_slider;
    
    Subscription<WaterAmountEvent> water_amount_sub;

    // Start is called before the first frame update
    void Start()
    {
        water_amount_sub = EventBus.Subscribe<WaterAmountEvent>(_OnWaterAmountEvent);
    }

    void _OnWaterAmountEvent(WaterAmountEvent e) {
        float new_scale = e.current_water / e.max_water;

        water_slider.value = new_scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
