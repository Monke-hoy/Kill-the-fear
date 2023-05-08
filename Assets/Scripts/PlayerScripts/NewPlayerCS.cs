using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerCS : MonoBehaviour
{
    private enum Sprites { withPistol, withShotGun, withRifle, withHummer , nearWall, none }

    private Sprites CurrentSprite = Sprites.withHummer;

    private Player player;


    //расстояние, на котором скин будет меняться на spriteNearWall
    float DistForSpriteNearWall = 0.15f;
    private RangeFinder rangeFinder;

    //спрайт, используемый до момента приближения к стене
    private Sprites spriteBeforeWall = Sprites.withHummer;


    //спрайт на персе в данный момент
    private SpriteRenderer current_sprite;

    Animator animator;



    private void Awake() 
    {
        rangeFinder = GetComponentInChildren<RangeFinder>();
        current_sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Update()
    {


        //спрайт меняется, когда подходишь к стене, и возвращается, когда отходишь
        if (rangeFinder.GetDistToTarget <= DistForSpriteNearWall && CurrentSprite != Sprites.withHummer) 
        {
            CurrentSprite = Sprites.nearWall;
            animator.Play("PlayerNearWall");
        }
        else 
        {
            CurrentSprite = spriteBeforeWall; 
            switch (CurrentSprite) 
        {
            case Sprites.withPistol:
                animator.Play("PlayerWithPistol");
                break;
            case Sprites.withRifle:
                animator.Play("PlayerWithRiffle");
                break;
            case Sprites.withShotGun:
                animator.Play("PlayerWithShotgun");
                break;
            case Sprites.withHummer:
                if (!hit) animator.Play("PlayerWithHammer");
            break;
        }
            
        }


        //удар кувалдой
        if (CurrentSprite == Sprites.withHummer && Input.GetButtonDown("Fire1") && !hit && !player.playerIsDead)
        {
            hit = true;
            animator.SetBool("hit", true);
            //задержка перед откатом переменной
            Invoke("Hit", 0.233f);
        }

    }

    //переменная, активирующая атаку кувалдой
    bool hit = false;
    //метод, откатывающий переменную атакu
    void  Hit() {
        hit = false;
        animator.SetBool("hit", false);
    }


    public void changeSprite(int NumberOfGun)
    { 
        switch (NumberOfGun) 
        {
            case 1:
                if (CurrentSprite != Sprites.withPistol)
                {
                    CurrentSprite = spriteBeforeWall = Sprites.withPistol;
                    
                    animator.Play("PlayerWithPistol");
                }
                break;
            case 2:
                if (CurrentSprite != Sprites.withRifle)
                {
                    CurrentSprite = spriteBeforeWall = Sprites.withRifle;
                    animator.Play("PlayerWithRiffle");
                }
                break;
            case 3:
                if (CurrentSprite != Sprites.withShotGun)
                {
                    CurrentSprite = spriteBeforeWall = Sprites.withShotGun;
                    animator.Play("PlayerWithShotgun");
                }
                break;
            case 0:
                if (CurrentSprite != Sprites.withHummer)
                {
                    CurrentSprite = spriteBeforeWall = Sprites.withHummer;
                    animator.Play("PlayerWithHammer");
                }
                break;


        }
    }
    
}
