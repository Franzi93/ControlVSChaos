using Duality;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private RenderGrid renderGrid;
        
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
            
            // TODO: Setup renderGrid with actual level content

            renderGrid.Setup();

            // render grid here or in game controller?
            foreach (Spawn spawn in spawns)
            {
                //get grid position
                Vector3 pos = renderGrid.GetRenderPositionFromCellPosition(spawn.pos.x, spawn.pos.y);

                spawnedObjects.Add(Instantiate(spawn.obj, pos, Quaternion.identity, transform));
                
            }
            
            //place goal
        }
    }
}

public class Enemy
{ }