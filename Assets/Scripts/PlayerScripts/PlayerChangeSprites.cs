using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSprites : MonoBehaviour
{

    //спрайт на персе в данный момент
    private SpriteRenderer current_sprite;
//
    //все спрайты
    [SerializeField]
    private Sprite spriteWithPistol;
    [SerializeField]
    private Sprite spriteWithShotGun;
    [SerializeField]
    private Sprite spriteWithRiffle;
    [SerializeField]
    private Sprite spriteNearWall;

    //спрайт, используемый до момента приближения к стене
    private Sprite spriteBeforeWall;
    //расстояние, на котором скин будет меняться на spriteNearWall
    float DistForSpriteNearWall = 0.15f;
    private RangeFinder rangeFinder;



    private void Start() 
    {

        current_sprite = GetComponent<SpriteRenderer>();
        current_sprite.sprite = spriteBeforeWall = spriteWithPistol;
        rangeFinder = GetComponentInChildren<RangeFinder>();
        
    }

    private void Update()
    {
        //спрайт меняется, когда подходишь к стене, и возвращается, когда отходишь
        if (rangeFinder.GetDistToTarget <= DistForSpriteNearWall) 
        {
            current_sprite.sprite = spriteNearWall;
        }
        else {current_sprite.sprite = spriteBeforeWall; }


        //смена спрайта от нажатия кнопки(смены оружия)

        if (Input.GetKey("1") && current_sprite.sprite != spriteWithPistol) 
        { 
            current_sprite.sprite = spriteBeforeWall = spriteWithPistol;


        }

        if (Input.GetKey("2") && current_sprite.sprite != spriteWithRiffle) 
        { 
            current_sprite.sprite = spriteBeforeWall = spriteWithRiffle;

        }

        if (Input.GetKey("3") && current_sprite.sprite != spriteWithShotGun)
        {
            current_sprite.sprite = spriteBeforeWall = spriteWithShotGun;

        }

    }
    


}
