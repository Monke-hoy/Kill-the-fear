using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStack : Item
{

    public enum BulletsType { pistolBullets, rifleBullets, shotgunBullets }


    // Максимальное количество в стеке
    protected int stack_capacity;

    public int GetStackCapacity => stack_capacity;


    // Тип пуль
    protected BulletsType bulletsType;

    public BulletsType GetbulletsType => bulletsType;

}
