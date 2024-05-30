using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeController : MonoBehaviour
{
    public bool BulletTimeOverride = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BulletTimeOverride || Input.GetAxisRaw("Fire1") > 0) {
            Time.timeScale = 0.25f;
        } else {
            Time.timeScale = 1.0f;
        }
    }
}
