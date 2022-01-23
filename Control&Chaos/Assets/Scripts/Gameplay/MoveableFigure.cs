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

        public event System.Action<MoveableFigure> onFigureKilled;
     

        public virtual void MoveTo(EDirection direction)
        {
        }

        public bool MoveToDirection(EDirection direction)
        {
            Vector2Int newCell = gameGrid.GetCoordsInDirection(gridCoord.x, gridCoord.y, direction, 1);
            bool moveValid = gameGrid.IsValidCellPosition(newCell.x,newCell.y);
            if (moveValid)
            {
                transform.position = renderGrid.GetRenderPositionFromCellPosition(newCell.x, newCell.y);

                gridCoord = newCell;

            }
            return moveValid;
        }

        public GameCell GetCurrentCell()
        {
            GameCell cell = gameGrid.GetCell(gridCoord);
            return cell;

        }

        public void SetCurrentCell()
        {
         //   GetCurrentCell().figure = this;
        }
        public void KilledFigure(MoveableFigure figure)
        {
            onFigureKilled(figure);
        }
    }
}
