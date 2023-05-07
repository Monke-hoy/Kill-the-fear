using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    //Единственный объект на карте, который отслеживает и убивает убитых 

    private static EnemyManager instance;

    public static EnemyManager Instance => instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);


    }


    public void Start()
    {

        /*
         * Записываю все объекты на старте в множество 
        */


        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            FloorItem item_data = item.GetComponent<FloorItem>();

            int item_id = item_data.getId;

            int item_scene = item_data.GetCurrentSceneIndex;

            AddToItemsList(item_id, item_scene);
        }

    }


    private HashSet<(int, int)> SetOfDead = new HashSet<(int, int)>();

    private HashSet<(int, int)> SetOfItems = new HashSet<(int, int)>();

    public HashSet<(int, int)> SetOfDeadEdit
    {
        get { return SetOfDead; }
        set { SetOfDead = value; }
    }


    // Добавляем в мертвых
    public void AddToDeadList(int id, int sceneId)
    {
        SetOfDead.Add((id, sceneId));
    }

    // Добавлен в множество предметов на уровне
    public void AddToItemsList(int id, int sceneId)
    { 
        SetOfItems.Add((id, sceneId));
    }

    // Удаляет предмет по Id
    public void RemoveFromItemList(int id, int scene_id)
    {
        SetOfItems.Remove((id, scene_id)); 
    }







    // Список неактивных объектов
    private List<Enemy> NotActiveCorpses = new List<Enemy>();




    // Отправляем прямиком в ад
    public void ToHell()
    {
        int current_scene_index = SceneManager.GetActiveScene().buildIndex;

        foreach (var elem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = elem.GetComponent<Enemy>();

            // Если ID, и SceneId в списке, а так же объект не был убит - тогда Destroy. Если был убит - не уничтожаем труп
            if (SetOfDead.Contains((enemy.GetId, enemy.GetSceneId)) && enemy.IsDead == false)
            {
                Destroy(elem);
            }

            if (enemy.IsDead == true && enemy.GetSceneId != current_scene_index)
            {
                // Добавляю в список неактивных объектов
                NotActiveCorpses.Add(enemy);

                // Делаю труп неактивным, если он из другой сцены
                enemy.transform.parent.gameObject.SetActive(false);
            }
            
        }

        for (int i = NotActiveCorpses.Count - 1; i >= 0; i--)
        {
            Enemy corpse = NotActiveCorpses[i];
            Debug.Log("Обнаружен неактивный объект, его SceneId = " + corpse.GetSceneId);

            Debug.Log("Индекс текущей сцены = " + current_scene_index);

            if (corpse.GetSceneId == current_scene_index)
            {
                Debug.Log("Индекс трупа = индексу сцены, объект должен быть активирован");

                // Удаляю из списка неактивных объектов
                NotActiveCorpses.RemoveAt(i);

                // Активирую объект в текущей сцене
                corpse.transform.parent.gameObject.SetActive(true);
            }
        }
    }






    public void DestroyAllCorpses()
    {
        foreach (var elem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = elem.GetComponent<Enemy>();

            // Если активный объект это труп, - то уничтожаем
            if (enemy.IsDead == true)
            {
                Destroy(elem);
            }

        }



        for (int i = NotActiveCorpses.Count - 1; i >= 0; i--)
        {
            Enemy corpse = NotActiveCorpses[i];

            // Удаляю из списка неактивных объектов
            NotActiveCorpses.RemoveAt(i);

            // Уничтожаю объект 
            Destroy(corpse);

        }
    }






    // Уничтожает все предметы на этаже, которых нет в списке
    // Делает неактивными тех, у которых другой индекс сцены
    // Делает активными тех, у которых индекс сцены равен индексу текущей сцены

    private int current_scene_index;
    public void KillAllNecessaryItems()
    {

        // Включаю все выключенные в прошлой сцене объекты
        foreach (GameObject enabled_item in DisabledItems)
        {
            enabled_item.SetActive(true);
        }


        current_scene_index = SceneManager.GetActiveScene().buildIndex;

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            FloorItem item_data = item.GetComponent<FloorItem>();

            int item_id = item_data.getId;

            int item_scene = item_data.GetCurrentSceneIndex;

            bool item_in_inventory = item_data.get_in_inventory_status;

            if (!item_in_inventory)
            {
                if (!SetOfItems.Contains((item_id, item_scene))) { Destroy(item); }
                else if (item_scene != current_scene_index && item.transform.parent == null) { item.SetActive(false); DisabledItems.Add(item); }

            }
        }

    }


    private List<GameObject> DisabledItems = new List<GameObject>();







    public void DestroyAllItemsOnGround()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            FloorItem item_data = item.GetComponent<FloorItem>();

            int item_id = item_data.getId;

            int item_scene = item_data.GetCurrentSceneIndex;

            bool item_in_inventory = item_data.get_in_inventory_status;

            if (!item_in_inventory)
            {
                RemoveFromItemList(item_id, item_scene);
                Destroy(item);
            }
        }

        foreach (GameObject enabled_item in DisabledItems)
        {

            FloorItem item_data = enabled_item.GetComponent<FloorItem>();

            int item_id = item_data.getId;

            int item_scene = item_data.GetCurrentSceneIndex;

            RemoveFromItemList(item_id, item_scene);
        }


    }







    // Для дебага
    public void ShowMeTheDead()
    {
        foreach (var elem in SetOfDead)
        {
            Debug.Log(elem);
        }
    }

    public void ShowMeItems()
    {
        foreach (var elem in SetOfItems)
        {
            Debug.Log(elem);
        }
    }

    public void DestroyReaper()
    {
        Destroy(this.gameObject);
    }

}
