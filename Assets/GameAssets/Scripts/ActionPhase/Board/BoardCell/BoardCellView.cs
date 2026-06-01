using System.Collections;
using GameAssets.Scripts.Tools;
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
        [SerializeField] private ParticleSystem highlightedParticleSystem;
        [SerializeField] private SortingGroup sortingGroup;
        
        private Block.BlockColorData _usedBlockColorData;
        
        private Coroutine _updateVisualsCoroutine;


        #region Visual state

        #region Empty
        
        public void SetEmptyVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();

            highlightedParticleSystem.gameObject.SetActive(false);
            usedBlock.gameObject.SetActive(false);
            cellSpriteRenderer.color = emptyColor;
        }

        public void DoHideAnimation()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            highlightedParticleSystem.gameObject.SetActive(false);
            
            usedBlock.DoHideAnimation();
            cellSpriteRenderer.color = emptyColor;
        }

        #endregion
        
        public void SetHighlightedVisuals(Block.BlockColorData blocksColorData)
        {
            StopPreviousUpdateVisualsCoroutine();
            
            highlightedParticleSystem.gameObject.SetActive(true);
            highlightedParticleSystem.startColor = blocksColorData.mainColor;
            
            sortingGroup.sortingOrder = initialSortingOrder + 1;
            
            usedBlock.SetBlockColors(blocksColorData);
        }
        
        public void SetUnhighlightedVisuals()
        {
            StopPreviousUpdateVisualsCoroutine();
            
            highlightedParticleSystem.gameObject.SetActive(false);
            
            sortingGroup.sortingOrder = initialSortingOrder;
            
            usedBlock.SetBlockColors(_usedBlockColorData);
        }

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
            highlightedParticleSystem.gameObject.SetActive(false);
            
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