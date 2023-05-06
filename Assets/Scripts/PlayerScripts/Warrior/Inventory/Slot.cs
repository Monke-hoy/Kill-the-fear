using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*
     *  Слот пустой ???
    */


    protected bool IsEmpty = true;

    protected bool IsSetted = false;

    private GameObject face_UI;

    private InventoryMenu inventoryMenu;

    public bool SlotIsEmpty
    {
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }



    /*
     * Предмет в слоте  
    */


    protected Item item;

    public Item item_in_slot
    {
        get { return item; }
        set { item = value; }
    }



    /*
     * Объект в слоте 
    */



    public GameObject internal_object;

    public GameObject object_in_slot
    {
        get { return internal_object; }
        set { internal_object = value; }
    }




    /*
     * Дефолтная позиция слота
    */


    private Vector3 defaultPosition;

    public Vector3 SlotDefaultPosition => defaultPosition;

    private Face_UI_manager face_UI_manager;

    private TextMeshProUGUI text_in_slot;

    private InventoryManager inventoryManager;


    public TextMeshProUGUI get_text_in_slot => text_in_slot;



    /*
     * 
     * Методы 
     * слота  
     * 
    */






    public virtual void SetItem(Item item, GameObject itemObj)
    {
        // Обновляю текст слота 
        text_in_slot.text = inventoryManager.GetAmmoData(item, itemObj);

        this.item = item;

        internal_object = itemObj;

        DontDestroyOnLoad(itemObj);

        IsEmpty = false;

        face_UI_manager.UpdatePic();

    }








    public void ClearClot()
    {
        text_in_slot.text = "";

        item = null;

        internal_object = null;

        IsEmpty = true;

        face_UI_manager.UpdatePic();
    }







    public void UpdateSlotTextData() 
    {
        if (item != null && object_in_slot != null)
            text_in_slot.text = inventoryManager.GetAmmoData(item, object_in_slot);
        else
            text_in_slot.text = "";
    }







    private void Start()
    {

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        if (transform.gameObject.tag == "ItemSlot")
        {
            Items_UI_starter items_UI_Starter = transform.parent.gameObject.GetComponent<Items_UI_starter>();

            if (items_UI_Starter != null)
            {
                // Получаем дефолтную позицию объекта из Dictionary
                defaultPosition = items_UI_Starter.GetDefaultPosition(transform.gameObject.name);

            }
        }
        else if (transform.gameObject.tag == "GunSlot")
        {
            defaultPosition = transform.GetChild(1).position;
        }
        else if (transform.gameObject.tag == "EdgedWeaponSlot")
        {
            defaultPosition = transform.GetChild(1).position;
        }


    }



    private void Awake()
    {
        text_in_slot = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        inventoryMenu = GameObject.Find("Main Camera").GetComponent<InventoryMenu>();

        face_UI = inventoryMenu.GetFaceUI;

        face_UI_manager = face_UI.GetComponent<Face_UI_manager>();
    }



}
