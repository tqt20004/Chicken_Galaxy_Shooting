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
    }
    private void OnDisable()
    {
        WeaponConfigurator.OnChangedBaseStat -= ReceiveChangeStat;
    }
    private void Awake()
    {
        moveComponent = GetComponent<MoveController>();
        shotController = GetComponent<ShootController>();
        healthComponent = GetComponent<HealthComponent>();
    }
    private void Start() {  }
    //public void SetAuto()
    //{
    //    WeaponConfigurator.Instance.A_Event();
    //    healthComponent.maxHealth = healthComponent.health = playerStat.maxHealth;
    //}
    public void ReceiveChangeStat(BaseStat stat)
    {
        playerStat = stat;
    }
    public void Init(BaseStat baseStat)
    {
        ReceiveChangeStat(baseStat);
        healthComponent.maxHealth = healthComponent.health = playerStat.maxHealth;
    }



    //public void TakeDamage(int dmg)
    //{
    //    playerStat.UpdateHealth(-dmg);
    //    var x = playerStat.GetCurrentHealth();
    //    if (x <= 0) IsDead = true;
    //}
}