using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class Gun : MonoBehaviour
{
    //������
    protected enum Guns { pistol, shotgun, assaultRifle, none };

    //������ ����
    public enum ShootMode { auto, semiAuto, off };

    //��������� ������
    protected Guns current_gun = Guns.none;
    //����� �������� 
    protected ShootMode shootMode = ShootMode.off;
    public ShootMode GetShootMode() => shootMode;


    //��������� ������
    protected float delayBetweenShots;
    protected float lastShotTime = Mathf.NegativeInfinity;
    protected int damage;
    protected float bulletSpeed;
    protected float pelletsDeviation = 3;
    protected float pelletsSpread = 5;

    //��������� �����
    private bool isTriggerPulled = false;
    public bool GetIsTriggered() => isTriggerPulled;

    public void PullTheTrigger()
    {
        isTriggerPulled = !isTriggerPulled;
        if (isTriggerPulled)
        {
            //��� ���������� ��������
            if ((shootMode == ShootMode.semiAuto) ) { Shoot(); }
        }
    }

    
    private Bullet bullet;
    private GameObject bulletPrefab;
    private Transform firePoint;

    protected virtual void Shoot()
    {
        if (Time.time - lastShotTime < delayBetweenShots) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        
    }

    public void ChangeGun(int numberOfGun)
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
            case 3:
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
        }
    }

}
