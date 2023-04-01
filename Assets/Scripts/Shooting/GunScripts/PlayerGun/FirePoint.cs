using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    private enum FirePoints { PistolPointType, RifleTypePoint, None}

    private FirePoints currentFirePoint = FirePoints.None;

    [SerializeField]
    private GameObject PistolPoint;

    [SerializeField]
    private GameObject RiflePoint;

    private GameObject currentPoint = null;

    public Transform GetCurrentTransform => currentPoint.transform;

    public GameObject GetPP => PistolPoint;

    public GameObject GetRP => RiflePoint;


    public void UpdateCurrentPoint(ref Transform pointTransform)
    {
        pointTransform = currentPoint.transform;
        
    }

    public void ChoosePoint(int NumOfGun)
    { 
        switch (NumOfGun) 
        {
            case 1:
                if (currentFirePoint != FirePoints.PistolPointType)
                { 
                    currentFirePoint = FirePoints.PistolPointType;
                    currentPoint = PistolPoint;
                    
                }
                break;
            case 2:
                if (currentFirePoint != FirePoints.RifleTypePoint)
                {
                    currentFirePoint = FirePoints.RifleTypePoint;
                    currentPoint = RiflePoint;
                    
                }
                break;
            case 3:
                if (currentFirePoint != FirePoints.RifleTypePoint)
                {
                    currentFirePoint = FirePoints.RifleTypePoint;
                    currentPoint = RiflePoint;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
       //Debug.Log(currentPoint.tag);
    }
}
