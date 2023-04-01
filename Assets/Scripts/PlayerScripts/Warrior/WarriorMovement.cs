using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{

    //��������� ����������� LookDirection
    private float StartWarriorDir;

    private Vector2 LookDirection;
    public Vector2 WarriorLookDir => LookDirection;

    //���� �����������
    private sbyte DirectionSignX = 0;
    private sbyte DirectionSignY = 0;

    //������ ��������� ��� ���������� ���� ��������, ���� �� ������ ��������
    private const double Brakes = 0.2;
    //������������ ��������
    private float MaxSpeed = 0.7f;
    //����������� ����������
    private const float BrakeFactor = 6f;

    //���������� ���������
    private float WarriorSpeed = 1f;
    [SerializeField]
    private Rigidbody2D Warrior;
    
    private Camera cam;

    [SerializeField]
    private Transform WarriorAxis;

    //�������
    Vector2 MovementDirection;
    Vector2 MousePosition;




    //�������� �� ��� X
    private float SpeedOnX() => Warrior.velocity.x;

    //�������� �� ��� Y
    private float SpeedOnY() => Warrior.velocity.y;


    //��������� ��������� ���� ������ ��������

    private void ReverseImpulseX(float Speed)
    {
        Warrior.AddForce(new Vector2(DirectionSignX * (-1), 0) * Speed, ForceMode2D.Impulse);
        if (Mathf.Abs(SpeedOnX()) <= Brakes) Warrior.velocity = new Vector2(0, SpeedOnY());
    }
        

    private void ReverseImpulseY(float Speed)
    {
        Warrior.AddForce(new Vector2(0, DirectionSignY * (-1)) * Speed, ForceMode2D.Impulse);
        if (Mathf.Abs(SpeedOnY()) <= Brakes) Warrior.velocity = new Vector2(SpeedOnX(), 0);
    }

    //��������� ��������� ���� ������ ������ � �������� ����������� (������� ��� ��� ���������)

    private void ReverseImpulseX(float Speed, float BrakeSpeed)
    {
        Warrior.AddForce(new Vector2(DirectionSignX * (-1), 0) * Speed * BrakeSpeed , ForceMode2D.Impulse);
        if (Mathf.Abs(SpeedOnX()) <= Brakes) Warrior.velocity = new Vector2(0, SpeedOnY());
    }

    private void ReverseImpulseY(float Speed, float BrakeSpeed)
    {
        Warrior.AddForce(new Vector2(0, DirectionSignY * (-1)) * Speed * BrakeSpeed, ForceMode2D.Impulse);
        if (Mathf.Abs(SpeedOnY()) <= Brakes) Warrior.velocity = new Vector2(SpeedOnX(), 0);
    }



    private FirePoint firePoints;

    private void Awake()
    {
        // � �� ���� ��� ��� ��������, �� ������� ���, � ������ ������� ��� �����


        firePoints = GetComponent<FirePoint>();

        // Parent Object diference = 21
        StartWarriorDir = WarriorAxis.rotation.z * Mathf.Rad2Deg;

        // Rifle, shotgun diference = 30
        StartRifleDir = firePoints.GetRP.transform.rotation.z * Mathf.Rad2Deg;
        RifleDifference = StartWarriorDir + StartRifleDir;

        Vector3 PointA = firePoints.GetRP.transform.position;
        Vector3 PointB = firePoints.GetPP.transform.position;

        Vector3 DifVectorA = PointB - PointA;
        

        float DifAngle = Vector3.Angle(DifVectorA, firePoints.GetRP.transform.right);

        // Pistol Diference == 27
        StartPistolDir = firePoints.GetPP.transform.rotation.z * Mathf.Rad2Deg;

        PistolDifference = DifAngle - StartPistolDir + (StartRifleDir - StartPistolDir)/2;

        // ������ �� ����� ������������ ��� �������� �� ������ �����
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }



    
    private enum GunsAD {PistolAD, RifleAD, None};

    private GunsAD currentADchecker = GunsAD.None;

    // ���������� �� ������ ����������� ������� �����
    private float StartRifleDir;

    private float StartPistolDir;

    // ���������� �� ������ ����������� ������
    private float RifleDifference;

    private float PistolDifference;

    // ����� ��� ����������� ����������� ���������

    private float ModPistolDir;


    // ���������� �����������

    private float CurrentAngleDifference;

    public float currentAngleDifference
    {
        get { return CurrentAngleDifference; }
    }


    public void SwitchAD(int NumberOfGun)
    {
        switch (NumberOfGun)
        {
            case 1:
                if (currentADchecker != GunsAD.PistolAD)
                { 
                    currentADchecker = GunsAD.PistolAD;
                    CurrentAngleDifference = PistolDifference;
                }
                break;
            case 2:
                if (currentADchecker != GunsAD.RifleAD)
                {
                    currentADchecker = GunsAD.RifleAD;
                    CurrentAngleDifference = RifleDifference;
                }
                break;
            case 3:
                if (currentADchecker != GunsAD.RifleAD)
                {
                    currentADchecker = GunsAD.RifleAD;
                    CurrentAngleDifference = RifleDifference;
                }
                break;
        }
    }





    void Update()
    {
        //����������� ��������
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");
        //������� �������
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        //��������� ��� �������� ����� �� shift ��� control
        if (Input.GetKey(KeyCode.LeftShift)) MaxSpeed = 1;
        else if (Input.GetKey(KeyCode.LeftControl)) MaxSpeed = 0.5f;
        else MaxSpeed = 0.7f;


    }

    private void FixedUpdate()
    {

        //������� ���������
        LookDirection = MousePosition - Warrior.position;

        Warrior.rotation = (Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg + CurrentAngleDifference);

        //��������� (�������) ������ �� ��� � � �������� ������������ ��������
        if ( Mathf.Abs( SpeedOnX() ) < MaxSpeed) Warrior.AddForce(new Vector2(MovementDirection.x, 0) * WarriorSpeed, ForceMode2D.Impulse);
        else Warrior.velocity = new Vector2(MaxSpeed * DirectionSignX, SpeedOnY());

        //��������� (�������) ������ �� ��� Y � �������� ������������ ��������
        if ( Mathf.Abs( SpeedOnY() ) < MaxSpeed) Warrior.AddForce(new Vector2(0, MovementDirection.y) * WarriorSpeed, ForceMode2D.Impulse);
        else Warrior.velocity = new Vector2(SpeedOnX(), MaxSpeed * DirectionSignY);






        /*���� ����������� �� X (Key A, D) ���������� �� ������������ ����������� ��������,
         * �� ���� ���������� �������� ���������, �������, ��� �������, ������ ��������� �������� ��� ���������� �������.
         * ������� ����������� ������, ����������� ��������� �������� �� ����������� ����������
         */
        if (DirectionSignX != MovementDirection.x)
        {
            if ( ( MovementDirection.x != 0 ) & ( DirectionSignX != 0 ) )
            {
                ReverseImpulseX(WarriorSpeed, BrakeFactor);
                DirectionSignX = 0;
            }
        }

        /*���� ����������� �� Y (Key W, S) ���������� �� ������������ ����������� ��������,
         * �� ���� ���������� �������� ���������, �������, ��� �������, ������ ��������� �������� ��� ���������� �������.
         * ������� ����������� ������, ����������� ��������� �������� �� ����������� ����������
         */
        if (DirectionSignY != MovementDirection.y)
        {
            if ( (MovementDirection.y != 0) & (DirectionSignY != 0) )
            {
                ReverseImpulseY(WarriorSpeed, BrakeFactor);
                DirectionSignY = 0;
            }
        }





        //���������� ��������� �� ��� � ���� �� ������ ������
        if (SpeedOnX() > 0) this.DirectionSignX = 1;

        else if (SpeedOnX() < 0) this.DirectionSignX = -1;

        if ((MovementDirection.x == 0) & ( (SpeedOnX() != 0) ))
        {
            ReverseImpulseX(WarriorSpeed);
        }



        //���������� ��������� �� ��� Y ���� �� ������ ������
        if (SpeedOnY() > 0) this.DirectionSignY = 1;

        else if (SpeedOnY() < 0) this.DirectionSignY = -1;

        if ((MovementDirection.y == 0) & ((SpeedOnY() != 0)))
        {
            ReverseImpulseY(WarriorSpeed);
        }

    }
}
