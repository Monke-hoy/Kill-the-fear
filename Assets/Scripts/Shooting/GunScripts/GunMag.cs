using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMag : MonoBehaviour
{

    protected GameObject magObject;

    public GameObject SetMagToGun { set { magObject = value; } }

    public GameObject GetMagInGun { get { return magObject; } }




    private void Start()
    {
        magObject = transform.GetChild(0).gameObject;

        magObject.GetComponent<SpriteRenderer>().sprite = null;
        magObject.GetComponent<Collider2D>().enabled = false;
    }


}
