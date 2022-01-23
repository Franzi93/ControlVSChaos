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
            DebugController.instance.AddAction("Finish level", FinishedLevel);
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



        public void FinishedLevel()
        {
            currentLevel.Cleanup();
            Destroy(currentLevel.gameObject);

            if ((levelPrefabs.Length - 1) < (currentLevelIndex + 1))
            {
                GameFinishedEvent();
            }
            else
            {
                StartLevel(currentLevelIndex + 1);
            }
        }

        public void Won()
        {
            Debug.Log("Game Won");
        }

        public void Lost()
        {
            Debug.Log("Game Lost");
        }

    }
}
