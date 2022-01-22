using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] levelPrefabs;
    [SerializeField] UIController uiController;

    private int currentLevelIndex;
    private Level currentLevel;

    public event System.Action GameFinishedEvent;
      

    private void Awake()
    {
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
            Destroy(currentLevel);
        }

        currentLevel = Instantiate(levelPrefabs[currentLevelIndex]).GetComponent<Level>();
    }


    public void FinishedLevel()
    {
        if (levelPrefabs.Length - 1 < currentLevelIndex + 1)
        {
            GameFinishedEvent();
        }
        else
        {
            StartLevel(currentLevelIndex + 1);
        }
    }

}
