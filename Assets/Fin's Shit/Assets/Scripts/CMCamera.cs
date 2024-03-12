using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class CMCamera : NetworkBehaviour
{

    //[SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            //freeLook = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
            //freeLook.Follow = this.gameObject.transform;
            //freeLook.LookAt = this.gameObject.transform;

            virtualCamera = CinemachineVirtualCamera.FindObjectOfType<CinemachineVirtualCamera>();
            virtualCamera.Follow = this.gameObject.transform;
            virtualCamera.LookAt = this.gameObject.transform;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
