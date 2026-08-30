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
        
        HealthComponent health = other.gameObject.GetComponent<HealthComponent>();

        // Case 1: target on PLAYER -> Call GAMEEVENTS!
        if (other.CompareTag("Player") || other.GetComponent<PlayerEntity>() != null)
        {
            GameEvents.RequestDamagePlayer?.Invoke(damage);
            return;
        }
        //CAse 2: Not Player -> Call IDamageable and minus health
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage);
        }

        //if (health != null)
        //{
        //    Debug.Log("get2" + other.name);
        //    health.TakeDamage(damage);
        //}
    }
    public void ChangeDamage(int amount)
    {
        this.damage = amount;
    }
}
