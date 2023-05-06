using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;


public class InventoryManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    private List<SecondArmSlot> second_arm_slots = new List<SecondArmSlot>();

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private RectTransform am_UI;

    private RectTransform item_UI;

    private Gun gun;

    private Shooting shooting;

    private InventoryMenu inventoryMenu;

    private PauseMenu pauseMenu;

    private GameObject inventory;

    [SerializeField] private GameObject faceUI;

    public GameObject getFaceUI => faceUI;

    [SerializeField] private GameObject onFadeScreen;


    






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
            else if (am_UI.GetChild(i).GetComponent<SecondArmSlot>() != null)
            {
                second_arm_slots.Add(am_UI.GetChild(i).GetComponent<SecondArmSlot>());
            }
        }

        item_UI = GameObject.Find("ItemsUI").GetComponent<RectTransform>();


        // Получаем все дефолтные слоты
        for (int i = 0; i < item_UI.childCount; i++)
        {
            if (item_UI.GetChild(i).GetComponent<ItemSlot>() != null)
            {
                itemSlots.Add(item_UI.GetChild(i).GetComponent<ItemSlot>());
            }
        }

        // Отключаю рендеринг картинке
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = false;

        // Загрузка инвентаря (матрица позиций слотов)
        Invoke("EndInventoryLoad", 0.1f);

        // Получаю скрипт оружия
        gun = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        inventoryMenu = GetComponent<InventoryMenu>();

        inventory = GameObject.Find("Inventory");

        // Выключаю на старте FaceUI, и включаю после того как затемнение пройдет 
        onFadeScreen.SetActive(true);

        Invoke("TurnOffFadeScreen", 0.1f);

    }








    private void EndInventoryLoad()
    {
        inventory.SetActive(false);
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = true;
    }


    private void TurnOffFadeScreen() => onFadeScreen.SetActive(false);

    private void ActivateFaceUI() => faceUI.SetActive(true);




    public void LoadInventoryOnFade()
    {
        inventory.SetActive(true);

        onFadeScreen.SetActive(true);

        Invoke("ActivateFaceUI", 0.2f);

        Invoke("TurnOffFadeScreen", 0.2f);

        Invoke("EndInventoryLoad", 0.2f);
    }








    private bool is_reloading = false;



    private bool block_input = false;

    public bool set_input_block_status { set { block_input = value; } }








    private void Update() 
    {
        if (Input.GetKey(KeyCode.R) && !is_reloading && !block_input)
        {
            // Получаю слот текущего оружия
            GameObject current_gun_slot = gun.GetCurrentSlot();

            // Получаю оружие в слоте 
            GameObject gun_in_slot = current_gun_slot.GetComponent<Slot>().object_in_slot;

            // Получаю время перезарядки
            float reload_time = gun.get_current_reload_time;

            Gun.Guns gun_type = gun.GetGunType();

            // Ищу слот с непустым магазином
            List<Slot> mag_slots = null;

            if (gun_type == Gun.Guns.hammer || gun_type == Gun.Guns.none) { return; }

            if (gun_type == Gun.Guns.shotgun)
            {
                // Получаю слоты с пулями для дробовика
                List<Slot> slots_with_shotgun_bullets = SearchSlotWithBulletStack();

                // Получаю данные дробовика
                gun_shotgun shotgun_data = gun_in_slot.GetComponent<FloorItem>().getItem as gun_shotgun;

                // Получаю недостающее количество патрон
                int missing_bullets_count = shotgun_data.GetCapasity - gun_in_slot.transform.childCount;

                StartCoroutine(ReloadShotgun(current_gun_slot.GetComponent<Slot>(), slots_with_shotgun_bullets, missing_bullets_count, shotgun_data.GetBulletGrabTime, shotgun_data.GetBulletLoadTime));
            }
            else
            {
                mag_slots = SearchSlotWithMag(gun_type);

                if (mag_slots != null && gun_in_slot != null)
                {
                    StartCoroutine(ReloadGun(current_gun_slot, mag_slots, reload_time));
                }

            }

        }
    }







    public List<Slot> debug_slots = new List<Slot>();

    private List<Slot> SearchSlotWithMag(Gun.Guns gun_type)
    {
        List <Slot> slots_with_mags = new List<Slot>();

        foreach (Slot slot in itemSlots)
        {

            if (gun_type == Gun.Guns.assaultRifle)
            {
                // Получаю предмет в слоте
                RifleMag mag_in_slot = slot.item_in_slot as RifleMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    slots_with_mags.Add(slot);
            }
            else
            {
                // Получаю предмет в слоте
                PistolMag mag_in_slot = slot.item_in_slot as PistolMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    slots_with_mags.Add(slot);
            }
        }

        if (slots_with_mags.Count > 0) 
        {
            // Сортирую список найденных магазинов
            slots_with_mags = slots_with_mags.OrderByDescending(slot => slot.object_in_slot.transform.childCount).ToList();

            debug_slots = slots_with_mags;
            return slots_with_mags;
        }
        return null;
    }








    private List<Slot> SearchSlotWithBulletStack()
    {
        List<Slot> found_slots = new List<Slot>();

        foreach (Slot slot in itemSlots)
        {
            // Получаю предмет в слоте
            shotgun_bullets internal_bullet_stack = slot.item_in_slot as shotgun_bullets;
            if (internal_bullet_stack != null && slot.object_in_slot.transform.childCount > 0)
            { 
                found_slots.Add(slot);
            }
        }
        
        return found_slots;
    }








    IEnumerator ReloadGun(GameObject gun_slot, List<Slot> mag_slots, float reload_time)
    {
        is_reloading = true;

        shooting.set_reload_status = true;

        inventoryMenu.set_reloading_status = true;

        
        yield return new WaitForSeconds(reload_time);

        if (block_reload)
        {
            Debug.Log("Перезарядка была остановлена");
            block_reload = false;
            is_reloading = false;
            shooting.set_reload_status = false;
            inventoryMenu.set_reloading_status = false;
            yield break;
        }

        bool flag = false;
        
        SetMagToGun(gun_slot, mag_slots.First().gameObject, out flag);

        Debug.Log($"Замена прошла успешо, = {flag}");
        
        is_reloading = false;

        shooting.set_reload_status = false;
        inventoryMenu.set_reloading_status = false;
    }








    private void LoadBulletToShotgun(Slot gun_slot, List<Slot> slots_with_bullets, out bool bullet_is_loadad)
    {

        bullet_is_loadad = false;

        if (gun_slot != null && slots_with_bullets.Count > 0)
        {
            // Получаю оружие в слоте
            GameObject shotgun = gun_slot.object_in_slot;

            // Получаю данные дробовика
            gun_shotgun shotgun_data = shotgun.GetComponent<FloorItem>().getItem as gun_shotgun;

            // Если патронов в дробовике меньше чем его запас
            if (shotgun.transform.childCount < shotgun_data.GetCapasity)
            {
                foreach (Slot slot in slots_with_bullets)
                {
                    // Получаю стек пуль из слота
                    GameObject bullets = slot.object_in_slot;

                    // Получаю данные стека
                    bullet_stack_data stack_data = bullets.GetComponent<bullet_stack_data>();

                    // Получаю пулю из стека
                    GameObject bullet = stack_data.TakeBullet();

                    shotgun_capacity ShotgunCapacity = shotgun.GetComponent<shotgun_capacity>();

                    if (bullet != null) 
                    {
                        GameObject returned_bullet = null;

                        // Обновляю данные стека пуль из инвентаря и стека пуль дробовика
                        ShotgunCapacity.LoadBullet(bullet, out returned_bullet);

                        if (returned_bullet != null)
                        {
                            Debug.Log("Пуля не была добавлена в дробовик");
                            stack_data.LoadBullet(returned_bullet, out returned_bullet);
                            Debug.Log($"Пуля была возвращена обратно в стек? = {returned_bullet == null}");
                            return;
                        }

                        // Добавляю пулю в дробавик
                        bullet.transform.SetParent(shotgun.transform);

                        int slot_current_index = gun.get_current_slot - 1;
                        
                        // Обновляю данные в текущем слоте
                        gun.UpdateGun(slot_current_index);

                        bullet_is_loadad = true;

                        return; 
                    }
                }
            }

        }
    }








    public bool block_reload = false;

    public bool block_current_reload { set { block_reload = value; } }

    IEnumerator ReloadShotgun(Slot gun_slot, List<Slot> slots_with_bullets, int missing_num_of_bullets, float grab_time, float load_time)
    {
        is_reloading = true;

        shooting.set_reload_status = true;
        inventoryMenu.set_reloading_status = true;


        // Заряжаю патрон в дробовик
        for (int i = 0; i < missing_num_of_bullets; i++)
        {

            // Проверка на необходимость прервать перезарядку
            if (block_reload) break;

            // Достаю патрон из инвентаря 
            yield return new WaitForSeconds(grab_time);

            // Задержка перед зарядкой одного патрона
            yield return new WaitForSeconds(load_time);
            
            bool bullet_was_loaded = false;
            
            // Зарядка одного патрона
            LoadBulletToShotgun(gun_slot, slots_with_bullets, out bullet_was_loaded);




            /*
             * Проверяю добавился ли патрон
            */

            // Если пуля не была заряжена в дробовик - завершаю перезарядку
            if (!bullet_was_loaded) break;

        }

        is_reloading = false;

        shooting.set_reload_status = false;

        inventoryMenu.set_reloading_status = false;

        // Возвращаю дефолтное значение
        block_reload = false;


    }












    public void MainInventoryManager(Item item, GameObject itemObj)
    {


        if (item == null || itemObj == null) { return; }

        /*
         * Если передаваемый объект является оружием 
        */


        if (item.itemType == ItemType.gun)
        {

            foreach (AmmunitionGunSlot slot in am_gun_slots)
            {

                if (slot.object_in_slot == itemObj) { return; }

                bool SuccessGunAddition = false;

                GrabGunItem(item, itemObj, slot, out SuccessGunAddition);


                if (SuccessGunAddition) return;

            }
        }



        /*
         * Если передаваемый предмет не является оружием или броней 
        */


        if (item.itemType != ItemType.gun && item.itemType != ItemType.secondaty_arms && item.itemType != ItemType.armor && item.itemType != ItemType.edged_weapon) 
        {

            foreach (ItemSlot slot in itemSlots)
            {

                if (slot.object_in_slot == itemObj) { return; }

                bool success = false;

                GrabDefaultItem(item, itemObj, slot, out success);


                if (success) return;
            }
        }




        if (item.itemType == ItemType.edged_weapon)
        {
            GrabEdgedWeapon(item, itemObj);
        }


    }









    private void GrabEdgedWeapon(Item item, GameObject itemObj)
    {

        Slot slot = second_arm_slots[0];

        if (slot.SlotIsEmpty)
        {

            // Получаю картинку предмета
            Transform image = slot.transform.GetChild(1);




            /*
             *  Настраиваю подобранный предмет
            */

            // Убираю спрайт объекта на земле 
            itemObj.GetComponent<SpriteRenderer>().sprite = null;

            // Выключаю коллайдер пердмету на земле, чтобы его нельзя было подобрать дважды
            itemObj.GetComponent<Collider2D>().enabled = false;




            /*
             *  Устанавливаю изображение предмета, которого передал в слот
            */

            image.GetComponent<Image>().sprite = item.GetInventoryIcon;

            image.GetComponent<Image>().enabled = true;




            /*
             * Настраиваю скрипт слота
            */

            // Передаю в слот предмет и представляющий его объект 
            slot.SetItem(item, itemObj);


        }
    }








    private void GrabItem(Item item, GameObject TransmittedObject, Slot slot, out bool success)
    {
        
        success = false;



        if (item != null && TransmittedObject != null && slot != null)
        {
            
            // Получаю картинку предмета
            Transform item_image_transform = slot.transform.GetChild(1);

            // Получаю данные предмета 
            FloorItem item_data = TransmittedObject.GetComponent<FloorItem>();

            // Устанавливаю фалаг, что предмет находится в инвентаре
            item_data.set_in_inventory_status = true;



            /*
             *  Настраиваю подобранный предмет
            */


            // Убираю спрайт объекта на земле 
            TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;

            // Выключаю коллайдер пердмету на земле, чтобы его нельзя было подобрать дважды
            TransmittedObject.GetComponent<Collider2D>().enabled = false;




            /*
             *  Устанавливаю изображение предмета, которого передал в слот
            */

            item_image_transform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            item_image_transform.GetComponent<Image>().enabled = true;




            /*
             * Настраиваю скрипт слота
            */


            // Передаю в слот предмет и представляющий его объект 
            slot.SetItem(item, TransmittedObject);




            /*
             * Успешное завершение передачи оружия в слот 
             */


            // Добавление предмета прошло успешно
            success = true;
        }
        else 
        {
            success = false;        
        }

    }










    private void GrabDefaultItem(Item item, GameObject TransmittedObject, ItemSlot slot, out bool success)
    { 
        success = false;

        if (slot.SlotIsEmpty)
        {
            // Кладу подобранный предмет в инвентарь
            GrabItem(item, TransmittedObject, slot, out success);
        }
        else
        {
            // Слот был занят другим предметом, добавление прошло неуспешно
            success = false;
        }
    }









    private void GrabGunItem(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {
                // Кладу подобранный предмет в инвентарь
                GrabItem(item, TransmittedObject, slot, out SuccessGunAddition);

            }
            else
            {
                // Данный предмет не является оружием, поэтому его нельзя добавить в этот слот
                SuccessGunAddition = false;
            }
        }
        else
        {
            // Слот занят. Добавление прошло неуспешно
            SuccessGunAddition = false;
        }
    }










    private void SetItemToEmptySlot(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;

        if (item != null && TransmittedObject != null && slot != null)
        {


            // Получаю Transform картинки, в которую хочу передать предмет
            Transform InputImageTransform = slot.transform.GetChild(1);

            // Получаю Transform картинки, текущей картинки передаваемого предмета
            Transform currentImageTransform = current_slot.transform.GetChild(1);




            /*
             * Устанавливаю изображение в слот, в который передали и включаю её
            */


            InputImageTransform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            InputImageTransform.GetComponent<Image>().enabled = true;




            /*
             * Настраиваю скрипт слота, в который передали 
            */


            // Передаю в слот предмет и представляющий его объект
            slot.SetItem(item, TransmittedObject);




            /*
             * Настраиваю скрипт слота, из которого передавали предмет 
            */


            // Очищаю слот 
            current_slot.ClearClot();




            /*
             * Настраиваю картинку слота, из которой передавали предмет  
            */

            // Удаляю изображение предмета, который передавали
            currentImageTransform.GetComponent<Image>().sprite = null;

            // Возвращаю картинку дефолтное на место
            currentImageTransform.position = current_slot.SlotDefaultPosition;

            // Делаю ее неактивной
            currentImageTransform.GetComponent<Image>().enabled = false;



            int slot_current_index = gun.get_current_slot - 1;

            // Обновляю данные в текущем слоте
            gun.UpdateGun(slot_current_index);



            /*
             * Успешная передача предмета
            */


            // Добавление предмета прошло успешно
            ItemAdded = true;


        }
        else 
        {
            // Добавлени предмета прошло не успешно
            ItemAdded = false;
        }

        
    }









    private void SetItemWithReplace(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        

        /*
         * Получаю картинки слотов
        */


        // Получаю Transform картинки, в которую хочу передать предмет
        Transform InputImageTransform = slot.transform.GetChild(1);

        // Получаю Transform картинки, текущей картинки передаваемого предмета
        Transform currentImageTransform = current_slot.transform.GetChild(1);




        /*
         * Меняю местами картинки (в иерархии) и ставлю их на индекс под номером 1
        */


        InputImageTransform.SetParent(current_slot.transform);

        InputImageTransform.SetSiblingIndex((current_slot.transform.childCount - 1) - 1);

        currentImageTransform.SetParent(slot.transform);

        currentImageTransform.SetSiblingIndex((slot.transform.childCount - 1) - 1);


        /*
         * Меняю местами картинки (визуально)
        */


        // Возвращаю исходную картинку на место
        currentImageTransform.position = slot.SlotDefaultPosition;

        InputImageTransform.position = current_slot.SlotDefaultPosition;




        /*
         * Настраиваю скрипты слотов, в который передали 
        */


        // Получаю объект, который сейчас находится в CurrentSlot
        GameObject current_obj = current_slot.object_in_slot;

        // Получаю предмет, который сейчас находится в CurrentSlot
        Item item_current = current_slot.item_in_slot;

        // Получаю объект, который сейчас находится в InputSlot
        GameObject input_obj = slot.object_in_slot;

        // Получаю предмет, который сейчас находится в CurrentSlot
        Item item_input = slot.item_in_slot;

        // Устанавливаю их в новый слот
        slot.SetItem(item_current, current_obj);

        // Устанавливаю их в новый слот 
        current_slot.SetItem(item_input, input_obj);




        int slot_current_index = gun.get_current_slot - 1;

        // Обновляю данные в текущем слоте
        gun.UpdateGun(slot_current_index);




        /*
         * Успешная передача предмета
        */


        // Замена предмета прошла успешно
        ItemAdded = true;


    }










    public void PutItemToSlot(Item item, GameObject TransmittedObject, ItemSlot slot, ItemSlot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;


        if (item.itemType == ItemType.gun || item.itemType == ItemType.armor || item.itemType == ItemType.secondaty_arms || item.itemType == ItemType.edged_weapon)
        {
            // Добавление прошло не успешно 
            ItemAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {
                // Устанавливаю предмет в пустой слот
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out ItemAdded);

            }
            else
            {
                // Устанавливаю предмет в занятый другим предметом слот
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out ItemAdded);
            }
        }


    }









    //          предмет, который передаем,         куда передаём,          выходной результат    
    public void PutWeaponToSlot(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, AmmunitionGunSlot current_slot, out bool GunIsAdded)
    {


        GunIsAdded = false;


        if (item.itemType != ItemType.gun)
        {
            // Предмет не является оружием, добавление прошло не успешно 
            GunIsAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {

                // Устанавливаю оружие в пустой слот
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out GunIsAdded);

            }
            else
            {
                // Устанавливаю оружие в занятый другим предметом слот
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out GunIsAdded);

            }
        }


    }









    public void DropItemFromInventory(Item item, GameObject currentObject, Slot slot, out bool SuccessDrop)
    {
        SuccessDrop = false;

        if (item != null && currentObject != null)
        {
            
            // Настраиваю слот, в котором лежал предмет
            slot.ClearClot();




            /*
             * Убираю картинку предмету в инвентаре
            */


            // Получаю картинку
            Image image_component = slot.transform.GetChild(1).gameObject.GetComponent<Image>();

            // Убираю картинку
            image_component.sprite = null;

            // Делаю картинку неактивной
            image_component.enabled = false;




            /*
             *  Ставлю картинку на дефолтное место
            */


            // Получаю объект картинки
            GameObject image_obj = slot.transform.GetChild(1).gameObject;

            // Ставлю на дефолтное место 
            image_obj.transform.position = slot.SlotDefaultPosition;




            /*
             * Вызываю текущий слот, чтобы обновить данные текущего оружия 
            */

            int slot_current_index = gun.get_current_slot - 1;

            // Обновляю данные в текущем слоте
            gun.UpdateGun(slot_current_index);



            /*
             * Выбрасываем предмет из инвентаря 
            */


            // Устанавливаю выброшенному объекту рандомную позицию
            DropItemToRandomPoint(currentObject);

            // Получаю данные предмета
            FloorItem item_data = currentObject.GetComponent<FloorItem>();

            // Удаляю старую информацию об этом предмете
            EnemyManager.Instance.RemoveFromItemList(item_data.getId, item_data.GetCurrentSceneIndex);

            // Устанавливаю новый индекс сцены
            item_data.UpdateSceneIndex();

            // Добавляю новую информацию 
            EnemyManager.Instance.AddToItemsList(item_data.getId, item_data.GetCurrentSceneIndex);

            // Устанавливаю флаг, что предмет находится не в инвентаре
            item_data.set_in_inventory_status = false;


            /*
             * Устанавливаю спрайт предмету 
            */


            // Получаю спрайт предмета
            Sprite item_floor_image = item.GetFloorIcon;

            // Устанавливаю спрайт
            currentObject.GetComponent<SpriteRenderer>().sprite = item_floor_image;

            // Включаю коллайдер обратно, чтобы его можно было подобрать
            currentObject.GetComponent<Collider2D>().enabled = true;




            /*
             * Успешное завершение
            */

            SuccessDrop = true;


        }
    }









    public void SetMagToGun(GameObject input_slot, GameObject current_slot, out bool successLoad)
    {

        successLoad = false;

        // Получаю данные входнохо слота
        Slot input_slot_data = input_slot.GetComponent<Slot>();

        // Получаю оружие во входном слоте
        GameObject gun_in_input_slot = input_slot_data.object_in_slot;

        // Получаю данные текущего слота
        Slot current_slot_data = current_slot.GetComponent<Slot>();

        // Получаю предмет в исходном слоте
        GameObject mag_in_current_slot = current_slot_data.object_in_slot;




        /*
         * Проверяю на то, есть ли магазин в объекте
        */

        if (gun_in_input_slot.transform.childCount > 0)
        {

            /*
             * Если объект заряжен
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {

                // Устанавливаю магазин в переменную оружия
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // Получаю магазин в объекте
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // Убираю магазин в свободное плавание в иерархии проекта
                    dropped_gun_mag.transform.SetParent(null);

                    // Стввлю магазин из инвентаря как дочерний объект оружия
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // Обновляю текст слота, который является индикатором патрон 
                    input_slot_data.UpdateSlotTextData();

                    // Устанавливаю магазины в инвентаре
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    int slot_current_index = gun.get_current_slot - 1;

                    // Обновляю данные в текущем слоте
                    gun.UpdateGun(slot_current_index);

                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // Устанавливаю магазин в переменную оружия
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // Получаю магазин в объекте
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // Убираю магазин в свободное плавание в иерархии проекта
                    dropped_gun_mag.transform.SetParent(null);

                    // Стввлю магазин из инвентаря как дочерний объект оружия
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // Обновляю текст слота, который является индикатором патрон 
                    input_slot_data.UpdateSlotTextData();

                    // Устанавливаю магазины в инвентаре
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    int slot_current_index = gun.get_current_slot - 1;

                    // Обновляю данные в текущем слоте
                    gun.UpdateGun(slot_current_index);

                }

                successLoad = is_loaded;
            }

        }
        else
        {

            /*
             * Если объект не заряжен
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {
                // Устанавливаю магазин в переменную штурмовой винтовки
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                int slot_current_index = gun.get_current_slot - 1;

                // Обновляю данные в текущем слоте
                gun.UpdateGun(slot_current_index);

                if (is_loaded) 
                {
                    // Устанавливаю магазин в инвентаре
                    SetMagInInventory(input_slot, current_slot);
                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // Устанавливаю магазин в переменную пистолета
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                int slot_current_index = gun.get_current_slot - 1;

                // Обновляю данные в текущем слоте
                gun.UpdateGun(slot_current_index);

                if (is_loaded)
                {
                    // Устанавливаю магазин в инвентаре
                    SetMagInInventory(input_slot, current_slot);
                }
                
                successLoad = is_loaded;

            }

            successLoad = is_loaded;


        }
    }









    private void SetMagInInventory(GameObject input_slot, GameObject current_slot)
    {

        // Получаю магазин в текущей картинке
        GameObject mag_in_pic = current_slot.GetComponent<Slot>().object_in_slot;

        // Получаю оружие во входном слоте
        GameObject gun_in_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // Получаю картинку оружия
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // Получаю картинку текущего слота
        Image transmitted_picture = current_slot.transform.GetChild(1).gameObject.GetComponent<Image>();




        // Устанавливаю магазин как дочерний объект оружия 
        mag_in_pic.transform.SetParent(gun_in_slot.transform);

        // Убираю картинку магазина
        transmitted_picture.GetComponent<Image>().sprite = null;

        // Выключаю картинку
        transmitted_picture.GetComponent<Image>().enabled = false;

        // Ставлю картинку на дефолтную позицию
        transmitted_picture.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;

        // Ставлю спрайт заряженного оружия 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Отчищаю слот
        current_slot.GetComponent<Slot>().ClearClot();


    }









    private void SetMagInInventoryWithReplace(GameObject input_slot, GameObject current_slot, GameObject dropped_gun_mag)
    {
        // Получаю картинку оружия
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // Получаю оружие в слоте
        GameObject gun_in_input_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // Получаю картинку текущего слота
        GameObject current_image = current_slot.transform.GetChild(1).gameObject;




        // Ставлю спрайт заряженного оружия 
        gun_image.sprite = gun_in_input_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Обновляю картинку магазина
        current_image.GetComponent<Image>().sprite = dropped_gun_mag.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Ставлю картинку на дефолтную позицию
        current_image.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;

        // Обновляю слот инвентаря
        current_slot.GetComponent<Slot>().SetItem(dropped_gun_mag.GetComponent<FloorItem>().getItem, dropped_gun_mag);

    }









    public void ResetInventory()
    {
        foreach (Slot slot in am_gun_slots)
        {
            ResetSlot(slot);
        }

        foreach (Slot slot in itemSlots)
        {
            ResetSlot(slot);
        }
    }









    private void ResetSlot(Slot slot)
    {
        // Уничтожаю объект в слоте
        Destroy(slot.object_in_slot);

        // Отчищаем слот
        slot.ClearClot();

        // Убираем картинку
        slot.transform.GetChild(1).GetComponent<Image>().sprite = null;

        // Делаем картинку неактивной
        slot.transform.GetChild(1).GetComponent<Image>().enabled = false;
    }









    public string GetAmmoData(Item item, GameObject Object)
    {
        if (item.itemType == ItemType.gun)
        {
            switch (item.GetName)
            {
                case "Rifle":
                    if (Object.transform.childCount > 0)
                    {
                        // Получаю количество пуль в магазине
                        int bullets_amount = Object.transform.GetChild(0).GetComponent<mag>().current_bullet_count;

                        return $"{bullets_amount}/30";
                    }
                    else { return ""; }
                    break;
                case "Pistol":
                    if (Object.transform.childCount > 0)
                    {
                        // Получаю количество пуль в магазине
                        int bullets_count = Object.transform.GetChild(0).GetComponent<mag>().current_bullet_count;

                        return $"{bullets_count}/7";
                    }
                    else { return ""; }
                    break;
                case "Shotgun":
                    // Получаю количество пуль в магазине
                    int slug_count = Object.GetComponent<shotgun_capacity>().current_bullet_count;

                    return $"{slug_count}/8";
                    break;
            }
        }
        else if (item.itemType == ItemType.mag)
        {
            // Получаю текущее количество пуль
            mag mag_data = Object.GetComponent<mag>();

            int current_count = 0;

            if (mag_data != null) { current_count = mag_data.get_current_bullet_count; }

            int capacity = mag_data.get_capacity;

            return $"{current_count}/{capacity}";
        }
        else if (item.itemType == ItemType.bullet)
        {
            bullet_stack_data bsd = Object.GetComponent<bullet_stack_data>();


            // Получаю текущее количество пуль
            int current_count = 0;

            if (bsd != null) { current_count = bsd.get_current_bullet_count; } else { return ""; }

            // Получаю Capacity
            int capacity = bsd.get_capacity;

            return $"{current_count}/{capacity}";


        }
        return "";
    }









    public void UpdateAllSlots() 
    {
        foreach (Slot slot in am_gun_slots)
        { 
            slot.UpdateSlotTextData();
        }

        foreach (Slot slot in itemSlots)
        {
            slot.UpdateSlotTextData();
        }
    }









    private int maxTries = 200;
    private float minDistance = 0.1f;
    private float maxDistance = 0.3f;
    private float rayLength = 0.08f;
    private int rayCount = 8;
    private Vector2 playerPosition;

    private List<Vector2> positions = new List<Vector2>();

    private void DropItemToRandomPoint(GameObject itemObject)
    {
        positions.Clear();

        int tries = 0;
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        while (tries < maxTries)
        {
            float distance = Random.Range(minDistance, maxDistance);
            float angle = Random.Range(0f, 360f);
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            if (!IsDirectionBlocked(direction, distance))
            {
                Vector2 position = playerPosition + direction * distance;

                if (!IsPositionBlocked(position))
                {
                    itemObject.transform.position = position;
                    return;
                }
            }

            tries++;
        }

        // Если все попытки окакзались неудачными, тогда выбрасываю на позицию игрока
        itemObject.transform.position = playerPosition;
    }









    private bool IsDirectionBlocked(Vector2 direction, float dist)
    {
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, dist, LayerMask.GetMask("Environment"));
        return hit.collider != null;
    }









    private bool IsPositionBlocked(Vector2 position)
    {
        
        if (positions.Contains(position))
        {
            return true;
        }
        

        float angleStep = 360f / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = angleStep * i;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(position, direction, rayLength, LayerMask.GetMask("Environment"));
            if (hit.collider != null)
            {
                positions.Add(position);
                return true;
            }
        }

        return false;
    }



}
