using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletStack;


[CreateAssetMenu(fileName = "shotgun_bullets", menuName = "ScriptableObject/Items/Bullets/shotgun_bullets", order = 3)]
public class shotgun_bullets : BulletStack
{


    private const int shotgun_bullets_stack_capacity = 32;

    private void OnEnable()
    {
        stack_capacity = shotgun_bullets_stack_capacity;

        bulletsType = BulletsType.shotgunBullets;

        itemType = ItemType.bullet;
    }
}
