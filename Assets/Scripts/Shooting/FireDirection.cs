using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FireDirection : MonoBehaviour
{
    //��� ��������� ����������� ���� ������� AngleDifference 
    [SerializeField]
    WarriorMovement wm;
    
    //�����
    private GameObject player;
    //����� ������ 
    private PlayerGun gun;

    //����������� ��������
    private Vector2 fireDirection;

    //����������� ��������, �������� ��������, ����� ��������� � ������ ��������
    public Vector2 GetFireDir => fireDirection;


    //������� ������� ����� � ����������� ����������� transform  (currentPoint)
    private FirePoint firePoints;

    private Transform firePointTransform;



    private void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePointTransform = firePoints.GetCurrentTransform;

        player = GameObject.FindGameObjectWithTag("Player"); 

    }

    void FixedUpdate()
    {
        firePoints.UpdateCurrentPoint(ref firePointTransform);

        //���� ������� ����� ������� � �������� (������ ��� �����)
        //float RotateAngle = wm.currentAngleDifference;
        float RotateAngle = 0;

        //�������
        Quaternion q = Quaternion.AngleAxis(RotateAngle, Vector3.forward);

        //����������� ������� ���
        Vector3 StartDirection = firePointTransform.right;
        

        //��� ��� ���������� ���� ���� fireDir
        fireDirection = q * StartDirection;
    }
}
