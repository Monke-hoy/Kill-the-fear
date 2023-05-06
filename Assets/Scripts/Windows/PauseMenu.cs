using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject GetPauseMenu => pauseMenu;

    private GameObject Face_UI;

    private InventoryMenu inventoryMenu;

    private InventoryManager inventoryManager;

    private GameManagerScript gameManagerScript;

    private Vector3 beforeOpeningPosition;

    private bool DeathWindowIsActive = false;

    private bool pauseWindowIsNotActive = true;

    public bool PauseWindowIsNotActive => pauseWindowIsNotActive;

    private bool is_reloading = false;

    public bool set_reload_status { set { is_reloading = value; } } 





    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    private bool InventoryWindowIsActive = false;

    public bool inventoryWindowIsActive
    {
        get { return InventoryWindowIsActive; }
        set { InventoryWindowIsActive = value; }
    }

    public void Pause()
    {
        pauseWindowIsNotActive = !pauseWindowIsNotActive;
        if (pauseWindowIsNotActive)
            Resume();
        else
        {

            /*
             * Открываю окно паузы 
            */


            // Выключаю ввод для перезарядки на R
            inventoryManager.set_input_block_status = true;

            // Выключаю возможность перезарядки на R
            inventoryManager.block_current_reload = true;


            // Замораживаю игрока
            gameManagerScript.FreezePlayer();
            CursorManager.Instance.SetMenuCursor();
            pauseMenu.SetActive(true);

            
            // Выключаю ввод инвентарю
            inventoryMenu.pauseWindowIsActive = true;

            // Сохраняем текущую позицию курсора
            beforeOpeningPosition = Input.mousePosition;

            Time.timeScale = 0f;

            // Выключаю лицевой UI
            Face_UI.SetActive(false);


        }
    }

    public void Resume()
    {
        // Включаю ввод для перезарядки на R
        inventoryManager.set_input_block_status = false;

        // Включаю возможность перезарядки на R
        inventoryManager.block_current_reload = false;

        pauseWindowIsNotActive = true;

        // Размораживаю игрока
        gameManagerScript.UnfreezePlayer();
        CursorManager.Instance.SetScopeCursor();
        pauseMenu.SetActive(false);

        // Включаю ввод инвентарю
        inventoryMenu.pauseWindowIsActive = false;


        // Устанавливаю курсор
        Mouse.current.WarpCursorPosition(beforeOpeningPosition);

        InputState.Change(Mouse.current.position, beforeOpeningPosition);

        Time.timeScale = 1f;

        Face_UI.SetActive(true);
    }








    private void ActivateInventoryInput() => inventoryMenu.pauseWindowIsActive = false;








    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        //Уничтожаю то что не уничтожается при переходе, в меню оно не нужно
        inventoryManager.ResetInventory();
        Destroy(gameManagerScript.GetE_image);
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
        SceneManager.LoadScene(sceneID);
    }




    private void ActivateFaceUI() => Face_UI.SetActive(true);



    private void Start()
    {
        inventoryMenu = GetComponent<InventoryMenu>();

        gameManagerScript = GetComponent<GameManagerScript>();

        inventoryManager = GetComponent<InventoryManager>();

        Face_UI = inventoryManager.getFaceUI;
    }


    private void Update()
    {
        // Если персонаж умер - тогда окно паузы нельзя вызвать
        if (DeathWindowIsActive || InventoryWindowIsActive)
            return;


        // Вызов паузы на клавишу escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        


    }
}
