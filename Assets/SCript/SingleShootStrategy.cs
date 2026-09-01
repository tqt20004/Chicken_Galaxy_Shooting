using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ShootStrategyRegister
{
    public readonly static Dictionary<string, IShootStrategy> strategies = new Dictionary<string, IShootStrategy>()
    {
        { "SingleShoot", new SingleShootStrategy() },
        { "TripleSpreadShoot", new TripleSpreadShootStrategy() }
    };
    public static IShootStrategy GetStrategy(string strategyName)
    {
        if (strategies.TryGetValue(strategyName, out IShootStrategy strategy))
        {
            return strategy;
        }
        else
        {
            Debug.LogWarning($"Shoot strategy '{strategyName}' not found. Using default SingleShootStrategy.");
            return new SingleShootStrategy(); // Default strategy
        }
    }
}
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
public class TripleSpreadShootStrategy : IShootStrategy
{
    private float spreadAngle = 15f; // angle
    private float damagePerBullet = 15f; // Sát thương mỗi viên

    public void Shoot(Vector3 spawnPosition, GameObject bulletPrefab, float bulletSpeed)
    {
        // 3 góc bắn: Lệch trái 15 độ, Bắn thẳng ở giữa 0 độ, Lệch phải 15 độ
        float[] angles = new float[] { -spreadAngle, 0f, spreadAngle };

        foreach (float angle in angles)
        {
            // 1. Tính góc xoay đạn theo trục Y
            Quaternion bulletRotation = Quaternion.Euler(0f, angle, 0f);

            // 2. Đóng gói dữ liệu đạn
            BulletSpawnData data = new BulletSpawnData
            {
                prefab = bulletPrefab,
                position = spawnPosition,
                speed = bulletSpeed,
                rotation = bulletRotation,
                damage = damagePerBullet
            };

            // 3. Bắn event sinh đạn
            WeaponEvent.EmitSpawnBullet(data);
        }
    }
}
