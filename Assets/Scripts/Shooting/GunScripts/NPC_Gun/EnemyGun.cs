using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{ 
                     
    private EnemyBullet bullet;

    private EnemySound enemySound;

    public EnemySound GetEnemySound => enemySound;
        
    private GameObject enemy;

                           
    private EnemyMovement correction;

    [SerializeField]
    private GameObject EnemybulletPrefab;

    [SerializeField]
    private Transform EnemyFirePoint;

    //Номер ствола от 1 до 3
    [SerializeField]
    private int numOfGun;

    //Дефолтное значение, которое встанет вместо numOfGun, если введут некорректное значение
    private int DefaultGunNum = 3;
    public int GetNumOfGun => numOfGun;

    public void EnemyShoot() => Shoot();

    void Start()
    {
        enemySound = GetComponent<EnemySound>();
        correction = GetComponent<EnemyMovement>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        bullet = enemy.GetComponent<EnemyBullet>();

        //Проверка для некорректных значений
        if ((numOfGun < 1) && (numOfGun > 3)) { numOfGun = DefaultGunNum; }
    }

    protected override void Shoot()
    {

        if (Time.time - lastShotTime < delayBetweenShots) { return; }

        lastShotTime = Time.time;

        GetComponent<AudioSource>().PlayOneShot(enemySound.GetCurrentSound);
        
        bullet = Instantiate(EnemybulletPrefab, EnemyFirePoint.position, EnemyFirePoint.rotation).GetComponent<EnemyBullet>();
        
        bullet.damage = damage;
        
        bullet.bulletSpeed = bulletSpeed;
        
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) *  Mathf.Rad2Deg  + correction.angleDifference + 3f;
                for (int i = -8; i < 8; ++i)
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








    public override void ChangeGun(int numberOfGun)
    {
        switch (numberOfGun)
        {
            case 1:
                if (current_gun != Guns.pistol)
                {
                    current_gun = Guns.pistol;
                    delayBetweenShots = 0.3f;
                    damage = 34;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 2:
                if (current_gun != Guns.assaultRifle)
                {
                    current_gun = Guns.assaultRifle;
                    delayBetweenShots = 0.1f;
                    damage = 18;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.auto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 3:
                if (current_gun != Guns.shotgun)
                {
                    current_gun = Guns.shotgun;
                    delayBetweenShots = 1.0f;
                    damage = 11;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
        }
    }


}