using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health = 100f;
    public float playerHealth { get { return health; } set { health = value; } }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}