using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum States
{
    Intro,
    Playing,
    Outro,
    Pause
}
[System.Serializable]
public struct Stages
{
    public WaveData[] waveList;
}

public class GameManager : MonoBehaviour
{
    public int score;
    public int gold;

    public States curState;
    public Stages curStage;
    public WaveManager waveManager;
    public Action<States> OnChangedGameState;
    public Action<int> OnChangedScore;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartPause();
    }
    private void OnEnable()
    {
        waveManager.OnEndStage += StartOutro;
        HealthComponent.OnDeath += ReceiveDeath;
        GameEvents.RequestChangeGameStates += ChangeStates;
    }

    private void StartOutro()
    {
        ChangeStates(States.Outro);
        OnChangedGameState(States.Outro);
    }
    private void OnDisable()
    {
        waveManager.OnEndStage -= StartOutro;
        HealthComponent.OnDeath -= ReceiveDeath;
        GameEvents.RequestChangeGameStates -= ChangeStates;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.L)) test(); 
        switch (curState)
        {
            case States.Intro:
                IntroUpdate();
                break;
            case States.Playing:
                PlayingUpdate();
                break;
            case States.Outro:
                OutroUpdate();
                break;
            case States.Pause:
                PauseUpdate();
                break;

        }
        
        
    }

    private void test()
    {
         FirebaseManager.Instance.SaveGameProgressAsync(score,gold);
    }

    private void IntroUpdate()
    {
        if (Time.time > 5)
        {
            StartPlaying();
        }
    }
    private void StartPause()
    {
        ChangeStates(States.Pause);
        OnChangedGameState?.Invoke(curState);

    }

    private void PauseUpdate()
    {

    }


    private void StartIntro()
    {
        ChangeStates(States.Intro);
        OnChangedGameState?.Invoke(curState);

    }

    private void StartPlaying()
    {
        ChangeStates(States.Playing);
        OnChangedGameState?.Invoke(curState);
        waveManager.GetStage(curStage);
    }

    private void PlayingUpdate()
    {

    }
    private void OutroUpdate()
    {

    }

    private void ChangeStates(States state)
    {
        curState = state;
        Debug.Log("current State:" + curState);
    }
    ///bonus
    void ReceiveDeath()
    {
        //if(entityData.category == Category.Obstacle) score += 10;
        score += 10;
        OnChangedScore?.Invoke(score);
    }
}



///quick designed code  
[System.Serializable]
public class Stat
{
    [System.NonSerialized] // this var is not allowed to saveJson
    public BaseStat curBaseStat;

    int currentHealth;
    int maxHealth;
    public void Init()
    {
        maxHealth = curBaseStat.maxHealth;
    }
    public void UpgradeHealth(int digital)
    {
        maxHealth += digital;
    } 
    public void UpdateHealth(int x)
    {
        var temp = currentHealth + x;
        currentHealth = UnityEngine.Mathf.Clamp(temp, 0, maxHealth);
        Debug.Log(currentHealth);
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
[CreateAssetMenu(fileName = "newSpaceShip", menuName = "Data/SpaceShip")]
public class BaseStat : ScriptableObject
{
    public string name;
    public int id;
    public int maxHealth;
    public GameObject skinPrefab;
    public float moveSpeed;
    public SpaceShipType shipType;
    public Sprite sprite;
}
public enum SpaceShipType { newbie , basic , normal , advance , god}
//Entity Player Including :skin , stat ,moveCompo , ShotController
