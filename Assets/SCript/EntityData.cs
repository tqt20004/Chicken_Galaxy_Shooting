using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category
{
    Enemy,
    Obstacle
}

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Data/Entity Data")]
public class EntityData : ScriptableObject
{
    [Header("Base Config")]
    public string enemyName;
    public int maxHealth = 100;
    public float moveSpeed = 5f;
    public int touchDamage = 10;
    public Category category;  
    public LayerMask layerMask;

    [Header("Prefab")]
    public GameObject prefab; 

    [Header("How to Move")]
    public IMoveStrategy moveStrategyType = new FallDownStrategy();
}
