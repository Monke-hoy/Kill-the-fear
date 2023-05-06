using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun_capacity : bullets_capacity
{

    private const int shotgun_stack_capacity = 8;



    private void Start()
    {
        // ќпредел€ю емкость дробовика
        capacity = shotgun_stack_capacity;

        // »нициализирую стек
        bullets = new Stack<GameObject>();

        // «аполн€ю дробовик пул€ми 
        if (bulletPrefab != null)
        {
            for (int i = 0; i < capacity; i++)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, transform);
                bulletInstance.SetActive(false);
                bullets.Push(bulletInstance);

            }


        }


        // »нициализирую текущее количество патрон
        current_bullet_count = bullets.Count;
    }


}
