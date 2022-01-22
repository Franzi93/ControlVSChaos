using Duality;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
    public class Level : MonoBehaviour
    {
        public Vector2Int goalPos;
        public Spawn[] spawns;

        private List<GameObject> spawnedObjects; 

        [System.Serializable]
        public class Spawn
        {
            public Vector2Int pos;
            public GameObject obj;
            public ECharacterType type;
        }

        public void Cleanup()
        {
            foreach (GameObject obj in spawnedObjects)
            {
                Destroy(obj);
            }
        }

        public void Setup()
        {
            spawnedObjects = new List<GameObject>();

            // render grid here or in game controller?
            foreach (Spawn s in spawns)
            {
                //get grid position
                Vector3 pos = Vector3.one;

                spawnedObjects.Add(Instantiate(s.obj, pos, Quaternion.identity));
                
            }
            //place goal
        }
    }
}

public class Enemy
{ }