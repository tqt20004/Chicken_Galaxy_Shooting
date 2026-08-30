using System;
using System.Collections.Generic;
using UnityEngine;

public enum WaveStates
{
    WaveIntro,   // Chờ vài giây chuẩn bị cho mỗi đầu Wave (Hiện chữ "Wave 1")
    WaveCombat,  // Vòng lặp chính: Đếm time, quét Timeline, spawn quái liên tục
    WaveOutro,   // Đã spawn hết quái, chờ người chơi dọn sạch mống quái trên bàn cờ
    StageEnd     // Hết sạch toàn bộ Wave, nằm im chờ GameManager hạ màn
}

public class WaveManager : MonoBehaviour
{
    public Transform[] spawnPointArray;
    public WaveStates curWaveState; // Enum quản lý cục bộ dòng đời của từng WAVE

    public WaveData[] waveDataArray;
    public WaveData curWaveData;
    public int curIndexWave = 0;

    public float waveTimer;
    private int currentEventIndex = 0;
    private bool isWaveActive = false;

    // Timer phụ để đếm ngược thời gian chờ ở WaveIntro
    private float introTimer;

    //public static Action<EntityData, Vector3> OnSpawnEntity;
    public Action<WaveStates> OnChangedWaveState;
    public Action OnEndStage; // Bắn lên cho GameManager hứng khi xong hết mảng Wave

    
    void Start()
    {
        // Khi GameManager chuyển sang StagePlaying, nó sẽ kích hoạt hàm này. 
        // Ở đây tạm gọi ở Start để test độc lập.
        //StartStageWaves();
    }
    public void GetStage(Stages stage)
    {
        waveDataArray = stage.waveList;
        StartStageWaves();

    }

    public void StartStageWaves()
    {
        curIndexWave = 0;

        LoadWaveData();
    }

    private void LoadWaveData()
    {
        // Chốt chặn an toàn: Hết sạch Wave trong Stage thì chuyển sang trạng thái kết thúc
        if (curIndexWave >= waveDataArray.Length)
        {
            ChangeState(WaveStates.StageEnd);
            OnEndStage?.Invoke(); // Bắn tín hiệu: "GameManager ơi, dọn sạch Stage rồi, End Stage đi!"
            return;
        }

        curWaveData = waveDataArray[curIndexWave];

        // Reset thông số, đưa Wave về trạng thái chờ chuẩn bị (WaveIntro)
        introTimer = 0f;
        ChangeState(WaveStates.WaveIntro);
        Debug.Log($"--- CHUẨN BỊ WAVE {curIndexWave + 1} ---");
    }

    void Update()
    {
        // Vận hành máy trạng thái cục bộ của WaveManager
        switch (curWaveState)
        {
            case WaveStates.WaveIntro:
                UpdateWaveIntro();
                break;
            case WaveStates.WaveCombat:
                UpdateWaveCombat();
                break;
            case WaveStates.WaveOutro:
                UpdateWaveOutro();
                break;
            case WaveStates.StageEnd:
                // Nằm im, việc còn lại là của GameManager
                break;
        }
    }

    // 1. XỬ LÝ TRẠNG THÁI CHỜ ĐẦU WAVE
    private void UpdateWaveIntro()
    {
        introTimer += Time.deltaTime;
        // Cho người chơi 3 giây chuẩn bị (nhặt đồ, nạp đạn...) rồi mới cho quái ra
        if (introTimer >= 3f)
        {
            StartWaveCombat();
        }
    }

    private void StartWaveCombat()
    {
        waveTimer = 0f;
        currentEventIndex = 0;
        isWaveActive = true;
        ChangeState(WaveStates.WaveCombat);
        Debug.Log($"--- START FIGHTING WAVE {curIndexWave + 1} ---");
    }

    // 2. XỬ LÝ TRẠNG THÁI CHIẾN ĐẤU (Quét Timeline Spawn Quái)
    private void UpdateWaveCombat()
    {
        if (!isWaveActive || curWaveData == null) return;

        waveTimer += Time.deltaTime;

        while (currentEventIndex < curWaveData.waveList.Count &&
               waveTimer >= curWaveData.waveList[currentEventIndex].spawnTime)
        {
            SpawnEntity(curWaveData.waveList[currentEventIndex]);
            currentEventIndex++;
        }

        // Khi timeline chạy hết sự kiện spawn của Wave này
        if (currentEventIndex >= curWaveData.waveList.Count)
        {
            isWaveActive = false;
            ChangeState(WaveStates.WaveOutro); // Chuyển sang Outro của Wave để check dọn map
            Debug.Log($"Wave {curIndexWave + 1} đã spawn xong quái, chờ dọn sạch...");
        }
    }

    // 3. XỬ LÝ TRẠNG THÁI CHỜ SẠCH QUÁI
    private void UpdateWaveOutro()
    {
        // TODO: Liên kết với hệ thống quản lý Entity/Enemy của ông
        bool isAllEnemiesInWaveDead = true; // Giả lập: Người chơi đã bắn chết sạch quái trên map

        if (isAllEnemiesInWaveDead)
        {
            Debug.Log($"Dọn sạch hoàn toàn Wave {curIndexWave + 1}!");
            curIndexWave++; // Tăng chỉ số sang Wave tiếp theo
            LoadWaveData(); // Quay lại vòng lặp nạp dữ liệu Wave mới
        }
    }

    public void ChangeState(WaveStates newState)
    {
        curWaveState = newState;
        OnChangedWaveState?.Invoke(newState);
    }

    private void SpawnEntity(WaveElement waveElement)
    {

        int index = Mathf.Clamp(waveElement.spawnPointIndex, 0, spawnPointArray.Length - 1);
        Vector3 spawnPos = spawnPointArray[index].position;
        
        GameEvents.RequestSpawnEnemy?.Invoke(waveElement.entity, spawnPos);
    }
}