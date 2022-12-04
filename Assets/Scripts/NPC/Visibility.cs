using UnityEngine;

public class Visibility : MonoBehaviour
{
    private GameObject enemy;
    private Enemy enemyScript;
    public Rigidbody2D player;
    public bool isVisible = false;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    Rigidbody2D rb2d;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        Vector2 direction = player.position - rb2d.position;
        Vector2 spreadOnSides = new Vector2(direction.y, -direction.x).normalized * circleCollider.radius;
        LayerMask mask = LayerMask.GetMask("Player", "Bullets", "Enemy");
        if (Physics2D.Raycast(rb2d.position, direction, direction.magnitude, ~mask.value) &&
            Physics2D.Raycast(rb2d.position + spreadOnSides, direction - spreadOnSides, (direction - spreadOnSides).magnitude, ~mask.value) &&
            Physics2D.Raycast(rb2d.position - spreadOnSides, direction + spreadOnSides, (direction + spreadOnSides).magnitude, ~mask.value))                                                                                                                                                                                               
        { 
            isVisible = false;
            spriteRenderer.enabled = false;
            enemyScript.enemyIsKillable = false;
        }
        else
        {
            isVisible = true;
            spriteRenderer.enabled = true;
            enemyScript.enemyIsKillable = true;
        }
    }
}
