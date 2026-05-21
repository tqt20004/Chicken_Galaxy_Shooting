using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootStrategy
{
    void Shoot(Vector3 spawnPosition, GameObject bulletPrefab, float bulletSpeed);
}
