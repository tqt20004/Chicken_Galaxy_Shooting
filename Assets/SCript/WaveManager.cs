using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public WaveData curWaveData;
   
    public float waveTimer;
    private int currentEventIndex = 0;
    private bool isWaveActive = false;

    public static Action<EntityData> OnSpawnEntity;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (!isWaveActive || curWaveData == null) return;

        waveTimer += Time.deltaTime;

        // Kiểm tra xem đã đến giờ spawn của sự kiện tiếp theo chưa
        while (currentEventIndex < curWaveData.waveList.Count &&
               waveTimer >= curWaveData.waveList[currentEventIndex].spawnTime)
        {
            SpawnEntity(curWaveData.waveList[currentEventIndex]);
            currentEventIndex++;
        }

        // Kiểm tra kết thúc wave
        if (currentEventIndex >= curWaveData.waveList.Count)
        {
            isWaveActive = false;
            Debug.Log("Wave đã hoàn tất!");
        }
    }

    public void StartWave()
    {
        waveTimer = 0f;
        currentEventIndex = 0;
        isWaveActive = true;
    }

    private void SpawnEntity(WaveElement waveElement)
    {
        // Gửi Data quái + Tọa độ cho Factory
        OnSpawnEntity?.Invoke(waveElement.entity);
    }
}