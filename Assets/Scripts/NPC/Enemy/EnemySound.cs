using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private enum GunSounds {Pistol, ShotGun, Rifle, None};

    private GunSounds SoundChecker = GunSounds.None;

    [SerializeField]
    private AudioClip PistolSound;

    [SerializeField]
    private AudioClip ShotGunSound;

    [SerializeField]
    private AudioClip RifleSound;

    private AudioClip CurrentSound = null;

    public AudioClip GetCurrentSound => CurrentSound;
    public void ChangeGunSound(int numOfGun)
    {
        switch (numOfGun)
        {
            case 1:
                if (SoundChecker != GunSounds.Pistol)
                { 
                    SoundChecker = GunSounds.Pistol;
                    CurrentSound = PistolSound;
                }
                break;
            case 2:
                if (SoundChecker != GunSounds.Rifle)
                {
                    SoundChecker = GunSounds.Rifle;
                    CurrentSound = RifleSound;
                }
                break;
            case 3:
                if (SoundChecker != GunSounds.ShotGun)
                {
                    SoundChecker = GunSounds.ShotGun;
                    CurrentSound = ShotGunSound;
                }
                break;
        }
    }
}
