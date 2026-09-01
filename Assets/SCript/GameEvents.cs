using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<BaseStat> RequestChangeShip;

    public static Action<BaseStat> OnShipChanged;

    public static Action<States> RequestChangeGameStates;

    public static Action<States> OnChangeGameStates;

    public static Action RequestSpawnPlayer;
    public static Action OnSpawnPlayer;
    public static Action OnPlayerDie;

    public static Action<EntityData, Vector3> RequestSpawnEnemy;
    public static Action<Vector3> OnEnemyDie;


    public static Action<int> RequestDamagePlayer;
    public static Action<int> RequestHealPlayer;

    public static Action<int> OnCoinCollected;
}