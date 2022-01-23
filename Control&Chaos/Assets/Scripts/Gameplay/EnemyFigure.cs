using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class EnemyFigure : MoveableFigure
    {
        public override void MoveTo(EDirection direction)
        {
            GameCell oldCell = GetCurrentCell();

            if (MoveToDirection(direction))
            {
                GameCell newCell = GetCurrentCell();

                if (newCell.figure && newCell.figure.type == ECharacterType.Player)
                {
                    KilledFigure(newCell.figure);
                }

                oldCell.figure = null;
                newCell.figure = this;
            }
           
        }
    }
}
