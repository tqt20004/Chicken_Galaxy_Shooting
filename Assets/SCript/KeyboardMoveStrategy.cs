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

    public float GetTargetX()
    {
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        return keyboardInput;
    }
}
