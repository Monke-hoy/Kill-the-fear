using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "ScriptableObject/Items/Weapons/Hammer", order = 1)]
public class Hammer_weapon : root_item_gun
{


    private void OnEnable()
    {
        gunType = Gun.Guns.hammer;

        shootMode = Gun.ShootMode.off;

        delayBetweenShots = 0;

        damage = 0;

        bulletSpeed = 0;

        spriteIndex = 0;

        soundIndex = 0;

        firePointIndex = 1;

        AD_index = 1;

        second_damage = 100;

        coolDown = 0.6f;

        itemType = ItemType.edged_weapon;
    }

}
