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
        public Sprite sprite;

        public virtual void Use(MoveableFigure figure) { }
    }


    public class AttackAbility : Ability
    {
        public override void Use(MoveableFigure figure)
        {
            throw new System.NotImplementedException();
        }
    }
}