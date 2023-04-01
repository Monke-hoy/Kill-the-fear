using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FireDirection : MonoBehaviour
{
    //Для получения актуального угла разницы AngleDifference 
    [SerializeField]
    WarriorMovement wm;
    
    //Игрок
    private GameObject player;
    //Ствол игрока 
    private PlayerGun gun;

    //Направление стрельбы
    private Vector2 fireDirection;

    //Направление стрельбы, значение которого, можно присвоить в других скриптах
    public Vector2 GetFireDir => fireDirection;


    //Получаю огневую точку с необходимым компонентом transform  (currentPoint)
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

        //Угол разницы между стволом и спрайтом (раньше был нужен)
        //float RotateAngle = wm.currentAngleDifference;
        float RotateAngle = 0;

        //Поворот
        Quaternion q = Quaternion.AngleAxis(RotateAngle, Vector3.forward);

        //Направление красной оси
        Vector3 StartDirection = firePointTransform.right;
        

        //Это уже повернутый куда надо fireDir
        fireDirection = q * StartDirection;
    }
}
