using UnityEngine;

namespace Cinemachine.Examples
{
    public class ActivateOnKeypress : MonoBehaviour
    {
        public int PriorityBoostAmount = 10;

        Cinemachine.CinemachineVirtualCameraBase vcam;
        bool boosted = false;

        void Start()
        {
            vcam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
        }

        void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (vcam != null)
            {
                if (Input.GetMouseButton(1))
                {
                    if (!boosted)
                    {
                        vcam.Priority += PriorityBoostAmount;
                        boosted = true;
                    }
                }
                else if (boosted)
                {
                    vcam.Priority -= PriorityBoostAmount;
                    boosted = false;
                }
            }
#else
            InputSystemHelper.EnableBackendsWarningMessage();
#endif
        }
    }
}