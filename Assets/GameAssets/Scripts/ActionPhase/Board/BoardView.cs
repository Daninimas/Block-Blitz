using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] RectTransform cellsParent;
        
        public RectTransform GetCellsParent()
        {
            return cellsParent;
        }

        public bool IsPositionInsideRect(Vector3 worldPos)
        {
            Vector2 localPoint = cellsParent.InverseTransformPoint(worldPos);
            Rect cellsParentRect = cellsParent.rect;
            
            if(localPoint.x < cellsParentRect.xMin  || localPoint.x > cellsParentRect.xMax || 
               localPoint.y < cellsParentRect.yMin || localPoint.y > cellsParentRect.yMax)
                return false;
            
            return true;
        }
        
        public Vector2 GetHoverRelativePosition(Vector3 worldPos)
        {
            Vector2 localPoint = cellsParent.InverseTransformPoint(worldPos);
            
            Vector2 relativePos = new Vector2();
            relativePos.x = Tools.Tools.NormalizeValues(0f, 1f, cellsParent.rect.xMin, cellsParent.rect.xMax, localPoint.x);
            relativePos.y = Tools.Tools.NormalizeValues(0f, 1f, cellsParent.rect.yMin, cellsParent.rect.yMax, -localPoint.y);
            
            return relativePos;
        }
    }
}