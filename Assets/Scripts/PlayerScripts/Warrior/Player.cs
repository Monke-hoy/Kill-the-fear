using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHpUI hpUI;

    private GameManagerScript gameManager;


    private const int DefaultHP = 100;

    public int GetDefaultHP => DefaultHP;



    private int health = 100;
    public int playerHealth { get { return health; } set { health = value; } }



    private bool isDead;

    public bool playerIsDead
    { 
        get { return isDead; }
        set { isDead = value; }
    }

    private void Start()
    {
        gameManager = GameObject.Find("Main Camera").GetComponent<GameManagerScript>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health >= 0) { hpUI.SetHealth(health); }
        else { hpUI.SetHealth(0); }
        if (health <= 0 && !isDead) 
        {
            isDead = true;
            gameManager.gameOver();
        }
    }

}