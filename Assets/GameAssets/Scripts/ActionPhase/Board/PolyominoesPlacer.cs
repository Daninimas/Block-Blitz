using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class PolyominoesPlacer
    {
        private Board _board;
        
        public bool CheckIfPolyominoCanBePlaced(RectTransform polyominoRectTransform)
        {
            _board = polyominoRectTransform.gameObject.GetComponent<Board>();
            
            return false;
        }
    }
}