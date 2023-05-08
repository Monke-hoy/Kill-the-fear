using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    //������������ ������ �� �����, ������� ����������� � ������� ������ 

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
         * ��������� ��� ������� �� ������ � ��������� 
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


    // ��������� � �������
    public void AddToDeadList(int id, int sceneId)
    {
        SetOfDead.Add((id, sceneId));
    }

    // �������� � ��������� ��������� �� ������
    public void AddToItemsList(int id, int sceneId)
    { 
        SetOfItems.Add((id, sceneId));
    }

    // ������� ������� �� Id
    public void RemoveFromItemList(int id, int scene_id)
    {
        SetOfItems.Remove((id, scene_id)); 
    }







    // ������ ���������� ��������
    private List<Enemy> NotActiveCorpses = new List<Enemy>();




    // ���������� �������� � ��
    public void ToHell()
    {
        int current_scene_index = SceneManager.GetActiveScene().buildIndex;

        foreach (var elem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = elem.GetComponent<Enemy>();

            // ���� ID, � SceneId � ������, � ��� �� ������ �� ��� ���� - ����� Destroy. ���� ��� ���� - �� ���������� ����
            if (SetOfDead.Contains((enemy.GetId, enemy.GetSceneId)) && enemy.IsDead == false)
            {
                Destroy(elem);
            }

            if (enemy.IsDead == true && enemy.GetSceneId != current_scene_index)
            {
                // �������� � ������ ���������� ��������
                NotActiveCorpses.Add(enemy);

                // ����� ���� ����������, ���� �� �� ������ �����
                enemy.transform.parent.gameObject.SetActive(false);
            }
            
        }

        for (int i = NotActiveCorpses.Count - 1; i >= 0; i--)
        {
            Enemy corpse = NotActiveCorpses[i];
            Debug.Log("��������� ���������� ������, ��� SceneId = " + corpse.GetSceneId);

            Debug.Log("������ ������� ����� = " + current_scene_index);

            if (corpse.GetSceneId == current_scene_index)
            {
                Debug.Log("������ ����� = ������� �����, ������ ������ ���� �����������");

                // ������ �� ������ ���������� ��������
                NotActiveCorpses.RemoveAt(i);

                // ��������� ������ � ������� �����
                corpse.transform.parent.gameObject.SetActive(true);
            }
        }
    }






    public void DestroyAllCorpses()
    {
        foreach (var elem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = elem.GetComponent<Enemy>();

            // ���� �������� ������ ��� ����, - �� ����������
            if (enemy.IsDead == true)
            {
                Destroy(elem);
            }

        }



        for (int i = NotActiveCorpses.Count - 1; i >= 0; i--)
        {
            Enemy corpse = NotActiveCorpses[i];

            // ������ �� ������ ���������� ��������
            NotActiveCorpses.RemoveAt(i);

            // ��������� ������ 
            Destroy(corpse);

        }
    }






    // ���������� ��� �������� �� �����, ������� ��� � ������
    // ������ ����������� ���, � ������� ������ ������ �����
    // ������ ��������� ���, � ������� ������ ����� ����� ������� ������� �����

    private int current_scene_index;
    public void KillAllNecessaryItems()
    {

        // ������� ��� ����������� � ������� ����� �������
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







    // ��� ������
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
