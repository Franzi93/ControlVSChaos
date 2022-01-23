using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class ConstantRotation : MonoBehaviour
    {

        public float xRot = 0;
        public float yRot = 0;
        public float zRot = 0;

        void Update()
        {
            transform.Rotate(xRot * Time.deltaTime, yRot * Time.deltaTime, zRot * Time.deltaTime);
        }
    }
}
