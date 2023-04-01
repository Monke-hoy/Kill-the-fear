using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //�������
    private GameObject player;
    //��������� �� ������� �������
    private RangeFinder rangeFinder;

    //������������ �� ������
    private RaycastHit2D WallHit;
    //������������ � ���������
    private RaycastHit2D EnemyHit;

    //����� ������ ���� (������)
    private float deathTime;


    //������� RB2D ������ ���� (������)
    [SerializeField]
    private Rigidbody2D PlayerBulletRB;

    public Rigidbody2D GetPlayerBulletRB => PlayerBulletRB;



    //������� Collider ������ ���� (������)
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
        // ��������� ������������ � ������, ������ ���� �� ���� ������������ �� ������
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

        // ��������� ���������� �� RayCast �� ������

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