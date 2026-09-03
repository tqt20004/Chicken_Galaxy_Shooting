using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    float speed;
    IObjectPool<Bullet> objectPool;
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
       this.speed = speed;
        Fly();
    }
    public void Fly()
    {
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }
        else
        {
            Debug.LogError("RIGIDBODY NULL!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
     //delete this
        if (collision.gameObject.GetComponent<Bullet>() != null) return;
        Debug.Log("2");
        //if (collision.Hea)
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerEntity>() != null) return;
        if (other.GetComponent<Bullet>() != null) return;
        ReleaseToPool();
        //Destroy(this.gameObject);

    }

    public void SetPool(IObjectPool<Bullet> pool)
    {
        this.objectPool = pool;
    }
    private void ReleaseToPool()
    {
        // Reset lại vận tốc vật lý trước khi cất
        if (rb != null) rb.velocity = Vector3.zero;
        // Trả về kho nếu kho có tồn tại, không thì Destroy luôn
        if (objectPool != null)
        {
            objectPool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
