using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    //Получаю огневую точку с необходимым компонентом transform  (currentPoint)
    private FirePoint firePoints;

    private Transform firePointTransform;

    [SerializeField]
    RangeFinder rf;


    [SerializeField]
    LineRenderer lineRenderer;


    private void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePointTransform = firePoints.GetCurrentTransform;
    }


    private void FixedUpdate()
    {
        //Обновляю актуальную огневую точку:
        firePoints.UpdateCurrentPoint(ref firePointTransform);

        //Массив точек для LineRender
        Vector3[] positions = new Vector3[2];
        positions[0] = firePointTransform.position;
        positions[1] = rf.GetHit.point;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
        //Debug.Log(rf.GetDistToTarget);

    }
}

