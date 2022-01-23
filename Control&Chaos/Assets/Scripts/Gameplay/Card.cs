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
                enemiesDone = 0;
                foreach (MoveableFigure m in enemies)
                {

                    if (!m.isAlive)
                    { continue; }
                    ++enemiesToDO;
                    chaosAbility.Use(m,()=> {
                        ++enemiesDone;
                        if (enemiesDone == enemiesToDO)
                        {
                            doneCallback?.Invoke();
                        }
                    });
                }
            });

        }

        private int enemiesDone;
        private int enemiesToDO;
        public void CheckEnemyDone(int enemyAmount, System.Action doneCallback)
        {
            if (enemiesDone == enemyAmount)
            {
             doneCallback?.Invoke();
            }
        }


    }
}