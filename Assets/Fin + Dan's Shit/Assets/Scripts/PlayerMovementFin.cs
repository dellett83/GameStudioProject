using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFin : NetworkBehaviour
{
    private float speed = 10.0f;
    private float rotationSpeed = 100.0f;
    private float jumpStrength = 5.0f; 
    private bool isGrounded = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);

        Vector3 movement = transform.forward * verticalInput * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        //Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);
        //transform.Translate(movement * speed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            //isGrounded = false;
        }
    }
}
