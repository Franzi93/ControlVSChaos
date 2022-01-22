using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class EnemyFigure : MoveableFigure
    {
        public override void MoveTo(EDirection direction)
        {
            gameGrid.GetCellInDirection(gridCoord.x, gridCoord.y, direction, 1);
        }
    }
}
