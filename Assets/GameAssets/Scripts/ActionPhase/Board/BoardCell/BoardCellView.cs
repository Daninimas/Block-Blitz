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
        
        private Block.BlockColorData _usedBlockColorData;
        
        private Coroutine _updateVisualsCoroutine;


        #region Visual state

        #region Empty
        
        public void SetEmptyVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.gameObject.SetActive(false);
            cellSpriteRenderer.color = emptyColor;
        }

        public void DoHideAnimation()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.DoHideAnimation();
            cellSpriteRenderer.color = emptyColor;
        }

        #endregion
        
        public void SetHighlightedVisuals(Block.BlockColorData blocksColorData)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.SetBlockColors(blocksColorData);
        }
        
        public void SetUnhighlightedVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            usedBlock.SetBlockColors(_usedBlockColorData);
        }

        #region Used
        
        public void SetUsedVisuals(Block.BlockColorData blocksColorData)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            _usedBlockColorData = blocksColorData;
            
            usedBlock.gameObject.SetActive(true);
            usedBlock.SetBlockColors(blocksColorData);
            usedBlock.DoShowAnimation();
        }
        
        public void SetUsedVisualsWithDelay(Block.BlockColorData blocksColorData, float delay)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            _usedBlockColorData = blocksColorData;

            _updateVisualsCoroutine = StartCoroutine(WaitAndSetUsedVisuals(blocksColorData, delay));
        }
        
        private IEnumerator WaitAndSetUsedVisuals(Block.BlockColorData blocksColorData, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            usedBlock.gameObject.SetActive(true);
            usedBlock.SetBlockColors(blocksColorData);
            usedBlock.DoShowAnimation();
            
            _updateVisualsCoroutine = null;
        }

        #endregion

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