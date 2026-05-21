using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    float speed;
    // Start is called before the first frame update
    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    public void Init(float speed)
    {
        Debug.Log(speed+"zzzzzzzz");
       this.speed = speed;
        Fly();
    }
    public void Fly()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.forward * speed;
        }
        else
        {
            Debug.LogError("RIGIDBODY NULL!");
        }
    }
}
