using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnEvent", menuName = "Data/SpawnEvent")]

public class SpawnEvent : ScriptableObject
{
   public EntityData entityData;
    public float time;
}
