using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    //������� ������� ����� � ����������� ����������� transform  (currentPoint)
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
        //�������� ���������� ������� �����:
        firePoints.UpdateCurrentPoint(ref firePointTransform);

        //������ ����� ��� LineRender
        Vector3[] positions = new Vector3[2];
        positions[0] = firePointTransform.position;
        positions[1] = rf.GetHit.point;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
        //Debug.Log(rf.GetDistToTarget);

    }
}

