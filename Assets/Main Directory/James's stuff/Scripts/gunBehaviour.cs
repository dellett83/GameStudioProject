using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gunBehaviour : NetworkBehaviour
{
    public PlayerHUD HUDScript;

    public DitzelGames.FastIK.FastIKFabric leftHandIKScript;
    public DitzelGames.FastIK.FastIKFabric rightHandIKScript;

    [SyncVar]
    public ragdollPlayerMovement playerMovementScript;

    //[SyncVar]
    public Team teamScript;

    [SyncVar(hook = nameof(OnRagdollStateChanged))]
    public bool isRagdoll = false;
    [SyncVar]
    public bool isRagdollChanged = false;

    private Camera camera;
    private GameObject crosshair;

    public GameObject bullet;
    public GameObject shootFromHere;

    public GameObject leftHold;
    public GameObject rightHold;

    public GameObject gunTargetLocation;
    public GameObject hips;
    public GameObject rightHand;

    public AudioSource gunShot;

    public TMP_Text ammoText;

    public int maxAmmo;
    private int currentAmmo;
    public float fireRate;
    public float reloadTime;
    public float playerRecoil;

    private float reloadTimer = 0;
    private bool reloading = false;

    private float fireRateTimer = 0;
    private bool fireing;

    private Vector3[] aimPoints = new Vector3[4];
    private bool empty = true;

    private Quaternion startingRotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        //if (isLocalPlayer)
        //{
            camera = Camera.main;
            crosshair = GameObject.Find("Crosshair");

            leftHandIKScript.enabled = true;
            rightHandIKScript.enabled = true;

            currentAmmo = maxAmmo;

            startingRotation = transform.rotation;
        //}
    }

    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        // In FixedUpdate to slow down how often it changes position to make it smoother.
        // Also code so ugly it doesn't deserve to run more than 50 times a second.

        // Shoot ray from gun and put crosshair over where the ray hits
        RaycastHit aimPoint;
        Physics.Raycast(shootFromHere.transform.position, transform.forward, out aimPoint, Mathf.Infinity);

        // DO NOT LOOK AT THIS
        // Basically holds the last 4 points that were aims at and then averages out where those points were
        // and puts the corsshair there in an attempt to smooth out the movement
        if (empty)
        {
            // Makes sure array is full (crosshair doesnt work for first 4/50 of a second but flip you)
            bool found = false;
            for (int i = 0; i < aimPoints.Length; i++)
            {
                if (aimPoints[i] == null)
                {
                    aimPoints[i] = camera.WorldToScreenPoint(aimPoint.point);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                empty = false;
            }
        }
        else
        {
            // Cycles last 3 positions up getting rid of the 4th and adds the current position
            aimPoints[0] = aimPoints[1];
            aimPoints[1] = aimPoints[2];
            aimPoints[2] = aimPoints[3];
            aimPoints[3] = camera.WorldToScreenPoint(aimPoint.point);

            // Finds average position of the last 4 positions
            Vector3 averagePos = (aimPoints[0] + aimPoints[1] + aimPoints[2] + aimPoints[3]) / 4;
            crosshair.transform.position = averagePos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (isServer && !fireing && !isRagdoll && !reloading && currentAmmo > 0 && Input.GetMouseButton(0))
        //{
        //    GameObject spawnBullet = Instantiate(NetworkManager.singleton.spawnPrefabs[0], shootFromHere.transform.position, transform.rotation);
        //    NetworkServer.Spawn(spawnBullet);
        //}

        if (!isLocalPlayer)
        {
            return;
        }

        HUDScript.ammoCount = currentAmmo;

        if (isRagdoll != playerMovementScript.ragdoll) // Update our ragdoll state if it's not the same as the players
        {
            isRagdollChanged = true;
            CmdSetRagdollState(playerMovementScript.ragdoll);
        }

        if (isRagdollChanged && isRagdoll) // Wasn't a ragdoll and now is
        {
            // Reset "just changed"
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
            if (Input.GetMouseButton(1))
            {
                transform.rotation = Quaternion.Euler(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
            }
            else
            {
                //// Make gun aim vertically based on camera horizontal rotation
                transform.localRotation = Quaternion.Euler(camera.transform.eulerAngles.x - 90, hips.transform.rotation.y + 90, hips.transform.rotation.z);
            }
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

        

        // Shoots bullet if is able to shoot
        if (!fireing && !isRagdoll && !reloading && currentAmmo > 0 && Input.GetMouseButton(0))
        {
            gunShot.Play();
            currentAmmo--;
            // Creates a bullet pointing in the correct direciton
            //GameObject spawnBullet = Instantiate(bullet, shootFromHere.transform.position, transform.rotation);
            ShootBullet(shootFromHere.transform.position, transform.rotation);
            playerMovementScript.GunRecoil(playerRecoil);
            fireing = true;
        }

        // Reloads automatically and manually
        if (currentAmmo <= 0 || Input.GetKeyDown("r"))
        {
            reloading = true;
        }
    }

    [Command]
    void CmdSetRagdollState(bool ragdollState)
    {
        isRagdoll = ragdollState;
    }

    [Command]
    void ShootBullet(Vector3 position, Quaternion rotation)
    {
        if (!NetworkServer.active)
        {
            Debug.LogError("Server not active. Cannot spawn bullet.");
            //return;
        }

        GameObject spawnBullet = Instantiate(NetworkManager.singleton.spawnPrefabs[0], position, rotation);
        bulletBehaviour bulletsTeamScript = spawnBullet.GetComponent<bulletBehaviour>();
        bulletsTeamScript.teamID = teamScript.teamID;
        NetworkServer.Spawn(spawnBullet);
    }

    void OnRagdollStateChanged(bool oldState, bool newState)
    {
        UpdateIKScripts(newState);
    }

    void UpdateIKScripts(bool ragdollState)
    {
        leftHandIKScript.enabled = !ragdollState;
        rightHandIKScript.enabled = !ragdollState;

        if (!ragdollState)
        {
            transform.position = gunTargetLocation.transform.position;
        }
    }
}
