using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeFinder : MonoBehaviour
{
    

    public RaycastHit2D GetHit => HitLookDir;
    
    //Лазерный дальномер
    private RaycastHit2D HitLookDir;

    
    [SerializeField]
    private FireDirection fireDirection;


    //Дистанция до цели. По дефолту = 1, иначе выстрела не будет
    private float DistToTarget = 1f;

    public float GetDistToTarget => DistToTarget;



    //Максимальная дистанция на которой работает луч
    private const byte OneHundredMeters = 100;


    //Получаю огневую точку с необходимым компонентом transform  (currentPoint)
    private FirePoint firePoints;

    private Transform firePointTransform;


    private void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePointTransform = firePoints.GetCurrentTransform;
    }



    void FixedUpdate()
    {
        // Обновляю актуальную огневую точку 
        firePoints.UpdateCurrentPoint(ref firePointTransform);

        //Луч до цели который нужен для определения дистанции до цели. Столкновение луча с объектом 
        HitLookDir = Physics2D.Raycast(firePointTransform.position, new Vector2(fireDirection.GetFireDir.x, fireDirection.GetFireDir.y), OneHundredMeters, LayerMask.GetMask("Environment"));
        
        //Дистанция до цели 
        DistToTarget = Vector2.Distance(new Vector2(firePointTransform.position.x, firePointTransform.position.y), HitLookDir.point);
      
    }
}
