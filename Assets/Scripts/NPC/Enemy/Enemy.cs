using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Id;

    public int GetId => Id;

    [SerializeField] private int SceneId;

    public int GetSceneId => SceneId;

    private int health = 100;
    public int enemyHealth { get { return health; } set { health = value; } }

    public Sprite DeadEnemySprite;
    public bool IsDead = false;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }

    void Die()
    {
        EnemyManager.Instance.AddToDeadList(Id, SceneId);
        GetComponent<Visibility>().spriteRenderer.sprite = DeadEnemySprite;
        IsDead = true;
        GetComponent<CircleCollider2D>().enabled = false;
        this.transform.Rotate(0, 0, 60f);
    }
}
