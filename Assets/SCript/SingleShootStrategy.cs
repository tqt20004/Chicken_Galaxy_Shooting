using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShootStrategy : IShootStrategy
{
    public void Shoot(Vector3 spawnPosition, GameObject bulletPrefab, float bulletSpeed)
    {
        //GameObject bullet = Object.Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        BulletSpawnData data = new BulletSpawnData();
        data.prefab = bulletPrefab;
        data.speed = bulletSpeed;
        data.position = spawnPosition;
        WeaponEvent.EmitSpawnBullet(data);
    }
}
