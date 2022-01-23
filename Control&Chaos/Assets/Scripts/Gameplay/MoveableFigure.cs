using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Duality
{
    public class MoveableFigure : MonoBehaviour
    {
        public ECharacterType type;
        public EEnemyType enemyType;

        public Vector2Int gridCoord;

        public GameGrid gameGrid;
        public RenderGrid renderGrid;

        public Animator animator;

        public event System.Action<MoveableFigure> onFigureKilled;
        public System.Action onArrivedLocation;

        public virtual void MoveTo(EDirection direction, System.Action doneCallback)
        {
        }

        protected bool MoveToDirection(EDirection direction)
        {
            Vector2Int newCell = gameGrid.GetCoordsInDirection(gridCoord.x, gridCoord.y, direction, 1);
            bool moveValid = gameGrid.IsValidCellPosition(newCell.x,newCell.y);
            if (moveValid)
            {
                UpdateAnimator(renderGrid.GetRenderPositionFromCellPosition(newCell.x, newCell.y));
                transform.DOMove(renderGrid.GetRenderPositionFromCellPosition(newCell.x, newCell.y), 0.8f).SetEase(Ease.InOutQuad).OnComplete(ArrivedLocation);
                //transform.position = renderGrid.GetRenderPositionFromCellPosition(newCell.x, newCell.y);

                gridCoord = newCell;
            }
            else
            {
                ArrivedLocation();
            }
            return moveValid;
        }

        private void UpdateAnimator(Vector3 newPos)
        {
            if (newPos.x > transform.position.x)
            {
                animator.SetFloat("Horizontal", 1f);
            }
            else if (newPos.x < transform.position.x)
            {
                animator.SetFloat("Horizontal", -1f);
            }
            else if (newPos.y > transform.position.y)
            {
                animator.SetFloat("Vertical", 1f);
            }
            else if (newPos.y < transform.position.y)
            {
                animator.SetFloat("Vertical", -1f);
            }
        }

       
        private void ResetAnimator()
        {
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }

        public GameCell GetCurrentCell()
        {
            GameCell cell = gameGrid.GetCell(gridCoord);
            return cell;

        }

        public void SetCurrentCell()
        {
            if (!gameGrid.IsValidCellPosition(gridCoord.x, gridCoord.y))
            {
                throw new System.Exception("SetCurrentCell not possible, cell not in grid "+gridCoord);
            }
            GetCurrentCell().figure = this;
        }
        public void KilledFigure(MoveableFigure figure)
        {
            onFigureKilled(figure);
        }
        public void ArrivedLocation()
        {
            ResetAnimator();
            Debug.Log("ArrivedLocation");
            onArrivedLocation();
        }


    }
}
