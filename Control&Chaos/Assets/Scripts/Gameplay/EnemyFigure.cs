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
            onDoneAbility = doneCallback;

            Vector2Int newCellPos = gameGrid.GetCoordsInDirection(gridCoord.x, gridCoord.y, direction, 1);
            bool cellValid = gameGrid.IsValidCellPosition(newCellPos.x, newCellPos.y);


            if (cellValid)
            {
                GameCell newCell = gameGrid.GetCell(newCellPos);

                if (newCell.Contains(ECharacterType.Enemy))
                {
                    onDoneAbility();
                    return;
                }
                MoveToPosition(newCellPos,()=> {

                    if (newCell.Contains(ECharacterType.Player))
                    {
                        KilledFigure(newCell.figure);
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
                if (cell.Contains(ECharacterType.Player))
                {
                    KilledFigure(cell.figure);
                }
            }
            StartCoroutine(WaitForSeconds(1f, onDoneAbility));
        }



    }
}
