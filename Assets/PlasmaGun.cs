using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlasmaGun : MonoBehaviour
{
    public Plasma PlasmaPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FirePlasmaAtIntervals());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FirePlasmaAtIntervals() {
        while (true) {
            yield return new WaitForSeconds(2.0f);
            FirePlasma();
        }
    }

    public void FirePlasma() {
        Plasma plasma = Instantiate(PlasmaPrefab);
        plasma.transform.position = transform.position + transform.up * 1.5f;
        plasma.transform.rotation = Quaternion.LookRotation(transform.up);
        Rigidbody rigidbody = plasma.GetComponent<Rigidbody>();
        rigidbody.AddForce(plasma.transform.forward * plasma.Velocity, ForceMode.VelocityChange);
    }
}
