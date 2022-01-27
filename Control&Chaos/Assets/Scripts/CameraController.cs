using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class CameraController : MonoBehaviour
    {
        public enum CameraPerspective {INGAME, MENU}

        public Animator cameraAnimator;
        
        public void SwitchCameraPerspective(CameraPerspective cameraPerspective)
        {
            switch (cameraPerspective)
            {
                case CameraPerspective.INGAME:
                    cameraAnimator.SetTrigger("Game");
                    break;
                case CameraPerspective.MENU:
                    cameraAnimator.ResetTrigger("Game");
                    break;
            }

        }


    }
}
