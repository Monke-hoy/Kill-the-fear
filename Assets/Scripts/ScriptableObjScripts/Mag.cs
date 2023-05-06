using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : Item
{
    public enum MagType { RifleMag, PistolMag }


    /*
     * Емкость магазина 
    */

    protected int capacity;

    public int GetCapacity => capacity;


    /*
     * Тип магазина 
    */

    protected MagType magType;

    public MagType GetMagType => magType;


}
