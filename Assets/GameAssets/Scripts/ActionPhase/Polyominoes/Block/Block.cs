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


        public void SetBlockColors(BlockColorData blockColorData)
        {
            _currentBlockColorData = blockColorData;
            
            mainSpriteRenderer.color = _currentBlockColorData.mainColor;
            glowSpriteRenderer.color = _currentBlockColorData.glowColor;
            semiGlowSpriteRenderer.color = _currentBlockColorData.semiGlowColor;
            topSpriteRenderer.color = _currentBlockColorData.topColor;
            bottomSpriteRenderer.color = _currentBlockColorData.bottomColor;
            leftSpriteRenderer.color = _currentBlockColorData.leftColor;
            rightSpriteRenderer.color = _currentBlockColorData.rightColor;
        }

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