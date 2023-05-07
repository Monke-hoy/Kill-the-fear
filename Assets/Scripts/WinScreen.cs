using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinScreen : MonoBehaviour
{

    public GameObject winScreen;

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

    
    public void FreezePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();

        pauseMenu = GetComponent<PauseMenu>();
        inventoryMenu = GetComponent<InventoryMenu>();

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

    public void Restart()
    {

        //Устанавливаю курсор
        CursorManager.Instance.SetScopeCursor();

        // Перезагружаю сцену
        SceneManager.LoadScene(StartSceneIndex);

        //Включаю затемнение
        transition.StartDeathTransition();

        winScreen.SetActive(false);

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

        // Отчищаю список убитых у жнеца
        enemyReaper.SetOfDeadEdit.Clear();




    }

    public void NextLevel(int sceneID)
    {
        winScreen.SetActive(false);
        SceneManager.LoadScene(sceneID);
    }
    
    public void Home(int sceneID)
    {
        winScreen.SetActive(false);
        SceneManager.LoadScene(sceneID);

        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        //Уничтожаю то что не уничтожается при переходе, в меню оно не нужно
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }

    //Вызов окна победы
    public void win()
    {
        if (false)
        {
            winScreen.SetActive(true);
        }
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
}
