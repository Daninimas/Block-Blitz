using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardCellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Space(10)]
        [Header("Visual states")]
        [Header("Normal")]
        [SerializeField] private Color normalColor;
        [SerializeField] private Sprite normalSprite;
        [Header("Used")]
        [SerializeField] private Color usedColor;
        [Header("Hover")]
        [SerializeField] private Color hoverColor;
        [Header("Highlighted")]
        [SerializeField] private Color highlightedColor;


        #region Visual state

        public void SetVisualState(VisualCellState state)
        {
            switch (state)
            {
                case VisualCellState.Normal:
                    SetNormalVisuals();
                    break;
                case VisualCellState.Hovered:
                    SetHoverVisuals();
                    break;
                case VisualCellState.Used:
                    SetUsedVisuals();
                    break;
                case VisualCellState.Highlighted:
                    SetHighlightedVisuals();
                    break;
            }
        }

        private void SetNormalVisuals()
        {
            spriteRenderer.color = normalColor;
            spriteRenderer.sprite = normalSprite;
        }
        
        private void SetHoverVisuals()
        {
            spriteRenderer.color = hoverColor;
        }
        
        private void SetUsedVisuals()
        {
            spriteRenderer.color = usedColor;
        }
        
        private void SetHighlightedVisuals()
        {
            spriteRenderer.color = highlightedColor;
        }

        #endregion
    }
}