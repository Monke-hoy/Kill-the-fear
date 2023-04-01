using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    // ��� ����� �� ������� ����� ����������
    [SerializeField] private string sceneName;

    public string GetSceneName => sceneName;

    // ��������� ������� ��������� � �������-��������� 
    private Vector3 playerPosition;

    private CanvasTransition canvasTransition;

    private void Start()
    {
        canvasTransition = GameObject.Find("LevelChanger").GetComponent<CanvasTransition>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = collision.transform.position;

            canvasTransition.StartTransitionFadein();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = playerPosition;

        }
    }
}
