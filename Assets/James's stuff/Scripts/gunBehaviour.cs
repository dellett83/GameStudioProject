using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBehaviour : MonoBehaviour
{
    public Camera camera;
    public GameObject crosshair;

    public GameObject bullet;
    public GameObject shootFromHere;

    public GameObject leftHold;
    public GameObject rightHold;

    public Rigidbody leftHandRB;
    public Rigidbody rightHandRB;

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
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
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

        // Make gun aim horizontally based on camera horizontal rotation
        transform.localRotation = Quaternion.Euler(camera.transform.eulerAngles.x + 10, 0, camera.transform.eulerAngles.z + 10);

        // Shoot ray from gun and put crosshair over where the ray hits
        RaycastHit aimPoint;
        Physics.Raycast(transform.position, transform.forward, out aimPoint, Mathf.Infinity);

        crosshair.transform.position = camera.WorldToScreenPoint(aimPoint.point);

        // Shoots bullet is able to shoot
        if (!reloading && currentAmmo > 0 && Input.GetMouseButtonDown(0))
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
