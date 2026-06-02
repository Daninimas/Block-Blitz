using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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
        [SerializeField] private int initialSortingOrder;
        [Header("Hover")]
        [SerializeField] private Color hoverColor;
        [Header("Highlighted")]
        [SerializeField] private SpriteRenderer highlightedAuraSpriteRenderer;
        [SerializeField] private ParticleSystem highlightedParticleSystem;
        [SerializeField] private SortingGroup sortingGroup;
        
        private Block.BlockColorData _usedBlockColorData;
        
        private Coroutine _updateVisualsCoroutine;


        public void SetUp()
        {
            usedBlock.SetUp(null, 
                ActionPhaseManager.Instance.blockColorsDirectory.GetHighlightedBlockColor());
            
            HideHighlightedElements();
            SetEmptyVisuals();
        }

        #region Visual state

        #region Empty
        
        public void SetEmptyVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();

            HideHighlightedElements();
            usedBlock.gameObject.SetActive(false);
            cellSpriteRenderer.color = emptyColor;
        }

        public void DoHideAnimation()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            HideHighlightedElements();
            
            usedBlock.DoHideAnimation();
            cellSpriteRenderer.color = emptyColor;
        }

        #endregion

        #region Highlighted
        
        public void SetHighlightedVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            highlightedParticleSystem.gameObject.SetActive(true);
            
            highlightedAuraSpriteRenderer.gameObject.SetActive(true);
            
            sortingGroup.sortingOrder = initialSortingOrder + 1;
            
            usedBlock.Highlight();
        }
        
        public void SetUnhighlightedVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            HideHighlightedElements();
            
            sortingGroup.sortingOrder = initialSortingOrder;
            
            usedBlock.Unhighlight();
        }
        
        private void HideHighlightedElements()
        {
            highlightedParticleSystem.gameObject.SetActive(false);
            highlightedAuraSpriteRenderer.gameObject.SetActive(false);
        }

        #endregion

        #region Used
        
        public void SetUsedVisualsInstant(Block.BlockColorData blocksColorData)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            _usedBlockColorData = blocksColorData;
            
            SetUsedVisuals(blocksColorData);
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
            
            SetUsedVisuals(blocksColorData);
            
            _updateVisualsCoroutine = null;
        }

        private void SetUsedVisuals(Block.BlockColorData blocksColorData)
        {
            HideHighlightedElements();
            
            usedBlock.gameObject.SetActive(true);
            usedBlock.SetBlockColors(blocksColorData);
            usedBlock.DoShowAnimation();
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