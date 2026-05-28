using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class Cell : MonoBehaviour
    {
        [System.Serializable]
        public class CellColorData
        {
            public Color mainColor;
            public Color glowColor;
            public Color semiGlowColor;
            public Color topColor;
            public Color bottomColor;
            public Color leftColor;
            public Color rightColor;
        }
        
        [SerializeField] private SpriteRenderer mainSpriteRenderer;
        [SerializeField] private SpriteRenderer glowSpriteRenderer;
        [SerializeField] private SpriteRenderer semiGlowSpriteRenderer;
        [SerializeField] private SpriteRenderer topSpriteRenderer;
        [SerializeField] private SpriteRenderer bottomSpriteRenderer;
        [SerializeField] private SpriteRenderer leftSpriteRenderer;
        [SerializeField] private SpriteRenderer rightSpriteRenderer;
        
        private CellColorData _currentCellColorData;


        public void SetCellColors(CellColorData cellColorData)
        {
            _currentCellColorData = cellColorData;
            
            mainSpriteRenderer.color = _currentCellColorData.mainColor;
            glowSpriteRenderer.color = _currentCellColorData.glowColor;
            semiGlowSpriteRenderer.color = _currentCellColorData.semiGlowColor;
            topSpriteRenderer.color = _currentCellColorData.topColor;
            bottomSpriteRenderer.color = _currentCellColorData.bottomColor;
            leftSpriteRenderer.color = _currentCellColorData.leftColor;
            rightSpriteRenderer.color = _currentCellColorData.rightColor;
        }
    }
}