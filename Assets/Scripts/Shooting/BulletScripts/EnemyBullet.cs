using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{

    //RB2D пули врага
    [SerializeField]
    private Rigidbody2D EnemyBulletRB;

    //Коллайдер пули врага
    [SerializeField]
    private BoxCollider2D EnemyBulletCollider;

    //Enemy bullet is hit
    private RaycastHit2D EnemyHit;

    //Время жизни после столкновения коллайдера
    private float deathTime;



    void Start()
    {
        BulletSpeed(EnemyBulletRB);
    }

    void Update()
    {
        EnemyHit = hitTheWall(EnemyBulletRB, EnemyBulletCollider);
        deathTime = DeathTime(EnemyHit);

        //Если столкновение со стеной или другим объектом                                                
        if (EnemyHit) { Destroy(gameObject, deathTime); }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null) { Destroy(gameObject, deathTime); }
        //player.TakeDamage(damage); 
    }
}