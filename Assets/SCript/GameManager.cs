using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum States
{
    Intro,
    Playing,
    Outro
}
[System.Serializable]
public struct Stages
{
    public WaveData[] waveList;
}

public class GameManager : MonoBehaviour
{
    public States curState;
    public Stages curStage;
    public WaveManager waveManager;
    public Action<States> OnChangedGameState;
    // Start is called before the first frame update
    void Start()
    {
        StartIntro();
    }
    private void OnEnable()
    {
        waveManager.OnEndStage += StartOutro;
    }

    private void StartOutro()
    {
        ChangeStates(States.Outro);
        OnChangedGameState(States.Outro);
    }

    private void OnDisable()
    {
        waveManager.OnEndStage
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        
    }
    private void IntroUpdate()
    {
        if (Time.time > 10)
        {
            StartPlaying();
        }
    }
    private void StartIntro()
    {
        curState = States.Intro;
        ChangeStates(curState);
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
    }
}
