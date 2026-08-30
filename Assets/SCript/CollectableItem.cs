using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Runtime Data & Effect")]
    public ItemData itemData;          // Cục ItemData được tiêm vào
    private IItemEffect itemEffect;     // Chiến lược tác dụng (Gold / Heal / WeaponBuff)

    [Header("Movement & Magnet Settings")]
    public float fallSpeed = 2.5f;       // Tốc độ trôi từ từ xuống dưới
    public float magnetDistance = 5f;    // Khoảng cách bắt đầu hút nam châm
    public float flyToPlayerSpeed = 15f; // Tốc độ hút vèo vào tàu
    public float lifeTime = 10f;         // Tự hủy nếu người chơi không nhặt

    private Transform playerTransform;

    // 💉 HÀM TIÊM DATA & EFFECT KHI FACTORY SINH RA:
    public void Init(ItemData data, IItemEffect effect)
    {
        this.itemData = data;
        this.itemEffect = effect;

        // Tìm tàu của Player trong scene để làm mục tiêu hút
        var player = FindObjectOfType<PlayerEntity>();
        if (player != null) playerTransform = player.transform;

        // Tự hủy sau 10 giây nếu rơi ra ngoài màn hình
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // 1. Đồng tiền / Hộp quà tự xoay vòng tròn cho đẹp mắt
        transform.Rotate(Vector3.up * 180f * Time.deltaTime);

        // 2. Cơ chế Nam Châm: Nếu Player ở gần -> Hút vèo về phía tàu
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= magnetDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, flyToPlayerSpeed * Time.deltaTime);
                return;
            }
        }

        // 3. Nếu chưa vào vùng nam châm -> Trôi từ từ từ trên xuống dưới (Z-)
        transform.position += Vector3.back * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Khi chạm vào Player
        if (other.TryGetComponent<PlayerEntity>(out var player))
        {
            // 💥 KÍCH HOẠT HIỆU ỨNG ĐÃ ĐƯỢC TIÊM (Ăn Vàng / Hồi Máu / Đổi Súng)
            if (itemEffect != null && itemData != null)
            {
                itemEffect.ApplyEffect(player, itemData.effectValue);
            }

            // Nhặt xong tự hủy
            Destroy(gameObject);
        }
    }
}