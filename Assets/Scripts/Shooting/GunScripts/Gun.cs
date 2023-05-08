using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class Gun : MonoBehaviour
{
    //������
    public enum Guns { pistol, shotgun, assaultRifle, hammer, none };

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
    protected float pelletsDeviation = 1;
    protected float pelletsSpread = 0.5f;

    //��������� �����
    private bool isTriggerPulled = false;
    public bool TriggerIsPulled
    {
        get { return isTriggerPulled; }
        set { isTriggerPulled = value; }
    }





    // ������ ������ ��������� (��� ������)
    private AmmunitionGunSlot[] gun_slots = new AmmunitionGunSlot[3];

    private SecondArmSlot[] secondArmSlots = new SecondArmSlot[2];

    private PlayerGunSounds playerSounds;

    private NewPlayerCS changingSprites;

    private FirePoint firePoints;

    private WarriorMovement wm;


    private void Awake()
    {

        gun_slots[0] = GameObject.Find("GunSlot(2)").GetComponent<AmmunitionGunSlot>();
        gun_slots[1] = GameObject.Find("GunSlot(3)").GetComponent<AmmunitionGunSlot>();
        gun_slots[2] = GameObject.Find("GunSlot(1)").GetComponent<AmmunitionGunSlot>();
        
        secondArmSlots[0] = GameObject.Find("SecondArmSlot(1)").GetComponent<SecondArmSlot>();


        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        playerGun = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();



        changingSprites = GetComponent<NewPlayerCS>();

        playerSounds = GetComponent<PlayerGunSounds>();

        firePoints = GetComponent<FirePoint>();

        wm = GetComponent<WarriorMovement>();


    }







    private PlayerGun playerGun;

    public void PullTheTrigger()
    {
        isTriggerPulled = !isTriggerPulled;
        if (isTriggerPulled)
        {
            //��� ���������� ��������
            if ((shootMode == ShootMode.semiAuto) ) { Shoot(); }
            if (current_gun == Guns.hammer) { playerGun.Kick(); }
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













    // ������� ������
    protected bullets_capacity current_capacity;

    // ������
    protected GameObject gunObject;

    private Shooting shootingScript;

    protected root_item_gun root_weapon;

    public root_item_gun get_root_weapon => root_weapon;

    private int current_slot;
    public int get_current_slot => current_slot;





    public GameObject GetCurrentSlot()
    {
        if (current_gun != Guns.hammer)
        {
            return gun_slots[current_slot - 1].gameObject;
        }
        else
            return secondArmSlots[0].gameObject;
    }

    public Guns GetGunType() => current_gun;

    private float current_reload_time;

    public float get_current_reload_time => current_reload_time;








    private int[] gun_indexes = { 0, 1, 2 };
    public void UpdateGun(int index)
    {

        if (!gun_indexes.Contains(index)) { UpdateHummer(); return; }

        if (gun_slots[index].object_in_slot != null)
        {
            // ������� ���������� �� ������ � ���� �����
            root_item_gun gun_data = gun_slots[index].object_in_slot.GetComponent<FloorItem>().getItem as root_item_gun;

            isTriggerPulled = false;
            current_gun = gun_data.GetGunType;
            delayBetweenShots = gun_data.GetDelayBetweenShots;
            damage = gun_data.GetDamage;
            bulletSpeed = gun_data.GetBulletSpeed;
            shootMode = gun_data.GetShootMode;
            lastShotTime = gun_data.GetLastShotTime;
            current_reload_time = gun_data.GetReloadTime;
            current_slot = index + 1;

            // ������� ������ � ������� �� ����
            gunObject = gun_slots[index].object_in_slot;
            shootingScript.set_root_gun = gunObject.GetComponent<FloorItem>().getItem as root_item_gun;


            changingSprites.changeSprite(gun_data.GetSpriteIndex);
            playerSounds.ChangePlayerSound(gun_data.GetSoundIndex);
            firePoints.ChoosePoint(gun_data.GetFirePointIndex);
            wm.SwitchAD(gun_data.get_AD_index);


            if (current_gun != Guns.shotgun)
            {
                current_capacity = gunObject.GetComponent<GunMag>().GetMagInGun.GetComponent<mag>();
            }
            else
            {
                current_capacity = gunObject.GetComponent<shotgun_capacity>();
            }
        }
        else
        {
            UpdateHummer();
        }
    }








    private void UpdateHummer()
    {
        // ������� ���������� �� ������ � ���� �����
        root_item_gun gun_data = secondArmSlots[0].object_in_slot.GetComponent<FloorItem>().getItem as root_item_gun;

        isTriggerPulled = false;
        current_gun = gun_data.GetGunType;
        delayBetweenShots = gun_data.GetDelayBetweenShots;
        damage = gun_data.GetDamage;
        bulletSpeed = gun_data.GetBulletSpeed;
        shootMode = gun_data.GetShootMode;
        lastShotTime = gun_data.GetLastShotTime;
        current_slot = 0;

        // ������� ������ � ������� �� ����
        gunObject = secondArmSlots[0].object_in_slot;
        root_weapon = gunObject.GetComponent<FloorItem>().getItem as root_item_gun;
        shootingScript.set_root_gun = root_weapon;
        changingSprites.changeSprite(0);
    }








    public virtual void ChangeGun(int numberOfGun)
    {
        switch (numberOfGun)
        {
            case 1:
                if (gun_slots[0] != null)
                {
                    UpdateGun(0);
                }
                break;
            case 2:
                if (gun_slots[1] != null)
                {
                    UpdateGun(1);
                }
                break;
            case 3:
                if (gun_slots[2] != null)
                {
                    UpdateGun(2);
                }
                break;
            case 0:
                if (secondArmSlots[0] != null)
                {
                    // �������
                    if (secondArmSlots[0].object_in_slot != null)
                    {
                        UpdateHummer();
                    }
                }
                break;
        }
    }




}
