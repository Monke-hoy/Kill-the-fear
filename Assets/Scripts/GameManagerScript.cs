using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;

    private GameObject player;

    private Player playerParams;

    private Shooting playerShooting;

    private PauseMenu pauseMenu;

    private InventoryMenu inventoryMenu;

    private int StartSceneIndex;

    private Vector3 SpawnPointPosition;

    private EnemyManager enemyReaper;

    private CanvasTransition transition;

    private Gun gun;


    public void gameOver()
    {
        // ���� ����� ���� - �������� ���� ����� 
        if (!pauseMenu.PauseWindowIsNotActive)
        {
            pauseMenu.Resume();
        }


        // ���� ����� ���� - �������� ���� ���������
        if (!inventoryMenu.inventoryWindowIsNotActive)
        {
            inventoryMenu.InventoryClose();
        }


        //������������ ������
        CursorManager.Instance.SetMenuCursor();

        gameOverUi.SetActive(true);

        // ����������� ������
        FreezePlayer();

        // �������� �������� ���� �����������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = false;
            }
        }

        // �������� ���� ��� �����
        pauseMenu.deathWindowIsActive = true;

        // �������� ���� ��� ���������
        inventoryMenu.deathWindowIsActive = true;

        

    }


    public void FreezePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();

        pauseMenu = GetComponent<PauseMenu>();
        inventoryMenu = GetComponent<InventoryMenu>();

        enemyReaper = GameObject.Find("EnemyReaper").GetComponent<EnemyManager>();
        gun = player.GetComponent<PlayerGun>();

        // �������� ������������, �������
        player.GetComponent<WarriorMovement>().enabled = false;

        // �������� ������
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // ���� �� ����� ���� ������ (��� ���� �������) ����� � ������� ��������� - � ��� ��������
        gun.TriggerIsPulled = false;

        // ������� ��������� ���� � ������ ������ Shooting, ����� ������������ ���� � ���������
        playerShooting.enabled = false;
    }

    public void UnfreezePlayer()
    {
        // ������� ������������, �������
        player.GetComponent<WarriorMovement>().enabled = true;

        // ������� ������
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // ������� ��������� ���� � ������ ������ Shooting, ����� ������ �� ������������ ���� � ���������
        playerShooting.enabled = true;
    }


    private void Start()
    {
        // ������� �����, � ������� �� ���������� �����������
        StartSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ������� ������� ����� ������ ������
        SpawnPointPosition = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        // ������� ����� �������� ��������
        transition = GameObject.Find("LevelChanger").GetComponent<CanvasTransition>();
    }



    public void Restart()
    {

        //������������ ������
        CursorManager.Instance.SetScopeCursor();

        // ������������ �����
        SceneManager.LoadScene(StartSceneIndex);

        //������� ����������
        transition.StartDeathTransition();

        gameOverUi.SetActive(false);

        // ��������� ������ �� ��������� �������
        player.transform.position = SpawnPointPosition;

        //��������� HP ������ � ��������� ���������, �� ������ ������ ��� � ������
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        UnfreezePlayer();

        // ������� �������� ���� �����������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = true;
            }
        }

        // �������� ���� ��� �����
        pauseMenu.deathWindowIsActive = false;

        // �������� ���� ��� ���������
        inventoryMenu.deathWindowIsActive = false;

        // ������� ������ ������ � �����
        enemyReaper.SetOfDeadEdit.Clear();

        
        

    }



    public void Home(int sceneID)
    {
        gameOverUi.SetActive(false);
        SceneManager.LoadScene(sceneID);

        //������������ ������
        CursorManager.Instance.SetMenuCursor();

        //��������� �� ��� �� ������������ ��� ��������, � ���� ��� �� �����
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }
}
