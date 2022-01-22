using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Duality
{

    public class Card
    {
        private Ability controlAbility;
        private Ability chaosAbility;
        private EEnemyType enemyType;

        public Card(Ability _controlAbility, Ability _chaosAbility, EEnemyType _enemyType)
        {
            controlAbility = _controlAbility;
            chaosAbility = _chaosAbility;
            enemyType = _enemyType;
        }

    }
}