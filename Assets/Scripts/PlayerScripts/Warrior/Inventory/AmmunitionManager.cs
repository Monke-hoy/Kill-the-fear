using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    private RectTransform am_UI;



    private void Start()
    {

        am_UI = GameObject.Find("AmmunitionUI").GetComponent<RectTransform>();

        // Получаем все слоты для огнестрельного оружия
        for (int i = 0; i < am_UI.childCount; i++)
        {
            if (am_UI.GetChild(i).GetComponent<AmmunitionGunSlot>() != null)
            {
                am_gun_slots.Add(am_UI.GetChild(i).GetComponent<AmmunitionGunSlot>());
            }
        }

        GameObject.Find("Inventory").SetActive(false);

    }


    private bool SuccessAdd = false;
    public void PutElem(Item item)
    {
        int count = 0;
        foreach (AmmunitionGunSlot slot in am_gun_slots)
        {
            // Проверка на пустотку и на оружие 
            slot.AddItem(item, out SuccessAdd);

            
            count++;

            // Если элемент добавился - завершаем перебор слотов, ставим дефолтное значение
            if (SuccessAdd)
            { 
                SuccessAdd = false;
                
                return;

            }
        }
    }



}
