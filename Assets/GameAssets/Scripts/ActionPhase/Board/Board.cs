using System;
using System.Collections.Generic;
using System.Linq;
using GameAssets.Scripts.Managers.Audio;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameAssets.Scripts.ActionPhase
{
    public class Board : IController
    {
        private readonly BoardView _boardView;
        private readonly BoardModel _boardModel;
        
        private readonly BoardCell[,] _grid;
        
        private readonly List<BoardCell> _hoveredCells = new ();
        
        private readonly List<int> _highlightedFullRows = new ();
        private readonly List<int> _highlightedFullColumns = new ();
        
        private bool _interactable;
        
        public Vector2Int GridSize => new Vector2Int(_grid.GetLength(1), _grid.GetLength(0));
        
        public event Action<List<int>, List<int>> OnScoredFullRowsAndColumns;
        
        
        #region Event subscription

        private void SubscribeEvents()
        {
            UnsubscribeEvents();
            
            ActionPhaseManager.Instance.OnGameOver += PlayGameOverAnimation;
        }

        private void UnsubscribeEvents()
        {
            ActionPhaseManager.Instance.OnGameOver -= PlayGameOverAnimation;
        }

        #endregion

        #region Construction
        
        Board(BoardView view, BoardModel boardModel)
        {
            _boardView = view;
            _boardModel = boardModel;
            
            _grid = ActionPhaseManager.Instance.boardCellsFactory.CreateBoardCells(_boardModel.boardData.gridSize, 
                _boardView.GetCellsParent());
            
            _boardView.InitializeData(_boardModel.boardData.gridSize);

            SubscribeEvents();
            
            _interactable = true;
        }

        public class Builder
        {
            public Board Build(BoardView view, BoardData boardData)
            {
                return new Board(view, new BoardModel(boardData));
            }
        }
        
        #endregion
        
        #region Destruction

        public void Destroy()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        
        // Returns if the polyomino can be placed
        public bool HoverByPolyomino(Polyomino polyomino)
        {
            if (!_interactable)
                return false;
            
            ClearHoveredCells();
            //ClearHighlightedRowsAndColumns();

            if (!_boardView.IsPositionInsideRect(polyomino.BlockContainerTransform.position))
                return false;
            
            Vector3 polyominoTopLeftCorner = polyomino.GetTopLeftBlockPosition();
            
            var localRelativePos = _boardView.GetRelativePosToTopLeftCorner(polyominoTopLeftCorner);
            Vector2Int gridPos = new ()
            {
                x = (int)Tools.Tools.NormalizeValues(0f, _grid.GetLength(1), 0f, 1f, localRelativePos.x),
                y = (int)Tools.Tools.NormalizeValues(0f, _grid.GetLength(0), 0f, 1f, localRelativePos.y)
            };

            (bool, List<Vector2Int>) canPlacePolyominoInPos = CanPlacePolyominoInPos(polyomino.blocksShape, gridPos);

            if (!canPlacePolyominoInPos.Item1)
            {
                ClearHighlightedRowsAndColumns();
                return false;
            }
            
            SetHoveredCells(canPlacePolyominoInPos.Item2);
                
            HighlightFullRows(polyomino.blocksColorData);
            HighlightFullColumns(polyomino.blocksColorData);

            return true;
        }

        /// <summary>
        /// Check if the polyomino can be placed in the selected position
        /// </summary>
        /// <returns>
        /// bool -> return if the polyomino can be placed
        /// List<Vector2Int> -> list of checked and valid cells, ONLY used for the hover cells
        /// </returns>
        private (bool, List<Vector2Int>) CanPlacePolyominoInPos(int[,] polyominoShape, Vector2Int polyominoGridPos)
        {
            List<Vector2Int> validCells = new List<Vector2Int>();
            bool allCellsAreValid = true;
            
            for (int r = 0; r < polyominoShape.GetLength(0); r++)
            {
                if (!allCellsAreValid)
                    break;
                
                for (int c = 0; c < polyominoShape.GetLength(1); c++)
                {
                    if(polyominoShape[r, c] == 0)
                        continue;
                    
                    Vector2Int cellPosToCheck = polyominoGridPos + new Vector2Int(c, r);

                    if (!IsValidCell(cellPosToCheck))
                    {
                        allCellsAreValid = false;
                        break;
                    }
                    
                    validCells.Add(cellPosToCheck);
                }
            }

            return (allCellsAreValid, validCells);
        }

        private bool IsValidCell(Vector2Int gridPos)
        {
            int gridPosCol = gridPos.y;
            int gridPosRow = gridPos.x;
            
            if(gridPosCol < 0 || gridPosCol >= _grid.GetLength(1) 
                             || gridPosRow < 0 || gridPosRow >= _grid.GetLength(0)
                             || _grid[gridPosCol, gridPosRow].currentState == CellState.Used)
                return false;
            
            return true;
        }

        public void ClearHoveredCells()
        {
            foreach (var hoveredCell in _hoveredCells)
            {
                if(hoveredCell.currentState == CellState.Hovered)
                    hoveredCell.SetFree();
            }
            _hoveredCells.Clear();
        }

        private void SetHoveredCells(List<Vector2Int> hoveredCellsIndex)
        {
            foreach (var hoveredCellIndex in hoveredCellsIndex)
            {
                BoardCell hoveredCell = _grid[hoveredCellIndex.y, hoveredCellIndex.x];
                hoveredCell.SetHovered();
                _hoveredCells.Add(hoveredCell);
            }
        }

        #region Confirm polyomino placement

        public void PlacePolyominoInLastHoveredPos(Polyomino polyomino)
        {
            ConfirmHoveredCellsAsPlacedPolyomino(polyomino);
        }

        private void ConfirmHoveredCellsAsPlacedPolyomino(Polyomino polyomino)
        {
            SetHoveredCellsAsUsedWithAnimation(polyomino.blocksColorData);
            _hoveredCells.Clear();
            
            ScoreFullRowsAndColumns();
            _highlightedFullRows.Clear();
            _highlightedFullColumns.Clear();
        }

        private void SetHoveredCellsAsUsedWithAnimation(Block.BlockColorData blocksColorData)
        {
            float currentDelay = _boardModel.boardData.useCellsAnimationStartDelay;
            
            foreach (var hoveredCell in _hoveredCells)
            {
                hoveredCell.SetUsed(blocksColorData, currentDelay);
                
                currentDelay += _boardModel.boardData.useCellsAnimationInterDelay;
            }
        }

        private void ScoreFullRowsAndColumns()
        {
            // Rows
            foreach (var highlightedFullRow in _highlightedFullRows)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    _grid[highlightedFullRow, c].ClearUsedBlock();
                }
            }
            
            // Columns
            foreach (var highlightedFullColumn in _highlightedFullColumns)
            {
                for (int r = 0; r < _grid.GetLength(0); r++)
                {
                    _grid[r, highlightedFullColumn].ClearUsedBlock();
                }
            }
            
            if(_highlightedFullColumns.Count == 0 && _highlightedFullRows.Count == 0)
                return;
            
            AudioManager.Instance.PlaySound("ScoreLine");
            
            OnScoredFullRowsAndColumns?.Invoke(_highlightedFullRows, _highlightedFullColumns);
        }

        #endregion


        #region Highlight Rows and Columns

        private void ClearHighlightedRowsAndColumns()
        {
            UpdateHighlightedToRowCells(_highlightedFullRows,false);
            UpdateHighlightedToColumnCells(_highlightedFullColumns, false);
            
            _highlightedFullRows.Clear();
            _highlightedFullColumns.Clear();
        }

        #region Rows

        private void HighlightFullRows(Block.BlockColorData polyominoBlocksColorData)
        {
            CalculateHighlightedFullRows();
            
            UpdateHighlightedToRowCells(_highlightedFullRows, true, polyominoBlocksColorData);
        }

        private void CalculateHighlightedFullRows()
        {
            List<int> highlightedFullRows = new List<int>();
            
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                bool isFullRow = true;
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    if (_grid[r, c].currentState != CellState.Used && _grid[r, c].currentState != CellState.Hovered)
                    {
                        isFullRow = false;
                        break;
                    }
                }
                
                if(isFullRow)
                    highlightedFullRows.Add(r);
            }
            
            List<int> rowsToUnhighlight = _highlightedFullRows.Except(highlightedFullRows).ToList();
            UpdateHighlightedToRowCells(rowsToUnhighlight, false);
            
            _highlightedFullRows.Clear();
            _highlightedFullRows.AddRange(highlightedFullRows);
        }

        private void UpdateHighlightedToRowCells(List<int> rowsToUpdate, bool isHighlighted, 
            Block.BlockColorData blocksColorData = null)
        {
            foreach (var rowToUpdateHighlight in rowsToUpdate)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    var cell = _grid[rowToUpdateHighlight, c];
                    
                    if(isHighlighted)
                        cell.SetHighlighted(blocksColorData);
                    else
                        cell.SetUnhighlighted();
                }
            }
        }
        
        #endregion



        #region Columns

        private void HighlightFullColumns(Block.BlockColorData polyominoBlocksColorData)
        {
            CalculateHighlightFullColumns();

            UpdateHighlightedToColumnCells(_highlightedFullColumns, true, polyominoBlocksColorData);
        }
        
        private void CalculateHighlightFullColumns()
        {
            List<int> highlightedFullColumns = new List<int>();
            
            for (int c = 0; c < _grid.GetLength(1); c++)
            {
                bool isFullColumn = true;
                for (int r = 0; r < _grid.GetLength(0); r++)
                {
                    if (_grid[r, c].currentState != CellState.Used && _grid[r, c].currentState != CellState.Hovered)
                    {
                        isFullColumn = false;
                        break;
                    }
                }
                
                if(isFullColumn)
                    highlightedFullColumns.Add(c);
            }
            
            List<int> columnsToUnhighlight = _highlightedFullColumns.Except(highlightedFullColumns).ToList();
            UpdateHighlightedToColumnCells(columnsToUnhighlight, false);
            
            _highlightedFullColumns.Clear();
            _highlightedFullColumns.AddRange(highlightedFullColumns);
        }
        
        private void UpdateHighlightedToColumnCells(List<int> columnsToUpdate, bool isHighlighted, 
            Block.BlockColorData blocksColorData = null)
        {
            foreach (var columnToHighlight in columnsToUpdate)
            {
                for (int r = 0; r < _grid.GetLength(0); r++)
                {
                    var cell = _grid[r, columnToHighlight];
                    
                    if(isHighlighted)
                        cell.SetHighlighted(blocksColorData);
                    else
                        cell.SetUnhighlighted();
                }
            }
        }
        
        #endregion
        
        #endregion
        

        #region Check if polyomino can be placed

        public bool CheckIfPolyominoCanBePlacedInAllGrid(int[,] polyominoShape)
        {
            int polyominoRows = polyominoShape.GetLength(0);
            int polyominoCols = polyominoShape.GetLength(1);

            for (int r = 0; r < _grid.GetLength(0) - polyominoRows + 1; r++)
            {
                for (int c = 0; c < _grid.GetLength(1) - polyominoCols + 1; c++)
                {
                    var gridPos = new Vector2Int(c, r);
                    (bool, List<Vector2Int>) canPlacePolyominoInPos = CanPlacePolyominoInPos(polyominoShape, gridPos);
                    
                    if(canPlacePolyominoInPos.Item1)
                        return true;
                }
            }

            return false;
        }

        #endregion


        #region Game over animation

        private void PlayGameOverAnimation()
        {
            float[] fillColumnsAnimationDelays = SetFillColumnsAnimationVelocities();
            var polyominoFactory = ActionPhaseManager.Instance.polyominoFactory;
            float startDelay = _boardModel.boardData.gameOverAnimationStartDelay;

            for (int c = 0; c < _grid.GetLength(1); c++)
            {
                float currentDelay = fillColumnsAnimationDelays[c] + startDelay;
                
                for (int r = _grid.GetLength(0) - 1 ; r >= 0 ; r--)
                {
                    if (_grid[r, c].currentState != CellState.Used)
                    {
                        _grid[r, c].SetUsed(polyominoFactory.GetRandomPolyominoCellsColor(), currentDelay);
                    }
                    
                    currentDelay += fillColumnsAnimationDelays[c];
                }
            }
        }
        
        private float[] SetFillColumnsAnimationVelocities()
        {
            float[] fillColumnsAnimationDelays = new float[_grid.GetLength(1)];

            for (int i = 0; i < fillColumnsAnimationDelays.Length; i++)
            {
                fillColumnsAnimationDelays[i] = Random.Range(_boardModel.boardData.minColumnFillVelocity, 
                    _boardModel.boardData.maxColumnFillVelocity);
            }

            return fillColumnsAnimationDelays;
        }

        #endregion
    }
}