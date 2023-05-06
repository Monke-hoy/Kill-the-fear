using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTransition : MonoBehaviour
{

    private Animator animator;

    private InventoryMenu inventoryMenu;

    private InventoryManager inventoryManager;

    private string sceneName;
    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; }
    }

    private void Start()
    {
        animator = GameObject.Find("LevelChanger").GetComponent<Animator>();

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        inventoryMenu = GameObject.Find("Main Camera").GetComponent<InventoryMenu>();
    }

    public void StartTransitionFadein()
    {
        animator.Play("Fadein");
    }

    public void StartTransitionFade()
    {
        animator.Play("Fade");
    }

    public void StartTransitionFadeOut()
    {
        animator.Play("FadeOut");
    }

    public void StartDeathTransition()
    {
        animator.Play("DeathFade", -1 ,0f);
    }










    //После завершения эффекта затемнения (на 60-м кадре анимации вызывается этот метод)
    public void OnFadeComplite()
    {
        inventoryManager.LoadInventoryOnFade();

        Invoke("TurnOnInventoryInput", 0.2f);
        
        StartTransitionFade();

        StartCoroutine(LoadSceneAsync());
    }


    private void TurnOnInventoryInput() => inventoryMenu.Set_blocking_status = false;










    

    // Загружаем сцену асинхронкой, метод ToHell гарантированно будет вызван в новой сцене
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        EnemyManager.Instance.ToHell();

        EnemyManager.Instance.KillAllNecessaryItems();
    }
}
