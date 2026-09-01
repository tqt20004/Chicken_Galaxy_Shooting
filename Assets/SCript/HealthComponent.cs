using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour , IDamageable
{
    public int health;
    public int maxHealth;
    //public Stat healthStat;
    public  Action<float> OnHealthChanged; 
    public   Action<Vector3> OnDeath;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Die();
            return;
        }
        OnHealthChanged?.Invoke(health);
    }
    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHealthChanged?.Invoke(health); //Afterwards add Invoke to know heal or damage
    }

    public void Die()
    {
        var currentPos = GetCurrentPos();
        OnDeath?.Invoke(currentPos);
        Destroy(gameObject);
    }
    Vector3 GetCurrentPos()
    {
        return this.transform.position;
    }
}
