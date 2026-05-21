using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private void OnEnable()
    {
        WeaponEvent.OnRequestSpawnBullet += HandleSpawnBullet;
    }

    private void OnDisable()
    {
        WeaponEvent.OnRequestSpawnBullet -= HandleSpawnBullet;
    }

    private void HandleSpawnBullet(BulletSpawnData info)
    {
        Debug.Log("get" + info.speed);
        // Ở đây có thể kết hợp Object Pooling để lấy đạn ra thay vì Instantiate
        GameObject bulletObj = Instantiate(info.prefab, info.position, Quaternion.identity);

        Bullet bulletScript;
        bool x = bulletObj.TryGetComponent<Bullet>(out bulletScript);
        if (x) bulletScript.Init(info.speed);
        
        //if (bulletObj.TryGetComponent<Bullet>(out var bulletLogic))
        //{
        //    // Inject data vào viên đạn để nó tự bay và tự gây damage
        //    bulletLogic.Init(info.speed);
        //}
    }
}