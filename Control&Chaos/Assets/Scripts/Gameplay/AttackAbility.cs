using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    [CreateAssetMenu(fileName = "AttackAbility", menuName = "ScriptableObjects/AttackAbility", order = 1)]
    public class AttackAbility : Ability
    {

        public override void Use(MoveableFigure figure, System.Action doneCallback)
        {
            figure.Attack( doneCallback);
        }
    }
}
