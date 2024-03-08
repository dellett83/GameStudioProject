using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController cc;
    public float speed = 0.5f;
    GameObject cam;
    Vector3 camEulerAngles;
    float moveX;
    float moveZ;
    public Vector3 movement;
    public float jumpHeight = 3f;
    public float pushPower;
    float ySpeed;

    
    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("MainCamera");
    }

    
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        //rotates character with the camera
        camEulerAngles = cam.transform.localEulerAngles;
        camEulerAngles = new Vector3 (0, camEulerAngles.y, 0);
        transform.localEulerAngles = camEulerAngles;

        //x and z movement
        movement = transform.forward * moveZ;
        movement += transform.right * moveX;

        //applies a jump to the y velocity
        if(cc.isGrounded)
        {
            ySpeed = 0;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                ySpeed = jumpHeight;
            }
        }

        //applies gravity
        ySpeed += Physics.gravity.y * Time.deltaTime;
        movement.y = ySpeed;

        cc.Move(movement * speed * Time.deltaTime);

    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //if its null do nothing
        if(body == null || body.isKinematic)
        {
            return;
        }
        //dont want to move objects that are below us
        if(hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower;

    }


}
