using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSprites : MonoBehaviour
{

    private enum Sprites { withPistol, withShotGun, withRifle, withHummer , none }

    private Sprites CurrentSprite = Sprites.none;


    //расстояние, на котором скин будет меняться на spriteNearWall
    float DistForSpriteNearWall = 0.15f;
    private RangeFinder rangeFinder;


    private void Awake() 
    {
        rangeFinder = GetComponentInChildren<RangeFinder>();
        current_sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {


        //спрайт меняется, когда подходишь к стене, и возвращается, когда отходишь
        if (rangeFinder.GetDistToTarget <= DistForSpriteNearWall && current_sprite.sprite != spriteWithHammer) 
        {
            current_sprite.sprite = spriteNearWall;
        }
        else {current_sprite.sprite = spriteBeforeWall; }

    }

    //все спрайты
    [SerializeField]
    private Sprite spriteWithPistol;
    [SerializeField]
    private Sprite spriteWithShotGun;
    [SerializeField]
    private Sprite spriteWithRiffle;
    [SerializeField]
    private Sprite spriteNearWall;
    [SerializeField]
    private Sprite spriteWithHammer;

    //спрайт, используемый до момента приближения к стене
    private Sprite spriteBeforeWall = null;


    //спрайт на персе в данный момент
    private SpriteRenderer current_sprite;

    public void changeSprite(int NumberOfGun)
    { 
        switch (NumberOfGun) 
        {
            case 1:
                if (CurrentSprite != Sprites.withPistol)
                {
                    current_sprite.sprite = spriteBeforeWall = spriteWithPistol;
                }
                break;
            case 2:
                if (CurrentSprite != Sprites.withRifle)
                {
                    current_sprite.sprite = spriteBeforeWall = spriteWithRiffle;
                }
                break;
            case 3:
                if (CurrentSprite != Sprites.withShotGun)
                {
                    current_sprite.sprite = spriteBeforeWall = spriteWithShotGun;
                }
                break;
            case 0:
                if (CurrentSprite != Sprites.withHummer)
                {
                    current_sprite.sprite = spriteBeforeWall = spriteWithHammer;
                }
                break;


        }
    }
    


}
