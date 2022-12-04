using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{    

    private Vector2 LookDirection;
    public Vector2 WarriorLookDir => LookDirection;

    //Знак направления
    private sbyte DirectionSignX = 0;
    private sbyte DirectionSignY = 0;

    //Полная остановка при достижении этой скорости, если не растет скорость
    private const double Brakes = 0.2;
    //Максимальная скорость
    private float MaxSpeed = 0.7f;
    //Коэффициент торможения
    private const float BrakeFactor = 6f;

    //Компоненты персонажа
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


    //Векторы
    private Vector2 MovementDirection;
    private Vector2 MousePosition;
    public Vector2 GetMousePos => MousePosition;




    //Скорость по оси X
    private float SpeedOnX() => Warrior.velocity.x;

    //Скорость по оси Y
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
        //Направлкние движения
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");
        //Позиция курсора
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        //Замедлить или ускорить перса на shift или control
        if (Input.GetKey(KeyCode.LeftShift)) MaxSpeed = 1;
        else if (Input.GetKey(KeyCode.LeftControl)) MaxSpeed = 0.5f;
        else MaxSpeed = 0.7f;


    }

    private void FixedUpdate()
    {

        //Поворот персонажа
        LookDirection = MousePosition - Warrior.position;
        
        float Distance = Vector2.Distance(WarriorAxis.position, MousePosition);

        Warrior.rotation = (Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg + gun.angleDifference);
            

        //Ускорение (импульс) игрока по оси Х в пределах максимальной скорости
        if ( Mathf.Abs( SpeedOnX() ) < MaxSpeed) Warrior.AddForce(new Vector2(MovementDirection.x, 0) * WarriorSpeed, ForceMode2D.Impulse);
        else Warrior.velocity = new Vector2(MaxSpeed * DirectionSignX, SpeedOnY());

        //Ускорение (импульс) игрока по оси Y в пределах максимальной скорости
        if ( Mathf.Abs( SpeedOnY() ) < MaxSpeed) Warrior.AddForce(new Vector2(0, MovementDirection.y) * WarriorSpeed, ForceMode2D.Impulse);
        else Warrior.velocity = new Vector2(SpeedOnX(), MaxSpeed * DirectionSignY);






        /*Если направление по X (Key A, D) отличается от фактического направления движения,
         * то перс тормозится обратным импульсом, который, как правило, больше обратного импульса при отпущенных кнопках.
         * Импульс становиться больше, посредством умножения скорости на коэффициент торможения
         */
        if (DirectionSignX != MovementDirection.x)
        {
            if ( ( MovementDirection.x != 0 ) & ( DirectionSignX != 0 ) )
            {
                ReverseImpulseX(WarriorSpeed, BrakeFactor);
                DirectionSignX = 0;
            }
        }

        /*Если направление по Y (Key W, S) отличается от фактического направления движения,
         * то перс тормозится обратным импульсом, который, как правило, больше обратного импульса при отпущенных кнопках.
         * Импульс становиться больше, посредством умножения скорости на коэффициент торможения
         */
        if (DirectionSignY != MovementDirection.y)
        {
            if ( (MovementDirection.y != 0) & (DirectionSignY != 0) )
            {
                ReverseImpulseY(WarriorSpeed, BrakeFactor);
                DirectionSignY = 0;
            }
        }





        //Торможение персонажа по оси Х если не зажаты кнопки
        if (SpeedOnX() > 0) this.DirectionSignX = 1;

        else if (SpeedOnX() < 0) this.DirectionSignX = -1;

        if ((MovementDirection.x == 0) & ( (SpeedOnX() != 0) ))
        {
            ReverseImpulseX(WarriorSpeed);
        }



        //Торможение персонажа по оси Y если не зажаты кнопки
        if (SpeedOnY() > 0) this.DirectionSignY = 1;

        else if (SpeedOnY() < 0) this.DirectionSignY = -1;

        if ((MovementDirection.y == 0) & ((SpeedOnY() != 0)))
        {
            ReverseImpulseY(WarriorSpeed);
        }

    }
}
