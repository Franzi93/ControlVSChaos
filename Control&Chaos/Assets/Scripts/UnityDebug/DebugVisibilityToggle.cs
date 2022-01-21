using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dmdrn.UnityDebug
{
    public class DebugVisibilityToggle : MonoBehaviour
    {
        public bool showOnAwake;

        private DebugController.Action showHideAction;

        void Start()
        {
            gameObject.SetActive(showOnAwake);

            showHideAction = DebugController.instance.AddAction(
                string.Format("Show/Hide Object \"{0}\"", name),
                () => gameObject.SetActive(!gameObject.activeSelf)
            );
        }


        void OnDestroy()
        {
            if (showHideAction != null)
            {
                DebugController.instance.Remove(showHideAction);
            }
        }
    }
}
