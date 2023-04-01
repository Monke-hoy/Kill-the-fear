using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Если этот скрипт висит на объекте, то тогда этот объект будет единственным на сцене
    //А также этот объект не будет уничтожен при переходе на другую сцену

    private static PlayerManager instance;

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
}
