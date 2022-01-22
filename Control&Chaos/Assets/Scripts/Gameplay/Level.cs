using Duality;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private RenderGrid renderGrid;
        [SerializeField] private GameGrid gameGrid;

        public Vector2Int goalPos;
        public Spawn[] spawns;

        private List<MoveableFigure> spawnedObjects;

        [System.Serializable]
        public class Spawn
        {
            public Vector2Int pos;
            public GameObject obj;
            public ECharacterType type;
            public EEnemyType enemyType;
        }

        public void Cleanup()
        {
            foreach (MoveableFigure obj in spawnedObjects)
            {
                Destroy(obj.gameObject);
            }
        }

        public void Setup()
        {
            spawnedObjects = new List<MoveableFigure>();
            
            // TODO: Setup renderGrid with actual level content

            renderGrid.Setup();

            // render grid here or in game controller?
            foreach (Spawn spawn in spawns)
            {
                //get grid position
                Vector3 pos = renderGrid.GetRenderPositionFromCellPosition(spawn.pos.x, spawn.pos.y);
                MoveableFigure figure = Instantiate(spawn.obj, pos, Quaternion.identity, transform).GetComponent<MoveableFigure>();
                spawnedObjects.Add(figure);
                figure.enemyType = spawn.enemyType;
                figure.type = spawn.type;
                figure.gameGrid = gameGrid;
                figure.gridCoord = spawn.pos;
            }
            
            //place goal
        }

        public List<MoveableFigure> GetAllEnemysOfType(EEnemyType type)
        {
            List<MoveableFigure> figures = new List<MoveableFigure>(); 
            foreach (MoveableFigure obj in spawnedObjects)
            {
                if (obj.enemyType.Equals(type))
                {
                    figures.Add(obj);
                }
            }
            return figures;
        }
        public MoveableFigure GetPlayer()
        {
            foreach (MoveableFigure obj in spawnedObjects)
            {
                if (obj.type.Equals(ECharacterType.Player))
                {
                    return obj;
                }
            }
            return null;
        }

        public void ExecuteCard(Card card)
        {
            card.Execute(GetPlayer(), GetAllEnemysOfType(card.GetEnemyType()));
        }
    }
}
