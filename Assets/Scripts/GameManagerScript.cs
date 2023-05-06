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
        // Если игрок умер - закрываю окно паузы 
        if (!pauseMenu.PauseWindowIsNotActive)
        {
            pauseMenu.Resume();
        }


        // Если игрок умер - закрываю окно инвентаря
        if (!inventoryMenu.inventoryWindowIsNotActive)
        {
            inventoryMenu.InventoryClose();
        }

        // Выключаю ввод для перезарядки на R
        inventoryManager.set_input_block_status = true;

        // Выключаю возможность перезарядки на R
        inventoryManager.block_current_reload = true;

        // Выключаю лицевой UI
        Face_UI.SetActive(false);


        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        gameOverUi.SetActive(true);

        // Замораживаю игрока
        FreezePlayer();

        // Выключаю стрельбу всем террористам
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = false;
            }
        }

        // Отключаю ввод для паузы
        pauseMenu.deathWindowIsActive = true;

        // Отключаю ввод для инвентаря
        inventoryMenu.deathWindowIsActive = true;

        

    }


    public void FreezePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();

        enemyReaper = GameObject.Find("EnemyReaper").GetComponent<EnemyManager>();
        gun = player.GetComponent<PlayerGun>();

        // Выключаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = false;

        // Выключаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Если во время окна смерти (или чего другого) курок в зажатом положении - я его отключаю
        gun.TriggerIsPulled = false;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы игнорировать ввод у персонажа
        playerShooting.enabled = false;
    }

    public void UnfreezePlayer()
    {
        // Включаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = true;

        // Включаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы больше не игнорировать ввод у персонажа
        playerShooting.enabled = true;
    }


    private void Start()
    {
        // Делаю кнопку подбора не разрушаемой при переходе на другой этаж
        DontDestroyOnLoad(E_image);

        pauseMenu = GetComponent<PauseMenu>();

        inventoryMenu = GetComponent<InventoryMenu>();

        // Получаю сцену, с которой мы изначально загрузились
        StartSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Получаю позицию точки спавна игрока
        SpawnPointPosition = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        // Получаю скипт анимаций перехода
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

        // Выключаю ввод для перезарядки на R
        inventoryManager.set_input_block_status = false;

        // Выключаю возможность перезарядки на R
        inventoryManager.block_current_reload = false;

        // Ставлю дефолтный слот, чтобы обновить данные
        gun.ChangeGun(0);

        //Устанавливаю курсор
        CursorManager.Instance.SetScopeCursor();

        // Перезагружаю сцену
        SceneManager.LoadScene(StartSceneIndex);

        // Перезагружаю инвентарь
        inventoryManager.ResetInventory();

        // Убираю все лишние предметы
        EnemyManager.Instance.DestroyAllItemsOnGround();

        //Включаю затемнение
        transition.StartDeathTransition();

        gameOverUi.SetActive(false);

        // Возвращаю игрока на стартовую позицию
        player.transform.position = SpawnPointPosition;

        //Возвращаю HP игрока в дефолтное состояния, на данный момент оно в минусе
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        UnfreezePlayer();

        // Включаю стрельбу всем террористам
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = true;
            }
        }

        // Разрешаю ввод для паузы
        pauseMenu.deathWindowIsActive = false;

        // Разрешаю ввод для инвентаря
        inventoryMenu.deathWindowIsActive = false;

        // Включаю лицевой UI
        Face_UI.SetActive(true);

        // Отчищаю список убитых у жнеца
        enemyReaper.SetOfDeadEdit.Clear();

    }






    public void Home(int sceneID)
    {
        gameOverUi.SetActive(false);
        SceneManager.LoadScene(sceneID);

        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        //Уничтожаю то что не уничтожается при переходе, в меню оно не нужно
        Destroy(E_image);
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }
}
