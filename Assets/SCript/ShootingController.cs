using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Transform firePoint;       
    public GameObject bulletPrefab;   
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;     // Cứ 0.2 giây bắn 1 viên

    private IShootStrategy currentShootStrategy;
    private float nextFireTime;

    private void OnEnable()
    {
        WeaponConfigurator.OnChangedBulletPrefab += ChangeBullet;
        WeaponConfigurator.OnChangedShooting += ChangeShootStrategy;
    }

    private void OnDisable()
    {
        WeaponConfigurator.OnChangedBulletPrefab -= ChangeBullet;
        WeaponConfigurator.OnChangedShooting -= ChangeShootStrategy;
    }
    void Start()
    {
        //ChangeShootStrategy(new SingleShootStrategy());
        ChangeShootStrategy(new TripleSpreadShootStrategy());
    }
    

    public void ChangeShootStrategy(IShootStrategy newStrategy)
    {
        currentShootStrategy = newStrategy;
    }
    public void ChangeBullet(GameObject prefab)
    {
        this.bulletPrefab = prefab;
    }
    void Update()
    {
        if (currentShootStrategy == null) return;

        // Giữ chuột trái hoặc bấm nút Cách để nã đạn
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            currentShootStrategy.Shoot(firePoint.position, bulletPrefab, bulletSpeed);
        }
    }
}
