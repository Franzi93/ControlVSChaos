using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    [CreateAssetMenu(fileName = "DirectionAbility", menuName = "ScriptableObjects/DirectionAbility", order = 1)]
    public class DirectionAbility : Ability
    {
        public EDirection direction;

        public override void Use(MoveableFigure figure)
        {
            throw new System.NotImplementedException();
        }
    }
}
