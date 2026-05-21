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
        //bulletSpeed = 50f; // override
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //if (rb != null) rb.velocity = Vector3.forward * bulletSpeed;

        //Object.Destroy(bullet, 2f);
    }
}
