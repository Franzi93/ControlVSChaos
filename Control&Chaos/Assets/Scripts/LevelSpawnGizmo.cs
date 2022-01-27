using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class LevelSpawnGizmo : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, new Vector3(10, 1, 10));
        }
    }
}
