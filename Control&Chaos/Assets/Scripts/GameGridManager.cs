using UnityEngine;
using UnityEngine.Tilemaps;

namespace Duality
{
    public class GameGridManager : MonoBehaviour
    {
        // Manages the connection between logic (GameGrid) and rendering (Tilemap)

        [SerializeField] private Tilemap tilemap;

        public GameObject testPlayer;

        private GameGrid gameGrid;
        private Vector2Int playerPosition = new Vector2Int(0, 0);

        private void Awake()
        {
            gameGrid = new GameGrid(20, 20);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MovePlayerTo(playerPosition.x + 1, playerPosition.y);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MovePlayerTo(playerPosition.x - 1, playerPosition.y);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MovePlayerTo(playerPosition.x, playerPosition.y + 1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MovePlayerTo(playerPosition.x, playerPosition.y - 1);
            }
        }

        public void MovePlayerTo(Vector2Int pos)
        {
            MovePlayerTo(pos.x, pos.y);
        }

        public void MovePlayerTo(int xPos, int yPos)
        {
            // Remove from old tile
            // Add to new tile
            // Move the visual representation

            Vector3Int cellPosition = new Vector3Int(xPos, yPos, 0);
            // TileBase tile = tilemap.GetTile(cellPosition);
            // tile.GetTileData();
                
            testPlayer.transform.position = tilemap.CellToWorld(cellPosition)  + tilemap.GetLayoutCellCenter();

            playerPosition = new Vector2Int(xPos, yPos);

            // Vector2 newVisualPosition = default;
            //
            // testPlayer.transform.position = newVisualPosition;
        }

        public void MovePlayerToSpawn()
        {
            // tilemap.getTiles
        }
    }
}