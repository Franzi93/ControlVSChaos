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



        private int currentLevelIndex;
        private Level currentLevel;

        public event System.Action GameFinishedEvent;


        private void Start()
        {
            DebugController.instance.AddAction("Finish level", FinishedLevel);

        }


        public void StartGame()
        {
            StartLevel(0);

            uiController.OpenInGameMenu();
        }


        private void StartLevel(int index)
        {
            currentLevelIndex = index;


            currentLevel = Instantiate(levelPrefabs[currentLevelIndex]).GetComponent<Level>();
            currentLevel.Setup();

            cardSystem.CreateCards();
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

    }
}
