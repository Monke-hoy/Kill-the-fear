using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "ScriptableObject/Items/Guns/Shotgun", order = 3)]
public class gun_shotgun : root_item_gun
{

    private int Capasity = 8;

    private float BulletLoadTime = 0.25f;

    private float BulletGrabTime = 1f;



    public int GetCapasity => Capasity;

    public float GetBulletLoadTime => BulletLoadTime;

    public float GetBulletGrabTime => BulletGrabTime;









    private void OnEnable()
    {

        gunType = Gun.Guns.shotgun;

        shootMode = Gun.ShootMode.semiAuto;

        delayBetweenShots = 1.0f;

        damage = 11;

        bulletSpeed = 10f;

        itemType = ItemType.gun;

        spriteIndex = 3;

        soundIndex = 3;

        firePointIndex = 3;

        AD_index = 3;

        // Время перезарядки для одного патрона
        reload_time = 0.7f;

    }
}
