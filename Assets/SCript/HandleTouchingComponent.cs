using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleTouchingComponent : MonoBehaviour
{
    public int damage = 10;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //PlayerEntity player = other.GetComponent<PlayerEntity>();
        //Debug.Log("touch1");
        //if (player != null)
        //{
        //    player.playerStat.UpdateHealth(damage);
        //    return;
        //}
        HealthComponent health = other.gameObject.GetComponent<HealthComponent>();

        if (health != null)
        {
            Debug.Log("get2" + other.name);
            health.TakeDamage(damage);
        }
    }
    public void ChangeDamage(int amount)
    {
        this.damage = amount;
    }
}
