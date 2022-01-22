using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    // Logical representation of the game world
    public class GameGrid
    {
        private readonly GameTile[,] tiles;
        private readonly int width;
        private readonly int height;

        public GameGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            tiles = new GameTile[width, height];
        }

        public void SetTile(int xPos, int yPos, GameTile tile)
        {
            if (0 <= xPos && xPos < width && 0 <= yPos && yPos < height)
            {
                tiles[xPos, yPos] = tile;
            }
        }

        public GameTile GetTile(int xPos, int yPos)
        {
            if (0 <= xPos && xPos < width && 0 <= yPos && yPos < height)
            {
                return tiles[xPos, yPos];
            }

            return null;
        }
    }
}
