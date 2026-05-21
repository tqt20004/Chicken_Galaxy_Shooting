using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
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
    public void Init(float speed)
    {
        Debug.Log(speed+"zzzzzzzz");
       this.speed = speed;
    }
}
