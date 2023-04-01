using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTransition : MonoBehaviour
{
    
    private Animator animator;

    private TriggerScript triggerScript;

    private string SceneName;

    private void Start()
    {
        animator = GameObject.Find("LevelChanger").GetComponent<Animator>();
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

    //После завершения эффекта затемнения (на 60-м кадре анимации вызывается этот метод)
    public void OnFadeComplite()
    {
        triggerScript = GameObject.FindGameObjectWithTag("ChangeSceneTrigger").GetComponent<TriggerScript>();
        SceneName = triggerScript.GetSceneName;

        StartTransitionFade();

        StartCoroutine(LoadSceneAsync());
    }

    // Загружаем сцену асинхронкой, метод ToHell гарантированно будет вызван в новой сцене
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        EnemyManager.instance.ToHell();
    }
}
