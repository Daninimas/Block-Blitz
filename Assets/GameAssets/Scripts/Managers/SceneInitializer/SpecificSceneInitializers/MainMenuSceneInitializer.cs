using GameAssets.Scripts.Managers.Audio;
using GameAssets.Scripts.Managers.SceneController;
using GameAssets.Scripts.UI.Screens.Common;
using GameAssets.Scripts.UI.Screens.MainMenu;

namespace GameAssets.Scripts.Managers.SceneInitializer
{
    public class MainMenuSceneInitializer : SceneInitializerBase
    {
        protected override void OnEnable()
        {
            OnManagersLoaded += ShowMainMenuScreen;
            base.OnEnable();
        }
        
        protected override void OnDisable()
        {
            OnManagersLoaded -= ShowMainMenuScreen;
            base.OnDisable();
        }
        

        private void ShowMainMenuScreen()
        {
            var currentOpenedScreenData = ScreenManager.ScreenManager.Instance.GetCurrentOpenedScreen();
            if(currentOpenedScreenData.Item2 != null && currentOpenedScreenData.Item2 == typeof(LoadScreen))
            {
                ScreenManager.ScreenManager.Instance.Hide<LoadScreen>();
            }
            
            PlayMainMenuMusic();
            
            ScreenManager.ScreenManager.Instance.Show<MainMenuScreen>();
            OnManagersLoaded -= ShowMainMenuScreen;
        }
        
        private void PlayMainMenuMusic()
        {
            AudioManager.Instance.PlayMusic("MainMenuMusic");
        }
    }
}