using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextureTiler : MonoBehaviour
{
    public float tileX = 1;
    public float tileY = 1;
    public bool IsFloor = true;
    private Mesh mesh;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        mesh = GetComponent<MeshFilter>().mesh;
        if (IsFloor) {
            material.mainTextureScale = new Vector2(mesh.bounds.size.x * transform.localScale.x * tileX,
                                                    mesh.bounds.size.z * transform.localScale.z * tileY);
        } else {
            material.mainTextureScale = new Vector2(mesh.bounds.size.x * transform.localScale.x * tileX,
                                                    mesh.bounds.size.y * transform.localScale.y * tileY);
        }
    }

    void Update()
    {
        
    }
}
