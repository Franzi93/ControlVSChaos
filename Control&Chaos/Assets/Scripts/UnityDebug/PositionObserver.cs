using Dmdrn.UnityDebug;
using UnityEngine;

public class PositionObserver : MonoBehaviour
{
        
    void OnEnable()
    {
        DebugController.instance.WatchValue(name, () => GetPosition());
    }

    void OnDisable()
    {
        DebugController.instance.UnwatchValue(name);
    }

    private string GetPosition()
    {
        Vector3 position = gameObject.transform.parent.position;
        return string.Format("({0} , {1} , {2})", position.x.ToString("F3"), position.y.ToString("F3"), position.z.ToString("F3"));
    }
}