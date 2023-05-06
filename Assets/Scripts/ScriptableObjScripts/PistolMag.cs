using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PistolMag", menuName = "ScriptableObject/Items/Mags/PistolMag", order = 2)]
public class PistolMag : Mag
{
    private const int pistol_mag_capacity = 7;

    private void OnEnable()
    {
        capacity = pistol_mag_capacity;

        magType = MagType.PistolMag;

        itemType = ItemType.mag;
    }
}
