using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardCellsFactory : MonoBehaviour
    {
        [SerializeField] private BoardCell cellPrefab;

        public BoardCell[,] CreateBoardCells(Vector2Int gridSize, Transform cellsParent)
        {
            BoardCell[,] newGrid = new BoardCell[gridSize.x, gridSize.y];
            
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    newGrid[x, y] = Instantiate(cellPrefab, cellsParent);
                }
            }
            
            return newGrid;
        }
    }
}