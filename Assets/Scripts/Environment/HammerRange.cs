using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRange : MonoBehaviour
{

    private int hammer_damage;

    private void Awake()
    {
        Hammer_weapon hw = GameObject.Find("SecondArmSlot(1)").GetComponent<SecondArmSlot>().internal_object.GetComponent<FloorItem>().getItem as Hammer_weapon;
        hammer_damage = hw.GetSecondDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(hammer_damage);
        }

    }
}
