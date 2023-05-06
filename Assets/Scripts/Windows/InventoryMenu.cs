using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InventoryMenu : MonoBehaviour

{

    [SerializeField] private GameObject inventoryWindow;

    [SerializeField] private GameObject faceUI;

    public GameObject GetFaceUI => faceUI;

    public GameObject GetInventoryWindow => inventoryWindow;

    private GameManagerScript gameManagerScript;

    private InventoryManager inventoryManager;

    private PauseMenu pauseMenu;

    private Vector3 beforeOpeningPosition;

    private bool InventoryWindowIsNotActive = true;

    private bool InputIsBlocked = false;

    private bool is_reloading = false;

    public bool set_reloading_status { set { is_reloading = value; }  }

    public bool inventoryWindowIsNotActive => InventoryWindowIsNotActive;


    private bool DeathWindowIsActive = false;

    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    private bool PauseWindowIsActive = false;

    public bool pauseWindowIsActive
    {
        get { return PauseWindowIsActive; }
        set { PauseWindowIsActive = value; }
    }

    public bool Set_blocking_status { set { InputIsBlocked = value; } }


    private void Start()
    {
        gameManagerScript = GetComponent<GameManagerScript>();

        pauseMenu = GetComponent<PauseMenu>();

        inventoryManager = GetComponent<InventoryManager>();
    }

    public void Inventory()
    {
        
        InventoryWindowIsNotActive = !InventoryWindowIsNotActive;
        if (InventoryWindowIsNotActive)
        {
            InventoryClose();
        }
        else
        {

            /*
             * Открываю инвентарь 
            */

            // Обновляю текст слотов инвентаря
            inventoryManager.UpdateAllSlots();

            // Блокирую перезарядку на R во время открытого инвентаря 
            inventoryManager.set_input_block_status = true;

            // Блокирую процесс перезарядки
            inventoryManager.block_current_reload = true;

            gameManagerScript.FreezePlayer();
            CursorManager.Instance.SetMenuCursor();
            inventoryWindow.SetActive(true);

            // Выключаю ввод паузе
            pauseMenu.inventoryWindowIsActive = true;
            

            // Сохраняем текущую позицию курсора
            beforeOpeningPosition = Input.mousePosition;


            // Убираю лицевой UI
            faceUI.SetActive(false);
        }


    }

    public void InventoryClose()
    {

        // Убираю блокировку перезарядки оружия на R во время открытого инвентаря
        inventoryManager.set_input_block_status = false;

        // Деблокирую процесс перезарядки
        inventoryManager.block_current_reload = false;

        InventoryWindowIsNotActive = true;

        gameManagerScript.UnfreezePlayer();
        CursorManager.Instance.SetScopeCursor();
        inventoryWindow.SetActive(false);

        // Включаю ввод паузе
        Invoke("ActivatePauseInput", 0.2f);

        InputIsBlocked = true;

        Invoke("TurnOnInventoery", 0.3f);

        // Включаю лицевой UI
        faceUI.SetActive(true);

        Mouse.current.WarpCursorPosition(beforeOpeningPosition);

        InputState.Change(Mouse.current.position, beforeOpeningPosition);

    }

    // Включает ввод для паузы
    private void ActivatePauseInput() => pauseMenu.inventoryWindowIsActive = false;

    // Включает ввод инвентарю
    private void TurnOnInventoery() => InputIsBlocked = false;







    private void Update()
    {
        // Если персонаж умер - тогда окно инвентаря нельзя вызвать
        if (DeathWindowIsActive || PauseWindowIsActive || InputIsBlocked || is_reloading)
            return;

        // Вызов инвентаря на клавишу I
        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory();
        }


        if (Input.GetKeyDown(KeyCode.Escape) && !InventoryWindowIsNotActive)
        {
            InventoryClose();
        }

    }
}
