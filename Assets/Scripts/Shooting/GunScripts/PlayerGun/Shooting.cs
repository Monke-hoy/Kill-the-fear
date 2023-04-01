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

    private int currentGun = 1;

    private void Start()
    {
        wm = GetComponent<WarriorMovement>();

        wm.SwitchAD(currentGun);
    }

    void Awake()
    {
        firePoints = GetComponent<FirePoint>();

        playerGun.ChangeGun(currentGun);

        firePoints.ChoosePoint(currentGun);

        playerSounds.ChangePlayerSound(currentGun);

    }

    void Update()
    {
        //Выбор ствола
        if (Input.GetKey("1"))      { playerGun.ChangeGun(1); playerSounds.ChangePlayerSound(1); firePoints.ChoosePoint(1); wm.SwitchAD(1); }
        else if (Input.GetKey("2")) { playerGun.ChangeGun(2); playerSounds.ChangePlayerSound(2); firePoints.ChoosePoint(2); wm.SwitchAD(2); }
        else if (Input.GetKey("3")) { playerGun.ChangeGun(3); playerSounds.ChangePlayerSound(3); firePoints.ChoosePoint(3); wm.SwitchAD(3); }
        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }


        if (playerGun.GetIsTriggered())
        {
            //Выстрел
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto)) { playerGun.PlayerShoot(); }
        }
        
    }
}
