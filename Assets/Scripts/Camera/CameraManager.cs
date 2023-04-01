using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // MainCamera - ��� ������������ ������ �� �����. ���� ������ ��� ���
    // ��� �� ����� ������������ ��� �������� �� ������ �����

    private static CameraManager instance;

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
