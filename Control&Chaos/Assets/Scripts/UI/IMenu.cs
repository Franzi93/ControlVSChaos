using UnityEngine;

namespace Duality
{
    public abstract class AMenu : MonoBehaviour
    {
        protected bool setupDone = false;

        protected AppController app;
        protected UIController uiController;

        public void SetControllers(AppController appController, UIController uiController)
        {
            app = appController;
            this.uiController = uiController;
        }
        
        public abstract void SetupButtons();

        public bool IsAlreadySetup()
        {
            return setupDone;
        }
    }
}