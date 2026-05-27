using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase.Score
{
    public class ScoreController
    {
        private readonly ScoreView  _scoreView;

        public int CurrentScore { get; private set; }

        private int[] multipleLinesFactor = new int[] { 0, 2, 4, 6, 10, 15, 20 };
        
        
        #region Event subscription
        private void SubscribeEvents()
        {
            UnsubscribeEvents();
            
            Polyomino.OnPolyominoSuccessfullyPlaced += UpdateCurrentScoreAfterPolyominoPlaced;
            ActionPhaseManager.Instance.board.OnScoredFullRowsAndColumns += UpdateCurrentScoreAfterRowsOrColumnsCompleted;
        }

        private void UnsubscribeEvents()
        {
            Polyomino.OnPolyominoSuccessfullyPlaced -= UpdateCurrentScoreAfterPolyominoPlaced;
            ActionPhaseManager.Instance.board.OnScoredFullRowsAndColumns -= UpdateCurrentScoreAfterRowsOrColumnsCompleted;
        }

        #endregion
        
        #region Construction
        
        ScoreController(ScoreView view)
        {
            CurrentScore = 0;
            
            _scoreView = view;
            _scoreView.UpdateScoreText(CurrentScore);
            
            SubscribeEvents();
        }

        public class Builder
        {
            public ScoreController Build(ScoreView view)
            {
                return new ScoreController(view);
            }
        }
        
        #endregion
        
        #region Destruction

        ~ScoreController()
        {
            UnsubscribeEvents();
        }

        #endregion


        private void UpdateCurrentScoreAfterPolyominoPlaced(Polyomino placedPolyomino)
        {
            uint polyominoCellsCount = placedPolyomino.GetCellsCount();
            
            CurrentScore += (int)polyominoCellsCount;
            _scoreView.UpdateScoreText(CurrentScore);
        }

        private void UpdateCurrentScoreAfterRowsOrColumnsCompleted(List<int> completedRows, List<int> completedColumns)
        {
            Vector2Int gridSize = ActionPhaseManager.Instance.board.GridSize;
            
            int earnedPoints = completedRows.Count * gridSize.x + completedColumns.Count * gridSize.y;

            int multipleRowsExtra = multipleLinesFactor[completedRows.Count];
            int multipleColumnsExtra = multipleLinesFactor[completedColumns.Count];
            
            earnedPoints += earnedPoints * multipleRowsExtra;
            earnedPoints += earnedPoints * multipleColumnsExtra;
            
            if(completedColumns.Count > 0 && completedRows.Count > 0)
                earnedPoints *= completedRows.Count + completedColumns.Count;
            
            Log.Trace("ScoreController", $"Scored {earnedPoints} points for completing {completedRows.Count} rows and {completedColumns.Count} columns!");
            
            CurrentScore += earnedPoints;
            _scoreView.UpdateScoreText(CurrentScore, earnedPoints);
        }
    }
}