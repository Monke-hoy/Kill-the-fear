using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Id;

    public int GetId => Id;

    [SerializeField] private int SceneId;

    public int GetSceneId => SceneId;

    private int health = 100;
    public int enemyHealth { get { return health; } set { health = value; } }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }

    void Die()
    {
        EnemyManager.instance.AddToDeadList(Id, SceneId);
        Destroy(gameObject);
    }
}
