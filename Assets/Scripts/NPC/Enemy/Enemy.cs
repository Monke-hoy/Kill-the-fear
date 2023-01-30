using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 100;
    public float enemyHealth { get { return health; } set { health = value; } }

    private bool IsKillable = true;
    public bool enemyIsKillable { get { return IsKillable ; } set { IsKillable = value; } }

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
