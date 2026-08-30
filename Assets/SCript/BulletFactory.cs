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
        // 2. NẠP DAMAGE RIÊNG CỦA SÚNG VÀO HandleTouchingComponent!
        //if (bulletObj.TryGetComponent<HandleTouchingComponent>(out var touchComp))
        //{
        //    touchComp.ChangeDamage((int)info.damage);
        //}
        //if (bulletObj.TryGetComponent<Bullet>(out var bulletLogic))
        //{
        //    // Inject data vào viên đạn để nó tự bay và tự gây damage
        //    bulletLogic.Init(info.speed);
        //}
    }
}