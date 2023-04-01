using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //Стрелок
    private GameObject player;
    //Дальномер от позиции стрелка
    private RangeFinder rangeFinder;

    //Столкновение со стеной
    private RaycastHit2D WallHit;
    //Столкновение с существом
    private RaycastHit2D EnemyHit;

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
        rangeFinder = player.GetComponentInChildren<RangeFinder>();

        BulletSpeed(PlayerBulletRB);
    }


    private bool hasHitWall = false;



    void FixedUpdate()
    {
        // Проверяет столкновение с врагом, только если не было столкновения со стеной
        if (!hasHitWall)
        {
            EnemyHit = hitTheEnemy(PlayerBulletRB, PlayerBulletCollider);
            if (EnemyHit)
            {
                Enemy enemy = EnemyHit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }

        // Проверяет столкнулся ли RayCast со стеной

        WallHit = hitTheWall(PlayerBulletRB, PlayerBulletCollider);
        if (WallHit)
        {
            hasHitWall = true;
            float deathTime = DeathTime(WallHit);
            if (rangeFinder.GetDistToTarget < 0.55f)
            {
                Destroy(gameObject, deathTime);
            }
            else
                { Destroy(gameObject, deathTime); }
        }
    }

}