using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class AllCardTypes : ScriptableObject
{
    public List<CardDetails> FullTypesList = new();
    public List<CardDetails> CommonCards = new();
    public List<CardDetails> CurseCards = new();
    
    // Awake is called when an instance of ScriptableObject is created
    void Awake() {
        string[] guids = AssetDatabase.FindAssets("t:CardDetails", new string[]{"Assets/Cards"});
        foreach (string guid in guids) {
            CardDetails card = AssetDatabase.LoadAssetAtPath<CardDetails>(AssetDatabase.GUIDToAssetPath(guid));
            FullTypesList.Add(card);
            if (card.IsCurse) {
                CurseCards.Add(card);
            } else {
                CommonCards.Add(card);
            }
        }
    }
}
