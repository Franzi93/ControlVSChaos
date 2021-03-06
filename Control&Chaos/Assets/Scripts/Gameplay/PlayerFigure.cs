using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class PlayerFigure : MoveableFigure
    {
        public bool reachedGoal;

        public override void MoveTo(EDirection direction, System.Action doneCallback)
        {
            GameCell oldCell = GetCurrentCell();
            onDoneAbility = doneCallback;

            Vector2Int newCellPos = gameGrid.GetCoordsInDirection(gridCoord.x, gridCoord.y, direction, 1);
            bool cellValid = gameGrid.IsValidCellPosition(newCellPos.x,newCellPos.y);

            if (cellValid)
            {

                MoveToPosition(newCellPos,()=> {

                    GameCell newCell = gameGrid.GetCell(newCellPos);
                    if (newCell.Contains(ECharacterType.Enemy))
                    {
                        //Kill figure
                        KilledFigure(newCell.figure);
                    }

                    if (newCell.type == ECellType.Goal)
                    {
                        reachedGoal = true;
                    }

                    oldCell.figure = null;
                    SetCurrentCell();

                    ArrivedLocation();
                });

            }
            else
            {
                onDoneAbility();
            }
         
        }

        public override void Attack(System.Action doneCallback)
        {
            onDoneAbility = doneCallback;
            animator.SetTrigger("Attack");
            foreach (GameCell cell in gameGrid.GetAllSurroundingCells(gridCoord.x, gridCoord.y))
            {
                if (cell.figure && cell.figure.type == ECharacterType.Enemy)
                {
                    KilledFigure(cell.figure);
                }
            }
            StartCoroutine(WaitForSeconds(1f,onDoneAbility));
        }
    }
}
