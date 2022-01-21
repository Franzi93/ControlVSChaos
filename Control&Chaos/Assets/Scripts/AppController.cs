using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dmdrn.UnityDebug;

public class AppController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DebugController.instance.AddAction("test", () => { Debug.Log("test"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
