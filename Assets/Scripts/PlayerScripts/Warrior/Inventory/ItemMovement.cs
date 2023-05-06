using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Gun;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // смещение между позицией картинки и позицией мыши

    private UnityEngine.Transform parentTransform;

    private GameObject current_slot;

    private bool in_inventory_range = true;

    private bool is_dragging = false;

    private PointerEventData ItemEventData;

    private RectTransform inventoryTransform;

    private GameObject Camera_main;

    private RectTransform inventoryRootTransform;

    private RectTransform canvasTransform;
    
    private Vector2 item_hotspot;

    private Rigidbody2D image_rb;

    private InventoryMenu inventoryMenu;

    private Slot transmitted_slot;



    private void Start()
    {

        inventoryMenu = GameObject.Find("Main Camera").GetComponent<InventoryMenu>();

        // Получаю объект камеры на старте
        Camera_main = GameObject.Find("Main Camera");

        // Получаю RectTransform корня инвентаря
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();

        //Получаю RectTransform инвентаря  
        inventoryTransform = GameObject.Find("Inventory").GetComponent<RectTransform>();

        // Получаю Transform канваса
        canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();




        /*
         * Получаю hotspot перетаскиваемой картинки (Из-за локальных координат ItemsUI, ammunitionUI)
        */

        item_hotspot = new Vector2 (inventoryTransform.position.x - inventoryRootTransform.position.x, inventoryTransform.position.y - inventoryRootTransform.position.y);

        image_rb = GetComponent<Rigidbody2D>();

    }








    private string text_on_begin_drag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Получаю слот предмета, который перетаскиваем
        transmitted_slot = transform.parent.GetComponent<Slot>();

        // Запоминаю текст, чтобы потом его включить, на случай если предмет не будет перемещен в другой слот
        text_on_begin_drag = transmitted_slot.get_text_in_slot.text;

        // Убираю текст на время перетаскивания
        transmitted_slot.get_text_in_slot.text = string.Empty;

        // Выключаю ввод инвентарю во время перетаскивания
        inventoryMenu.Set_blocking_status = true;

        ItemEventData = eventData;

        // Запоминаю Transform родительского объекта
        parentTransform = transform.parent;

        // Устанавливаю родительский объект в качестве инвентаря
        transform.SetParent(inventoryRootTransform);

        // Рассчитываю смещение между позицией картинки и позицией мыши
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );




        /*
         * Ставлю приоритет рендеринга перетаскиваемой картинке выше всех
        */

        transform.SetSiblingIndex(canvasTransform.childCount - 1);

        // Отключаю Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = false;

        // Отключаю скорость
        image_rb.velocity = Vector2.zero;

        is_dragging = true;

    }








    RaycastResult cursorEvent;

    private string[] borders = { "Gun_border", "Item_border" };

    private string[] images = { "GunImage", "ItemImage" };




    public void OnDrag(PointerEventData eventData)
    {

        // Получаю графический луч
        cursorEvent = eventData.pointerCurrentRaycast;

        // Получаю объект, который под курсором мыши
        GameObject TargetObject = cursorEvent.gameObject;




        /*
         * Попытка получить слот, в который передаем предмет 
        */


        if (TargetObject != null)
        {
            // Проверяю если TargetObject является границей слота
            CheckCurrentObjectOnBorders(TargetObject, ref current_slot);

            // Проверяю если TargetObject является картинкой в слоте 
            CheckCurrentObjectOnImages(TargetObject, ref current_slot);
        }
        else
        {

            // Обновляю current_slot, если луч не встречает объектов
            current_slot = null;
        }




        /*
         *  Проверка на нахождение курсора мыши в пределах корня инвентаря  
        */


        // Позиция мыши на экране
        Vector3 mousePosition = Input.mousePosition;

        // Проверка на нахожнение курсора мыши в пределах корня инвентаря 
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(inventoryRootTransform, mousePosition);




        if (contains)
        {
            in_inventory_range = true;
            //target_on_slot = false;
        }
        else if (!contains)
        {
            in_inventory_range = false;
        }


    }









    private void CheckCurrentObjectOnBorders(GameObject current_object, ref GameObject current_slot)
    {
        if (borders.Contains(current_object.name))
        {
            // Получаю слот оружия, либо предмета
            current_slot = GetSlot(current_object);
        }

    }








    private void CheckCurrentObjectOnImages(GameObject current_object, ref GameObject current_slot)
    {
        if (images.Contains(current_object.name))
        {
            // Если картинка не является картинкой перетаскиваемого объекта 
            if (current_object != transform.gameObject)
            {
                // Получаю слот картинки оружия, либо предмета
                current_slot = GetSlot(current_object);
            }
        }
    }










    private GameObject GetSlot(GameObject current_object)
    {
        // Получаю слот, в котором находится текущий объект
        GameObject item_border_parent = current_object.transform.parent.gameObject;

        return item_border_parent;

    }








    
    
    private void Update()
    {
        
        if (is_dragging)
        {
            // Обновляю позицию картинки на Canvas
            Vector2 targetPosition = ItemEventData.position - offset + item_hotspot;

            Vector2 direction = (targetPosition - (transform as RectTransform).anchoredPosition).normalized;
            image_rb.AddForce(direction * 10000);
            (transform as RectTransform).anchoredPosition = targetPosition;
        }

    }

    












    public void OnEndDrag(PointerEventData eventData)
    {

        is_dragging = false;

        // Включаю ввод инвентарю
        inventoryMenu.Set_blocking_status = false;


        // Включаю Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = true;

        // Сбрасываю скорость объекта
        image_rb.velocity = Vector2.zero;

        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия (для объекта, который перетаскиваем)
        transform.SetParent(parentTransform);

        int lastIndexInParent = transform.parent.childCount - 1;

        transform.SetSiblingIndex(lastIndexInParent - 1);


        /*
         *  Если обнаружен оружейный слот 
        */


        if (current_slot != null && current_slot.tag == "GunSlot")
        {
            // Получаю слот текущего предмета с магазином
            ItemSlot current_slot_with_mag = transform.parent.gameObject.GetComponent<ItemSlot>();

            if (current_slot_with_mag != null)
            {

                // Получаю текущий объект, который находится в слоте
                GameObject current_item = current_slot_with_mag.object_in_slot;

                // Если это магазин
                if (current_item.GetComponent<mag>() != null)
                {

                    // Получаю данные входного слота
                    Slot current_slot_data = current_slot.GetComponent<Slot>();

                    if (current_slot_data.object_in_slot != null)
                    {
                        bool succesLoad = false;

                        Camera_main.GetComponent<InventoryManager>().SetMagToGun(current_slot, transform.parent.gameObject, out succesLoad);

                        if (succesLoad)
                        {
                            return;
                        }
                        else
                        {
                            transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

                            // Включаю текст обратно
                            transmitted_slot.get_text_in_slot.text = text_on_begin_drag;
                        }
                    }

                }
            }









            // Получаю слот текущего предмета с оружием
            AmmunitionGunSlot current_slot_with_gun = transform.parent.gameObject.GetComponent<AmmunitionGunSlot>();

            // Получаю оружие, которое передаю
            GameObject gun = transform.parent.gameObject.GetComponent<Slot>().object_in_slot;

            // Получаю предмет, который передаю 
            Item item = gun.GetComponent<FloorItem>().getItem;

            // Получаю слот, в который хочу передать оружие
            AmmunitionGunSlot slot = current_slot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            Camera_main.GetComponent<InventoryManager>().PutWeaponToSlot(item, gun, slot, current_slot_with_gun, out SuccessAddition);

            if (!SuccessAddition)
            {

                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

                // Включаю текст обратно
                transmitted_slot.get_text_in_slot.text = text_on_begin_drag;

            }


            return;

        }
        



        /*
         * Если обнаружен слот Item_UI 
        */


        if (current_slot != null && current_slot.tag == "ItemSlot")
        {

            // Получаю объект, который передаю
            GameObject itemObject = transform.parent.gameObject.GetComponent<Slot>().object_in_slot;

            // Получаю предмет, который передаю 
            Item item = itemObject.GetComponent<FloorItem>().getItem;

            // Получаю данные слота, в который хочу передать оружие
            ItemSlot slot = current_slot.GetComponent<ItemSlot>();

            // Получаю данные текущего слота
            ItemSlot current_pic_slot = transform.parent.gameObject.GetComponent<ItemSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            Camera_main.GetComponent<InventoryManager>().PutItemToSlot(item, itemObject, slot, current_pic_slot, out SuccessAddition);

            if (!SuccessAddition)
            {
                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

                // Включаю текст обратно
                transmitted_slot.get_text_in_slot.text = text_on_begin_drag;
            }


            return;
        }




        /*
         *  Если индикаторов не обнаруженно, и курсор находиться в пределах инвентаря 
        */


        if (in_inventory_range == true)
        {
            // Включаю текст обратно
             transmitted_slot.get_text_in_slot.text = text_on_begin_drag;

            // Устанавливаю картинку на исходную позицию
            transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
        }
        else
        {

            // Получаю выбрасываемый объект
            GameObject DroppedObj = transform.parent.gameObject.GetComponent<Slot>().object_in_slot;

            // Получаю выбрасываемый предмет
            Item item = DroppedObj.GetComponent<FloorItem>().getItem;

            // Получаю слот из которого выбросили предмет
            GameObject slot = transform.parent.gameObject;
            
            // Получаю данные слота
            Slot item_slot = slot.GetComponent<Slot>();


            if (DroppedObj != null && item != null && item_slot != null)
            {
                bool drop_is_success;

                Camera_main.GetComponent<InventoryManager>().DropItemFromInventory(item, DroppedObj, item_slot, out drop_is_success);

                if (!drop_is_success)
                {
                    // Устанавливаю картинку на исходную позицию
                    transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

                    // Включаю текст обратно
                    transmitted_slot.get_text_in_slot.text = text_on_begin_drag;
                }

            }
            else 
            {
                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

                // Включаю текст обратно
                transmitted_slot.get_text_in_slot.text = text_on_begin_drag;
            }

        }

        
    }
}