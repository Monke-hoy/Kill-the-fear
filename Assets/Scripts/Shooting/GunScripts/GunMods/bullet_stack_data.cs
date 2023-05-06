using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_stack_data : MonoBehaviour
{

    // Емкость стека с патронами
    private int capacity;

    // Текущее количество патрон в стеке
    private int current_bullet_count;

    // Стек патронов
    private Stack<GameObject> bullets;

    // Префаб пули 
    [SerializeField] private GameObject bulletPrefab;




    public int get_current_bullet_count => current_bullet_count;

    public int get_capacity => capacity;



    private void Start()
    {
        // Получаю емкость стека с патронами 
        BulletStack bullets_stack_item = GetComponent<FloorItem>().getItem as BulletStack;
        capacity = bullets_stack_item.GetStackCapacity;

        // Инициализирую стек
        bullets = new Stack<GameObject>();

        // Заполняю стек пулями 
        if (bulletPrefab != null)
        {
            for (int i = 0; i < capacity; i++)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, transform);
                bulletInstance.SetActive(false);
                bullets.Push(bulletInstance);

            }
        }


        // Инициализирую текущее количество патрон
        current_bullet_count = bullets.Count;
    }


    public GameObject TakeBullet()
    {
        if (current_bullet_count > 0)
        {
            GameObject takenBullet = bullets.Pop();

            current_bullet_count--;

            return takenBullet;
        }
        else
        {

            return null;
        }
    }


    public void LoadBullet(GameObject bullet, out GameObject load_result)
    {
        // Если добавление пройдет не успешно - на выход возвращаем передаваемую пулю
        load_result = bullet;

        if (current_bullet_count < capacity)
        {
            bullets.Push(bullet);

            current_bullet_count++;

            // Если добавление прошло успешно, то подаем на выход null
            load_result = null;
        }
    }




}
