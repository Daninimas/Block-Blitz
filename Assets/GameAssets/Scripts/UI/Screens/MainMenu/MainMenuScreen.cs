using GameAssets.Scripts.Managers.SceneController;
using GameAssets.Scripts.Managers.ScreenManager;

namespace GameAssets.Scripts.UI.Screens.MainMenu
{
    public class MainMenuScreen : ScreenBase
    {
        public void LoadActionPhaseScene()
        {
            SceneController.Instance.LoadScene(SceneController.SceneName.ActionPhase);
            
            canvasGroup.interactable = false;
        }
    }
}