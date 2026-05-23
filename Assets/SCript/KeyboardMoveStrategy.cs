using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMoveStrategy :  IMoveStrategy
{
    void Start()
    {
        Debug.Log("KeyBoard");
    }

    public Vector3 GetTargetDirection()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); // A/D
        float inputZ = Input.GetAxisRaw("Vertical");   // W/S

        return new Vector3(inputX, 0f, inputZ).normalized;
    }
}
public class FallDownStrategy : IMoveStrategy
{
    public Vector3 GetTargetDirection()
    {
        Vector3 dir = Vector3.back;
        return dir;
    }
}
