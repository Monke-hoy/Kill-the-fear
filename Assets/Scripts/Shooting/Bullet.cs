using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class Bullet : MonoBehaviour
{

    public Gun gun;
 
    public BoxCollider2D collider;

    private RaycastHit2D HitTheWall;

    private float DeathTime;

    Rigidbody2D rb2d;

    private GameObject Warrior;
    private Gun UserGun;
    private WarriorMovement correction;

    private const float LifeTime = 2;
    private float LifeTimer = 0;


    public float bulletSpeed = 10f;
    public int damage = 10;

    public float[] GunRange;

    void Awake()
    {
        float bulletRange = Random.Range(GunRange[0], GunRange[1]);

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(bulletSpeed * rb2d.transform.right.x + bulletRange, bulletSpeed * rb2d.transform.right.y + bulletRange);
        Warrior = GameObject.FindWithTag("Player");
        UserGun = Warrior.GetComponent<Gun>();
        correction = Warrior.GetComponent<WarriorMovement>();
        gun = Warrior.GetComponent<Gun>();
    }

    private void Update()
    {

        LifeTimer += Time.deltaTime;
        if (LifeTimer >= LifeTime) { Destroy(gameObject); LifeTimer = 0; }

        float CastAngle = Mathf.Atan2(rb2d.transform.right.y, rb2d.transform.right.x) * Mathf.Rad2Deg + gun.angleDifference;

        HitTheWall = Physics2D.Raycast(rb2d.position, rb2d.transform.right, collider.size.x, LayerMask.GetMask("Bullets", "Creatures"));

        
        
        DeathTime = Time.fixedDeltaTime + HitTheWall.distance * Time.deltaTime;



        if (HitTheWall)
        {

            if (UserGun.GetDistToTarget < 0.55f) 
            { 
                Destroy(gameObject, DeathTime / 3);
            }
            else { Destroy(gameObject, DeathTime); }


        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if ( (enemy != null)  ) 
        { 
            if (enemy.enemyIsKillable) { enemy.TakeDamage(damage); }
            Destroy(gameObject, DeathTime);
        }
    }




}
