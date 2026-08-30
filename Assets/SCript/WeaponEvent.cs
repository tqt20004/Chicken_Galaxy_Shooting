using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;

public struct BulletSpawnData
{
    public GameObject prefab;
    public Vector3 position;
    public float speed;
    public float damage;
    public Quaternion rotation;
}

public static class WeaponEvent
{
    public static Action<BulletSpawnData> OnRequestSpawnBullet;

    public static void EmitSpawnBullet(BulletSpawnData data)
    {
        OnRequestSpawnBullet?.Invoke(data);
    }
}