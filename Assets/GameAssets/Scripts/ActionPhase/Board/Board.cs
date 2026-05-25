using System;
using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameAssets.Scripts.ActionPhase
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        
        private BoardCell[,] _grid;
        
        private List<BoardCell> _hoveredCells = new ();


        public void SetUp(Vector2Int gridSize, BoardCellsFactory cellsFactory)
        {
            _grid = cellsFactory.CreateBoardCells(gridSize, boardView.GetCellsParent());
        }
        
        public bool HoverByPolyomino(RectTransform polyominoTransform, int[,] polyominoShape)
        {
            ClearHoveredCells();

            if (!boardView.IsPositionInsideRect(polyominoTransform.position))
                return false;
            
            List<Vector2Int> hoveredCells = new List<Vector2Int>();
            
            Vector3 polyominoTopLeftCorner = polyominoTransform.GetWorldTopLeft();
            
            var localRelativePos = boardView.GetHoverRelativePosition(polyominoTopLeftCorner);
            Vector2Int gridPos = new ()
            {
                x = (int)Tools.Tools.NormalizeValues(0f, _grid.GetLength(1), 0f, 1f, localRelativePos.x),
                y = (int)Tools.Tools.NormalizeValues(0f, _grid.GetLength(0), 0f, 1f, localRelativePos.y)
            };

            bool allCellsAreValid = true;
            for (int r = 0; r < polyominoShape.GetLength(0); r++)
            {
                if (!allCellsAreValid)
                    break;
                
                for (int c = 0; c < polyominoShape.GetLength(1); c++)
                {
                    if(polyominoShape[r, c] == 0)
                        continue;
                    
                    Vector2Int cellPosToCheck = gridPos + new Vector2Int(c, r);

                    if (!IsValidCell(cellPosToCheck))
                    {
                        allCellsAreValid = false;
                        break;
                    }
                    
                    hoveredCells.Add(cellPosToCheck);
                }
            }
            
            if(allCellsAreValid)
                SetHoveredCells(hoveredCells);

            return allCellsAreValid;
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

        public void ConfirmHoveredCellsAsPlacedPolyomino()
        {
            foreach (var hoveredCell in _hoveredCells)
            {
                hoveredCell.SetState(CellState.Used);
            }
            
            ClearHoveredCells();
        }
    }
}