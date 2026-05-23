using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extentions 
{

}
public static class LayerMaskExtensions
{
    // trans LayerMask turn into Layer int 
    public static int ToLayerIndex(this LayerMask mask)
    {
        return mask.value > 0 ? Mathf.RoundToInt(Mathf.Log(mask.value, 2)) : 0;
    }
}
