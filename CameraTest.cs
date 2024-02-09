using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public Transform Player;
    public Transform PlayerHead;

    private Vector3 offset = new Vector3(0, 1, -5);

    private void Start()
    {
        transform.position = Player.position + offset;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            transform.position = PlayerHead.position;
            
        }
        else
        {
            transform.position = PlayerHead.position + offset;
            
        }
    }
}


