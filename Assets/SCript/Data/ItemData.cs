using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold,
    Heal,
    WeaponBuff
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;            
    public float effectValue = 1f;
    public string type;
    public GameObject prefab;            // Model 3D
    [Range(0f, 1f)] public float dropRate = 0.5f; // Drop-Rate (ex: 0.5 = 50%)
}