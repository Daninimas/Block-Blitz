using System;
using GameAssets.Scripts.Managers.ScreenManager;
using UnityEngine;

namespace GameAssets.Scripts.UI.Screens.Common
{
    public class LoadScreen : ScreenBase
    {
        [SerializeField] private Animator animator;
        
        public override void Show(Action Show)
        {
            base.Show(Show);
        }

        public override void Hide(Action Hide)
        {
            base.Hide(Hide);
            
            canvasGroup.alpha = 0;
        }
    }
}