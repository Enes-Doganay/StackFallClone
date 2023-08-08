using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 cameraPos;
    Transform player, win;
    float camOffset = 4f;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if(win == null)
        {
            win = GameObject.Find("win(Clone)").GetComponent<Transform>();
        }
        if(transform.position.y > player.position.y && transform.position.y > win.position.y + camOffset)
        {
            cameraPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x, cameraPos.y, transform.position.z);
        }
    }
}