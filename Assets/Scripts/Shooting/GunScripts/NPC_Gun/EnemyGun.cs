using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{ 
                     
    private EnemyBullet bullet;

    [SerializeField]
    private float damage_reducer;
        
    private GameObject enemy;

    private EnemyMovement correction;

    [SerializeField]
    private GameObject EnemybulletPrefab;

    [SerializeField]
    private Transform EnemyFirePoint;

    public void EnemyShoot() => Shoot();

    void Start()
    {
        correction = GetComponent<EnemyMovement>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        bullet = enemy.GetComponent<EnemyBullet>();
    }

    protected override void Shoot()
    {
        
        if (Time.time - lastShotTime < delayBetweenShots) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(EnemybulletPrefab, EnemyFirePoint.position, EnemyFirePoint.rotation).GetComponent<EnemyBullet>();
        bullet.damage = damage * damage_reducer;
        bullet.bulletSpeed = bulletSpeed;
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) *  Mathf.Rad2Deg + (360f - correction.angleDifference);
                for (int i = -4; i < 4; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation);
                        bullet = Instantiate(EnemybulletPrefab, EnemyFirePoint.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<EnemyBullet>();
                        bullet.damage = damage;
                        bullet.bulletSpeed = bulletSpeed;
                    }
                }
                break;
        }
    }
}