using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private GameObject indicatorParant;

    public GameObject GetIndicatorParent => indicatorParant;


    public void RememperParent(GameObject parent)
    { 
        indicatorParant = parent;
    }
}
