using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rifle", menuName = "ScriptableObject/Items/Guns/Rifle", order = 2)]
public class gun_rifle : root_item_gun
{

    [SerializeField] private Sprite unloaded_rifle_icon;

    [SerializeField] private Sprite unloaded_rifle_inventory_icon;





    public Sprite GetUnloadedDroppedIcon => unloaded_rifle_icon;

    public Sprite GetUnloadedInventoryIcon => unloaded_rifle_inventory_icon;

    




    private void OnEnable()
    {

        gunType = Gun.Guns.assaultRifle;

        shootMode = Gun.ShootMode.auto;

        delayBetweenShots = 0.1f;

        damage = 18;

        bulletSpeed = 10f;

        itemType = ItemType.gun;

        spriteIndex = 2;

        soundIndex = 2;

        firePointIndex = 2;

        AD_index = 2;

        // Три секунды
        reload_time = 3;

    }

}
