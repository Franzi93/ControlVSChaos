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

        public EEnemyType GetEnemyType()
        {
            return enemyType;
        }

        public void Execute(MoveableFigure player, List<MoveableFigure> enemies)
        {
            controlAbility.Use(player);
            foreach (MoveableFigure m in enemies)
            {
                chaosAbility.Use(m);
            }

        }

    }
}