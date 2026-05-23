using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWave", menuName = "Data/Wave")]

public class WaveData : ScriptableObject
{
    public string waveName;
    public List<WaveElement> waveList;
}
[System.Serializable] 
public struct WaveElement 
{
    public EntityData entity;
    public float spawnTime;   
}
