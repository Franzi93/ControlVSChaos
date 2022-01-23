using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class PlayerFigure : MoveableFigure
    {
        public System.Action reachedGoal;

        public override void MoveTo(EDirection direction)
        {
            GameCell oldCell = GetCurrentCell();

            if (MoveToDirection(direction))
            {
                GameCell newCell = GetCurrentCell();
               
                if (newCell.figure && newCell.figure.type == ECharacterType.Enemy)
                {
                    //Kill figure
                    KilledFigure(newCell.figure);
                }
                if (newCell.type == ECellType.Goal)
                {
                    reachedGoal();
                }
                oldCell.figure = null;
                SetCurrentCell();
            }
         
        }
    }
}
