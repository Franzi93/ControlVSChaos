using Duality;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private AMenu mainMenu;
    [SerializeField] private AMenu inGameMenu;
    [SerializeField] private AMenu winMenu;
    [SerializeField] private AMenu lostMenu;
    [SerializeField] private AMenu endOfGameMenu;

    private GameObject currentMenu;

    public void Setup(AppController app)
    {
        mainMenu.SetControllers(app,this);
        inGameMenu.SetControllers(app,this);
        winMenu.SetControllers(app,this);
        lostMenu.SetControllers(app,this);
        endOfGameMenu.SetControllers(app,this);
    }


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
        CloseCurrentMenu();
        menu.SetActive(true);
        currentMenu = menu;
    }

    public void OpenMainMenu()
    {
        
        mainMenu.SetupButtons();

        OpenMenu(mainMenu.gameObject);
    }

    public void OpenEndOfGameMenu()
    {

        OpenMenu(endOfGameMenu.gameObject);
    }

    public void OpenInGameMenu()
    {

        OpenMenu(inGameMenu.gameObject);
    }

    public void OpenMenu(EUIState state)
    {
        switch (state)
        {
            case EUIState.MainMenu:
                OpenMenu(mainMenu.gameObject);
                break;
            case EUIState.Lost:
                OpenMenu(lostMenu.gameObject);
                break;
            case EUIState.Win:
                OpenMenu(winMenu.gameObject);
                break;
            case EUIState.InGame:
                OpenMenu(inGameMenu.gameObject);
                break;
            case EUIState.EndOfGame:
                OpenMenu(endOfGameMenu.gameObject);
                break;
            default:
                Debug.LogError("OpenMenu: No menu for given state found!");
                break;
        }
    }
}
