using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfigurator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public IShootStrategy shootStrategy;
    public BaseStat stat;
    public static WeaponConfigurator Instance;
    public static Action<GameObject> OnChangedBulletPrefab;
    public static Action<IShootStrategy> OnChangedShooting;
    public static Action<BaseStat> OnChangedBaseStat;

    //public List<int> inventorySpaceShipIDList;
    public List<BaseStat> inventorySpaceShip;
    public Data spaceShipDataBase;

    private void Awake()
    {
        // Kiểm tra và xử lý trùng lặp Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void OnEnable()
    {
        GameEvents.RequestChangeShip += ChangeBaseStat;
    }

    private void OnDisable()
    {
        GameEvents.RequestChangeShip -= ChangeBaseStat;
    }


    // Start is called before the first frame update
    void Start()
    {
        shootStrategy = new SingleShootStrategy(); // set default
        A_Event();
        tempFeature.GetIt();
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
     public void ChangeBaseStat(BaseStat baseStat)
    {
        this.stat = baseStat;
        A_Event();
        GameEvents.OnShipChanged?.Invoke(stat);
    }

    ///old code (need replace by GameEvents )
    public void A_Event()
    {
        OnChangedBulletPrefab?.Invoke(bulletPrefab);
        OnChangedShooting?.Invoke(shootStrategy);
        OnChangedBaseStat?.Invoke(stat);
    }
}



/// ///temp code 
/// </summary>
public static class tempFeature 
{
    
    public static void GetIt()
    {
        foreach(var i in WeaponConfigurator.Instance.spaceShipDataBase.inventorySpaceShip)
        {
            foreach (var j in FirebaseManager.Instance.baseStatID)
            {
                if (i.id == j) WeaponConfigurator.Instance.inventorySpaceShip.Add(i);
            }
        }
    }
}
[CreateAssetMenu(fileName = "newDataBaseSpaceShip", menuName = "Data/DataBase InventorySpaceShip")]

public class Data : ScriptableObject
{
    public List<BaseStat> inventorySpaceShip;

}
