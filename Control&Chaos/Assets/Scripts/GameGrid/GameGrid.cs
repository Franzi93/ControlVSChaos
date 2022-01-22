using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    /// <summary>
    /// Logical representation of the game world
    /// </summary>
    public class GameGrid
    {
        private readonly GameCell[,] cells;
        private readonly int width;
        private readonly int height;

        public GameGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new GameCell[width, height];
        }

        public void SetCell(int xPos, int yPos, GameCell cell)
        {
            if (0 <= xPos && xPos < width && 0 <= yPos && yPos < height)
            {
                cells[xPos, yPos] = cell;
            }
        }

        public GameCell GetCell(int xPos, int yPos)
        {
            if (0 <= xPos && xPos < width && 0 <= yPos && yPos < height)
            {
                return cells[xPos, yPos];
            }

            return null;
        }

        public List<GameCell> GetAllSuroundingCells(int x, int y, int radius = 1)
        {
            return null;
        }

        public GameCell GetCellInDirection(int x, int y, EDirection direction, int steps)
        {
            return null;
        }
    }
}
