using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeFinder : MonoBehaviour
{
    

    public RaycastHit2D GetHit => HitLookDir;
    
    //�������� ���������
    private RaycastHit2D HitLookDir;

    
    [SerializeField]
    private FireDirection fireDirection;


    //��������� �� ����. �� ������� = 1, ����� �������� �� �����
    private float DistToTarget = 1f;

    public float GetDistToTarget => DistToTarget;



    //������������ ��������� �� ������� �������� ���
    private const byte OneHundredMeters = 100;


    //������� ������� ����� � ����������� ����������� transform  (currentPoint)
    private FirePoint firePoints;

    private Transform firePointTransform;


    private void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePointTransform = firePoints.GetCurrentTransform;
    }



    void FixedUpdate()
    {
        // �������� ���������� ������� ����� 
        firePoints.UpdateCurrentPoint(ref firePointTransform);

        //��� �� ���� ������� ����� ��� ����������� ��������� �� ����. ������������ ���� � �������� 
        HitLookDir = Physics2D.Raycast(firePointTransform.position, new Vector2(fireDirection.GetFireDir.x, fireDirection.GetFireDir.y), OneHundredMeters, LayerMask.GetMask("Player", "Environment"));
        
        //��������� �� ���� 
        DistToTarget = Vector2.Distance(new Vector2(firePointTransform.position.x, firePointTransform.position.y), HitLookDir.point);
      
    }
}
