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

        public bool isAlive;

        public Vector2Int gridCoord;

        public GameGrid gameGrid;
        public RenderGrid renderGrid;

        public Animator animator;
        public GameObject deathVFX;

        public event System.Action<MoveableFigure> onFigureKilled;
        public System.Action onDoneAbility;

        private void Start()
        {
            isAlive = true;
        }

        public virtual void MoveTo(EDirection direction, System.Action doneCallback)
        {
        }

        public virtual void Attack( System.Action doneCallback)
        {
        }

        protected void MoveToPosition(Vector2Int newCell, System.Action callback)
        {
            UpdateAnimator(renderGrid.GetRenderPositionFromCellPosition(newCell.x, newCell.y));
            transform.DOMove(renderGrid.GetRenderPositionFromCellPosition(newCell.x, newCell.y), 0.8f).SetEase(Ease.InOutQuad).OnComplete(()=>callback());

            gridCoord = newCell;
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
            else if (newPos.z > transform.position.z)
            {
                animator.SetFloat("Vertical", 1f);
            }
            else if (newPos.z < transform.position.z)
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
            onDoneAbility();
        }

        protected IEnumerator WaitForSeconds(float seconds, System.Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }

        public void Die()
        {
            GetCurrentCell().figure = null;
            isAlive = false;
            gameObject.SetActive(false);

        }
    }
}
