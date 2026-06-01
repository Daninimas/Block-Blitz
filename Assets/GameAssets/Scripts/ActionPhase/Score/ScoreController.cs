using System;
using System.Collections.Generic;
using GameAssets.Scripts.Managers.SaveData;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreController : IController
    {
        private readonly ScoreView _scoreView;
        private readonly ScoreModel _scoreModel;
        
        public event Action<int> OnAddedScore;
        public event Action<int> OnNewRecordReached;
        
        
        #region Event subscription
        private void SubscribeEvents()
        {
            UnsubscribeEvents();
            
            Polyomino.OnPolyominoSuccessfullyPlaced += UpdateCurrentScoreAfterPolyominoPlaced;
            ActionPhaseManager.Instance.board.OnScoredFullRowsAndColumns += UpdateCurrentScoreAfterRowsOrColumnsCompleted;
            ActionPhaseManager.Instance.OnGameOver += SaveNewHiScore;
        }

        private void UnsubscribeEvents()
        {
            Polyomino.OnPolyominoSuccessfullyPlaced -= UpdateCurrentScoreAfterPolyominoPlaced;
            ActionPhaseManager.Instance.board.OnScoredFullRowsAndColumns -= UpdateCurrentScoreAfterRowsOrColumnsCompleted;
            ActionPhaseManager.Instance.OnGameOver -= SaveNewHiScore;
        }

        #endregion
        
        #region Construction
        
        ScoreController(ScoreView view, ScoreModel scoreModel)
        {
            _scoreModel = scoreModel;
            _scoreModel.CurrentScore = 0;
            
            _scoreView = view;
            _scoreView.UpdateScoreText(scoreModel.CurrentScore);
            _scoreView.SetHiScoreText(scoreModel.LastHiScore);
            
            SubscribeEvents();
        }

        public class Builder
        {
            public ScoreController Build(ScoreView view, ScoreData scoreData)
            {
                var scoreModel = new ScoreModel(scoreData);
                return new ScoreController(view, scoreModel);
            }
        }
        
        #endregion
        
        #region Destruction

        public void Destroy()
        {
            UnsubscribeEvents();
        }

        #endregion


        #region Manage scored points
        
        private void UpdateCurrentScoreAfterPolyominoPlaced(Polyomino placedPolyomino)
        {
            uint polyominoCellsCount = placedPolyomino.GetBlocksCount();
            
            _scoreModel.CurrentScore += (int)polyominoCellsCount;
            _scoreView.UpdateScoreText(_scoreModel.CurrentScore);

            CheckIfNewRecordIsReached();
        }

        private void UpdateCurrentScoreAfterRowsOrColumnsCompleted(List<int> completedRows, List<int> completedColumns)
        {
            Vector2Int gridSize = ActionPhaseManager.Instance.board.GridSize;
            
            int earnedPoints = completedRows.Count * gridSize.x + completedColumns.Count * gridSize.y;

            int multipleRowsExtra = _scoreModel.MultipleLinesFactor[completedRows.Count];
            int multipleColumnsExtra = _scoreModel.MultipleLinesFactor[completedColumns.Count];
            
            earnedPoints += earnedPoints * multipleRowsExtra;
            earnedPoints += earnedPoints * multipleColumnsExtra;
            
            if(completedColumns.Count > 0 && completedRows.Count > 0)
                earnedPoints *= completedRows.Count + completedColumns.Count;
            
            _scoreModel.CurrentScore += earnedPoints;
            _scoreView.UpdateScoreText(_scoreModel.CurrentScore, earnedPoints);
            
            OnAddedScore?.Invoke(earnedPoints);
        }

        private void CheckIfNewRecordIsReached()
        {
            // If it is not the first playthrough, check if the current score is higher than the previous hi score and if so, trigger the event
            if (_scoreModel.LastHiScore > 0 && _scoreModel.CurrentScore > _scoreModel.LastHiScore)            
            {
                OnNewRecordReached?.Invoke(_scoreModel.CurrentScore);
            }
        }

        #endregion

        public int GetCurrentScore()
        {
            return _scoreModel.CurrentScore;
        }

        public int GetLastHiScore()
        {
            return _scoreModel.CurrentScore > _scoreModel.LastHiScore ? _scoreModel.CurrentScore : _scoreModel.LastHiScore;
        }

        private void SaveNewHiScore()
        {
            if (_scoreModel.CurrentScore <= _scoreModel.LastHiScore) 
                return;
            
            var saveData = SaveDataManager.Instance.GetSaveData();
            saveData.hiScore = _scoreModel.CurrentScore;
            SaveDataManager.Instance.SaveData(saveData);
        }
    }
}