using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    private static CanvasManager instance;

    public static CanvasManager Instance => instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);


    }

    public void DestroyCanvas()
    {
        Destroy(this.gameObject);
    }
}
