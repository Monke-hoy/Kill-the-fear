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

    private InventoryMenu inventoryMenu;

    private GameManagerScript gameManagerScript;

    private Vector3 beforeOpeningPosition;

    private bool DeathWindowIsActive = false;

    private bool pauseWindowIsNotActive = true;

    public bool PauseWindowIsNotActive => pauseWindowIsNotActive;





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
            // Çàìîðàæèâàþ èãðîêà
            gameManagerScript.FreezePlayer();
            CursorManager.Instance.SetMenuCursor();
            pauseMenu.SetActive(true);

            
            // Âûêëþ÷àþ ââîä èíâåíòàðþ
            inventoryMenu.pauseWindowIsActive = true;

            // Ñîõðàíÿåì òåêóùóþ ïîçèöèþ êóðñîðà
            beforeOpeningPosition = Input.mousePosition;

            Time.timeScale = 0f;

        }
    }

    public void Resume()
    {
        pauseWindowIsNotActive = true;

        // Ðàçìîðàæèâàþ èãðîêà
        gameManagerScript.UnfreezePlayer();
        CursorManager.Instance.SetScopeCursor();
        pauseMenu.SetActive(false);

        // Âêëþ÷àþ ââîä èíâåíòàðþ
        inventoryMenu.pauseWindowIsActive = false;


        // Óñòàíàâëèâàþ êóðñîð
        Mouse.current.WarpCursorPosition(beforeOpeningPosition);

        InputState.Change(Mouse.current.position, beforeOpeningPosition);

        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        //Óñòàíàâëèâàþ êóðñîð
        CursorManager.Instance.SetMenuCursor();
        
        //Óíè÷òîæàþ òî ÷òî íå óíè÷òîæàåòñÿ ïðè ïåðåõîäå, â ìåíþ îíî íå íóæíî
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
        SceneManager.LoadScene(sceneID);
    }


    private void Start()
    {
        inventoryMenu = GetComponent<InventoryMenu>();

        gameManagerScript = GetComponent<GameManagerScript>();
    }


    private void Update()
    {
        // Åñëè ïåðñîíàæ óìåð - òîãäà îêíî ïàóçû íåëüçÿ âûçâàòü
        if (DeathWindowIsActive || InventoryWindowIsActive)
            return;


        
        // Âûçîâ ïàóçû íà êëàâèøó escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            


        // Âûçîâ ïàóçû íà êëàâèøó escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Pause();
        }
        


    }
}
