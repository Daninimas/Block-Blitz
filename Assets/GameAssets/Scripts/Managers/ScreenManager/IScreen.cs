using System;
using UnityEngine;

namespace GameAssets.Scripts.Managers.ScreenManager
{
    public interface IScreen 
    {
        public bool isVisible { get; set; }
        public Action OnShow { get; set; }
        public Action OnHide { get; set; }
        
        public GameObject owner { get; set; }

        public void Annulate();

        public void Reactivate();

        public void Show(Action OnShow=null);

        public void Hide(Action OnHide=null);

        public void Destroy();
        public void OnBack();
    }
}