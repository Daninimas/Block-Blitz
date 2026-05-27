using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardCellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Space(10)]
        [Header("Visual states")]
        [Header("Free")]
        [SerializeField] private Color freeColor;
        [SerializeField] private Sprite freeSprite;
        [Header("Used")]
        [SerializeField] private Color usedColor;
        [Header("Hover")]
        [SerializeField] private Color hoverColor;


        #region Visual state

        public void SetVisualState(CellState state)
        {
            switch (state)
            {
                case CellState.Free:
                    SetFreeVisuals();
                    break;
                case CellState.Hovered:
                    SetHoverVisuals();
                    break;
                case CellState.Used:
                    SetUsedVisuals();
                    break;
            }
        }

        private void SetFreeVisuals()
        {
            spriteRenderer.color = freeColor;
            spriteRenderer.sprite = freeSprite;
        }
        
        private void SetHoverVisuals()
        {
            spriteRenderer.color = hoverColor;
        }
        
        private void SetUsedVisuals()
        {
            spriteRenderer.color = usedColor;
        }

        #endregion
    }
}