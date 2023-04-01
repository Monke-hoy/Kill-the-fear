using UnityEngine;

public class Visibility : MonoBehaviour
{
    private Transform player;

    public Transform GetPlayerAxis => player;

    public bool isVisible = false;
    public SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
    }

    void FixedUpdate()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(player.position.x, player.position.y);
        Vector2 direction = playerPosition - position;
        Vector2 spreadOnSides = new Vector2(direction.y, -direction.x).normalized * circleCollider.radius;
        LayerMask mask = LayerMask.GetMask("Enemy", "Player", "Bullet");
        if (Physics2D.Raycast(position, direction, direction.magnitude, ~mask.value) &&
             Physics2D.Raycast(position + spreadOnSides, direction - spreadOnSides, (direction - spreadOnSides).magnitude, ~mask.value) &&
             Physics2D.Raycast(position - spreadOnSides, direction + spreadOnSides, (direction + spreadOnSides).magnitude, ~mask.value))
        {
            isVisible = false;
            spriteRenderer.enabled = false;
        }
        else
        {
            isVisible = true;
            spriteRenderer.enabled = true;
        }
    }
}
