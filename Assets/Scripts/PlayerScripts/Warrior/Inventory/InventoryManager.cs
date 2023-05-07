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



    [SerializeField] private AudioClip pistolReloadSound;

    [SerializeField] private AudioClip rifleReloadSound;

    [SerializeField] private AudioClip shotGunSlugLoadSound;

    private AudioSource playerAudioSourse;





    private void Start()
    {

        am_UI = GameObject.Find("AmmunitionUI").GetComponent<RectTransform>();


        // �������� ��� ����� ��� �������������� ������
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


        // �������� ��� ��������� �����
        for (int i = 0; i < item_UI.childCount; i++)
        {
            if (item_UI.GetChild(i).GetComponent<ItemSlot>() != null)
            {
                itemSlots.Add(item_UI.GetChild(i).GetComponent<ItemSlot>());
            }
        }

        // �������� ��������� ��������
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = false;

        // �������� ��������� (������� ������� ������)
        Invoke("EndInventoryLoad", 0.1f);

        // ������� ������ ������
        gun = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        inventoryMenu = GetComponent<InventoryMenu>();

        inventory = GameObject.Find("Inventory");

        // �������� �� ������ FaceUI, � ������� ����� ���� ��� ���������� ������� 
        onFadeScreen.SetActive(true);

        Invoke("TurnOffFadeScreen", 0.1f);

        // ������� �������� ����� ������
        playerAudioSourse = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

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
            // ������� ���� �������� ������
            GameObject current_gun_slot = gun.GetCurrentSlot();

            // ������� ������ � ����� 
            GameObject gun_in_slot = current_gun_slot.GetComponent<Slot>().object_in_slot;

            // ������� ����� �����������
            float reload_time = gun.get_current_reload_time;

            Gun.Guns gun_type = gun.GetGunType();

            // ��� ���� � �������� ���������
            List<Slot> mag_slots = null;

            if (gun_type == Gun.Guns.hammer || gun_type == Gun.Guns.none) { return; }

            if (gun_type == Gun.Guns.shotgun)
            {
                // ������� ����� � ������ ��� ���������
                List<Slot> slots_with_shotgun_bullets = SearchSlotWithBulletStack();

                // ������� ������ ���������
                gun_shotgun shotgun_data = gun_in_slot.GetComponent<FloorItem>().getItem as gun_shotgun;

                // ������� ����������� ���������� ������
                int missing_bullets_count = shotgun_data.GetCapasity - gun_in_slot.transform.childCount;

                StartCoroutine(ReloadShotgun(current_gun_slot.GetComponent<Slot>(), slots_with_shotgun_bullets, missing_bullets_count, shotgun_data.GetBulletGrabTime, shotgun_data.GetBulletLoadTime));
            }
            else
            {
                mag_slots = SearchSlotWithMag(gun_type);

                if (mag_slots != null && gun_in_slot != null)
                {
                    StartCoroutine(ReloadGun(current_gun_slot, mag_slots, reload_time, gun_type));
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
                // ������� ������� � �����
                RifleMag mag_in_slot = slot.item_in_slot as RifleMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    slots_with_mags.Add(slot);
            }
            else
            {
                // ������� ������� � �����
                PistolMag mag_in_slot = slot.item_in_slot as PistolMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    slots_with_mags.Add(slot);
            }
        }

        if (slots_with_mags.Count > 0) 
        {
            // �������� ������ ��������� ���������
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
            // ������� ������� � �����
            shotgun_bullets internal_bullet_stack = slot.item_in_slot as shotgun_bullets;
            if (internal_bullet_stack != null && slot.object_in_slot.transform.childCount > 0)
            { 
                found_slots.Add(slot);
            }
        }
        
        return found_slots;
    }








    IEnumerator ReloadGun(GameObject gun_slot, List<Slot> mag_slots, float reload_time, Guns gunType)
    {
        is_reloading = true;

        shooting.set_reload_status = true;

        inventoryMenu.set_reloading_status = true;
        
        yield return new WaitForSeconds(reload_time);

        if (block_reload)
        {
            Debug.Log("����������� ���� �����������");
            block_reload = false;
            is_reloading = false;
            shooting.set_reload_status = false;
            inventoryMenu.set_reloading_status = false;
            yield break;
        }

        if (gunType == Guns.pistol)
            playerAudioSourse.PlayOneShot(pistolReloadSound);
        else
            playerAudioSourse.PlayOneShot(rifleReloadSound);

        bool flag = false;
        
        SetMagToGun(gun_slot, mag_slots.First().gameObject, out flag);

        Debug.Log($"������ ������ ������, = {flag}");
        
        is_reloading = false;

        shooting.set_reload_status = false;
        inventoryMenu.set_reloading_status = false;
    }








    private void LoadBulletToShotgun(Slot gun_slot, List<Slot> slots_with_bullets, out bool bullet_is_loadad)
    {

        bullet_is_loadad = false;

        if (gun_slot != null && slots_with_bullets.Count > 0)
        {
            // ������� ������ � �����
            GameObject shotgun = gun_slot.object_in_slot;

            // ������� ������ ���������
            gun_shotgun shotgun_data = shotgun.GetComponent<FloorItem>().getItem as gun_shotgun;

            // ���� �������� � ��������� ������ ��� ��� �����
            if (shotgun.transform.childCount < shotgun_data.GetCapasity)
            {
                foreach (Slot slot in slots_with_bullets)
                {
                    // ������� ���� ���� �� �����
                    GameObject bullets = slot.object_in_slot;

                    // ������� ������ �����
                    bullet_stack_data stack_data = bullets.GetComponent<bullet_stack_data>();

                    // ������� ���� �� �����
                    GameObject bullet = stack_data.TakeBullet();

                    shotgun_capacity ShotgunCapacity = shotgun.GetComponent<shotgun_capacity>();

                    if (bullet != null) 
                    {
                        GameObject returned_bullet = null;

                        // �������� ������ ����� ���� �� ��������� � ����� ���� ���������
                        ShotgunCapacity.LoadBullet(bullet, out returned_bullet);

                        if (returned_bullet != null)
                        {
                            Debug.Log("���� �� ���� ��������� � ��������");
                            stack_data.LoadBullet(returned_bullet, out returned_bullet);
                            Debug.Log($"���� ���� ���������� ������� � ����? = {returned_bullet == null}");
                            return;
                        }

                        // �������� ���� � ��������
                        bullet.transform.SetParent(shotgun.transform);

                        int slot_current_index = gun.get_current_slot - 1;
                        
                        // �������� ������ � ������� �����
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


        // ������� ������ � ��������
        for (int i = 0; i < missing_num_of_bullets; i++)
        {

            // �������� �� ������������� �������� �����������
            if (block_reload) break;

            // ������ ������ �� ��������� 
            yield return new WaitForSeconds(grab_time);

            // �������� ����� �������� ������ �������
            yield return new WaitForSeconds(load_time);

            playerAudioSourse.PlayOneShot(shotGunSlugLoadSound);
            
            bool bullet_was_loaded = false;
            
            // ������� ������ �������
            LoadBulletToShotgun(gun_slot, slots_with_bullets, out bullet_was_loaded);




            /*
             * �������� ��������� �� ������
            */

            // ���� ���� �� ���� �������� � �������� - �������� �����������
            if (!bullet_was_loaded) break;

        }

        is_reloading = false;

        shooting.set_reload_status = false;

        inventoryMenu.set_reloading_status = false;

        // ��������� ��������� ��������
        block_reload = false;


    }












    public void MainInventoryManager(Item item, GameObject itemObj)
    {


        if (item == null || itemObj == null) { return; }

        /*
         * ���� ������������ ������ �������� ������� 
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
         * ���� ������������ ������� �� �������� ������� ��� ������ 
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

            // ������� �������� ��������
            Transform image = slot.transform.GetChild(1);




            /*
             *  ���������� ����������� �������
            */

            // ������ ������ ������� �� ����� 
            itemObj.GetComponent<SpriteRenderer>().sprite = null;

            // �������� ��������� �������� �� �����, ����� ��� ������ ���� ��������� ������
            itemObj.GetComponent<Collider2D>().enabled = false;




            /*
             *  ������������ ����������� ��������, �������� ������� � ����
            */

            image.GetComponent<Image>().sprite = item.GetInventoryIcon;

            image.GetComponent<Image>().enabled = true;




            /*
             * ���������� ������ �����
            */

            // ������� � ���� ������� � �������������� ��� ������ 
            slot.SetItem(item, itemObj);


        }
    }








    private void GrabItem(Item item, GameObject TransmittedObject, Slot slot, out bool success)
    {
        
        success = false;



        if (item != null && TransmittedObject != null && slot != null)
        {
            
            // ������� �������� ��������
            Transform item_image_transform = slot.transform.GetChild(1);

            // ������� ������ �������� 
            FloorItem item_data = TransmittedObject.GetComponent<FloorItem>();

            // ������������ �����, ��� ������� ��������� � ���������
            item_data.set_in_inventory_status = true;



            /*
             *  ���������� ����������� �������
            */


            // ������ ������ ������� �� ����� 
            TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;

            // �������� ��������� �������� �� �����, ����� ��� ������ ���� ��������� ������
            TransmittedObject.GetComponent<Collider2D>().enabled = false;




            /*
             *  ������������ ����������� ��������, �������� ������� � ����
            */

            item_image_transform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            item_image_transform.GetComponent<Image>().enabled = true;




            /*
             * ���������� ������ �����
            */


            // ������� � ���� ������� � �������������� ��� ������ 
            slot.SetItem(item, TransmittedObject);




            /*
             * �������� ���������� �������� ������ � ���� 
             */


            // ���������� �������� ������ �������
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
            // ����� ����������� ������� � ���������
            GrabItem(item, TransmittedObject, slot, out success);
        }
        else
        {
            // ���� ��� ����� ������ ���������, ���������� ������ ���������
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
                // ����� ����������� ������� � ���������
                GrabItem(item, TransmittedObject, slot, out SuccessGunAddition);

            }
            else
            {
                // ������ ������� �� �������� �������, ������� ��� ������ �������� � ���� ����
                SuccessGunAddition = false;
            }
        }
        else
        {
            // ���� �����. ���������� ������ ���������
            SuccessGunAddition = false;
        }
    }










    private void SetItemToEmptySlot(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;

        if (item != null && TransmittedObject != null && slot != null)
        {


            // ������� Transform ��������, � ������� ���� �������� �������
            Transform InputImageTransform = slot.transform.GetChild(1);

            // ������� Transform ��������, ������� �������� ������������� ��������
            Transform currentImageTransform = current_slot.transform.GetChild(1);




            /*
             * ������������ ����������� � ����, � ������� �������� � ������� �
            */


            InputImageTransform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            InputImageTransform.GetComponent<Image>().enabled = true;




            /*
             * ���������� ������ �����, � ������� �������� 
            */


            // ������� � ���� ������� � �������������� ��� ������
            slot.SetItem(item, TransmittedObject);




            /*
             * ���������� ������ �����, �� �������� ���������� ������� 
            */


            // ������ ���� 
            current_slot.ClearClot();




            /*
             * ���������� �������� �����, �� ������� ���������� �������  
            */

            // ������ ����������� ��������, ������� ����������
            currentImageTransform.GetComponent<Image>().sprite = null;

            // ��������� �������� ��������� �� �����
            currentImageTransform.position = current_slot.SlotDefaultPosition;

            // ����� �� ����������
            currentImageTransform.GetComponent<Image>().enabled = false;



            int slot_current_index = gun.get_current_slot - 1;

            // �������� ������ � ������� �����
            gun.UpdateGun(slot_current_index);



            /*
             * �������� �������� ��������
            */


            // ���������� �������� ������ �������
            ItemAdded = true;


        }
        else 
        {
            // ��������� �������� ������ �� �������
            ItemAdded = false;
        }

        
    }









    private void SetItemWithReplace(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        

        /*
         * ������� �������� ������
        */


        // ������� Transform ��������, � ������� ���� �������� �������
        Transform InputImageTransform = slot.transform.GetChild(1);

        // ������� Transform ��������, ������� �������� ������������� ��������
        Transform currentImageTransform = current_slot.transform.GetChild(1);




        /*
         * ����� ������� �������� (� ��������) � ������ �� �� ������ ��� ������� 1
        */


        InputImageTransform.SetParent(current_slot.transform);

        InputImageTransform.SetSiblingIndex((current_slot.transform.childCount - 1) - 1);

        currentImageTransform.SetParent(slot.transform);

        currentImageTransform.SetSiblingIndex((slot.transform.childCount - 1) - 1);


        /*
         * ����� ������� �������� (���������)
        */


        // ��������� �������� �������� �� �����
        currentImageTransform.position = slot.SlotDefaultPosition;

        InputImageTransform.position = current_slot.SlotDefaultPosition;




        /*
         * ���������� ������� ������, � ������� �������� 
        */


        // ������� ������, ������� ������ ��������� � CurrentSlot
        GameObject current_obj = current_slot.object_in_slot;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_current = current_slot.item_in_slot;

        // ������� ������, ������� ������ ��������� � InputSlot
        GameObject input_obj = slot.object_in_slot;

        // ������� �������, ������� ������ ��������� � CurrentSlot
        Item item_input = slot.item_in_slot;

        // ������������ �� � ����� ����
        slot.SetItem(item_current, current_obj);

        // ������������ �� � ����� ���� 
        current_slot.SetItem(item_input, input_obj);




        int slot_current_index = gun.get_current_slot - 1;

        // �������� ������ � ������� �����
        gun.UpdateGun(slot_current_index);




        /*
         * �������� �������� ��������
        */


        // ������ �������� ������ �������
        ItemAdded = true;


    }










    public void PutItemToSlot(Item item, GameObject TransmittedObject, ItemSlot slot, ItemSlot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;


        if (item.itemType == ItemType.gun || item.itemType == ItemType.armor || item.itemType == ItemType.secondaty_arms || item.itemType == ItemType.edged_weapon)
        {
            // ���������� ������ �� ������� 
            ItemAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {
                // ������������ ������� � ������ ����
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out ItemAdded);

            }
            else
            {
                // ������������ ������� � ������� ������ ��������� ����
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out ItemAdded);
            }
        }


    }









    //          �������, ������� ��������,         ���� �������,          �������� ���������    
    public void PutWeaponToSlot(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, AmmunitionGunSlot current_slot, out bool GunIsAdded)
    {


        GunIsAdded = false;


        if (item.itemType != ItemType.gun)
        {
            // ������� �� �������� �������, ���������� ������ �� ������� 
            GunIsAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {

                // ������������ ������ � ������ ����
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out GunIsAdded);

            }
            else
            {
                // ������������ ������ � ������� ������ ��������� ����
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out GunIsAdded);

            }
        }


    }









    public void DropItemFromInventory(Item item, GameObject currentObject, Slot slot, out bool SuccessDrop)
    {
        SuccessDrop = false;

        if (item != null && currentObject != null)
        {
            
            // ���������� ����, � ������� ����� �������
            slot.ClearClot();




            /*
             * ������ �������� �������� � ���������
            */


            // ������� ��������
            Image image_component = slot.transform.GetChild(1).gameObject.GetComponent<Image>();

            // ������ ��������
            image_component.sprite = null;

            // ����� �������� ����������
            image_component.enabled = false;




            /*
             *  ������ �������� �� ��������� �����
            */


            // ������� ������ ��������
            GameObject image_obj = slot.transform.GetChild(1).gameObject;

            // ������ �� ��������� ����� 
            image_obj.transform.position = slot.SlotDefaultPosition;




            /*
             * ������� ������� ����, ����� �������� ������ �������� ������ 
            */

            int slot_current_index = gun.get_current_slot - 1;

            // �������� ������ � ������� �����
            gun.UpdateGun(slot_current_index);



            /*
             * ����������� ������� �� ��������� 
            */


            // ������������ ������������ ������� ��������� �������
            DropItemToRandomPoint(currentObject);

            // ������� ������ ��������
            FloorItem item_data = currentObject.GetComponent<FloorItem>();

            // ������ ������ ���������� �� ���� ��������
            EnemyManager.Instance.RemoveFromItemList(item_data.getId, item_data.GetCurrentSceneIndex);

            // ������������ ����� ������ �����
            item_data.UpdateSceneIndex();

            // �������� ����� ���������� 
            EnemyManager.Instance.AddToItemsList(item_data.getId, item_data.GetCurrentSceneIndex);

            // ������������ ����, ��� ������� ��������� �� � ���������
            item_data.set_in_inventory_status = false;


            /*
             * ������������ ������ �������� 
            */


            // ������� ������ ��������
            Sprite item_floor_image = item.GetFloorIcon;

            // ������������ ������
            currentObject.GetComponent<SpriteRenderer>().sprite = item_floor_image;

            // ������� ��������� �������, ����� ��� ����� ���� ���������
            currentObject.GetComponent<Collider2D>().enabled = true;




            /*
             * �������� ����������
            */

            SuccessDrop = true;


        }
    }









    public void SetMagToGun(GameObject input_slot, GameObject current_slot, out bool successLoad)
    {

        successLoad = false;

        // ������� ������ �������� �����
        Slot input_slot_data = input_slot.GetComponent<Slot>();

        // ������� ������ �� ������� �����
        GameObject gun_in_input_slot = input_slot_data.object_in_slot;

        // ������� ������ �������� �����
        Slot current_slot_data = current_slot.GetComponent<Slot>();

        // ������� ������� � �������� �����
        GameObject mag_in_current_slot = current_slot_data.object_in_slot;




        /*
         * �������� �� ��, ���� �� ������� � �������
        */

        if (gun_in_input_slot.transform.childCount > 0)
        {

            /*
             * ���� ������ �������
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {

                // ������������ ������� � ���������� ������
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // ������� ������� � �������
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // ������ ������� � ��������� �������� � �������� �������
                    dropped_gun_mag.transform.SetParent(null);

                    // ������ ������� �� ��������� ��� �������� ������ ������
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // �������� ����� �����, ������� �������� ����������� ������ 
                    input_slot_data.UpdateSlotTextData();

                    // ������������ �������� � ���������
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    int slot_current_index = gun.get_current_slot - 1;

                    // �������� ������ � ������� �����
                    gun.UpdateGun(slot_current_index);

                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // ������������ ������� � ���������� ������
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // ������� ������� � �������
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // ������ ������� � ��������� �������� � �������� �������
                    dropped_gun_mag.transform.SetParent(null);

                    // ������ ������� �� ��������� ��� �������� ������ ������
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // �������� ����� �����, ������� �������� ����������� ������ 
                    input_slot_data.UpdateSlotTextData();

                    // ������������ �������� � ���������
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    int slot_current_index = gun.get_current_slot - 1;

                    // �������� ������ � ������� �����
                    gun.UpdateGun(slot_current_index);

                }

                successLoad = is_loaded;
            }

        }
        else
        {

            /*
             * ���� ������ �� �������
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {
                // ������������ ������� � ���������� ��������� ��������
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                int slot_current_index = gun.get_current_slot - 1;

                // �������� ������ � ������� �����
                gun.UpdateGun(slot_current_index);

                if (is_loaded) 
                {
                    // ������������ ������� � ���������
                    SetMagInInventory(input_slot, current_slot);
                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // ������������ ������� � ���������� ���������
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                int slot_current_index = gun.get_current_slot - 1;

                // �������� ������ � ������� �����
                gun.UpdateGun(slot_current_index);

                if (is_loaded)
                {
                    // ������������ ������� � ���������
                    SetMagInInventory(input_slot, current_slot);
                }
                
                successLoad = is_loaded;

            }

            successLoad = is_loaded;


        }
    }









    private void SetMagInInventory(GameObject input_slot, GameObject current_slot)
    {

        // ������� ������� � ������� ��������
        GameObject mag_in_pic = current_slot.GetComponent<Slot>().object_in_slot;

        // ������� ������ �� ������� �����
        GameObject gun_in_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // ������� �������� ������
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // ������� �������� �������� �����
        Image transmitted_picture = current_slot.transform.GetChild(1).gameObject.GetComponent<Image>();




        // ������������ ������� ��� �������� ������ ������ 
        mag_in_pic.transform.SetParent(gun_in_slot.transform);

        // ������ �������� ��������
        transmitted_picture.GetComponent<Image>().sprite = null;

        // �������� ��������
        transmitted_picture.GetComponent<Image>().enabled = false;

        // ������ �������� �� ��������� �������
        transmitted_picture.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;

        // ������ ������ ����������� ������ 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // ������� ����
        current_slot.GetComponent<Slot>().ClearClot();


    }









    private void SetMagInInventoryWithReplace(GameObject input_slot, GameObject current_slot, GameObject dropped_gun_mag)
    {
        // ������� �������� ������
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // ������� ������ � �����
        GameObject gun_in_input_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // ������� �������� �������� �����
        GameObject current_image = current_slot.transform.GetChild(1).gameObject;




        // ������ ������ ����������� ������ 
        gun_image.sprite = gun_in_input_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // �������� �������� ��������
        current_image.GetComponent<Image>().sprite = dropped_gun_mag.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // ������ �������� �� ��������� �������
        current_image.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;

        // �������� ���� ���������
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
        // ��������� ������ � �����
        Destroy(slot.object_in_slot);

        // �������� ����
        slot.ClearClot();

        // ������� ��������
        slot.transform.GetChild(1).GetComponent<Image>().sprite = null;

        // ������ �������� ����������
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
                        // ������� ���������� ���� � ��������
                        int bullets_amount = Object.transform.GetChild(0).GetComponent<mag>().current_bullet_count;

                        return $"{bullets_amount}/30";
                    }
                    else { return ""; }
                    break;
                case "Pistol":
                    if (Object.transform.childCount > 0)
                    {
                        // ������� ���������� ���� � ��������
                        int bullets_count = Object.transform.GetChild(0).GetComponent<mag>().current_bullet_count;

                        return $"{bullets_count}/7";
                    }
                    else { return ""; }
                    break;
                case "Shotgun":
                    // ������� ���������� ���� � ��������
                    int slug_count = Object.GetComponent<shotgun_capacity>().current_bullet_count;

                    return $"{slug_count}/8";
                    break;
            }
        }
        else if (item.itemType == ItemType.mag)
        {
            // ������� ������� ���������� ����
            mag mag_data = Object.GetComponent<mag>();

            int current_count = 0;

            if (mag_data != null) { current_count = mag_data.get_current_bullet_count; }

            int capacity = mag_data.get_capacity;

            return $"{current_count}/{capacity}";
        }
        else if (item.itemType == ItemType.bullet)
        {
            bullet_stack_data bsd = Object.GetComponent<bullet_stack_data>();


            // ������� ������� ���������� ����
            int current_count = 0;

            if (bsd != null) { current_count = bsd.get_current_bullet_count; } else { return ""; }

            // ������� Capacity
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

        // ���� ��� ������� ���������� ����������, ����� ���������� �� ������� ������
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
