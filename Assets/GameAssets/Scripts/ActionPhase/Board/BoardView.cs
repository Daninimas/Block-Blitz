using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] Transform cellsParent;
        
        private Rect _dimensions;


        public void SetUp(Vector2Int gridSize)
        {
            var center = cellsParent.position;
            var cellSize = ActionPhaseManager.Instance.CellSize;
            
            var topLeftPos = new Vector3(center.x - (gridSize.x/2f * cellSize.x), 
                center.y + (gridSize.y/2f * cellSize.y), 
                center.z);
            
            _dimensions.Set(topLeftPos.x, topLeftPos.y, (gridSize.x * cellSize.x), -(gridSize.y * cellSize.y));
        }
        
        public Transform GetCellsParent()
        {
            return cellsParent;
        }

        public bool IsPositionInsideRect(Vector3 worldPos)
        {
            if(worldPos.x < _dimensions.xMin  || worldPos.x > _dimensions.xMax || 
               worldPos.y < _dimensions.yMax || worldPos.y > _dimensions.yMin)
                return false;
            
            return true;
        }
        
        public Vector2 GetRelativePosToTopLeftCorner(Vector3 worldPos)
        {
            Vector2 relativePos = new ()
            {
                x = Tools.Tools.NormalizeValues(0f, 1f, _dimensions.xMin, _dimensions.xMax, worldPos.x),
                y = Tools.Tools.NormalizeValues(0f, 1f, _dimensions.yMin, _dimensions.yMax, worldPos.y)
            };

            return relativePos;
        }
    }
}