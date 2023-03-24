using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDirection : MonoBehaviour
{
    //�����
    private GameObject player;
    //����� ������ 
    private PlayerGun gun;
    //������ ����� ������ ��������
    private Transform firePoint;

    //����������� ��������
    private Vector2 fireDirection;

    //����������� ��������, �������� ��������, ����� ��������� � ������ ��������
    public Vector2 GetFireDir => fireDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        gun = player.GetComponent<PlayerGun>();
        firePoint = gun.GetComponent<Transform>();
    }

    void Update()
    {
        //������� ����������� �������� (������� ���, ��� �� ��� �)
        fireDirection = firePoint.right;
    }
}
