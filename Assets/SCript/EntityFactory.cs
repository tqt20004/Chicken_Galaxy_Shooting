using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityFactory : MonoBehaviour
{
    EntityData entityData;
    public GameObject patternPrefab;
    public GameObject player_plank_prefab;
    public IMoveStrategy curMoveStrategy;

    public BaseStat curBaseStat;
    private void OnEnable()
    {
        GameEvents.RequestSpawnEnemy += Spawn;
        GameEvents.OnShipChanged += GetData;
        GameEvents.RequestSpawnPlayer += SpawnPlayer;
    }
    private void OnDisable()
    {
        GameEvents.RequestSpawnEnemy -= Spawn;
        GameEvents.OnShipChanged -= GetData;
        GameEvents.RequestSpawnPlayer -= SpawnPlayer;
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Spawn(EntityData entityData, Vector3 spawnPoint)
    {
        Debug.Log($"<color=green>[EntityFactory] ĐANG SPAWN QUÁI: {entityData.enemyName} tại tọa độ: {spawnPoint}</color>");

        if (entityData.prefab != null) { patternPrefab = entityData.prefab; }
        Vector3 temp = spawnPoint;
        IMoveStrategy moveStrategy = ClassifyMoving(entityData);
        GameObject pattern =Instantiate(patternPrefab,temp,Quaternion.identity);
        Rigidbody rb = pattern.GetComponent<Rigidbody>() ?? pattern.AddComponent<Rigidbody>();
        HealthComponent healthComponent =pattern.GetComponent<HealthComponent>()?? pattern.AddComponent<HealthComponent>();
        healthComponent.maxHealth= healthComponent.health = entityData.maxHealth;
        HandleTouchingComponent handleTouchingComponent = pattern.GetComponent<HandleTouchingComponent>() ?? pattern.AddComponent<HandleTouchingComponent>();
        handleTouchingComponent.ChangeDamage(entityData.touchDamage);
        MoveController moveController = pattern.GetComponent<MoveController>() ?? pattern.AddComponent<MoveController>();

        //test new feature: zigzag move strategy
        moveStrategy = new ZigZagMoveStrategy();
        moveController.ChangeMoveStrategy(moveStrategy);
        moveController.moveSpeed = entityData.moveSpeed;
        pattern.layer = LayerMaskExtensions.ToLayerIndex(entityData.layerMask);
    }
    public void SpawnPlayer()
    {
        Debug.Log($"[Check Bug] curBaseStat có null không? => {curBaseStat == null}");

        if (curBaseStat != null)
        {
            Debug.Log($"[Check Bug] Tên của BaseStat là gì? => {curBaseStat.name}");
            Debug.Log($"[Check Bug] skinPrefab trong nó có null không? => {curBaseStat.skinPrefab == null}");
        }
        Vector3 vector3 = new Vector3(0, 1, 5);
        //Vector3 shootingPoint = new Vector3(0, 0, 1);
        //GameObject playerEntity = Instantiate(FirebaseManager.Instance.curBaseStat.skinPrefab, vector3, Quaternion.identity);
        GameObject playerEntity = Instantiate(curBaseStat.skinPrefab, vector3, Quaternion.identity);
        
        PlayerEntity playerEntityScript = playerEntity.AddComponent<PlayerEntity>();
        playerEntityScript.Init(curBaseStat);
        GameEvents.OnSpawnPlayer?.Invoke();
    }

    IMoveStrategy ClassifyMoving(EntityData entityData)
    {
        if(entityData.category == Category.Enemy) 
        {
            return new FallDownStrategy();
        }
        else if(entityData.category == Category.Obstacle)
        {
            return new FallDownStrategy();
        }
        return new FallDownStrategy();
    }
    public void Init(EntityData entityData)
    {
        this.entityData = entityData;
    }

    public void GetData(BaseStat baseStat)
    {
        this.curBaseStat = baseStat;
    }
}
