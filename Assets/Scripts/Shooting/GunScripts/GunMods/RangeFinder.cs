using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeFinder : MonoBehaviour
{
    
    private EnemyMovement wm = new EnemyMovement();

    public RaycastHit2D GetHit => HitLookDir;
    
    //Лазерный дальномер
    private RaycastHit2D HitLookDir;

    private RaycastHit2D Hit;

    //Стрелок от которого от которого будет измеряться дистанция
    [SerializeField]
    private Transform ShooterAxis;
    [SerializeField]
    private FireDirection fireDirection;


    //Дистанция до цели. По дефолту = 1, иначе выстрела не будет
    private float DistToTarget = 1f;

    public float GetDistToTarget => DistToTarget;



    //Максимальная дистанция на которой работает луч
    private const byte OneHundredMeters = 100;




    void Update()
    {
        //Луч до цели который нужен для определения дистанции до цели. Столкновение луча с объектом 
        HitLookDir = Physics2D.Raycast(ShooterAxis.position, new Vector2(fireDirection.GetFireDir.x, fireDirection.GetFireDir.y), OneHundredMeters, LayerMask.GetMask("Player", "Creatures"));
        //Дистанция до цели 
        DistToTarget = Vector2.Distance(new Vector2(ShooterAxis.position.x, ShooterAxis.position.y), HitLookDir.point);
      
    }
}
