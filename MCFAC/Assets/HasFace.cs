using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasFace : MonoBehaviour
{
    [SerializeField]
    GameObject current_face = null;

    [SerializeField]
    Transform face_location = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFace(GameObject new_face) { 
        if(current_face != null) {
            Destroy(current_face);
        }

        current_face = Instantiate(new_face, face_location);
    }

    public GameObject GetFace() {
        return current_face;
    }
}
