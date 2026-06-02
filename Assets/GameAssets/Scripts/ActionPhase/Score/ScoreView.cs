using GameAssets.Scripts.Managers.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private TMPro.TextMeshProUGUI hiScoreText;
        
        [Space(10)]
        [Header("Added score label configuration")]
        [SerializeField] public GameObject addedScoreLabel;
        [SerializeField] public TMPro.TextMeshProUGUI addedScoreText;
        [SerializeField] public Animator addedScoreAnimator;
        private static readonly int Show = Animator.StringToHash("Show");
        
        [Space(10)]
        [Header("New record message")]
        [SerializeField] public GameObject newRecordScreenMessage;
        [SerializeField] public GameObject newRecordLabel;


        public void SetUp(int currentScore, int lastHiScore)
        {
            HideNewRecordMessage();
            UpdateScoreText(currentScore);
            SetHiScoreText(lastHiScore);
            
            addedScoreLabel.SetActive(false);
        }
        
        public void UpdateScoreText(int currentScore, int earnedPoints)
        {
            UpdateScoreText(currentScore);
            ShowAddedScoreLabel(earnedPoints);
        }

        private void UpdateScoreText(int currentScore)
        {
            scoreText.text = currentScore.ToString();
        }
        
        private void ShowAddedScoreLabel(int earnedPoints)
        {
            addedScoreLabel.SetActive(true);
            addedScoreText.text = $"+{earnedPoints}";
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(addedScoreLabel.GetComponent<RectTransform>());
            
            addedScoreAnimator.SetTrigger(Show);
        }
        
        public void SetHiScoreText(int hiScoreValue)
        {
            hiScoreText.text = hiScoreValue.ToString();
        }
        
        public void ShowNewRecordLabel()
        {
            newRecordLabel.SetActive(true);
        }
        
        public void ShowNewRecordMessage()
        {
            AudioManager.Instance.PlaySound("NewRecord");
            newRecordScreenMessage.SetActive(true);
            
            ShowNewRecordLabel();
        }
        
        public void HideNewRecordMessage()
        {
            newRecordScreenMessage.SetActive(false);
            newRecordLabel.SetActive(false);
        }
    }
}