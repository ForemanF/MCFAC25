using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressToSeedVisual : MonoBehaviour
{
    [SerializeField]
    Transform scaler;

    public void SetProgress(float progress) {
        progress = Mathf.Clamp01(progress);
        Vector3 new_scaler = Vector3.one;
        new_scaler.y = progress;
        scaler.localScale = new_scaler;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
