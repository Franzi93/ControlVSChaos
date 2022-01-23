using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
  
    public class Ability : ScriptableObject
    {
        public Sprite playerSprite;
        public Sprite enemySprite;

        public virtual void Use(MoveableFigure figure, System.Action doneCallback) { }
    }

    
}