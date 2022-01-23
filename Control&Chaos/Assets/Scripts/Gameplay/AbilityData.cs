using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{
    public class AbilityData
    {


    }
    public class Ability : ScriptableObject
    {
        public Sprite playerSprite;
        public Sprite enemySprite;

        public virtual void Use(MoveableFigure figure, System.Action doneCallback) { }
    }


    public class AttackAbility : Ability
    {
        public override void Use(MoveableFigure figure, System.Action doneCallback)
        {
            throw new System.NotImplementedException();
        }
    }
}