using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class mag : bullets_capacity
{


    private void Start()
    {
        // Получаю емкость магазина
        Mag mag_item = GetComponent<FloorItem>().getItem as Mag;
        capacity = mag_item.GetCapacity;

        // Инициализирую стек
        bullets = new Stack<GameObject>();

        // Заполняю обойму пулями 
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


}
