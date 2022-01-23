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
            onDoneAbiliy = doneCallback;
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

        public override void Attack(System.Action doneCallback)
        {
            onDoneAbiliy = doneCallback;
            animator.SetTrigger("Attack");
            foreach (GameCell cell in gameGrid.GetAllSurroundingCells(gridCoord.x, gridCoord.y))
            {
                if (cell.figure && cell.figure.type == ECharacterType.Player)
                {
                    KilledFigure(cell.figure);
                }
            }
            onDoneAbiliy();
        }

    }
}
