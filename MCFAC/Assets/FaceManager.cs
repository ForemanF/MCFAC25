using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class FaceData { public GameObject face_pf; public float max = 1; public float min = 0; }
public class FaceManager : MonoBehaviour
{
    [SerializeField]
    List<FaceData> faces;

    Subscription<HealthEvent> health_sub;

    // Start is called before the first frame update
    void Start()
    {
        health_sub = EventBus.Subscribe<HealthEvent>(_OnHealthEvent);
    }

    void _OnHealthEvent(HealthEvent e) {

        // determine which face to use

        HasFace has_face = e.obj.GetComponent<HasFace>();
        if(has_face == null) {
            return;
        }

        GameObject current_face = has_face.GetFace();

        float health_percent = e.health / e.max_health;
        foreach(FaceData face_data in faces) { 
            // check if in range
            if(face_data.min <= health_percent && health_percent <= face_data.max) {
                has_face.ChangeFace(face_data.face_pf);
            }
        }

    }
}
