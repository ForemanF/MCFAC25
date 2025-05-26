using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class PtToVis { public PlantType plant_type; public Sprite image; }
public class CurrentSeedVisual : MonoBehaviour
{
    [SerializeField]
    List<PtToVis> pt_to_icon;

    [SerializeField]
    Image image_obj;

    Subscription<AquireSeedEvent> seed_sub;

    // Start is called before the first frame update
    void Start()
    {
        seed_sub = EventBus.Subscribe<AquireSeedEvent>(_OnAquireSeedEvent);
    }

    void _OnAquireSeedEvent(AquireSeedEvent e) {
        if(e.plant_type == PlantType.None) {
            image_obj.enabled = false;
            return;
        }

        image_obj.enabled = true;
        foreach(PtToVis ptv in pt_to_icon) {
            if (ptv.plant_type == e.plant_type)
            {
                image_obj.sprite = ptv.image;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
