using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class MoveableFigure : MonoBehaviour
    {
        public ECharacterType type;
        public EEnemyType enemyType;

        public Vector2Int gridCoord;

        public GameGrid gameGrid;
        public RenderGrid renderGrid; 
        
        public virtual void MoveTo(EDirection direction)
        {
            gameGrid.GetCellInDirection(gridCoord.x, gridCoord.y, direction,1);
        }

    }
}
