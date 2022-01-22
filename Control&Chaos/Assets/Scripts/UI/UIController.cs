using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject endOfGameMenu;
    [SerializeField] GameObject inGameMenu;

    private GameObject currentMenu;


    public void CloseCurrentMenu()
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

}
