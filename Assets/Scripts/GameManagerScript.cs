using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;

    private GameObject player;

    [SerializeField] private GameObject Face_UI;

    [SerializeField] private GameObject E_image;

    public GameObject GetE_image => E_image;

    private Player playerParams;

    private Shooting playerShooting;

    private PauseMenu pauseMenu;

    private InventoryMenu inventoryMenu;

    private InventoryManager inventoryManager;

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

        // �������� ���� ��� ����������� �� R
        inventoryManager.set_input_block_status = true;

        // �������� ����������� ����������� �� R
        inventoryManager.block_current_reload = true;

        // �������� ������� UI
        Face_UI.SetActive(false);


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
        // ����� ������ ������� �� ����������� ��� �������� �� ������ ����
        DontDestroyOnLoad(E_image);

        pauseMenu = GetComponent<PauseMenu>();

        inventoryMenu = GetComponent<InventoryMenu>();

        // ������� �����, � ������� �� ���������� �����������
        StartSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ������� ������� ����� ������ ������
        SpawnPointPosition = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        // ������� ����� �������� ��������
        transition = GameObject.Find("LevelChanger").GetComponent<CanvasTransition>();

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();


        if (!hasGameStarted)
        {
            Face_UI.SetActive(false);
            hasGameStarted = true;
            Invoke("ActivateFaceUI", 0.3f);
        }
    }




    private void ActivateFaceUI() => Face_UI.SetActive(true);

    private static bool hasGameStarted = false;





    public void Restart()
    {

        // �������� ���� ��� ����������� �� R
        inventoryManager.set_input_block_status = false;

        // �������� ����������� ����������� �� R
        inventoryManager.block_current_reload = false;

        // ������ ��������� ����, ����� �������� ������
        gun.ChangeGun(0);

        //������������ ������
        CursorManager.Instance.SetScopeCursor();

        // ������������ �����
        SceneManager.LoadScene(StartSceneIndex);

        // ������������ ���������
        inventoryManager.ResetInventory();

        // ������ ��� ������ ��������
        EnemyManager.Instance.DestroyAllItemsOnGround();

        // ������ �����
        EnemyManager.Instance.DestroyAllCorpses();

        //������� ����������
        transition.StartDeathTransition();

        gameOverUi.SetActive(false);

        // ��������� ������ �� ��������� �������
        player.transform.position = SpawnPointPosition;

        //��������� HP ������ � ��������� ���������, �� ������ ������ ��� � ������
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.hpUI.SetHealth(playerParams.GetDefaultHP);
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

        // ������� ������� UI
        Face_UI.SetActive(true);

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
        Destroy(E_image);
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }
}
