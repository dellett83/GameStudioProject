using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBehaviour : MonoBehaviour
{
    public DitzelGames.FastIK.FastIKFabric leftHandIKScript;
    public DitzelGames.FastIK.FastIKFabric rightHandIKScript;

    public ragdollPlayerMovement playerMovementScript;
    public bool isRagdoll = false;
    private bool isRagdollChanged = false;

    public Camera camera;
    public GameObject crosshair;

    public GameObject bullet;
    public GameObject shootFromHere;

    public GameObject leftHold;
    public GameObject rightHold;

    public GameObject gunTargetLocation;
    public GameObject hips;
    public GameObject rightHand;

    public float handHoldForce = 100f;

    public float maxAmmo;
    public float currentAmmo;

    public float reloadTime;
    public float reloadTimer = 0;
    public bool reloading = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        leftHandIKScript.enabled = true;
        rightHandIKScript.enabled = true;

        currentAmmo = maxAmmo;
        //transform.parent = transform.hips;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRagdoll != playerMovementScript.ragdoll)
        {
            isRagdollChanged = true;
            isRagdoll = playerMovementScript.ragdoll;
        }

        if (isRagdollChanged && isRagdoll) // Wasn't a ragdoll and now is
        {
            isRagdollChanged = false;

            leftHandIKScript.enabled = false;
            rightHandIKScript.enabled = false;
        }
        else if (isRagdollChanged && !isRagdoll) // Was a ragdoll and now isn't
        {
            isRagdollChanged = false;

            leftHandIKScript.enabled = true;
            rightHandIKScript.enabled = true;

            transform.position = gunTargetLocation.transform.position;
        }

        if (!isRagdoll)
        {
            // Make gun aim horizontally based on camera horizontal rotation
            transform.localRotation = Quaternion.Euler(camera.transform.eulerAngles.x - 10, 0, camera.transform.eulerAngles.z + 10);
        }
        else
        {
            transform.position = rightHand.transform.position;
            transform.rotation = Quaternion.Euler(-rightHand.transform.eulerAngles);
        }
        //transform.position = new Vector3(hips.transform.position.x + 0.285f, hips.transform.position.y + 0.3451f, hips.transform.position.z + 0.4258f);


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

        // Shoot ray from gun and put crosshair over where the ray hits
        RaycastHit aimPoint;
        Physics.Raycast(transform.position, transform.forward, out aimPoint, Mathf.Infinity);

        crosshair.transform.position = camera.WorldToScreenPoint(aimPoint.point);

        // Shoots bullet is able to shoot
        if (!isRagdoll && !reloading && currentAmmo > 0 && Input.GetMouseButtonDown(0))
        {
            currentAmmo--;
            // Creates a bullet pointing in the correct direciton
            Instantiate(bullet, shootFromHere.transform.position, transform.rotation);
        }

        // Reloads automatically and manually
        if (currentAmmo <= 0 || Input.GetKeyDown("r"))
        {
            reloading = true;
        }

        //Vector3 leftForceDirection = (leftHold.transform.position - leftHandRB.transform.position).normalized;
        //leftHandRB.AddForce(leftForceDirection * handHoldForce * Time.deltaTime, ForceMode.Impulse);

        //Vector3 rightForceDirection = (rightHold.transform.position - rightHandRB.transform.position).normalized;
        //rightHandRB.AddForce(rightForceDirection * handHoldForce * Time.deltaTime, ForceMode.Impulse);
    }
}
