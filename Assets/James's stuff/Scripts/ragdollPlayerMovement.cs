using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollPlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public new GameObject camera;
    public GameObject hips;
    public Rigidbody headRb;

    // Can make most private just public for initial development
    private bool canJump = true;
    private float jumpTimer = 0f;

    public float headFloatingForce = 10f;
    public float floatingHeight = 5f;
    public float floatingForce = 1f;
    public float floatForceDampener = 1f;
    public float jumpForce = 10f;
    public float movementAcceleration = 100f;
    public float movementSpeedLimit = 10f;
    public float stoppingForce = 10f;
    public float diveForce = 10f;
    public float ragdollTimerEnd = 3f;
    public float ragdollTimer = 0f;
    public bool ragdollTimerStart = false;
    public bool ragdoll = false;
    public float rotationForce = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Limit to how long can be in ragdoll mode
        if (ragdollTimerStart) ragdollTimer += Time.deltaTime;

        // Turn ragdoll mode off and reset variables
        if (ragdollTimer >= ragdollTimerEnd)
        {
            ragdoll = false;
            ragdollTimerStart = false;
            ragdollTimer = 0f;
        }

        // For testing, enter/exit ragdoll with left control
        if (Input.GetKeyDown("left ctrl"))
        {
            ragdoll = !ragdoll;
        }

        // Stop before we do code that allows ragdoll to be controlled
        if (ragdoll) return;

        // Dive mechanic, launch player forward with left shift
        if(Input.GetKeyDown("left shift"))
        {
            ragdoll = true;
            rb.AddForce((camera.transform.forward + new Vector3(0.0f, 1.0f, 0.0f)) * diveForce, ForceMode.Impulse);
            ragdollTimerStart = true;
            return;
        }

        // Jump cooldown started
        if(!canJump)
        {
            jumpTimer += Time.deltaTime;
        }

        // Without cooldown you can double jump
        if(jumpTimer < 0.5f)
        {
            canJump = false;
            jumpTimer = 0f;
        }

        // Always face direciton of camera
        float rotationDifference = camera.transform.eulerAngles.y - hips.transform.eulerAngles.y;

        if (rotationDifference > 180f)
        {
            rotationDifference -= 360f;
        }
        else if (rotationDifference < -180f)
        {
            rotationDifference += 360f;
        }

        float torqueStrength = rotationDifference * rotationForce;

        rb.AddTorque(Vector3.up * torqueStrength * Time.deltaTime);

        // Calculate correct movement direciton based on camera angle and player input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 forwardRelativeVertical = verticalInput * cameraForward;
        Vector3 rightRelativeHorizontal = horizontalInput * cameraRight;

        Vector3 cameraRelativeMovement = forwardRelativeVertical + rightRelativeHorizontal;


        // If no input, apply force in opposite direction to movement to stop moving
        if (cameraRelativeMovement == new Vector3(0f, 0f, 0f))
        {
            Vector3 normalizedVel = rb.velocity.normalized;
            normalizedVel.y = 0f;
            rb.AddForce(-normalizedVel * stoppingForce * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(cameraRelativeMovement * movementAcceleration * Time.deltaTime, ForceMode.Impulse);
        }

        // Velocity in only horizontal direction
        Vector3 nonVertVel = new Vector3 (rb.velocity.x, 0f, rb.velocity.z);

        // Speed in only horizontal is faster than speed limit
        if (nonVertVel.magnitude >= movementSpeedLimit)
        {
            // Limit speed but not in y
            float currentYVel = rb.velocity.y;

            Vector3 newVelocity = rb.velocity.normalized * movementSpeedLimit;

            newVelocity.y = currentYVel;

            rb.velocity = newVelocity;
        }


        // Check if they're within floating height of the ground
        RaycastHit[] hits;
        Ray ray = new Ray(hips.transform.position, -Vector3.up);
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.CompareTag("Ground") && hips.transform.position.y - hits[i].point.y < floatingHeight)
            {

                Debug.DrawLine(hips.transform.position, hits[i].point);
                // Apply correct force to make it float nicely (idk how it works)
                CharacterFloat(hits[i]);

                // Make it so player can jump if they touch the ground
                canJump = true;
            }
        }

        // Apply force upwards to the head
        headRb.AddForce(Vector3.up * headFloatingForce * Time.deltaTime);

        // Jump if press space
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            // Apply force upwards to jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            // Can't jump until touch ground again
            canJump = false;
        }
    }

    void CharacterFloat(RaycastHit hit)
    {
        Vector3 vel = rb.velocity;
        Vector3 rayDir = transform.TransformDirection(-Vector3.up);

        Vector3 otherVel = Vector3.zero;

        float rayDirVel = Vector3.Dot(rayDir, vel);
        float otherDirVel = Vector3.Dot(rayDir, otherVel);
        float relVel = rayDirVel - otherDirVel;
        float x = hit.distance - floatingHeight;
        float springForce = (x * floatingForce) - (relVel * floatForceDampener);

        rb.AddForce(rayDir * springForce * Time.deltaTime);
    }
}
