using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class EnemyFigure : MoveableFigure
    {
        public override void MoveTo(EDirection direction, System.Action doneCallback)
        {
            GameCell oldCell = GetCurrentCell();
            onArrivedLocation = doneCallback;
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
