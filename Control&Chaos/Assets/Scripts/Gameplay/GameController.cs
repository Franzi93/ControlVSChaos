using System;
using Dmdrn.UnityDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public event System.Action GameFinishedEvent;


        private void Start()
        {
            DebugController.instance.AddAction("Won level", Won);
            DebugController.instance.AddAction("Lost level", Lost);
            DebugController.instance.AddAction("Reshuffel", ReshuffleHand);
            cardSystem.onPlayedCard += PlayedCard;
            cardSystem.handIsEmpty += ReshuffleHand;
        }

        public void PlayedCard(Card card)
        {
            currentLevel.ExecuteCard(card);
        }


        public void StartGame()
        {
            StartLevel(0);

            uiController.OpenInGameMenu();
        }


        private void StartLevel(int index)
        {
            currentLevelIndex = index;

            currentLevel = Instantiate(levelPrefabs[currentLevelIndex], levelSpawnTransform.position, Quaternion.identity).GetComponent<Level>();
            currentLevel.Setup(Won,Lost);

            cardSystem.ReshuffleHand(currentLevel.GetAllRemainEnemyTypes());
        }

        private void ReshuffleHand()
        {
            cardSystem.ReshuffleHand(currentLevel.GetAllRemainEnemyTypes());
        }



        public void Cleanup()
        {
            currentLevel.Cleanup();
            Destroy(currentLevel.gameObject);
            cardSystem.RemoveAllCards();
        }

        public void Won()
        {
            Debug.Log("Game Won");
            Cleanup();

            if ((levelPrefabs.Length - 1) < (currentLevelIndex + 1))
            {
                uiController.OpenMenu(EUIState.EndOfGame);
                //GameFinishedEvent();
            }
            else
            {
                uiController.OpenMenu(EUIState.Win);
                //StartLevel(currentLevelIndex + 1);
            }
            
            
        }

        public void Lost()
        {
            Debug.Log("Game Lost");
            Cleanup();

            uiController.OpenMenu(EUIState.Lost);
        }

        public void NextLevel()
        {
            uiController.OpenMenu(EUIState.InGame);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Won();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                Lost();
            }
        }
    }
}
