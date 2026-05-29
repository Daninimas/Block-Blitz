using System;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.Managers.ScreenManager
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenBase : MonoBehaviour, IScreen
    {
        public bool isPersistant;
        public bool isVisible { get; set; }
        public bool wasOpened { get; set; }
        public Action OnShow { get; set; }
        public Action OnHide { get; set; }
        
        //public bool isPersistentEditor;
        public bool isBackBlocked;
        public bool destroyOnBack = true;

        public GameObject owner
        {
            get => gameObject;
            set { }
        }

        internal object dataModel;
        internal CanvasGroup canvasGroup;

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void Setup(object data)
        {
            dataModel = data;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(Action Show)
        {
            if (isVisible)
                Log.Warning("Screen", 
                    $"Cannot show screen [{GetType()}] because is already visible");

            transform.SetAsLastSibling();
            isVisible = true;
            wasOpened = true;
            
            Show?.Invoke();
            OnShow?.Invoke();
        }

        public virtual void Hide(Action Hide)
        {
            Hide?.Invoke();
            OnHide?.Invoke();
            if (!isVisible)
            {
                Log.Warning("Screen", 
                    $"Cannot hide screen [{GetType()}] because is already hidden");
                return;
            }

            isVisible = false;
        }

        public virtual void Annulate()
        {
            canvasGroup.DisableInteraction();
        }

        public virtual void Reactivate()
        {
            canvasGroup.EnableInteraction();
        }
        
        public virtual void Destroy()
        {
            Destroy(gameObject);
        }

        public virtual void OnBack()
        {
            
        }
    }
}