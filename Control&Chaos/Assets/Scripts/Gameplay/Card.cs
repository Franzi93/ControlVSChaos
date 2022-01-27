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

        #region queue
       

        private delegate IEnumerator AbilityRoutine(AbilityCommand command);
        private class AbilityCommand
        {
            public Ability ability;
            public MoveableFigure figure;
            public AbilityRoutine abilityRoutine;
        }
       
        private Queue<AbilityCommand> abilityQueue = new Queue<AbilityCommand>();
        private Coroutine currentAbilityRoutine;
        

        private void Enqueue(AbilityCommand command)
        {
            abilityQueue.Enqueue(command);
        }
        private void StartAbilityQueue(System.Action doneCallback)
        {
            if (currentAbilityRoutine != null)
            {
                throw new System.Exception("Ability routine is already running bro");
            }
            currentAbilityRoutine = AppController.instance.StartCoroutine(AbilityQueueRoutine(doneCallback));
        }

        private IEnumerator AbilityQueueRoutine(System.Action doneCallback)
        {
            while (abilityQueue.Count>0)
            {
                AbilityCommand command = abilityQueue.Dequeue();
                AbilityRoutine abilityRoutine = command.abilityRoutine;
                IEnumerator e = abilityRoutine(command);
                while (e.MoveNext())
                {
                    yield return null;
                }
            }
            doneCallback?.Invoke();
        }
        private IEnumerator UseAbilityRoutine(AbilityCommand command)
        {
            bool isDone = false;
            if (command.figure.isAlive)
            {
                command.ability.Use(command.figure, () => { isDone = true; });
                while (!isDone)
                {
                    yield return null;
                }
            }

        }
        #endregion

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
            AbilityCommand command = new AbilityCommand();
            command.ability = controlAbility;
            command.figure = player;
            command.abilityRoutine = UseAbilityRoutine;
            Enqueue(command);

            foreach (MoveableFigure m in enemies)
            {
                command = new AbilityCommand();
                command.ability = chaosAbility;
                command.figure = m;
                command.abilityRoutine = UseAbilityRoutine;
                Enqueue(command);
            }
            StartAbilityQueue(doneCallback);
        }
    }
}