using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGunSounds : MonoBehaviour
{

    [SerializeField]
    private AudioClip PistolSound;

    [SerializeField]
    private AudioClip RifleSound;

    [SerializeField]
    private AudioClip ShotgunSound;

    [SerializeField]
    private AudioClip none;

    private AudioClip currentSound = null;

    public void ChangePlayerSound(int numberOfGun)
    {
        switch (numberOfGun)
        {
            case 1:
                currentSound = PistolSound;
                break;
            case 2:
                currentSound = RifleSound;
                break;
            case 3:
                currentSound = ShotgunSound;
                break;
            default:
                currentSound = none;
                break;
        }
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(currentSound);
    }



}
