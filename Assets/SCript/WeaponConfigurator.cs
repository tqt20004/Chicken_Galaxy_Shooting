using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfigurator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public IShootStrategy shootStrategy;

    public static Action<GameObject> OnChangedBulletPrefab;
    public static Action<IShootStrategy> OnChangedShooting;
    // Start is called before the first frame update
    void Start()
    {
        shootStrategy = new SingleShootStrategy(); // set default
        A_Event();
    }
    // Update is called once per frame
    public void ChangeBullet(GameObject prefab)
    {
        this.bulletPrefab = prefab;
        A_Event();
    }
     public void ChangeShooting(IShootStrategy shootStrategy)
    {
        this.shootStrategy = shootStrategy;
        A_Event();
    }

    public void A_Event()
    {
        OnChangedBulletPrefab?.Invoke(bulletPrefab);
        OnChangedShooting?.Invoke(shootStrategy);
    }
}
