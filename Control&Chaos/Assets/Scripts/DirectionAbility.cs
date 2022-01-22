using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    [CreateAssetMenu(fileName = "DirectionAbility", menuName = "ScriptableObjects/DirectionAbility", order = 1)]
    public class DirectionAbility : Ability
    {
        public enum Direction { LEFT, UP, RIGHT, DOWN }
        public Direction direction;

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}
