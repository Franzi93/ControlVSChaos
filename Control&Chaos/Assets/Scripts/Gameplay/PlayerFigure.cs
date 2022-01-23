using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class PlayerFigure : MoveableFigure
    {
        public System.Action reachedGoal;

        public override void MoveTo(EDirection direction, System.Action doneCallback)
        {
            GameCell oldCell = GetCurrentCell();
            onDoneAbiliy = doneCallback;
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

        public override void Attack(System.Action doneCallback)
        {
            onDoneAbiliy = doneCallback;
            animator.SetTrigger("Attack");
            foreach (GameCell cell in gameGrid.GetAllSurroundingCells(gridCoord.x, gridCoord.y))
            {
                if (cell.figure && cell.figure.type == ECharacterType.Enemy)
                {
                    KilledFigure(cell.figure);
                }
            }
            onDoneAbiliy();
        }
    }
}
