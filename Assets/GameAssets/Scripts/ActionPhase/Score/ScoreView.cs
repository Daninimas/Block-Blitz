using GameAssets.Scripts.Managers.Audio;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private TMPro.TextMeshProUGUI hiScoreText;
        
        [Space(10)]
        [Header("New record message")]
        [SerializeField] public GameObject newRecordMessage;


        public void SetUp(int currentScore, int lastHiScore)
        {
            HideNewRecordMessage();
            UpdateScoreText(currentScore);
            SetHiScoreText(lastHiScore);
        }
        
        public void UpdateScoreText(int currentScore, int addedScore)
        {
            UpdateScoreText(currentScore);
        }
        
        public void UpdateScoreText(int currentScore)
        {
            scoreText.text = currentScore.ToString();
        }
        
        public void SetHiScoreText(int hiScoreValue)
        {
            hiScoreText.text = hiScoreValue.ToString();
        }
        
        public void ShowNewRecordMessage()
        {
            AudioManager.Instance.PlaySound("NewRecord");
            newRecordMessage.SetActive(true);
        }
        
        public void HideNewRecordMessage()
        {
            newRecordMessage.SetActive(false);
        }
    }
}