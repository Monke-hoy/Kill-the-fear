using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMag : MonoBehaviour
{
    private int capasity;

    public int MagCapasity
    { 
        get { return capasity; }
        set { capasity = value; }
    }

    private int current_count;

    public int currentMagCount 
    { 
        get { return current_count; } 
        set { current_count = value; } 
    }


}
