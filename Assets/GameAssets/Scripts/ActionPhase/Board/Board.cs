using System;
using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class Board
    {
        private readonly BoardView _boardView;
        private readonly BoardModel _boardModel;
        
        private readonly BoardCell[,] _grid;
        
        private readonly List<BoardCell> _hoveredCells = new ();
        
        private readonly List<int> _highlightedFullRows = new ();
        private readonly List<int> _highlightedFullColumns = new ();
        
        public event Action<List<int>, List<int>> OnScoredFullRowsAndColumns;

        #region Construction
        
        Board(BoardView view, BoardModel boardModel)
        {
            _boardView = view;
            _boardModel = boardModel;
            
            _grid = ActionPhaseManager.Instance.boardCellsFactory.CreateBoardCells(_boardModel.boardData.gridSize, 
                _boardView.GetCellsParent());
            
            _boardView.InitializeData(_boardModel.boardData.gridSize);
        }

        public class Builder
        {
            public Board Build(BoardView view, BoardData boardData)
            {
                return new Board(view, new BoardModel(boardData));
            }
        }
        
        #endregion
        
        public bool HoverByPolyomino(Polyomino polyomino)
        {
            ClearHoveredCells();
            ClearHighlightedRowsAndColumns();

            if (!_boardView.IsPositionInsideRect(polyomino.CellsContainerTransform.position))
                return false;
            
            Vector3 polyominoTopLeftCorner = polyomino.GetTopLeftCellPosition();
            
            var localRelativePos = _boardView.GetRelativePosToTopLeftCorner(polyominoTopLeftCorner);
            Vector2Int gridPos = new ()
            {
                x = (int)Tools.Tools.NormalizeValues(0f, _grid.GetLength(1), 0f, 1f, localRelativePos.x),
                y = (int)Tools.Tools.NormalizeValues(0f, _grid.GetLength(0), 0f, 1f, localRelativePos.y)
            };

            (bool, List<Vector2Int>) canPlacePolyominoInPos = CanPlacePolyominoInPos(polyomino.CellsShape, gridPos);
            
            if(!canPlacePolyominoInPos.Item1)
                return false;
            
            SetHoveredCells(canPlacePolyominoInPos.Item2);
                
            HighlightFullRows();
            HighlightFullColumns();

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
                             || _grid[gridPosCol, gridPosRow].CurrentState == CellState.Used)
                return false;
            
            return true;
        }

        public void ClearHoveredCells()
        {
            foreach (var hoveredCell in _hoveredCells)
            {
                if(hoveredCell.CurrentState == CellState.Hovered)
                    hoveredCell.SetState(CellState.Free);
            }
            _hoveredCells.Clear();
        }

        private void SetHoveredCells(List<Vector2Int> hoveredCellsIndex)
        {
            foreach (var hoveredCellIndex in hoveredCellsIndex)
            {
                BoardCell hoveredCell = _grid[hoveredCellIndex.y, hoveredCellIndex.x];
                hoveredCell.SetState(CellState.Hovered);
                _hoveredCells.Add(hoveredCell);
            }
        }

        #region Confirm polyomino placement

        public void PlacePolyominoInLastHoveredPos()
        {
            ConfirmHoveredCellsAsPlacedPolyomino();
        }

        private void ConfirmHoveredCellsAsPlacedPolyomino()
        {
            SetHoveredCellsAsUsed();
            ClearHoveredCells();
            
            ScoreFullRowsAndColumns();
            ClearHighlightedRowsAndColumns();
        }

        private void SetHoveredCellsAsUsed()
        {
            foreach (var hoveredCell in _hoveredCells)
            {
                hoveredCell.SetState(CellState.Used);
            }
        }

        private void ScoreFullRowsAndColumns()
        {
            // Rows
            foreach (var highlightedFullRow in _highlightedFullRows)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    _grid[highlightedFullRow, c].SetState(CellState.Free);
                }
            }
            
            // Columns
            foreach (var highlightedFullColumn in _highlightedFullColumns)
            {
                for (int r = 0; r < _grid.GetLength(0); r++)
                {
                    _grid[r, highlightedFullColumn].SetState(CellState.Free);
                }
            }
            
            OnScoredFullRowsAndColumns?.Invoke(_highlightedFullRows, _highlightedFullColumns);
        }

        #endregion
        
        
        
        private void ClearHighlightedRowsAndColumns()
        {
            _highlightedFullRows.Clear();
            _highlightedFullColumns.Clear();
        }

        private void HighlightFullRows()
        {
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                bool isFullRow = true;
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    if (_grid[r, c].CurrentState != CellState.Used && _grid[r, c].CurrentState != CellState.Hovered)
                    {
                        isFullRow = false;
                        break;
                    }
                }
                
                if(isFullRow)
                    _highlightedFullRows.Add(r);
            }
        }

        private void HighlightFullColumns()
        {
            for (int c = 0; c < _grid.GetLength(1); c++)
            {
                bool isFullColumn = true;
                for (int r = 0; r < _grid.GetLength(0); r++)
                {
                    if (_grid[r, c].CurrentState != CellState.Used && _grid[r, c].CurrentState != CellState.Hovered)
                    {
                        isFullColumn = false;
                        break;
                    }
                }
                
                if(isFullColumn)
                    _highlightedFullColumns.Add(c);
            }
        }

        #region Check if polyomino can be placed

        public bool CheckIfPolyominoCanBePlacedInAllGrid(int[,] polyominoShape)
        {
            int polyominoRows = polyominoShape.GetLength(0);
            int polyominoCols = polyominoShape.GetLength(1);

            for (int r = 0; r < _grid.GetLength(0) - polyominoRows; r++)
            {
                for (int c = 0; c < _grid.GetLength(1) - polyominoCols; c++)
                {
                    var gridPos = new Vector2Int(r, c);
                    (bool, List<Vector2Int>) canPlacePolyominoInPos = CanPlacePolyominoInPos(polyominoShape, gridPos);
                    
                    if(canPlacePolyominoInPos.Item1)
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}