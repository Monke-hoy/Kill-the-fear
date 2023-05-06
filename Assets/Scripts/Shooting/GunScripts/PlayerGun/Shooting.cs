using System.Linq;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //Ствол игрока 
    [SerializeField]
    private PlayerGun playerGun;

    [SerializeField]
    private PlayerGunSounds playerSounds;

    private FirePoint firePoints;

    private WarriorMovement wm;

    private PlayerChangeSprites changingSprites;

    private InventoryManager inventoryManager;




    private root_item_gun root_gun;

    public root_item_gun set_root_gun { set { root_gun = value; } }

    private bool is_reloading = false;

    public bool set_reload_status { set { is_reloading = value; }  }

    private int spriteIndex => root_gun.GetSpriteIndex;

    private int soundIndex => root_gun.GetSoundIndex;

    private int AD_Index => root_gun.get_AD_index;

    private int firePointIndex => root_gun.GetFirePointIndex;



    private int StartGun = 1;








    private void Start()
    {
        wm = GetComponent<WarriorMovement>();

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        wm.SwitchAD(StartGun);

        playerGun.ChangeGun(1);
    }









    void Awake()
    {

        firePoints = GetComponent<FirePoint>();

        firePoints.ChoosePoint(StartGun);

        playerSounds.ChangePlayerSound(StartGun);
        

        changingSprites = GetComponent<PlayerChangeSprites>();

        changingSprites.changeSprite(0);

    }







    KeyCode[] keys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha0, KeyCode.I };

    void Update()
    {



        /*
         * Останавливаю перезарядку, если нажата клавиша 
        */


        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key) || Input.GetButton("Fire1"))
            {
                if (is_reloading) { Debug.Log("Изменилось значение Blocking"); inventoryManager.block_current_reload = true; }
                break;
            }
        }






        

        if (!playerGun.TriggerIsPulled && !(is_reloading && playerGun.GetGunType() == Gun.Guns.shotgun))
        {

            //Выбор слота
            if (Input.GetKey("1"))      { playerGun.ChangeGun(1); }
            else if (Input.GetKey("2")) { playerGun.ChangeGun(2); }
            else if (Input.GetKey("3")) { playerGun.ChangeGun(3); }
            else if (Input.GetKey("0")) { playerGun.ChangeGun(0); }
        }





        // Дробовик не сможет стрелять, пока перезарядка не прекратиться
        if (is_reloading && playerGun.GetGunType() == Gun.Guns.shotgun) { return; }

        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }







        if (playerGun.TriggerIsPulled)
        {
            //Выстрел
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto)) { playerGun.PlayerShoot(); }
        }
        
    }
}
