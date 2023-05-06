using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    int score;
    void Start()
    {
        score = 0;
        scoreText.text = "Убито:" + score;
    }

    
}
