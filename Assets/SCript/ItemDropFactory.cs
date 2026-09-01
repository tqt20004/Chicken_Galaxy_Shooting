using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropFactory : MonoBehaviour
{
    [Header("Bảng Rơi Đồ (Drop Table)")]
    // Kéo thả tất cả các file ItemData (Coin, Heal, GunBuff) vào danh sách này trong Inspector
    public List<ItemData> dropTable;

    private void OnEnable()
    {
        // 📡 Đăng ký lắng nghe Tổng Đài: Quái chết ở đâu là tôi tới đó đúc đồ!
        GameEvents.OnEnemyDie += HandleSpawnLoot;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDie -= HandleSpawnLoot;
    }

    private void HandleSpawnLoot(Vector3 deathPosition)
    {
        if (dropTable == null || dropTable.Count == 0) return;

        foreach (var itemData in dropTable)
        {
            if (itemData == null || itemData.prefab == null) continue;

            // Kiểm tra tỷ lệ rơi của từng món (DropRate)
            if (Random.value <= itemData.dropRate)
            {
                // 1. Sinh GameObject Prefab tại vị trí quái chết
                GameObject itemObj = Instantiate(itemData.prefab, deathPosition, Quaternion.identity);

                // 2. Lấy component CollectableItem trên Prefab
                CollectableItem collectableItem = itemObj.GetComponent<CollectableItem>()
                                                  ?? itemObj.AddComponent<CollectableItem>();

                // 3. Phân loại Chiến lược tác dụng (Strategy) tương ứng
                IItemEffect effect = ClassifyEffect(itemData);

                // 4. 💉 TIÊM THẲNG DATA & STRATEGY VÀO OBJECT!
                collectableItem.Init(itemData, effect);

                break; // Mỗi con quái rơi 1 món (xóa dòng này nếu muốn rơi nhiều món cùng lúc)
            }
        }
    }

    // Hàm chọn Strategy tương ứng dựa vào ItemData
    private IItemEffect ClassifyEffect(ItemData data)
    {
        switch (data.itemType)
        {
            case ItemType.Gold:
                return new GoldItemEffect();

            case ItemType.Heal:
                return new HealItemEffect();

            case ItemType.WeaponBuff:
                // Truyền chuỗi string data.type (ví dụ "TripleSpreadShoot") vào Effect
                return new WeaponBuffItemEffect(data.type);

            default:
                return new GoldItemEffect();
        }
    }
}
