using System;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    private MoveController moveComponent;
    private ShootController shotController;
    private HealthComponent healthComponent;
    private GameObject currentSkin;
    public bool IsDead = false;

    public BaseStat playerStat { get; private set; }
    private void OnEnable()
    {
        WeaponConfigurator.OnChangedBaseStat += ReceiveChangeStat;
        GameEvents.RequestHealPlayer += HandleHeal;
        GameEvents.RequestDamagePlayer += HandleTakeDamage;
        if (healthComponent != null)
        {
            healthComponent.OnDeath += HandlePlayerDeath;
            //healthComponent.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void HandlePlayerDeath(Vector3 vector)
    {
        GameEvents.OnPlayerDie?.Invoke();
    }

    private void OnDisable()
    {
        WeaponConfigurator.OnChangedBaseStat -= ReceiveChangeStat;
        GameEvents.RequestDamagePlayer -= HandleTakeDamage;
        GameEvents.RequestHealPlayer -= HandleHeal;
        if (healthComponent != null)
        {
            healthComponent.OnDeath -= HandlePlayerDeath;
        }
    }
    private void Awake()
    {
        moveComponent = GetComponent<MoveController>();
        shotController = GetComponent<ShootController>();
        healthComponent = GetComponent<HealthComponent>();
    }
    private void Start() {  }

    public void ReceiveChangeStat(BaseStat stat)
    {
        playerStat = stat;
    }
    public void Init(BaseStat baseStat)
    {
        ReceiveChangeStat(baseStat);
        healthComponent.maxHealth = healthComponent.health = playerStat.maxHealth;
    }
    public void HandleHeal(int amount)
    {
        if (healthComponent == null) throw new System.Exception("HealthComponent is null");
        healthComponent.Heal(amount);
    }

    public void HandleTakeDamage(int damage)
    {
        if(healthComponent == null) throw new System.Exception("HealthComponent is null");
        healthComponent.TakeDamage(damage);
    }


}