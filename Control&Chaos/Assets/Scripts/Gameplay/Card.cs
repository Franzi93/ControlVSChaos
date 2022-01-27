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

        public Ability GetControlAbility()
        {
            return controlAbility;
        }

        public Ability GetChaosAbility()
        {
            return chaosAbility;
        }

        public EEnemyType GetEnemyType()
        {
            return enemyType;
        }

        public void Execute(MoveableFigure player, List<MoveableFigure> enemies, System.Action doneCallback)
        {
            controlAbility.Use(player,()=> {
                
                ExecuteEnemy(enemies, 0, doneCallback);
            });
        }

        private void ExecuteEnemy(List<MoveableFigure> enemies, int index, System.Action doneCallback)
        {
            //done when no enemies are there to control
            if (enemies.Count == 0)
            {
                doneCallback?.Invoke();
                return;
            }

            //check if current enemy is alive
            if (!enemies[index].isAlive)
            {
                if (enemies.Count == index + 1)
                {
                    doneCallback?.Invoke();
                }
                else
                {
                    ExecuteEnemy(enemies, (index + 1), doneCallback);
                }
                return;
            }

            //use ability on enemy
            chaosAbility.Use(enemies[index], () => {
                if (enemies.Count == index + 1)
                {
                    doneCallback?.Invoke();
                }
                else
                {
                    ExecuteEnemy(enemies, (index+1), doneCallback);
                }
            
            });
        }
    }
}