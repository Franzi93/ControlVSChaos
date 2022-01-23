using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dmdrn.UnityDebug;

public class InputSystem 
{
    private static int locks;
    public delegate void InputActivated(bool active);
    public static event InputActivated onLockChanged;
    public static bool isFree => locks == 0;

   public static void Lock()
   {
        Log.Message("Locked");


       if (locks == 0)
       {
           onLockChanged?.Invoke(false);
       }
       locks++;

   }

   public static void Lock(float duration, MonoBehaviour routineHolder)
   {
       Lock();
       routineHolder.StartCoroutine(Unlock(duration));
   }

    private static IEnumerator Unlock(float duration)
    {
        yield return new WaitForSeconds(duration);
        Unlock();
    }

   public static void Unlock()
   {
       Log.Message("Unlocked");
       if(locks == 0)
       {
           Debug.LogWarning("InputSystem: You try to unlock input though its not even locked!");
       }
    
       locks--;
       if(locks == 0)
       {
           onLockChanged?.Invoke(true);
       }
   }
   #region  PlattformDependent
    private static InputPlattform plattform;

    private static InputPlattform inputPlattform
    {get{
        if(plattform == null)
        {
    #if UNITY_EDITOR
            plattform = new InputPlattformEditor();
    #else
            plattform = new InputPlattformMobile();
    #endif

        }
        return plattform;
    }
    }

    public static Vector3 clickPosition
    {
        get { return inputPlattform.ClickPosition(); }
    }

    public static bool GetClickDown()
    {
        return inputPlattform.GetClickDown();
    }
    public static bool GetClick()
    {
        return inputPlattform.GetClick();
    }
    public static bool GetClickUp()
    {
        return inputPlattform.GetClickUp();

    }



}

public interface InputPlattform
{
     Vector3 ClickPosition();
     bool GetClickDown();
     bool GetClick();
     bool GetClickUp();

}
public class InputPlattformEditor : InputPlattform
{
    public Vector3 ClickPosition()
    {
        return Input.mousePosition; 
    }

    public bool GetClickDown()
    {
        return InputSystem.isFree && Input.GetMouseButtonDown(0);
    }
    public bool GetClick()
    {
        return InputSystem.isFree && Input.GetMouseButton(0);
    }
    public bool GetClickUp()
    {
        return InputSystem.isFree && Input.GetMouseButtonUp(0);
    }

}
public class InputPlattformMobile : InputPlattform
{
    public Vector3 ClickPosition()
    {
        if (Input.touches.Length < 1) return Vector2.zero;
        return Input.touches[0].position;
    } 

    public bool GetClickDown()
    {
        return InputSystem.isFree && Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began;
    }
    public bool GetClick()
    {
        return InputSystem.isFree && Input.touchCount>0 && (Input.touches[0].phase == TouchPhase.Moved||Input.touches[0].phase == TouchPhase.Stationary);
    }
    public bool GetClickUp()
    {
        return InputSystem.isFree && Input.touchCount>0 && (Input.touches[0].phase == TouchPhase.Canceled||Input.touches[0].phase == TouchPhase.Ended);
    }
    
}
   #endregion
