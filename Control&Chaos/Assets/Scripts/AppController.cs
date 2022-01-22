using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dmdrn.UnityDebug;
using UnityEngine.SceneManagement;
using Dmdrn;

public class AppController : MonoBehaviour
{
    private StateMachine<AppController> fsm;
    private MainMenuState mainMenuState;
    private InGameState inGameState;

    public static AppController instance;

    [SerializeField] UIController uiController;
    [SerializeField] GameController gameController;
    

    private void CreateFSM()
    {
        fsm = new StateMachine<AppController>(this);
        mainMenuState = fsm.NewState<MainMenuState>();
        inGameState = fsm.NewState<InGameState>();
    }


    private void Awake()
    {

        CreateFSM();

        fsm.SetState(mainMenuState);

        if (instance != null)
        { 
            throw new System.Exception("Too many instances of " + GetType());
        }
        instance = this;


        gameController.GameFinishedEvent += FinishedGame;
    }

    #region fsm


    public class InGameState : StateMachine<AppController>.State
    {
        
        public override void OnEnter()
        {
            base.OnEnter();

            Log.Message("Entering game...");

            owner.gameController.StartGame();
        }
        
    }
    public class MainMenuState : StateMachine<AppController>.State
    {

        public override void OnEnter()
        {
            base.OnEnter();
            owner.uiController.OpenMainMenu();
        }

        public void StartGame()
        {
            owner.fsm.SetState(owner.inGameState);
            
        }

    }

    private void FinishedGame()
    {
        if (fsm.IsInState(inGameState))
        {
            mainMenuState.StartGame();
        }
    }

    public void StartGame()
    {
        if (fsm.IsInState(mainMenuState))
        {
            mainMenuState.StartGame();
        }
    }


    public void QuitApp()
    {
        Application.Quit();
    }


    #endregion
    #region sceneManagement


    public AsyncOperation LoadScene(
        string sceneName,
        LoadSceneMode loadMode)
    {
        return LoadScene(sceneName, null, loadMode);
    }

    public AsyncOperation LoadScene(
        string sceneName,
        System.Action callback = null,
        LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        Log.MessageFormat("Loading Scene: {0}, Mode: {1}", sceneName, loadMode);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadMode);
        StartCoroutine(LoadSceneCoroutine(sceneName, operation, callback));

        return operation;
    }


    private IEnumerator LoadSceneCoroutine(
        string sceneName,
        AsyncOperation operation,
        System.Action callback)
    {

        while (!operation.isDone)
        {
            yield return null;
        }
        Log.MessageFormat("Scene {0} loaded", sceneName);

        callback?.Invoke();
    }

    public AsyncOperation UnloadScene(
         string sceneName,
         System.Action callback = null)
    {
        Log.MessageFormat("Unloading Scene: {0}", sceneName);

        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        StartCoroutine(UnloadSceneCoroutine(sceneName, operation, callback));

        return operation;
    }


    private IEnumerator UnloadSceneCoroutine(
        string sceneName,
        AsyncOperation operation,
        System.Action callback)
    {

        while (!operation.isDone)
        {
            yield return null;
        }

        Log.MessageFormat("Scene {0} unloaded", sceneName);

        callback?.Invoke();
    }
    

    private IEnumerator WaitAFrame(System.Action callback)
    {
        yield return null;
        callback();
    }
    #endregion
}
