using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorItem : MonoBehaviour
{
    [SerializeField] private Item item;

    public Item getItem => item;


    private bool OnPlayerTarget = false;

    private AmmunitionManager am;


    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.GetFloorIcon;

        am = GameObject.Find("Main Camera").GetComponent<AmmunitionManager>();

    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        { 
            OnPlayerTarget = true;
            
        }
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().CompareTag("Player"))
        {
           OnPlayerTarget = false;
        }
    }





    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && OnPlayerTarget)
        {
            am.PutElem(item);
        }
    }

}
