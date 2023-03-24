using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //Стрелок
    private GameObject player;
    //Дальномер от позиции стрелка
    private RangeFinder rangeFinder;
    //Player bullet is hit
    private RaycastHit2D PlayerHit;
    //Время смерти пули (игрока)
    private float deathTime;


    //Получаю RB2D нужной пули (игрока)
    [SerializeField]
    private Rigidbody2D PlayerBulletRB;

    public Rigidbody2D GetPlayerBulletRB => PlayerBulletRB;



    //Получаю Collider нужной пули (игрока)
    [SerializeField]
    private BoxCollider2D PlayerBulletCollider;

    public BoxCollider2D GetPlayerBulletCollider => PlayerBulletCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rangeFinder = player.GetComponent<RangeFinder>();

        BulletSpeed(PlayerBulletRB);
    }

    void FixedUpdate()
    {
        PlayerHit = hitTheWall(PlayerBulletRB, PlayerBulletCollider);
        deathTime = DeathTime(PlayerHit);

        //Если столкновение со стеной или другим объектом
        if (PlayerHit)
        {

            if (rangeFinder.GetDistToTarget < 0.55f)
            {
                Destroy(gameObject, deathTime);
                Time.timeScale = 1.0f;
            }
            else { Destroy(gameObject, deathTime); }


        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null) { enemy.TakeDamage(damage); Destroy(gameObject, deathTime); }
    }

}
