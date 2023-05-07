using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Id;

    public int GetId => Id;

    [SerializeField] private int SceneId;

    public int GetSceneId => SceneId;

    private int health = 100;
    public int enemyHealth { get { return health; } set { health = value; } }

    public Sprite DeadEnemySprite;
    public bool IsDead = false;










    private void ActivateVisibility() => GetComponent<Visibility>().enabled = true;









    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GetComponent<Visibility>().enabled = false;

        // ¬ключаю агр врагу через 200мс, чтобы между переходами на другие кровни он не стрел€л по игроку
        Invoke("ActivateVisibility", 0.2f);
    }






    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") { Destroy(transform.gameObject); }
    }








    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }









    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }











    void Die()
    {
        EnemyManager.Instance.AddToDeadList(Id, SceneId);
        GetComponent<Visibility>().spriteRenderer.sprite = DeadEnemySprite;
        IsDead = true;
        GetComponent<CircleCollider2D>().enabled = false;
        this.transform.Rotate(0, 0, 60f);

        // ƒелаю объект не уничтожаемым при загрузке, чтобы трупы не исчезали при переходе
        DontDestroyOnLoad(transform.parent.gameObject);
    }
}
