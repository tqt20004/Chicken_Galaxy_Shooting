using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class MoveController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float minX = -15f;
    public float maxX = 15f;
    public float minZ = -15f;
    public float maxZ = 15f;
    float targetX;

    public Transform playerTransform;

    private IMoveStrategy currentMoveStrategy = new KeyboardMoveStrategy();


    void Start()
    {
        //ChangeMoveStrategy(new KeyboardMoveStrategy());
        playerTransform = this.transform;
    }

    public void ChangeMoveStrategy(IMoveStrategy newStrategy)
    {
        currentMoveStrategy = newStrategy;
    }

    void Update()
    {
        if (currentMoveStrategy == null) return;

        // Lấy hướng đi 3D (X, 0, Z)
        Vector3 moveDirection = currentMoveStrategy.GetTargetDirection();

        // Tính vị trí mới trên mặt đất
        float newX = playerTransform.position.x + (moveDirection.x * moveSpeed * Time.deltaTime);
        float newZ = playerTransform.position.z + (moveDirection.z * moveSpeed * Time.deltaTime);

        // Giới hạn lại trong boong-ke map
        float clampedX = Mathf.Clamp(newX, minX, maxX);
        float clampedZ = Mathf.Clamp(newZ, minZ, maxZ);

        playerTransform.position = new Vector3(clampedX, playerTransform.position.y, clampedZ);

        //Debug.Log(currentMoveStrategy);
    }
}
    