using System.Collections;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class BoardCellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cellSpriteRenderer;
        [SerializeField] private Block usedBlock;
        
        [Space(10)]
        [Header("Visual states")]
        [Header("Empty")]
        [SerializeField] private Color emptyColor;
        [Header("Hover")]
        [SerializeField] private Color hoverColor;
        
        private Coroutine _updateVisualsCoroutine;


        #region Visual state

        public void SetEmptyVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.gameObject.SetActive(false);
            cellSpriteRenderer.color = emptyColor;
        }
        
        public void SetHighlightedVisuals(Block.BlockColorData blocksColorData)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.SetBlockColors(blocksColorData);
        }
        
        public void SetUsedVisuals(Block.BlockColorData blocksColorData)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.gameObject.SetActive(true);
            usedBlock.SetBlockColors(blocksColorData);
        }
        
        public void SetUsedVisualsWithDelay(Block.BlockColorData blocksColorData, float delay)
        {
            StopPreviousUpdateVisualsCoroutine();

            _updateVisualsCoroutine = StartCoroutine(WaitAndSetUsedVisuals(blocksColorData, delay));
        }
        
        private IEnumerator WaitAndSetUsedVisuals(Block.BlockColorData blocksColorData, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            usedBlock.gameObject.SetActive(true);
            usedBlock.SetBlockColors(blocksColorData);
            
            _updateVisualsCoroutine = null;
        }

        public void SetHoveredVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            cellSpriteRenderer.color = hoverColor;
        }

        #endregion

        private void StopPreviousUpdateVisualsCoroutine()
        {
            if (_updateVisualsCoroutine != null)
            {
                StopCoroutine(_updateVisualsCoroutine);
                _updateVisualsCoroutine = null;
            }
        }
    }
}