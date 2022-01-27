using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class RenderCell : MonoBehaviour
    {
        [SerializeField] private Transform characterTransform;
        
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Vector3 position = characterTransform.position;
            Vector3 lineEnd = position + Vector3.up;
            Gizmos.DrawSphere(lineEnd, 0.2f);
            Gizmos.DrawLine(position, lineEnd);
        }

        public Transform GetCharacterTransform()
        {
            return characterTransform;
        }
    }
}
