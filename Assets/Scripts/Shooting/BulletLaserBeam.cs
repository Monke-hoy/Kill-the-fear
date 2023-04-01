using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaserBeam : MonoBehaviour
{

    private BoxCollider2D bulletCollider;

    private Transform bulletTransform;
    
    private LineRenderer lineRenderer;

    void Start()
    {
        bulletTransform = GetComponent<Transform>();
        //Получаю Collider префаба пули
        bulletCollider = GetComponent<BoxCollider2D>();
        //Получаю LineRender префаба пули
        lineRenderer = GetComponent<LineRenderer>();



    }


    private void FixedUpdate()
    {
        Vector3[] positions = new Vector3[2];

        positions[0] = bulletTransform.position;

        positions[1] = (Vector2)positions[0] + (Vector2)bulletTransform.right * bulletCollider.size.x * 6.66f;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);


    }




}
