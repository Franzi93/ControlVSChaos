using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    /// <summary>
    /// Logical representation of the game world
    ///
    /// Grid is orientated as x: right, y: up
    /// </summary>
    public class GameGrid
    {
        private readonly GameCell[,] cells;
        private readonly int width;
        private readonly int height;

        public int Width => width;
        public int Height => height;

        public GameGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new GameCell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cells[i, j] = new GameCell();
                }
            }
        }
        
        public GameGrid(int width, int height, int[,] content)
        {
            this.width = width;
            this.height = height;
            cells = new GameCell[width, height];
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y].type = (ECellType) content[x, y];
                }
            }
        }

        public void SetCell(int x, int y, GameCell cell)
        {
            if (IsValidCellPosition(x, y))
            {
                cells[x, y] = cell;
            }
        }

        public GameCell GetCell(Vector2Int pos)
        {
            return GetCell(pos.x, pos.y);
        }

        public GameCell GetCell(int x, int y)
        {
            if (IsValidCellPosition(x, y))
            {
                return cells[x, y];
            }

            Debug.LogWarning("The cell you try to get is invalid. x:" + x + " y:" + y);

            return null;
        }

        /// <summary>
        /// Returns cells with radius 1, starting clockwise from North
        /// </summary>
        /// <param name="x">x position of center cell</param>
        /// <param name="y">y position of center cell</param>
        /// <returns></returns>
        public List<GameCell> GetAllSurroundingCells(int x, int y/*, int radius = 1*/)
        {
            List<GameCell> surroundingCells = new List<GameCell>();

            // N
            GameCell cachedCell = GetCell(x, y + 1);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }
            
            // NE
            cachedCell = GetCell(x + 1, y + 1);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }
            
            // E
            cachedCell = GetCell(x + 1, y);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }
            
            // SE
            cachedCell = GetCell(x + 1, y - 1);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }
            
            // S
            cachedCell = GetCell(x, y - 1);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }

            // SW
            cachedCell = GetCell(x - 1, y - 1);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }

            // W
            cachedCell = GetCell(x - 1, y);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }

            // NW
            cachedCell = GetCell(x - 1, y + 1);
            if (cachedCell != null)
            {
                surroundingCells.Add(cachedCell);
            }

            return surroundingCells;
        }

        public Vector2Int GetDirVector(EDirection direction)
        {
            Vector2Int dirV = Vector2Int.zero;
            switch (direction)
            {
                case EDirection.Down: dirV = Vector2Int.down; break;
                case EDirection.Up: dirV = Vector2Int.up; break;
                case EDirection.Right: dirV = Vector2Int.right; break;
                case EDirection.Left: dirV = Vector2Int.left; break;

            }
            return dirV;
        }

        public GameCell GetCellInDirection(int x, int y, EDirection direction, int steps)
        {
            Vector2Int dirV = GetDirVector(direction);


            return GetCell(new Vector2Int(x, y) + dirV * steps);
        }

    public Vector2Int GetCoordsInDirection(int x, int y, EDirection direction, int steps)
    {
            Vector2Int dirV = GetDirVector(direction);

            return (new Vector2Int(x, y) + dirV* steps);
        }

    public bool IsValidCellPosition(int x, int y)
        {
            return 0 <= x && x < width && 0 <= y && y < height;
        }
    }
}
