using System;
using Duality;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private RenderGrid renderGrid;
        [SerializeField] private int LevelNumber = 1;
        [SerializeField] private bool generateRenderGridOnSetup = false;
        private GameGrid gameGrid;

        [Header("Grid Setup Data")]
        public Vector2Int goalPos;
        public Spawn[] spawns;
        public int width = 20;
        public int height = 20;

        public GameObject goalPrefab;
        
        private List<MoveableFigure> spawnedObjects;

        public System.Action levelWon;
        public System.Action levelLost;



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

        public void Setup(System.Action won, System.Action lost)
        {
            levelLost = lost;
            levelWon = won;

            spawnedObjects = new List<MoveableFigure>();

            gameGrid = CreateGameGrid();

            if (generateRenderGridOnSetup)
            {
                renderGrid.SetupWithNewSize(width, height);
            }
            else
            {
                renderGrid.SetupFromExistingCells(width, height);
            }

            // render grid here or in game controller?
            foreach (Spawn spawn in spawns)
            {
                Vector3 pos = renderGrid.GetRenderPositionFromCellPosition(spawn.pos.x, spawn.pos.y);
                MoveableFigure figure = Instantiate(spawn.obj, pos, Quaternion.identity, transform).GetComponent<MoveableFigure>();
                spawnedObjects.Add(figure);

                figure.enemyType = spawn.enemyType;
                figure.type = spawn.type;
                figure.gameGrid = gameGrid;
                figure.renderGrid = renderGrid;
                figure.gridCoord = spawn.pos;

                figure.SetCurrentCell();
                figure.onFigureKilled += FigureKilled;
                
            }

            Vector3 goalPosWorldPosition = renderGrid.GetRenderPositionFromCellPosition(goalPos.x, goalPos.y);
            if (!gameGrid.IsValidCellPosition(goalPos.x, goalPos.y))
            {
                throw new Exception("The goal position is not on the grid");
            }
            gameGrid.GetCell(goalPos.x, goalPos.y).type = ECellType.Goal;
            Instantiate(goalPrefab, goalPosWorldPosition, Quaternion.identity, transform);


            CheckConsistencyBetweenLogicAndRendering();
        }

        private void CheckConsistencyBetweenLogicAndRendering()
        {
            int gameWidth = gameGrid.Width;
            int gameHeight = gameGrid.Height;

            int renderWidth = renderGrid.Width;
            int renderHeight = renderGrid.Height;

            if (gameWidth != renderWidth || gameHeight != renderHeight)
            {
                throw new Exception("There is an inconsistency between logic and rendering of map");
            }
        }

        public List<EEnemyType> GetAllRemainEnemyTypes()
        {
            List<EEnemyType> types = new List<EEnemyType>();
            foreach (MoveableFigure obj in spawnedObjects)
            {
                if (!types.Contains(obj.enemyType) && obj.enemyType != EEnemyType.None)
                {
                    types.Add(obj.enemyType);
                }
            }
            return types;
        }

        public List<MoveableFigure> GetAllEnemysOfType(EEnemyType type)
        {
            List<MoveableFigure> figures = new List<MoveableFigure>(); 
            foreach (MoveableFigure obj in spawnedObjects)
            {
                if (obj.type == ECharacterType.Player)
                {
                    continue;
                }
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
            InputSystem.Lock();
            card.Execute(GetPlayer(), GetAllEnemysOfType(card.GetEnemyType()),()=> 
            {
                CheckWinLoseConditions();
                InputSystem.Unlock();
            });

        }

        private void CheckWinLoseConditions()
        {
            // LOOSE: Player is dead
            // WIN: Reached goal or all enemies dead
            Debug.Log("check win loose");
            if (GetPlayer() == null || !GetPlayer().isAlive)
            {
                levelLost();
                return;
            }
            PlayerFigure player = GetPlayer().GetComponent<PlayerFigure>();
            if (GetAllRemainEnemyTypes().Count == 0 || player.reachedGoal)
            {
                levelWon();
                return;
            }
            
        }

        public void FigureKilled(MoveableFigure figure)
        {
            spawnedObjects.Remove(figure);

            figure.Die();

            Instantiate(figure.deathVFX, figure.transform.position, Quaternion.identity, transform);
        }

        private GameGrid CreateGameGrid()
        {
            return new GameGrid(width, height);
        }


#if UNITY_EDITOR
        void OnDrawGizmos()
        {

            if (generateRenderGridOnSetup)
            {
                renderGrid.SetupWithNewSize(width, height);
            }
            else
            {
                renderGrid.SetupFromExistingCells(width, height);
            }
            Vector3 goalPosWorldPosition = renderGrid.GetRenderPositionFromCellPosition(goalPos.x, goalPos.y);

            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(goalPosWorldPosition, 1);

            foreach (Spawn spawn in spawns)
            {
                if (spawn.type.Equals(ECharacterType.Player))
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Vector3 pos = renderGrid.GetRenderPositionFromCellPosition(spawn.pos.x, spawn.pos.y);
                UnityEditor.Handles.Label(pos, spawn.obj.name);
                Gizmos.DrawSphere(pos, 1);
            }
        }
#endif
    }
}
