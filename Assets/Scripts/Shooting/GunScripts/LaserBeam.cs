using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    [SerializeField]
    private Transform transform;

    [SerializeField]
    RangeFinder rf;


    [SerializeField]
    LineRenderer lineRenderer;

    private void FixedUpdate()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        positions[1] = rf.GetHit.point;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);

    }
}

