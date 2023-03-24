using UnityEngine;

public class Shooting : MonoBehaviour
{
    //����� ������ 
    [SerializeField]
    private PlayerGun playerGun;

    [SerializeField]
    private PlayerGunSounds playerSounds; 

    //���������, �� ��������� �������� ���������� ���������
    private RangeFinder rangeFinder;

    //����������� ��������� ��� �������� (����� �� ������������� ��������)
    private float MinFireDist = 0.35f;

    void Awake()
    {
        playerGun.ChangeGun(1);

        playerSounds.ChangePlayerSound(1);

        rangeFinder = playerGun.GetComponent<RangeFinder>();
    }

    void Update()
    {
        //����� ������
        if (Input.GetKey("1"))      { playerGun.ChangeGun(1); playerSounds.ChangePlayerSound(1); }
        else if (Input.GetKey("2")) { playerGun.ChangeGun(2); playerSounds.ChangePlayerSound(2); }
        else if (Input.GetKey("3")) { playerGun.ChangeGun(3); playerSounds.ChangePlayerSound(3); }
        else if (Input.GetKey("4")) { playerGun.ChangeGun(4); playerSounds.ChangePlayerSound(2); }
        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }


        if (playerGun.GetIsTriggered())
        {
            //�������
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto) & (rangeFinder.GetDistToTarget > MinFireDist)) { playerGun.PlayerShoot(); }
        }
        
    }
}
