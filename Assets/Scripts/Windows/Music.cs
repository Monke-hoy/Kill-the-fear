using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance;

    public static Music Instance => instance;
      public void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);

    }

    private void Update()
    {
        if (GameObject.FindWithTag("Player")) DestroyMusic();
    }

    public void DestroyMusic()
    {
        Destroy(this.gameObject);
    }
}
