using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBehaviour : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public Transform leftHandHoldPoint;
    public Transform rightHandHoldPoint;

    public Rigidbody leftHandrb;


    public Camera camera;
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(camera.transform.eulerAngles.x + 10, 0, camera.transform.eulerAngles.z + 10);

        //leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, leftHandHoldPoint.position, 1);
        //rightHand.transform.position = rightHandHoldPoint.position;



        RaycastHit aimPoint;
        Physics.Raycast(transform.position, transform.forward, out aimPoint, Mathf.Infinity);

        Debug.DrawLine(transform.position, aimPoint.point);

        crosshair.transform.position = camera.WorldToScreenPoint(aimPoint.point);
    }
}
