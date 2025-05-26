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


    [SerializeField]
    BoxCollider2D bounds; // Assign the boundary area in Inspector

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        // Get world bounds of the BoxCollider2D
        Bounds boxBounds = bounds.bounds;
        minBounds = boxBounds.min;
        maxBounds = boxBounds.max;

        // Get size of the player to avoid clipping
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 size = sr.bounds.extents;
            halfWidth = size.x;
            halfHeight = size.y;
        }
        else
        {
            halfWidth = 0.5f;
            halfHeight = 0.5f;
        }

        seed_sub = EventBus.Subscribe<AquireSeedEvent>(_OnAquireSeedEvent);
    }

    public void UpgradeWalkingSpeed(NumberUpgradeType num_upgrade_type, float val) { 
        if(num_upgrade_type == NumberUpgradeType.Multiply) {
            speed *= val;
        }
        else {
            speed += val;
        }
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
        // Basic movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY, 0f) * speed * Time.deltaTime;

        // Apply movement
        transform.position += move;

        // Clamp position within bounds (considering player's size)
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space)) { 
            if(current_active_seed == null) {
                return;
            }

            Instantiate(current_active_seed, transform.position, transform.rotation);
            current_active_seed = null;

            EventBus.Publish(new AquireSeedEvent(PlantType.None));
        }
        
    }
}
