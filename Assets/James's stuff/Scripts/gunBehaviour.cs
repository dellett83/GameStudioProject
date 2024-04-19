using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBehaviour : MonoBehaviour
{
    public DitzelGames.FastIK.FastIKFabric leftHandIKScript;
    public DitzelGames.FastIK.FastIKFabric rightHandIKScript;

    public ragdollPlayerMovement playerMovementScript;
    private bool isRagdoll = false;
    private bool isRagdollChanged = false;

    Camera camera;
    private GameObject crosshair;

    public GameObject bullet;
    public GameObject shootFromHere;

    public GameObject leftHold;
    public GameObject rightHold;

    public GameObject gunTargetLocation;
    public GameObject hips;
    public GameObject rightHand;

    public float handHoldForce = 100f;

    public float maxAmmo;
    private float currentAmmo;
    public float fireRate;
    public float reloadTime;

    private float reloadTimer = 0;
    private bool reloading = false;

    private float fireRateTimer = 0;
    private bool fireing;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        crosshair = GameObject.Find("Crosshair");

        leftHandIKScript.enabled = true;
        rightHandIKScript.enabled = true;

        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRagdoll != playerMovementScript.ragdoll) // Update our ragdoll state if it's not the same as the players
        {
            isRagdollChanged = true;
            isRagdoll = playerMovementScript.ragdoll;
        }

        if (isRagdollChanged && isRagdoll) // Wasn't a ragdoll and now is
        {
            // reset "just changed"
            isRagdollChanged = false;

            // Hands don't hold gun
            leftHandIKScript.enabled = false;
            rightHandIKScript.enabled = false;
        }
        else if (isRagdollChanged && !isRagdoll) // Was a ragdoll and now isn't
        {
            isRagdollChanged = false;

            // Hands hold gun
            leftHandIKScript.enabled = true;
            rightHandIKScript.enabled = true;

            // Put gun back where we want it to be
            transform.position = gunTargetLocation.transform.position;
        }

        if (!isRagdoll)
        {
            // Make gun aim vertically based on camera horizontal rotation
            transform.localRotation = Quaternion.Euler(camera.transform.eulerAngles.x, 0, 0);
        }
        else
        {
            // Gun moves with players right hand when ragdolled
            transform.position = rightHand.transform.position;
            transform.rotation = Quaternion.Euler(-rightHand.transform.eulerAngles);
        }

        // Increases reload timer when reloading
        if (reloading)
        {
            reloadTimer += Time.deltaTime;
            
            // Resets timer when enough time has passed and fills ammo
            if (reloadTimer > reloadTime)
            {
                reloading = false;
                reloadTimer = 0;
                currentAmmo = maxAmmo;
            }
        }

        // Increase fire timer when fireing (to do with fire rate of gun)
        if (fireing)
        {
            fireRateTimer += Time.deltaTime;

            // Resets timer when enough time has passed allowing player to shoot
            if (fireRateTimer > fireRate)
            {
                fireing = false;
                fireRateTimer = 0;
            }
        }

        // Shoot ray from gun and put crosshair over where the ray hits
        RaycastHit aimPoint;
        Physics.Raycast(transform.position, transform.forward, out aimPoint, Mathf.Infinity);

        crosshair.transform.position = camera.WorldToScreenPoint(aimPoint.point);

        // Shoots bullet if is able to shoot
        if (!fireing && !isRagdoll && !reloading && currentAmmo > 0 && Input.GetMouseButton(0))
        {
            currentAmmo--;
            // Creates a bullet pointing in the correct direciton
            Instantiate(bullet, shootFromHere.transform.position, transform.rotation);
            fireing = true;
        }

        // Reloads automatically and manually
        if (currentAmmo <= 0 || Input.GetKeyDown("r"))
        {
            reloading = true;
        }
    }
}
