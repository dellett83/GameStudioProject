using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    string XAxisInputName, YAxisInputName;

   
    void Start()
    {
        XAxisInputName = freeLook.m_XAxis.m_InputAxisName;
        YAxisInputName = freeLook.m_YAxis.m_InputAxisName;
        EmptyInputAxisNames();
    }

   
    void Update()
    {
        //when right button is pressed, input axis name is changed.
        if(Input.GetMouseButton(1))
        {
            freeLook.m_XAxis.m_InputAxisName = XAxisInputName;
            freeLook.m_YAxis.m_InputAxisName = YAxisInputName;
        }
        else
        {
            EmptyInputAxisNames();
        }
    }

    //makes the axis name null and the axis value 0 meaning it will not move with regular mouse movement
    void EmptyInputAxisNames()
    {
        freeLook.m_XAxis.m_InputAxisName = "";
        freeLook.m_YAxis.m_InputAxisName = "";
        freeLook.m_XAxis.m_InputAxisValue = 0f;
        freeLook.m_YAxis.m_InputAxisValue = 0f;
    }

}
