using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "ScriptableObject/Items/Guns/Pistol", order = 1)]
public class gun_pistol : root_item_gun
{

    [SerializeField] private Sprite unloaded_pistol_inventory_icon;

    public Sprite GetUnloadedInventoryIcon => unloaded_pistol_inventory_icon;




    private void OnEnable()
    {

        gunType = Gun.Guns.pistol;

        shootMode = Gun.ShootMode.semiAuto;

        delayBetweenShots = 0.3f;

        damage = 34;

        bulletSpeed = 10f;

        itemType = ItemType.gun;

        spriteIndex = 1;

        soundIndex = 1;

        firePointIndex = 1;

        AD_index = 1;

        // Две секунды
        reload_time = 2;
    }
}
