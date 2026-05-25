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


        /*public bool HoverByPolyomino(RaycastResult hit, int[,] polyominoShape)
        {
            ClearHoveredCells();
            List<Vector2Int> hoveredCells = new List<Vector2Int>();
            
            var localRelativePos = boardView.GetHoverRelativePosition(hit);
            Vector2 centerGridPos = new Vector2();
            centerGridPos.x = Tools.Tools.NormalizeValues(0f, _grid.GetLength(1), 0f, 1f, localRelativePos.x);
            centerGridPos.y = Tools.Tools.NormalizeValues(0f, _grid.GetLength(0), 0f, 1f, localRelativePos.y);
            
            Vector2 polyominoTopLeftCorner = centerGridPos - new Vector2(polyominoShape.GetLength(1)/2f,
                                                                                polyominoShape.GetLength(0)/2f);
            
            Vector2Int polyominoTopLeftCornerRound = new Vector2Int((int)Mathf.Round(polyominoTopLeftCorner.x), 
                (int)Mathf.Round(polyominoTopLeftCorner.y));

            hoveredCells.Add(polyominoTopLeftCornerRound);
            if (IsValidCell(polyominoTopLeftCornerRound))
            {
                SetHoveredCells(hoveredCells);
            }
        }*/
        
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
            
            Log.Trace("hola","gridPos " + gridPos + " localRelativePos "+ localRelativePos);

            hoveredCells.Add(gridPos);
            if (IsValidCell(gridPos))
            {
                SetHoveredCells(hoveredCells);
            }

            return false;
        }

        private bool IsValidCell(Vector2Int gridPos)
        {
            if(gridPos.x < 0 || gridPos.x >= _grid.GetLength(1) 
                             || gridPos.y < 0 || gridPos.y >= _grid.GetLength(0))
                return false;
            
            return true;
        }

        private void ClearHoveredCells()
        {
            foreach (var hoveredCell in _hoveredCells)
            {
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
    }
}