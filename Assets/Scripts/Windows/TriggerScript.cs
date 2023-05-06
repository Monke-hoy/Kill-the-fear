using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    // Имя сцены на которую будем переходить
    [SerializeField] private string sceneName;

    private InventoryManager inventoryManager;

    private InventoryMenu inventoryMenu;

    public string GetSceneName => sceneName;

    // Запоминет позицию вхождения в триггер-коллайдер 
    private Vector3 playerPosition;

    private CanvasTransition canvasTransition;

    private GameObject faceUI;

    private const float switch_on_time = 1.2f;

    private static bool hasGameStarted = false;


    private void Start()
    {
        canvasTransition = GameObject.Find("LevelChanger").GetComponent<CanvasTransition>();

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        inventoryMenu = GameObject.Find("Main Camera").GetComponent<InventoryMenu>();

        faceUI = inventoryManager.getFaceUI;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inventoryMenu.Set_blocking_status = true;

            faceUI.SetActive(false);

            canvasTransition.SceneName = this.sceneName;

            playerPosition = collision.transform.position;

            canvasTransition.StartTransitionFadein();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = playerPosition;

        }
    }

    private void GetFaceUI() => faceUI = GameObject.Find("FaceUI");


    
}
