using System;
using System.Collections;
using GameAssets.Scripts.Managers.Audio;
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
            public int lastHiScore;
        }
        
        [SerializeField] private TMPro.TextMeshProUGUI scoreValueText;
        [SerializeField] private TMPro.TextMeshProUGUI hiScoreValueText;
        [SerializeField] private GameObject[] newRecordElements;
        [SerializeField] private float playMusicDelay;

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
            hiScoreValueText.text = screenData.lastHiScore.ToString();
            
            ActivateNewRecordElements(screenData.finalScore >= screenData.lastHiScore);
        }

        private void ActivateNewRecordElements(bool isNewRecord)
        {
            foreach (GameObject newRecordElement in newRecordElements)
            {
                newRecordElement.SetActive(isNewRecord);
            }
        }

        public override void Show(Action Show)
        {
            base.Show(Show);

            StartCoroutine(PlayGameOverSoundWithDelay());
        }


        #region Buttons behaviour

        public void ReloadGamePressed()
        {
            ScreenManager.Instance.Show<LoadScreen>();
            
            SceneController.Instance.ReloadScene();
        }
        
        public void ExitButtonPressed()
        {
            Application.Quit();
        }

        #endregion

        public IEnumerator PlayGameOverSoundWithDelay()
        {
            yield return new WaitForSeconds(playMusicDelay);
            
            AudioManager.Instance.PlayMusic("GameOverMusic");
            AudioManager.Instance.PlaySound("GameOver");
            
        }
    }
}