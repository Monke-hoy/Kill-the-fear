using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum ItemType { armor, secondaty_arms, edged_weapon,  gun, mag, bullet, usable, default_item }

public class Item : ScriptableObject
{


    [SerializeField] private string ItemName;

    public string GetName => ItemName;

    [SerializeField] private Sprite InventoryIcon;

    public Sprite GetInventoryIcon => InventoryIcon;

    [SerializeField] private Sprite OnFloorIcon;

    public Sprite GetFloorIcon => OnFloorIcon;

    private int MaxItemCount;

    public ItemType itemType;

    [SerializeField] private GameObject scriptableGameObject;

    public GameObject ScriptableGameObject
    { 
        get { return scriptableGameObject; }
        set { scriptableGameObject = value; }
    }

}
