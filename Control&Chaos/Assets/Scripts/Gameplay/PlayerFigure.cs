using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class PlayerFigure : MoveableFigure
    {
        public override void MoveTo(EDirection direction)
        {
            if (MoveToDirection(direction))
            {
                GameCell newCell = GetCurrentCell();
               
                //if (newCell.figure && newCell.figure.type == ECharacterType.Enemy)
                //{
                //    //Kill figure
                //}
                //if (newCell.type == ECellType.Goal)
                //{
                //    //WIN
                //}
            }
         
        }
    }
}
