using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Face_UI_manager : MonoBehaviour
{


    // Массив оружейных слотов инвентаря 
    private AmmunitionGunSlot[] gun_slots = new AmmunitionGunSlot[3];

    private SecondArmSlot[] second_arm_slots = new SecondArmSlot[2]; 

    // Массив картинок в GunBar
    private GameObject[] gunBarPics = new GameObject[4];


    private void Awake()
    {
        // Получаю оружейные слоты
        gun_slots[0] = GameObject.Find("GunSlot(2)").GetComponent<AmmunitionGunSlot>();
        gun_slots[1] = GameObject.Find("GunSlot(3)").GetComponent<AmmunitionGunSlot>();
        gun_slots[2] = GameObject.Find("GunSlot(1)").GetComponent<AmmunitionGunSlot>();

        second_arm_slots[0] = GameObject.Find("SecondArmSlot(1)").GetComponent<SecondArmSlot>();



        // Получаю картинки в GunBar
        GameObject gunBar = GameObject.Find("GunBar");

        for (int i = 0; i < 3; i++)
        {
            gunBarPics[i] = gunBar.transform.GetChild(i).gameObject;
        }

        gunBarPics[3] = gunBar.transform.GetChild(3).gameObject;

    }

    public void UpdatePic()
    {


        /*
         * Обновляю слоты под огнестрельное оружие 
        */



        for (int i = 0; i < 3; i++)
        {
            GameObject slotBar = gunBarPics[i];

            Image slotBar_image = slotBar.transform.GetChild(1).gameObject.GetComponent<Image>();

            GameObject gun = gun_slots[i].object_in_slot;


            if (gun == null)
            {
                slotBar_image.sprite = null;
                slotBar_image.enabled = false;
            }
            else
            { 
                slotBar_image.sprite = gun.GetComponent<FloorItem>().getItem.GetFloorIcon;
                slotBar_image.enabled = true;
            }

        }



        /*
         * Обновляю слот под холодное оружие 
        */

        

        GameObject edged_weapon = second_arm_slots[0].object_in_slot;

        Image second_arm_image = gunBarPics[3].transform.GetChild(1).gameObject.GetComponent<Image>();

        if (edged_weapon == null)
        {
            second_arm_image.sprite = null;
            second_arm_image.enabled = false;
        }
        else 
        {
            second_arm_image.sprite = edged_weapon.GetComponent<FloorItem>().getItem.GetFloorIcon; ;
            second_arm_image.enabled = true;
        }
        

    }
        

}
