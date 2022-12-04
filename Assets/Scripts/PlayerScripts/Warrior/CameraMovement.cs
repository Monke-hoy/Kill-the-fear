using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
