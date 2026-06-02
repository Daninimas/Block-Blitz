using System;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class Block : MonoBehaviour
    {
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        [System.Serializable]
        public class BlockColorData : ICloneable
        {
            public Color mainColor;
            public Color glowColor;
            public Color semiGlowColor;
            public Color topColor;
            public Color bottomColor;
            public Color leftColor;
            public Color rightColor;
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
        
        [SerializeField] private SpriteRenderer mainSpriteRenderer;
        [SerializeField] private SpriteRenderer glowSpriteRenderer;
        [SerializeField] private SpriteRenderer semiGlowSpriteRenderer;
        [SerializeField] private SpriteRenderer topSpriteRenderer;
        [SerializeField] private SpriteRenderer bottomSpriteRenderer;
        [SerializeField] private SpriteRenderer leftSpriteRenderer;
        [SerializeField] private SpriteRenderer rightSpriteRenderer;
        
        [Header("Animator")]
        [SerializeField] private Animator blockAnimator;
        
        private BlockColorData _currentBlockColorData;
        private BlockColorData _highlightColorData;

        
        public void SetUp(BlockColorData blockColorData, BlockColorData highlightColorData)
        {
            SetBlockColors(blockColorData);
            _highlightColorData = highlightColorData;
        }

        public void SetBlockColors(BlockColorData blockColorData)
        {
            if(blockColorData == null)
                return;
            
            _currentBlockColorData = blockColorData;

            UpdateBlockColors(blockColorData);
        }

        private void UpdateBlockColors(BlockColorData blockColorData)
        {
            mainSpriteRenderer.color = blockColorData.mainColor;
            glowSpriteRenderer.color = blockColorData.glowColor;
            semiGlowSpriteRenderer.color = blockColorData.semiGlowColor;
            topSpriteRenderer.color = blockColorData.topColor;
            bottomSpriteRenderer.color = blockColorData.bottomColor;
            leftSpriteRenderer.color = blockColorData.leftColor;
            rightSpriteRenderer.color = blockColorData.rightColor;
        }

        #region Highlight

        public void Highlight()
        {
            UpdateBlockColors(_highlightColorData);
        }

        public void Unhighlight()
        {
            UpdateBlockColors(_currentBlockColorData);
        }

        #endregion

        #region Animations
        
        public void DoShowAnimation()
        {
            blockAnimator.SetTrigger(Show);
        }
        
        public void DoHideAnimation()
        {
            blockAnimator.SetTrigger(Hide);
        }
        
        #endregion
    }
}