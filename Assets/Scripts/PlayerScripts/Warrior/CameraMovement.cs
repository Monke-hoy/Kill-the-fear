using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private Shooting shooting;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
        // � ����� ������-�� ���� ������ ����������� � ������, ������� � ��� ������
        shooting = player.GetComponent<Shooting>();
        shooting.enabled = true;
    }
    void LateUpdate()
    {
        if (cam != null)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Vector3 MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position = new Vector3(MousePosition.x, MousePosition.y, cam.transform.position.z);
            }
            else if(!Input.GetButton("Fire2"))
            {
                cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);
            }
        }
    }
}
