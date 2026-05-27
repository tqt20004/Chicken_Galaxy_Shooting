using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFactory : MonoBehaviour
{
    EntityData entityData;
    public GameObject patternPrefab;
    public IMoveStrategy curMoveStrategy;
    private void OnEnable()
    {
        WaveManager.OnSpawnEntity += Spawn;
    }
    private void OnDisable()
    {
        WaveManager.OnSpawnEntity -= Spawn;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Spawn(EntityData entityData, Vector3 spawnPoint)
    {
        if(entityData.prefab != null) { patternPrefab = entityData.prefab; }
        Vector3 temp = spawnPoint;
        IMoveStrategy moveStrategy = ClassifyMoving(entityData);
        GameObject pattern =Instantiate(patternPrefab,temp,Quaternion.identity);
        Rigidbody rb = pattern.GetComponent<Rigidbody>() ?? pattern.AddComponent<Rigidbody>();
        HealthComponent healthComponent =pattern.GetComponent<HealthComponent>()?? pattern.AddComponent<HealthComponent>();
        healthComponent.maxHealth= healthComponent.health = entityData.maxHealth;
        HandleTouchingComponent handleTouchingComponent = pattern.GetComponent<HandleTouchingComponent>() ?? pattern.AddComponent<HandleTouchingComponent>();
        handleTouchingComponent.ChangeDamage(entityData.touchDamage);
        MoveController moveController = pattern.GetComponent<MoveController>() ?? pattern.AddComponent<MoveController>();
        moveController.ChangeMoveStrategy(moveStrategy);
        moveController.moveSpeed = entityData.moveSpeed;
        pattern.layer = LayerMaskExtensions.ToLayerIndex(entityData.layerMask);
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
}
