using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class MoveController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float minX = -5f;
    public float maxX = 5f;
    float targetX;

    public Transform playerTransform;

    private IMoveStrategy currentMoveStrategy;


    void Start()
    {
        ChangeMoveStrategy(new KeyboardMoveStrategy());  
    }

    public void ChangeMoveStrategy(IMoveStrategy newStrategy)
    {
        currentMoveStrategy = newStrategy;
    }

    void Update()
    {
        if (currentMoveStrategy == null) return;

        targetX = currentMoveStrategy.GetTargetX();

        float newX = playerTransform.position.x + (targetX * moveSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(newX, minX, maxX);

        playerTransform.position = new Vector3(clampedX, playerTransform.position.y, playerTransform.position.z);
    }
}
