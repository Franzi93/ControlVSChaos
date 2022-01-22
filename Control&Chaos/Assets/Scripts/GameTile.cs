using UnityEngine;

namespace Duality
{
    public class GameTile : MonoBehaviour
    {
        // TODO: Add list of objects on this tile
        // TODO: Add additional information if needed

        public bool hasPlayer = false;

        public EGameTile type = EGameTile.Empty;

        public Transform characterPosition;
    }
    
}