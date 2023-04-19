using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "ScriptableObject/Items/Guns/Pistol", order = 1)]
public class gun_pistol : Item 
{
    private Gun gunClass;

    private Gun.Guns gunType;

    private Gun.ShootMode shootMode;

    private float delayBetweenShots;

    private int damage;

    private float bulletSpeed;

    private float lastShotTime = Mathf.NegativeInfinity;




    public Gun.Guns GetGunType => gunType;

    public Gun.ShootMode GetShootMode => shootMode;

    public float GetDelayBetweenShots => delayBetweenShots;

    public int GetDamage => damage;

    public float GetBulletSpeed => bulletSpeed;

    public float GetLastShotTime => lastShotTime;




    private void Start()
    {
        gunClass = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

        gunType = Gun.Guns.pistol;

        shootMode = Gun.ShootMode.semiAuto;

        delayBetweenShots = 0.3f;

        damage = 34;

        bulletSpeed = 10f;

        itemType = ItemType.gun;
    }
}
