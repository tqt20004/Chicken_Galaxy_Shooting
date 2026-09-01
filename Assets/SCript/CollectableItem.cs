using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Runtime Data & Effect")]
    public ItemData itemData;
    private IItemEffect itemEffect;

    [Header("Movement & Magnet Settings")]
    public float fallSpeed = 2.5f;
    public float magnetDistance = 5f;
    public float flyToPlayerSpeed = 18f; // Tăng nhẹ tốc độ hút cho mượt
    public float lifeTime = 10f;

    private Transform playerTransform;
    private PlayerEntity cachedPlayer;

    public void Init(ItemData data, IItemEffect effect)
    {
        this.itemData = data;
        this.itemEffect = effect;

        cachedPlayer = FindObjectOfType<PlayerEntity>();
        if (cachedPlayer != null) playerTransform = cachedPlayer.transform;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // 1. Tự xoay vòng tròn
        transform.Rotate(Vector3.up * 180f * Time.deltaTime);

        // 2. Nam châm hút về Player
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // 👉 KHI ĐÃ HÚT VÀO SÁT NGƯỜI (< 0.8m) -> ĂN VÀ TỰ HỦY LUÔN!
            if (distance <= 0.8f)
            {
                Collect(cachedPlayer);
                return;
            }

            // Nếu trong tầm hút nam châm -> Bay vèo về Player
            if (distance <= magnetDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, flyToPlayerSpeed * Time.deltaTime);
                return;
            }
        }

        // 3. Nếu ở xa -> Trôi từ từ xuống dưới (Z-)
        transform.position += Vector3.back * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 👉 TÌM PLAYERENTITY Ở CẢ OBJECT CHA LẪN CON
        var player = other.GetComponentInParent<PlayerEntity>();
        if (player != null)
        {
            Collect(player);
        }
    }

    // Hàm thu thập chung
    private void Collect(PlayerEntity player)
    {
        if (itemEffect != null && itemData != null)
        {
            itemEffect.ApplyEffect(player, itemData.effectValue);
        }

        Destroy(gameObject);
    }
}