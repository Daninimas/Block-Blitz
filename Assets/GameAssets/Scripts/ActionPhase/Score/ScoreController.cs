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
        
        private bool _newRecordReached = false;
        
        
        #region Event subscription
        private void SubscribeEvents()
        {
            UnsubscribeEvents();
            
            ActionPhaseManager.Instance.board.OnPolyominoPlacedAndScored += UpdateCurrentScoreAfterPolyominoPlacedInBoard;
            ActionPhaseManager.Instance.OnGameOver += SaveNewHiScore;
        }

        private void UnsubscribeEvents()
        {
            ActionPhaseManager.Instance.board.OnPolyominoPlacedAndScored -= UpdateCurrentScoreAfterPolyominoPlacedInBoard;
            ActionPhaseManager.Instance.OnGameOver -= SaveNewHiScore;
        }

        #endregion
        
        #region Construction
        
        ScoreController(ScoreView view, ScoreModel scoreModel)
        {
            _scoreModel = scoreModel;
            _scoreModel.CurrentScore = 0;
            
            _scoreView = view;
            _scoreView.SetUp(_scoreModel.CurrentScore, _scoreModel.LastHiScore);
            
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

        private void UpdateCurrentScoreAfterPolyominoPlacedInBoard(Polyomino placedPolyomino, 
            List<int> completedRows, List<int> completedColumns)
        {
            int earnedPoints = 0;
            
            earnedPoints += CalculateScoreOfPlacedPolyomino(placedPolyomino);
            earnedPoints += CalculateScoreOfRowsAndColumnsCompleted(completedRows, completedColumns);
            
            _scoreModel.CurrentScore += earnedPoints;
            _scoreView.UpdateScoreText(_scoreModel.CurrentScore, earnedPoints);
            
            OnAddedScore?.Invoke(earnedPoints);

            CheckIfNewRecordIsReached();
        }
        
        private int CalculateScoreOfPlacedPolyomino(Polyomino placedPolyomino)
        {
            uint polyominoCellsCount = placedPolyomino.GetBlocksCount();
            
            return (int)polyominoCellsCount;
        }

        private int CalculateScoreOfRowsAndColumnsCompleted(List<int> completedRows, List<int> completedColumns)
        {
            Vector2Int gridSize = ActionPhaseManager.Instance.board.GridSize;
            
            int earnedPoints = completedRows.Count * gridSize.x + completedColumns.Count * gridSize.y;

            int multipleRowsExtra = _scoreModel.MultipleLinesFactor[completedRows.Count];
            int multipleColumnsExtra = _scoreModel.MultipleLinesFactor[completedColumns.Count];
            
            earnedPoints += earnedPoints * multipleRowsExtra;
            earnedPoints += earnedPoints * multipleColumnsExtra;
            
            if(completedColumns.Count > 0 && completedRows.Count > 0)
                earnedPoints *= completedRows.Count + completedColumns.Count;
            
            return earnedPoints;
        }

        private void CheckIfNewRecordIsReached()
        {
            if (_scoreModel.CurrentScore <= _scoreModel.LastHiScore) 
                return;
            _scoreView.ShowNewRecordLabel();
            _scoreView.SetHiScoreText(_scoreModel.CurrentScore);
                
                
            // If it is not the first playthrough, and the event has not been triggered yet
            if (_scoreModel.LastHiScore <= 0 || _newRecordReached) 
                return;
            
            _scoreView.ShowNewRecordMessage();
            OnNewRecordReached?.Invoke(_scoreModel.CurrentScore);
            _newRecordReached = true;
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