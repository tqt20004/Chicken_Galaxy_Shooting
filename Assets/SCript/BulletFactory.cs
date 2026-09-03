using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletFactory : MonoBehaviour
{
    [Header("=== DANH MỤC CÁC LOẠI ĐẠN TRONG GAME ===")]
    [Tooltip("Kéo tất cả các Prefab đạn bạn có vào đây và đặt tên ID cho nó!")]
    public List<BulletConfig> bulletCatalog = new List<BulletConfig>();

    private Dictionary<int, GameObject> prefabLookup = new Dictionary<int, GameObject>();

    private Dictionary<int, ObjectPool<Bullet>> pools = new Dictionary<int, ObjectPool<Bullet>>();

    private void OnEnable()
    {
        WeaponEvent.OnRequestSpawnBullet += HandleSpawnBullet;
    }

    private void OnDisable()
    {
        WeaponEvent.OnRequestSpawnBullet -= HandleSpawnBullet;
    }

    private void Awake()
    {
        // Nạp danh mục Prefab vào Dictionary
        foreach (var config in bulletCatalog)
        {
            if (config.bulletId != null && config.prefab != null)
            {
                prefabLookup[config.bulletId] = config.prefab;
            }
        }
    }

    private void HandleSpawnBullet(BulletSpawnData info)
    {
        int bulletTypeId = info.id;

        // 1. Kiểm tra nếu truyền sai hoặc truyền 0 -> Tự động chuyển về đạn mặc định (ID = 1)
        if (!prefabLookup.TryGetValue(bulletTypeId, out GameObject prefabTemplate))
        {
            Debug.LogWarning($"[Cảnh Báo] ID đạn [{bulletTypeId}] không tồn tại! Tự động chuyển về Đạn Mặc Định (ID = 1).");

            bulletTypeId = 1; // Ép về ID số 1

            // Nếu cả ID 1 cũng chưa kéo Prefab vào Catalog thì mới chịu thua
            if (!prefabLookup.TryGetValue(bulletTypeId, out prefabTemplate))
            {
                Debug.LogError("BulletFactory: Ngay cả Đạn Mặc Định (ID = 1) bạn cũng chưa kéo Prefab vào kìa!");
                return;
            }
        }

        // 2. Tạo Pool và lấy đạn ra bắn bình thường...
        if (!pools.ContainsKey(bulletTypeId))
        {
            pools[bulletTypeId] = new ObjectPool<Bullet>(
                createFunc: () => CreateBullet(prefabTemplate),
                actionOnGet: (b) => b.gameObject.SetActive(true),
                actionOnRelease: (b) => b.gameObject.SetActive(false),
                actionOnDestroy: (b) => Destroy(b.gameObject),
                defaultCapacity: 50,
                maxSize: 300
            );
        }

        Bullet bullet = pools[bulletTypeId].Get();
        bullet.transform.position = info.position;
        bullet.transform.rotation = info.rotation;
        bullet.SetPool(pools[bulletTypeId]);
        bullet.Init(info.speed);
    }
    private Bullet CreateBullet(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        return obj.GetComponent<Bullet>() ?? obj.AddComponent<Bullet>();
    }
    [System.Serializable]
    public struct BulletConfig
    {
        public int bulletId;       // Tên mã: "DEFAULT", "LASER", "ROCKET"
        public GameObject prefab;     // Kéo Prefab đạn tương ứng vào đây
    }
}