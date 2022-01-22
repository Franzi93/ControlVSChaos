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

        [SerializeField] List<Ability> abilities;


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

            if (currentLevel)
            {
                Destroy(currentLevel.gameObject);

            }

            currentLevel = Instantiate(levelPrefabs[currentLevelIndex]).GetComponent<Level>();
        }



        public void FinishedLevel()
        {
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
