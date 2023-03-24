using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeFinder : MonoBehaviour
{
    
    private EnemyMovement wm = new EnemyMovement();

    public RaycastHit2D GetHit => HitLookDir;
    
    //�������� ���������
    private RaycastHit2D HitLookDir;

    private RaycastHit2D Hit;

    //������� �� �������� �� �������� ����� ���������� ���������
    [SerializeField]
    private Transform ShooterAxis;
    [SerializeField]
    private FireDirection fireDirection;


    //��������� �� ����. �� ������� = 1, ����� �������� �� �����
    private float DistToTarget = 1f;

    public float GetDistToTarget => DistToTarget;



    //������������ ��������� �� ������� �������� ���
    private const byte OneHundredMeters = 100;




    void Update()
    {
        //��� �� ���� ������� ����� ��� ����������� ��������� �� ����. ������������ ���� � �������� 
        HitLookDir = Physics2D.Raycast(ShooterAxis.position, new Vector2(fireDirection.GetFireDir.x, fireDirection.GetFireDir.y), OneHundredMeters, LayerMask.GetMask("Player", "Creatures"));
        //��������� �� ���� 
        DistToTarget = Vector2.Distance(new Vector2(ShooterAxis.position.x, ShooterAxis.position.y), HitLookDir.point);
      
    }
}
