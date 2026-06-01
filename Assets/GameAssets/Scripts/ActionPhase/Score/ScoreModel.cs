using GameAssets.Scripts.Managers.SaveData;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreModel
    {
        private readonly ScoreData _scoreData;
        
        public int CurrentScore { get; set; }
        public int LastHiScore { get; private set; }
        public int[] MultipleLinesFactor => _scoreData.multipleLinesFactor;
        
        
        public ScoreModel(ScoreData scoreData)
        {
            _scoreData = scoreData;
            
            LastHiScore = SaveDataManager.Instance.GetSaveData().hiScore;
        }
    }
}