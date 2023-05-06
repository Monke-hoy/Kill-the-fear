using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class root_item_gun : Item
{
    protected Gun.Guns gunType;

    protected Gun.ShootMode shootMode;

    protected float delayBetweenShots;

    protected int damage;

    protected float bulletSpeed;

    protected float lastShotTime = Mathf.NegativeInfinity;

    protected int second_damage;


    // Индекс спрайта персонажа
    protected int spriteIndex;

    // Индекс звука выстрела
    protected int soundIndex;

    // Индекс огневой точки спрайта
    protected int firePointIndex;

    // Индекс разности направления ствола и спрайта
    protected int AD_index;

    // CoolDown для оружия ближнего боя
    protected float coolDown;

    // Время перезарядки
    protected float reload_time;





    public Gun.Guns GetGunType => gunType;

    public Gun.ShootMode GetShootMode => shootMode;

    public float GetDelayBetweenShots => delayBetweenShots;

    public int GetDamage => damage;

    public float GetBulletSpeed => bulletSpeed;

    public float GetLastShotTime => lastShotTime;

    public int GetSpriteIndex => spriteIndex;

    public int GetSoundIndex => soundIndex;

    public int GetFirePointIndex => firePointIndex;

    public int get_AD_index => AD_index;

    public int GetSecondDamage => second_damage;

    public float GetCooldown => coolDown;

    public float GetReloadTime => reload_time;  


}
