using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InventoryMenu : MonoBehaviour

{

    [SerializeField] private GameObject inventoryWindow;

    public GameObject GetInventoryWindow => inventoryWindow;

    private GameManagerScript gameManagerScript;

    private PauseMenu pauseMenu;

    private Vector3 beforeOpeningPosition;

    private bool InventoryWindowIsNotActive = true;

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


    private void Start()
    {
        gameManagerScript = GetComponent<GameManagerScript>();

        pauseMenu = GetComponent<PauseMenu>();
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
            gameManagerScript.FreezePlayer();
            CursorManager.Instance.SetMenuCursor();
            inventoryWindow.SetActive(true);

            // �������� ���� �����
            pauseMenu.inventoryWindowIsActive = true;
            

            // ��������� ������� ������� �������
            beforeOpeningPosition = Input.mousePosition;
        }
    }

    public void InventoryClose()
    {
        InventoryWindowIsNotActive = true;


        gameManagerScript.UnfreezePlayer();
        CursorManager.Instance.SetScopeCursor();
        inventoryWindow.SetActive(false);

        // ������� ���� �����
        Invoke("ActivatePauseInput", 0.2f);

        Mouse.current.WarpCursorPosition(beforeOpeningPosition);

        InputState.Change(Mouse.current.position, beforeOpeningPosition);

    }

    // �������� ���� ��� �����
    private void ActivatePauseInput() => pauseMenu.inventoryWindowIsActive = false;

    private void Update()
    {
        // ���� �������� ���� - ����� ���� ��������� ������ �������
        if (DeathWindowIsActive || PauseWindowIsActive)
            return;

        // ����� ��������� �� ������� I
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
