using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footIKBehaviour : MonoBehaviour
{
    public GameObject leftTarget;
    public GameObject rightTarget;

    public GameObject leftUpLeg;
    public GameObject rightUpLeg;

    public DitzelGames.FastIK.FastIKFabric leftFootIKScript;
    public DitzelGames.FastIK.FastIKFabric rightFootIKScript;

    public ragdollPlayerMovement playerMovementScript;
    public bool isRagdoll = false;

    public float detectFloorHeight = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovementScript.ragdoll)
        {
            leftFootIKScript.enabled = false;
            rightFootIKScript.enabled = false;

            return;
        }




        RaycastHit[] hits;
        Ray ray = new Ray(leftUpLeg.transform.position, -Vector3.up);
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.CompareTag("Ground") && transform.position.y - hits[i].point.y > detectFloorHeight)
            {

                leftFootIKScript.enabled = false;
                rightFootIKScript.enabled = false;
                //return;
            }
        }

        RaycastHit[] hitsl;
        Ray rayl = new Ray(leftUpLeg.transform.position, -Vector3.up);
        hitsl = Physics.RaycastAll(rayl);
        for (int i = 0; i < hitsl.Length; i++)
        {
            if (hitsl[i].transform.gameObject.CompareTag("Ground") && leftUpLeg.transform.position.y - hitsl[i].point.y < detectFloorHeight)
            {
                Debug.DrawLine(leftUpLeg.transform.position, hitsl[i].point);

                leftFootIKScript.enabled = true;
                leftTarget.transform.position = hitsl[i].point;
            }
        }

        RaycastHit[] hitsr;
        Ray rayr = new Ray(rightUpLeg.transform.position, -Vector3.up);
        hitsr = Physics.RaycastAll(rayr);
        for (int i = 0; i < hitsr.Length; i++)
        {
            if (hitsr[i].transform.gameObject.CompareTag("Ground") && rightUpLeg.transform.position.y - hitsr[i].point.y < detectFloorHeight)
            {
                Debug.DrawLine(rightUpLeg.transform.position, hitsr[i].point);

                rightFootIKScript.enabled = true;
                rightTarget.transform.position = hitsr[i].point;
            }
        }
    }
}
