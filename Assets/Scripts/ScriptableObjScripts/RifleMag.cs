using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RifleMag", menuName = "ScriptableObject/Items/Mags/RifleMag", order = 1)]
public class RifleMag : Mag
{

    private const int rifle_mag_capacity = 30;

    private void OnEnable()
    {
        capacity = rifle_mag_capacity;

        magType = MagType.RifleMag;

        itemType = ItemType.mag;
    }
}
