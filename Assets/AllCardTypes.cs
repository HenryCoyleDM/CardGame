using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class AllCardTypes : ScriptableObject
{
    public List<CardDetails> TypesList = new();
    
    // Awake is called when an instance of ScriptableObject is created
    void Awake() {
        string[] guids = AssetDatabase.FindAssets("t:CardDetails", new string[]{"Assets/Cards"});
        foreach (string guid in guids) {
            TypesList.Add(AssetDatabase.LoadAssetAtPath<CardDetails>(AssetDatabase.GUIDToAssetPath(guid)));
        }
    }
}
