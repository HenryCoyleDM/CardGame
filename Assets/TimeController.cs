using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") > 0) {
            Time.timeScale = 0.25f;
        } else {
            Time.timeScale = 1.0f;
        }
    }
}
