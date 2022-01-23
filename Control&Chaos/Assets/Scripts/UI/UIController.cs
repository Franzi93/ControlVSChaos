using System;
using System.Collections;
using System.Collections.Generic;
using Duality;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject lostMenu;
    [SerializeField] GameObject endOfGameMenu;

    private GameObject currentMenu;


    private void CloseCurrentMenu()
    {
        if (currentMenu)
        {
            currentMenu.SetActive(false);
        }
        currentMenu = null;
    }

    private void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        currentMenu = menu;
    }

    public void OpenMainMenu()
    {
        CloseCurrentMenu();

        OpenMenu(mainMenu);
    }

    public void OpenEndOfGameMenu()
    {
        CloseCurrentMenu();

        OpenMenu(endOfGameMenu);
    }

    public void OpenInGameMenu()
    {
        CloseCurrentMenu();

        OpenMenu(inGameMenu);
    }

    public void OpenMenu(EUIState state)
    {
        switch (state)
        {
            case EUIState.MainMenu:
                OpenMenu(mainMenu);
                break;
            case EUIState.Lost:
                OpenMenu(lostMenu);
                break;
            case EUIState.Win:
                OpenMenu(winMenu);
                break;
            case EUIState.InGame:
                OpenMenu(inGameMenu);
                break;
            case EUIState.EndOfGame:
                OpenMenu(endOfGameMenu);
                break;
            default:
                Debug.LogError("OpenMenu: No menu for given state found!");
                break;
        }
    }
}
