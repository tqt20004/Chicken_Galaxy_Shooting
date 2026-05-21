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
        // Đăng ký nghe Event X. Không cần kéo thả, tự động bắt link qua hệ thống Static Event
        WeaponConfigurator.OnChangedBulletPrefab += ChangeBullet;
    }

    private void OnDisable()
    {
        // Hủy đăng ký khi bị destroy để tránh tràn bộ nhớ (Memory Leak)
        WeaponConfigurator.OnChangedShooting -= ChangeShootStrategy;
    }
    void Start()
    {
        // SET MẶC ĐỊNH: Vào trận là bắn thẳng, class thuần nên xài "new" cực sạch
        ChangeShootStrategy(new SingleShootStrategy());
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
