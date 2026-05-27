using UnityEngine;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        
        public void UpdateScoreText(int currentScore, int addedScore)
        {
            UpdateScoreText(currentScore);
        }
        
        public void UpdateScoreText(int currentScore)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}