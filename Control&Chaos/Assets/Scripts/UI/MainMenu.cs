using UnityEngine;
using UnityEngine.UI;

namespace Duality
{
    public class MainMenu : AMenu
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button creditsButton;

        public override void SetupButtons()
        {
            if (setupDone)
                return;

            setupDone = true;
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            creditsButton.onClick.AddListener(OnCreditButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            AppController.instance.StartGame();
            app.StartGame();
        }

        private void OnExitButtonClicked()
        {
            Debug.Log("Exit");
            app.ExitGame();
        }

        private void OnCreditButtonClicked()
        {
            // TODO: Add
        }
    }
}