using UnityEngine;

namespace Dmdrn.UnityDebug
{
    public class DebugObjectScaler : MonoBehaviour
    {
        public float minScale = .01f;
        public float maxScale = 5f;

        private DebugController.TweakValue scaleValue;

        void Start()
        {
            scaleValue = DebugController.instance.AddTweakValue(
                string.Format("Object \"{0}\" Scale", name),
                minScale,
                maxScale,
                () => transform.localScale.x,
                (float value) => transform.localScale = (value * Vector3.one)
            );
        }


        void OnDestroy()
        {
            if (scaleValue != null)
            {
                DebugController.instance.Remove(scaleValue);
            }
        }
    }
}
