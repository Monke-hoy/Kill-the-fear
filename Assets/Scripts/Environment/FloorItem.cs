using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorItem : MonoBehaviour
{
    [SerializeField] private Item item;

    private GameObject GrabImage;

    public Item getItem => item;

    [SerializeField] private GameObject itemObject;

    public GameObject GetItemObject => itemObject;


    private bool OnPlayerTarget = false;

    private InventoryManager am;

    private Vector3 offset = new Vector3(0.1f, 0.1f, 0);



    private static HashSet<int> items_id = new HashSet<int>();

    // Для дебага
    public static void ShowMeSetOfId()
    { 
        foreach (int id in items_id) { Debug.Log(id); }
    }




    private bool id_has_been_placed = false;


    private int id = -1;

    public int getId => id;




    private int scene = -1;

    public int GetCurrentSceneIndex => scene;




    private bool in_inventory = false;

    public bool set_in_inventory_status { set { in_inventory = value; } }

    public bool get_in_inventory_status => in_inventory;



    private void Start()
    {
        am = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        GrabImage = GameObject.Find("Main Camera").GetComponent<GameManagerScript>().GetE_image;

    }





    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Сработает всего один раз на создании объекта
        if (!id_has_been_placed)
        {

            // Устанавливаю уникальный ID предмету
            SetId();

            // Устанавливаю индекс текущей сцены предмету
            SetCurrentSceneIndex();

            id_has_been_placed = true;

            DontDestroyOnLoad(transform.gameObject);

        }
    }






    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") { Destroy(transform.gameObject); }
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }







    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            OnPlayerTarget = true;
        }

    }









    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            // Даю кнопке новую позицию рядом с предметом, и делаю её активной
            SetGrabImage();
            GrabImage.SetActive(true);

        }
    }








    private void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.GetComponent<Collider2D>().CompareTag("Player"))
        {
            // Делаю кнопку неактивной
            GrabImage.SetActive(false);

            OnPlayerTarget = false;
        }
    }









    // Подбирает тот объект, на котором активна кнопка (спрайт) E
    private static Item target_item;

    private static GameObject target_object;
    private void SetGrabImage()
    {
        GrabImage.transform.position = transform.position + offset;

        target_item = item;

        target_object = itemObject;

    }










    public void SetId()
    {
        // Генерирую уникальный ID от 1 до 1000
        while (id == -1)
        {
            int new_random_id = Random.Range(0, 1000);

            // Если ID получился действительно уникальный - тогда присваиваем его в поле экземпляра 
            id = items_id.Contains(new_random_id) ? -1 : new_random_id;
        }
        items_id.Add(id);

    }









    public void SetCurrentSceneIndex()
    {
        // Получаю индекс сцены
        while (scene == -1)
        {
            scene = SceneManager.GetActiveScene().buildIndex;
        }
    }








    public void UpdateSceneIndex()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
    }











    private const float AddingCooldown = 0.2f;

    // Разрешает подбирать предметы по истечению определенного времени
    IEnumerator StartAllowAdding()
    {
        yield return new WaitForSeconds(AddingCooldown);
        FloorItem.is_added_now = false;
    }






    public static bool is_added_now = false;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && OnPlayerTarget)
        {
            if (!is_added_now) { is_added_now = true; am.MainInventoryManager(target_item, target_object); StartCoroutine(StartAllowAdding()); }

        }
    }

}
