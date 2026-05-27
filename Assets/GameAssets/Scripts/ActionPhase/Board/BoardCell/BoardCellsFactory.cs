using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardCellsFactory : MonoBehaviour
    {
        [SerializeField] private BoardCell cellPrefab;

        public BoardCell[,] CreateBoardCells(Vector2Int gridSize, Transform cellsParent)
        {
            int maxRows = gridSize.y;
            int maxCols = gridSize.x;
            
            BoardCell[,] newGrid = new BoardCell[maxRows, maxCols];
            
            for (var c = 0; c < maxCols; c++)
            {
                for (var r = 0; r < maxRows; r++)
                {
                    BoardCell newBoardCell = Instantiate(cellPrefab, cellsParent);
                    
                    newBoardCell.Initialize();
                    newGrid[r, c] = newBoardCell;
                    
                    SetCellInPosition(newBoardCell.transform, r, c, gridSize);
                }
            }
            
            return newGrid;
        }

        private void SetCellInPosition(Transform cellTransform, int r, int c, Vector2Int gridSize)
        {
            Vector2 cellSize = ActionPhaseManager.Instance.CellSize;
            Vector2 cellCenter = new Vector2(cellSize.x / 2f, cellSize.y / 2f);
            
            Vector2 topLeftCorner = new Vector2(gridSize.x * cellSize.x / 2f, gridSize.y * cellSize.y / 2f);
            
            cellTransform.localPosition = new Vector3(c * cellSize.x - topLeftCorner.x + cellCenter.x, 
                -r * cellSize.y + topLeftCorner.y - cellCenter.y, 0f);
        }
    }
}