using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using UnityEngine.UIElements;

public class ragdollPlayerMovement : NetworkBehaviour //MonoBehaviour
{
    public Rigidbody hipsRB;
    public Camera camera;
    public Rigidbody headRB;
    public GameObject spine;

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
    private float ragdollTimer = 0f;
    private bool ragdollTimerStart = false;
    public bool ragdoll = false;
    public float rotationForce = 100f;

    // Start is called before the first frame update
    void Start()
    {
        //if (isLocalPlayer)
        //{

            camera = Camera.main;

            var vcam = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
            vcam.Follow = transform;
            vcam.LookAt = GameObject.Find("mixamorig6:HeadTop_End").transform; // Change to whatever name of thing you want to look at

            var vcam2 = GameObject.Find("Scope Camera").GetComponent<CinemachineVirtualCamera>();
            vcam2.Follow = transform;
            vcam2.LookAt = GameObject.Find("mixamorig6:HeadTop_End").transform; // Change to whatever name of thing you want to look at

        //}
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if(!isLocalPlayer)
        //{
            //return;
        //}

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
        if (Input.GetKeyDown("left shift"))
        {
            ragdoll = true;
            hipsRB.AddForce((camera.transform.forward + new Vector3(0.0f, 1.0f, 0.0f)) * diveForce, ForceMode.Impulse);
            ragdollTimerStart = true;
            return;
        }

        

        // Jump cooldown started
        if (!canJump)
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
        float rotationDifference = camera.transform.eulerAngles.y - transform.eulerAngles.y;

        // Forgot why you need to do this
        if (rotationDifference > 180f)
        {
            rotationDifference -= 360f;
        }
        else if (rotationDifference < -180f)
        {
            rotationDifference += 360f;
        }

        float torqueStrength = rotationDifference * rotationForce;
        hipsRB.AddTorque(Vector3.up * torqueStrength * Time.deltaTime);

        // Calculate correct movement direciton based on camera angle and player input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0; // Don't want any movement in Y
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 forwardRelativeVertical = verticalInput * cameraForward;
        Vector3 rightRelativeHorizontal = horizontalInput * cameraRight;

        Vector3 cameraRelativeMovement = forwardRelativeVertical + rightRelativeHorizontal;


        // If no input, apply force in opposite direction to movement to stop moving (means if (for example) moving forward then user inputs right will take time for forward speed to bleed off (may change later))
        if (cameraRelativeMovement == new Vector3(0f, 0f, 0f))
        {
            Vector3 normalizedVel = hipsRB.velocity.normalized;
            normalizedVel.y = 0f;
            hipsRB.AddForce(-normalizedVel * stoppingForce * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            hipsRB.AddForce(cameraRelativeMovement * movementAcceleration * Time.deltaTime, ForceMode.Impulse);
        }

        // Velocity in only horizontal direction
        Vector3 nonVertVel = new Vector3 (hipsRB.velocity.x, 0f, hipsRB.velocity.z);

        // Speed in only horizontal is faster than speed limit
        if (nonVertVel.magnitude >= movementSpeedLimit)
        {
            // Limit speed but not in y
            float currentYVel = hipsRB.velocity.y;

            Vector3 newVelocity = hipsRB.velocity.normalized * movementSpeedLimit;

            newVelocity.y = currentYVel;

            hipsRB.velocity = newVelocity;
        }


        RaycastHit floatPoint = ReturnHighestGround();

        // If highest point is within floating distance, float
        if (transform.position.y - floatPoint.point.y < floatingHeight)
        {
            Debug.DrawLine(transform.position, floatPoint.point);
            // Apply correct force to make it float nicely (idk how it works)
            CharacterFloat(floatPoint);

            // Make it so player can jump if they touch the ground
            canJump = true;
        }

        // Apply force upwards to the head
        headRB.AddForce(Vector3.up * headFloatingForce * Time.deltaTime);

        // Jump if press space
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            // Apply force upwards to jump
            hipsRB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            // Can't jump until touch ground again
            canJump = false;
        }

        Quaternion currentRot = transform.rotation;
        transform.rotation = Quaternion.Euler(0, currentRot.eulerAngles.y, 0);

        Quaternion currentRot3 = spine.transform.rotation;
        //spine.transform.rotation = Quaternion.Euler(0, currentRot3.eulerAngles.y, 0);
    }

    void CharacterFloat(RaycastHit hit)
    {
        Vector3 vel = hipsRB.velocity;
        Vector3 rayDir = transform.TransformDirection(-Vector3.up);

        Vector3 otherVel = Vector3.zero;

        float rayDirVel = Vector3.Dot(rayDir, vel);
        float otherDirVel = Vector3.Dot(rayDir, otherVel);
        float relVel = rayDirVel - otherDirVel;
        float x = hit.distance - floatingHeight;
        float springForce = (x * floatingForce) - (relVel * floatForceDampener);

        hipsRB.AddForce(rayDir * springForce * Time.deltaTime);
    }

    RaycastHit ReturnHighestGround()
    {
        RaycastHit[] hits;
        Ray ray = new Ray(transform.position, -Vector3.up);
        hits = Physics.RaycastAll(ray);
        float highest = -10000f;
        RaycastHit floatPoint = new RaycastHit();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.CompareTag("Ground"))
            {
                if (hits[i].point.y > highest)
                {
                    highest = hits[i].point.y;
                    floatPoint = hits[i];
                }
            }
        }

        return floatPoint;
    }

    public void GunRecoil(float recoilForce)
    {

        //if (!isLocalPlayer)
        //{
            //return;
        //}


        hipsRB.AddForce(-hipsRB.transform.forward * recoilForce, ForceMode.Impulse);
    }
}
