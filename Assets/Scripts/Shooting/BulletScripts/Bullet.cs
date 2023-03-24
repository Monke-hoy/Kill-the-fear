using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class Bullet : MonoBehaviour
{

    private const float LifeTime = 2;
    private float LifeTimer = 0;


    public float bulletSpeed = 10f;
    public int damage = 10;


    public RaycastHit2D hitTheWall(Rigidbody2D rb2d, BoxCollider2D collider)
    {
        //Столкновение со стеной или другим объектом
        return Physics2D.Raycast(rb2d.position, rb2d.transform.right, collider.size.x * 1.9f, LayerMask.GetMask("Bullet", "Creatures"));
    }

    public float DeathTime(RaycastHit2D hit)
    {
        //Время смерти пули, после столкновения с коллизией (Для того чтобы пуля исчезла ближе к объекту). Коллайдер у пули больше самого спрайта 
        return Time.fixedDeltaTime + hit.distance * Time.deltaTime;
    }

    public Vector2 BulletSpeed(Rigidbody2D rb2d)
    {
        //Задается скорость пули
        rb2d.velocity =  new Vector2(bulletSpeed * rb2d.transform.right.x, bulletSpeed * rb2d.transform.right.y);
        return rb2d.velocity;
    }

    private void Update()
    {
        //Время жизни пули, если она не встретит коллизию
        LifeTimer += Time.deltaTime;
        if (LifeTimer >= LifeTime) { Destroy(gameObject); LifeTimer = 0; }
    }




}
