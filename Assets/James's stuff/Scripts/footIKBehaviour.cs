using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footIKBehaviour : MonoBehaviour
{
    public GameObject leftTarget;
    public GameObject rightTarget;

    public GameObject leftLerpFirst;
    public GameObject rightLerpFirst;

    private bool leftLerpingToFinal = false;
    private bool rightLerpingToFinal = false;

    public GameObject leftUpLeg;
    public GameObject rightUpLeg;

    public GameObject findHighestGround;

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

            float distance = (leftTarget.transform.position - highestGroundl.point).magnitude;

            if (distance > takeStepDistance)
            {
                Vector3 direction = (leftTarget.transform.position - highestGroundl.point).normalized;

                Vector3 oppositeDirection = -direction;

                Vector3 newPos = (oppositeDirection * takeStepDistance) + highestGroundl.point;

                findHighestGround.transform.position = newPos + new Vector3(0, 1, 0);

                RaycastHit highestPoint = ReturnHighestPoint(findHighestGround);

                leftTargetTarget = highestPoint.point;

                leftLerpingToFinal = false;
            }

            if (!leftLerpingToFinal)
            {
                leftTarget.transform.position = Vector3.Lerp(leftTarget.transform.position, leftLerpFirst.transform.position, lerpRate);

                float distanceToFirst = (leftTarget.transform.position - leftLerpFirst.transform.position).magnitude;
                if (distanceToFirst < 0.1) leftLerpingToFinal = true;
            }
            else
            {
                leftTarget.transform.position = Vector3.Lerp(leftTarget.transform.position, leftTargetTarget, lerpRate);
            }
            
        }


        RaycastHit highestGroundr = ReturnHighestGround(rightUpLeg);

        if (rightUpLeg.transform.position.y - highestGroundr.point.y < detectFloorHeight)
        {
            Debug.DrawLine(rightUpLeg.transform.position, highestGroundr.point);

            rightFootIKScript.enabled = true;

            float distance = (rightTarget.transform.position - highestGroundr.point).magnitude;

            if (distance > takeStepDistance)
            {
                Vector3 direction = (rightTarget.transform.position - highestGroundr.point).normalized;

                Vector3 oppositeDirection = -direction;

                Vector3 newPos = (oppositeDirection * takeStepDistance) + highestGroundr.point;

                findHighestGround.transform.position = newPos + new Vector3(0, 1, 0);

                RaycastHit highestPoint = ReturnHighestPoint(findHighestGround);

                rightTargetTarget = highestPoint.point;

                rightLerpingToFinal = false;
            }

            if (!rightLerpingToFinal)
            {
                rightTarget.transform.position = Vector3.Lerp(rightTarget.transform.position, rightLerpFirst.transform.position, lerpRate);

                float distanceToFirst = (rightTarget.transform.position - rightLerpFirst.transform.position).magnitude;
                if (distanceToFirst < 0.1) rightLerpingToFinal = true;
            }
            else
            {
                rightTarget.transform.position = Vector3.Lerp(rightTarget.transform.position, rightTargetTarget, lerpRate);
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

    RaycastHit ReturnHighestPoint(GameObject fromHere)
    {
        RaycastHit[] hits;
        Ray ray = new Ray(fromHere.transform.position, -Vector3.up);
        hits = Physics.RaycastAll(ray);
        float highest = -10000f;
        RaycastHit floatPoint = new RaycastHit();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].point.y > highest)
            {
                highest = hits[i].point.y;
                floatPoint = hits[i];
            }
        }

        return floatPoint;
    }
}
