using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBehaviour : MonoBehaviour
{
    public Camera camera;
    public GameObject crosshair;

    public GameObject bullet;
    public GameObject shootFromHere;

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
        if (reloading)
        {
            reloadTimer += Time.deltaTime;
            
            if (reloadTimer > reloadTime)
            {
                reloading = false;
                reloadTimer = 0;
                currentAmmo = maxAmmo;
            }
        }

        transform.localRotation = Quaternion.Euler(camera.transform.eulerAngles.x + 10, 0, camera.transform.eulerAngles.z + 10);

        RaycastHit aimPoint;
        Physics.Raycast(transform.position, transform.forward, out aimPoint, Mathf.Infinity);

        crosshair.transform.position = camera.WorldToScreenPoint(aimPoint.point);

        if (!reloading && currentAmmo > 0 && Input.GetMouseButtonDown(0))
        {
            currentAmmo--;
            Instantiate(bullet, shootFromHere.transform.position, transform.rotation);
        }

        if (currentAmmo <= 0 || Input.GetKeyDown("r"))
        {
            reloading = true;
        }
    }
}
