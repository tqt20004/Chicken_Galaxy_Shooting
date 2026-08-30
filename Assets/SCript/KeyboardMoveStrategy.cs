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

public class ZigZagMoveStrategy : IMoveStrategy
{
    private float frequency; // frequency 
    private float amplitude; // biendo

    // Constructor 
    public ZigZagMoveStrategy(float frequency = 6f, float amplitude = 4.5f)
    {
        this.frequency = frequency;
        this.amplitude = amplitude;
    }

    public Vector3 GetTargetDirection()
    {
        //Calc horizontal X position based on sine wave
        float horizontalX = Mathf.Sin(Time.time * frequency) * amplitude;

        // The vertical Z position is always -1 to move downwards
        Vector3 direction = new Vector3(horizontalX, 0f, -1f);

        return direction.normalized;
    }
}
