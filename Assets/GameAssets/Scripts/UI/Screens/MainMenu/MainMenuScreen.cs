using GameAssets.Scripts.Managers.SceneController;
using GameAssets.Scripts.Managers.ScreenManager;
using GameAssets.Scripts.UI.Screens.Common;

namespace GameAssets.Scripts.UI.Screens.MainMenu
{
    public class MainMenuScreen : ScreenBase
    {
        public void LoadActionPhaseScene()
        {
            ScreenManager.Instance.Show<LoadScreen>();
            
            SceneController.Instance.LoadScene(SceneController.SceneName.ActionPhase);
            
            canvasGroup.interactable = false;
        }
    }
}