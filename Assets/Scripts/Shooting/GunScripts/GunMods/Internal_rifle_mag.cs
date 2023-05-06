using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Internal_rifle_mag : GunMag
{









    public void LoadMagToGun(GameObject mag, out bool success_addition)
    {
        success_addition = false;

        if (mag != null)
        {
            FloorItem floorItem = mag.GetComponent<FloorItem>();
            if (floorItem != null)
            {
                Mag item_mag = floorItem.getItem as Mag;
                if (item_mag != null)
                {
                    if (item_mag.GetMagType == Mag.MagType.RifleMag)
                    {
                        magObject = mag;
                        success_addition = true;
                    }
                }
            }
        }
    }


}
