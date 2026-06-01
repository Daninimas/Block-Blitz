using UnityEngine;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private TMPro.TextMeshProUGUI hiScoreText;
        
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
    }
}