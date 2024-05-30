using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu]
public class CardDetails : ScriptableObject
{
    public string Name;
    public string Text;
    public Texture2D Image;
    public string ClassName;
    public Type CardClass {
        get {
            return Type.GetType(ClassName);
        }
    }
}
