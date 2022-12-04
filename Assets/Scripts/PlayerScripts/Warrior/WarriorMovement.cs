using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{    

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
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform GunAxis;
    [SerializeField]
    private Gun gun;
    [SerializeField]
    private Transform WarriorAxis;


    //�������
    private Vector2 MovementDirection;
    private Vector2 MousePosition;
    public Vector2 GetMousePos => MousePosition;




    //�������� �� ��� X
    private float SpeedOnX() => Warrior.velocity.x;

    //�������� �� ��� Y
    private float SpeedOnY() => Warrior.velocity.y;

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
        
        float Distance = Vector2.Distance(WarriorAxis.position, MousePosition);

        Warrior.rotation = (Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg + gun.angleDifference);
            

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
