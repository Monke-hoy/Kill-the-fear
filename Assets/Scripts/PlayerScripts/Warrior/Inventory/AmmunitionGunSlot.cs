using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static Gun;

public class AmmunitionGunSlot : MonoBehaviour
{
    private bool IsEmpty = true;

    public bool GetEmptyInfo => IsEmpty;


    public Item gun;

    private GameObject child;

    


    public void AddItem(Item item, out bool SuccessAddition)
    {
        if (IsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {
                gun = item;

                SuccessAddition = true;
                IsEmpty = false;


                Transform gunImageTransform = transform.GetChild(0);



                child = Instantiate(gun.ScriptableGameObject, transform);

                //child = gun.ScriptableGameObject;

                child.transform.SetParent(gunImageTransform);

                gunImageTransform.GetComponent<Image>().enabled = true;

                gunImageTransform.GetComponent<Image>().sprite = gun.GetInventoryIcon;

                /*
                transform.GetChild(0).GetComponent<Image>().sprite = gun.GetInventoryIcon;

                child.GetComponent<Image>().sprite = gun.GetInventoryIcon;
                */
            }
            else
            {
                SuccessAddition = false;
            }
        }
        else
        {
            SuccessAddition = false;
        }
    }
}
