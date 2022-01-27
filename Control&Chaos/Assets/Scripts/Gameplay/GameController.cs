using System;
using Dmdrn.UnityDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dmdrn;

namespace Duality
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] GameObject[] levelPrefabs;
        [SerializeField] UIController uiController;
        [SerializeField] CardSystem cardSystem;
        [SerializeField] Transform levelSpawnTransform;

        private int currentLevelIndex;
        private Level currentLevel;

        private StateMachine<GameController> fsm;
        private LevelWon levelWon;
        private LevelLost levelLost;
        private Idle idle;
        private GameWon gameWon;
        private InGameState inGameState;

        #region fsm

        public GameState GetGameState()
        {
            return (fsm.GetCurrentState() as GameState);
        }

        public class GameState : StateMachine<GameController>.State
        {
            public virtual void Won() { }
            public virtual void Lost() { }
        }

        public class Idle : GameState
        {
            public override void OnEnter()
            {
                base.OnEnter();

                owner.Cleanup();
            }
        }

        public class InGameState : GameState
        {
            public override void OnEnter()
            {
                base.OnEnter();

                owner.StartLevel(owner.currentLevelIndex);
                owner.uiController.OpenMenu(EUIState.InGame);
            }
            public override void Won()
            {
                Debug.Log("Game Won");

                if ((owner.levelPrefabs.Length - 1) < (owner.currentLevelIndex + 1))
                {
                    owner.fsm.SetState(owner.gameWon);
                }
                else
                {
                    owner.fsm.SetState(owner.levelWon);
                }


            }

            public override void Lost()
            {
                Debug.Log("Game Lost");

                owner.fsm.SetState(owner.levelLost);
            }

        }

        public class LevelWon : GameState
        {
            public override void OnEnter()
            {
                base.OnEnter();

                owner.Cleanup();
                owner.uiController.OpenMenu(EUIState.Win);

            }
        }

        public class LevelLost : GameState
        {
            public override void OnEnter()
            {
                base.OnEnter();

                owner.Cleanup();
                owner.uiController.OpenMenu(EUIState.Lost);
            }
        }

        public class GameWon : GameState
        {
            public override void OnEnter()
            {
                base.OnEnter();

                owner.uiController.OpenMenu(EUIState.EndOfGame);
            }
        }

        private void SetupFSM()
        {
            fsm = new StateMachine<GameController>(this);
            inGameState = fsm.NewState<InGameState>();
            levelLost = fsm.NewState<LevelLost>();
            levelWon = fsm.NewState<LevelWon>();
            idle = fsm.NewState<Idle>();
            gameWon = fsm.NewState<GameWon>();
            fsm.SetState(idle);
            
        }

        #endregion

        private void Start()
        {
            SetupFSM();

            cardSystem.onPlayedCard += PlayedCard;

            DebugController.instance.AddAction("Won level", GetGameState().Won);
            DebugController.instance.AddAction("Lost level", GetGameState().Lost);
            DebugController.instance.AddAction("Reshuffel", cardSystem.ReshuffleHand);
        }

        public void PlayedCard(Card card)
        {
            currentLevel.ExecuteCard(card);
        }


        public void StartGame()
        {
            currentLevelIndex = 0;
            fsm.SetState(inGameState);
        }


        private void StartLevel(int index)
        {
            if (index > levelPrefabs.Length - 1)
            {
                Debug.LogError("Level index is higher than amount");
            }

            currentLevel = Instantiate(levelPrefabs[index], levelSpawnTransform.position, Quaternion.identity).GetComponent<Level>();
            currentLevel.transform.position = new Vector3(((levelPrefabs[index].GetComponent<Level>().width/2) * 4) *-1,levelSpawnTransform.position.y, levelSpawnTransform.position.z);
            currentLevel.Setup(inGameState.Won, inGameState.Lost);

            cardSystem.currentLevel = currentLevel;

            cardSystem.ReshuffleHand();
        }
        
        public void NextLevel()
        {
            currentLevelIndex++;
            fsm.SetState(inGameState);
        }

        public void StopGame()
        {
            fsm.SetState(idle);
        }


        private void Cleanup()
        {
            InputSystem.Free();

            if (currentLevel)
            {
                currentLevel.Cleanup();
                Destroy(currentLevel.gameObject);
            }
            cardSystem.RemoveAllCards();
            cardSystem.StopAllCoroutines();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                GetGameState().Won();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                GetGameState().Lost();
            }
#endif
        }

    }
}
