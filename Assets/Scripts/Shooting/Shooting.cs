using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Gun gun;

    void Start()
    {
        gun.ChangeGun(1);
    }

    void Update()
    {
        if (Input.GetKey("1"))      { gun.ChangeGun(1); }
        else if (Input.GetKey("2")) { gun.ChangeGun(2); }
        else if (Input.GetKey("3")) { gun.ChangeGun(3); }
        //else if (Input.GetKey("4")) { gun.ChangeGun(4); }             Пока недоступно
        if (Input.GetButtonDown("Fire1")) { gun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { gun.PullTheTrigger(); }
    }
}
