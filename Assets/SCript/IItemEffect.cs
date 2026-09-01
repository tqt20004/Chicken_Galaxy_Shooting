using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. Interface hành vi vật phẩm
public interface IItemEffect
{
    void ApplyEffect(PlayerEntity player, float value);
}

// 2. Tác dụng: Nhận Vàng (Cộng tiền vào GameManager)
public class GoldItemEffect : IItemEffect
{
    public void ApplyEffect(PlayerEntity player, float value)
    {
        GameEvents.OnCoinCollected?.Invoke((int)value);
        Debug.Log($"<color=yellow>+{(int)value} Vàng!</color>");
    }
}

// 3. Tác dụng: Hồi Máu (Yêu cầu Player hồi máu)
public class HealItemEffect : IItemEffect
{
    public void ApplyEffect(PlayerEntity player, float value)
    {
        GameEvents.RequestHealPlayer?.Invoke((int)value);
        Debug.Log($"<color=green>Yêu cầu Hồi {(int)value} HP!</color>");
    }
}

// 4. Tác dụng: Buff Súng 3 Tia
public class WeaponBuffItemEffect : IItemEffect
{
    private string strategyKey;
    public WeaponBuffItemEffect(string strategyKey)
    {
        this.strategyKey = strategyKey;
    }
    public void ApplyEffect(PlayerEntity player, float value)
    {
        if (player.TryGetComponent<ShootController>(out var shootCtrl))
        {
            // Lấy chiến lược bắn từ ShootStrategyRegister theo đúng key!
            IShootStrategy newStrategy = ShootStrategyRegister.GetStrategy(strategyKey);
            shootCtrl.ChangeShootStrategy(newStrategy);
            Debug.Log($"<color=cyan>Kích hoạt BUFF: Súng {strategyKey}!</color>");
        }
    }
}