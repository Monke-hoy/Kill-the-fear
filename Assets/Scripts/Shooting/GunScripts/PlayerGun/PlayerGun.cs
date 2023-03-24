using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    //��� ����� ��������
    [SerializeField]
    private PlayerGunSounds playerSounds;

    //��������� � ������� ������
    [SerializeField]
    private RangeFinder rangeFinder;
    //����� ������� ���� (�� ������)
    [SerializeField]
    private Transform firePoint;
    //��������� ���� (��� ������ ���� PlayerBullet)

    [SerializeField]
    private GameObject bulletPrefab;

    public GameObject GetBulletPrefab => bulletPrefab;

    //��������� ��� ��������� (������� � LookDirection �������)
    private WarriorMovement correction;

    //����� � ������ ����
    private GameObject player;
    private PlayerBullet bullet;



    //����������� ��������� ��� ��������
    private float MinFireDist = 0.3f;



    public void PlayerShoot() => Shoot();




    protected override void Shoot()
    {
        if ((Time.time - lastShotTime < delayBetweenShots) || (rangeFinder.GetDistToTarget <= MinFireDist) ) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<PlayerBullet>();
        playerSounds.PlaySound();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - correction.angleDifference;
                for (int i = -4; i < 4; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation);
                        bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<PlayerBullet>();
                        bullet.damage = damage;
                        bullet.bulletSpeed = bulletSpeed;
                    }
                }
                
                break;
        }
    }



    void Start()
    {
        correction = GetComponent<WarriorMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        bullet = player.GetComponent<PlayerBullet>();
    }
}
