using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;

namespace GameAssets.Scripts.Managers.ScreenManager
{
    public class ScreenManager : Singleton<ScreenManager>, IManageable
    {
        private Stack<ScreenBase> _currentOpenedScreens = new Stack<ScreenBase>();

        private List<ScreenBase> _currentInstanciatedScreens = new List<ScreenBase>();

        public Transform root;

        private const string route = "UI/Screens/";
        private const string prefix = "UI_";

        private bool _isBackBlocked;

        #region IManageable

        public bool initialized { get; set; }

        public IEnumerator LoadAssets()
        {
            initialized = true;
            yield break;
        }

        public void UnloadData()
        {
        }

        #endregion

        #region Show

        public T Show<T>(object data = null, Action OnShow = null) where T : ScreenBase
        {
            Log.Info("Screen", $"Trying to show {typeof(T)}");

            ScreenBase screen = SetUpScreen(data, typeof(T));

            if (_currentOpenedScreens.Any())
                _currentOpenedScreens.Peek()?.Annulate();

            this.OnShow(screen);
            screen.Show(OnShow);

            Log.Info("Screen", $"{typeof(T)} shown");

            return screen.owner.GetComponent<ScreenBase>() as T;
        }

        private void OnShow(ScreenBase screen)
        {
            // If screen is visible and not on peek
            if (screen.isVisible && screen.GetType() != _currentOpenedScreens.Peek().GetType())
            {
                _currentOpenedScreens =
                    new Stack<ScreenBase>(_currentOpenedScreens.Where(p => p.GetType() != screen.GetType()).Reverse());
                _currentOpenedScreens.Push(screen);
            }
            else if (!screen.isVisible)
            {
                _currentOpenedScreens.Push(screen);
            }

            // If screen reopened
            if (screen.wasOpened)
            {
                screen.Reactivate();
            }
        }

        #endregion

        #region Hide

        public void Hide<T>(Action OnHide = null)
        {
            ScreenBase screen =
                _currentInstanciatedScreens.FirstOrDefault(p => p.GetType().ToString() == typeof(T).ToString());
            HideScreen(screen, typeof(T), OnHide);
        }

        public void Back(Action OnBack = null)
        {
            if (_isBackBlocked) return;
            (ScreenBase, Type) currentScreen = GetCurrentOpenedScreen();
            if (currentScreen.Item1 == null || currentScreen.Item2 == null || currentScreen.Item1.isBackBlocked) return;
            //HideScreen(currentScreen.Item1, currentScreen.Item2, OnBack);

            currentScreen.Item1.OnBack();

            if (currentScreen.Item1.destroyOnBack)
            {
                DestroyScreen(currentScreen.Item1, currentScreen.Item2, OnBack);
            }
            else
            {
                HideScreen(currentScreen.Item1, currentScreen.Item2, OnBack);
            }
        }

        private void HideScreen(ScreenBase screen, Type T, Action OnHide = null)
        {
            Log.Info("Screen", $"Trying to hide {T}");

            if (!_currentOpenedScreens.Any())
            {
                Log.Error("Screen", $"There are not opened screens so you cannot hide Screen {T}");
                return;
            }

            if (screen == null)
            {
                Log.Warning("Screen", $"Screen {T} does not exist");
                return;
            }

            if (!screen.isVisible)
            {
                Log.Warning("Screen", $"Screen {T} is not visible");
                return;
            }

            screen.Annulate();

            this.OnHide(screen, OnHide);

            Log.Info("Screen", $"{T} hidden");
        }

        private void OnHide(ScreenBase screen, Action OnHide = null)
        {
            screen.Hide(OnHide);
            // If screen is not on peek
            if (screen.GetType() != _currentOpenedScreens.Peek().GetType())
            {
                // Reverse needed in order to maintain the stack order
                _currentOpenedScreens = new Stack<ScreenBase>(
                    _currentOpenedScreens.Where(p => p.GetType() != screen.GetType()).Reverse());
            }
            else
                _currentOpenedScreens.Pop();

            if (_currentOpenedScreens.Any())
            {
                var currentOpenedScreen = _currentOpenedScreens.Peek();
                currentOpenedScreen.Reactivate();
                Log.Info("Screen", $"{currentOpenedScreen} is currently visible");
            }
        }

        #endregion

        #region Destroy

        public void Destroy<T>(Action OnDestroy = null) where T : ScreenBase
        {
            var screen =
                _currentInstanciatedScreens.FirstOrDefault(p => p.GetType().ToString() == typeof(T).ToString());

            DestroyScreen(screen, typeof(T), OnDestroy);
        }

        private void DestroyScreen(ScreenBase screen, Type T, Action OnDestroy = null)
        {
            Log.Info("Screen", $"Trying to destroy {T}");

            if (screen == null)
            {
                Log.Warning("Screen", $"Screen {T} does not exist");
                return;
            }

            HideScreen(screen, T);
            _currentInstanciatedScreens.Remove(screen);
            screen.Destroy();

            Log.Info("Screen", $"{T} destroyed");
        }

        #endregion

        public bool IsVisible<T>() where T : ScreenBase
        {
            ScreenBase screen =
                _currentInstanciatedScreens.FirstOrDefault(p => p.GetType().ToString() == typeof(T).ToString());

            if (screen == null)
                return false;

            return screen.isVisible;
        }

        public T GetScreen<T>() where T : ScreenBase
        {
            string screenType = typeof(T).ToString();
            ScreenBase screen = _currentInstanciatedScreens.FirstOrDefault(p => p.GetType().ToString() == screenType);

            if (screen == null)
            {
                Log.Warning("ScreenManager", $"Trying to get a screen that does not exist: {typeof(T)}");
                return null;
            }

            return screen.owner.GetComponent<ScreenBase>() as T;
        }

        public T SetUpScreen<T>() where T : ScreenBase
        {
            ScreenBase screen = SetUpScreen(null, typeof(T));
            return screen.owner.GetComponent<ScreenBase>() as T;
        }

        public (ScreenBase, Type) GetCurrentOpenedScreen()
        {
            if (_currentOpenedScreens.Count == 0)
            {
                Log.Warning("Screen", "Cannot get current opened screen because there is no screen opened");
                return (null, null);
            }

            ScreenBase screen = _currentOpenedScreens.Peek();
            Type type = screen.GetType();

            return (screen, type);
        }

        private ScreenBase SetUpScreen(object data, Type type)
        {
            string screenType = type.ToString();
            
            ScreenBase screen = _currentInstanciatedScreens.FirstOrDefault(p => p.GetType().ToString() == screenType);
            GameObject instance;

            if (screen == null)
                instance = Instantiate(Resources.Load<GameObject>(route + prefix + type.Name), root);
            else
                instance = screen.owner;

            screen = instance.GetComponent<ScreenBase>();
            
            OnSetUpScreen(screen);
            instance.GetComponent<ScreenBase>().Setup(data);

            return screen;
        }

        private void OnSetUpScreen(ScreenBase screen)
        {
            if (!_currentInstanciatedScreens.Contains(screen))
                _currentInstanciatedScreens.Add(screen);
            
            else screen.owner.transform.SetParent(root);
        }

        #region Scene Load

        #region Event subscription
        public void OnDisable()
        {
            SceneController.SceneController.Instance.OnSceneLoaded -= OnSceneLoaded;
        }

        public void OnEnable()
        {
            SceneController.SceneController.Instance.OnSceneLoaded += OnSceneLoaded;
        }
        
        #endregion

        private void OnSceneLoaded(SceneController.SceneController.SceneName scene)
        {
            ClearOpenedScreens();
        }

        private void ClearOpenedScreens()
        {
            Log.Info("ScreenManager", "Clearing opened screens...");
            if (_currentOpenedScreens == null) return;
            
            _currentOpenedScreens.Clear();
            _currentInstanciatedScreens.Clear();
        }

        #endregion
    }
}