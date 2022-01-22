using Duality;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
    public class Level : MonoBehaviour
    {
        public Vector2Int goalPos;
        public EnemySpawn[] enemySpawns;

        [System.Serializable]
        public class EnemySpawn
        {
            public Vector2Int pos;
            public GameObject obj;
            public ECharacterType type;
        }

        public void Setup()
        {
            //render grid
            //place player enemys and goal
            //
        }
    }
}

public class Enemy
{ }