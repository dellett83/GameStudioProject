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
    public float takeStepDistance = 0.5f;

    public float lerpRate = 0.5f;

    private Vector3 leftTargetTarget;
    private Vector3 rightTargetTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        leftFootIKScript.enabled = false;
        rightFootIKScript.enabled = false;

        // If we're a ragdoll, let feet be a ragdoll and end script there
        if (playerMovementScript.ragdoll)
        {
            return;
        }


        RaycastHit highestGroundl = ReturnHighestGround(leftUpLeg);

        if (leftUpLeg.transform.position.y - highestGroundl.point.y < detectFloorHeight)
        {
            Debug.DrawLine(leftUpLeg.transform.position, highestGroundl.point);

            leftFootIKScript.enabled = true;

            leftTarget.transform.position = Vector3.Lerp(leftTarget.transform.position, leftTargetTarget, lerpRate);


            float distance = (leftTarget.transform.position - highestGroundl.point).magnitude;

            if (distance > takeStepDistance)
            {
                Vector3 direction = (leftTarget.transform.position - highestGroundl.point).normalized;

                Vector3 oppositeDirection = -direction;

                Vector3 newPos = (oppositeDirection * takeStepDistance) + highestGroundl.point;

                leftTargetTarget = newPos;
            }
        }


        RaycastHit highestGroundr = ReturnHighestGround(rightUpLeg);

        if (rightUpLeg.transform.position.y - highestGroundr.point.y < detectFloorHeight)
        {
            Debug.DrawLine(rightUpLeg.transform.position, highestGroundr.point);

            rightFootIKScript.enabled = true;

            rightTarget.transform.position = Vector3.Lerp(rightTarget.transform.position, rightTargetTarget, lerpRate);


            float distance = (rightTarget.transform.position - highestGroundr.point).magnitude;

            if (distance > takeStepDistance)
            {
                Vector3 direction = (rightTarget.transform.position - highestGroundr.point).normalized;

                Vector3 oppositeDirection = -direction;

                Vector3 newPos = (oppositeDirection * takeStepDistance) + highestGroundr.point;

                rightTargetTarget = newPos;
            }
        }
    }

    RaycastHit ReturnHighestGround(GameObject fromHere)
    {
        RaycastHit[] hits;
        Ray ray = new Ray(fromHere.transform.position, -Vector3.up);
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
}
