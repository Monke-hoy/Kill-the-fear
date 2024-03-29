using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���� ���� ������ ����� �� �������, �� ����� ���� ������ ����� ������������ �� �����
    //� ����� ���� ������ �� ����� ��������� ��� �������� �� ������ �����

    private static PlayerManager instance;

    public static PlayerManager Instance => instance;

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

    public void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }
}
