using GameAssets.Scripts.Managers.SceneController;
using GameAssets.Scripts.Managers.ScreenManager;
using GameAssets.Scripts.UI.Screens.Common;
using UnityEngine;

namespace GameAssets.Scripts.UI.Screens
{
    public class GameOverScreen : ScreenBase
    {
        public class GameOverScreenData
        {
            public int finalScore;
        }
        
        [SerializeField] private TMPro.TextMeshProUGUI scoreValueText;

        public override void Setup(object data)
        {
            base.Setup(data);

            if (data is GameOverScreenData screenData)
            {
                SetScreenData(screenData);
            }
        }

        private void SetScreenData(GameOverScreenData screenData)
        {
            scoreValueText.text = screenData.finalScore.ToString();
        }


        #region Buttons behaviour

        public void ReloadGamePressed()
        {
            ScreenManager.Instance.Show<LoadScreen>();
            
            SceneController.Instance.ReloadScene();
        }

        #endregion
    }
}