using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class PtToPf { public PlantType plant_type; public GameObject pf; }
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    List<PtToPf> plant_to_pf;

    GameObject current_active_seed = null;
    Subscription<AquireSeedEvent> seed_sub;



    // Start is called before the first frame update
    void Start()
    {
        seed_sub = EventBus.Subscribe<AquireSeedEvent>(_OnAquireSeedEvent);
    }

    void _OnAquireSeedEvent(AquireSeedEvent e) {
        if(e.plant_type == PlantType.None) {
            return;
        }
        foreach(PtToPf ptp in plant_to_pf) { 
            if(ptp.plant_type == e.plant_type) {
                current_active_seed = ptp.pf;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 translation = new Vector3(horiz, vert) * Time.deltaTime * speed;
        transform.position += translation;

        if(Input.GetKeyDown(KeyCode.Space)) { 
            if(current_active_seed == null) {
                return;
            }

            Instantiate(current_active_seed, transform.position, transform.rotation);
            current_active_seed = null;

            EventBus.Publish(new AquireSeedEvent(PlantType.None));
        }
        
    }
}
