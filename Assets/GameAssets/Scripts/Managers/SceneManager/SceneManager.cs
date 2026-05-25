using System;
using System.Collections;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game scenes transitions.
/// </summary>

namespace Metrobots.Plugins.SceneController
{
    public class SceneController : Singleton<SceneController>
    {
        public enum SceneName
        {
            MainMenu,
            ActionPhase,
            Intermediate
        }

        public SceneName currentScene
        {
            get;
            private set;
        }

        public SceneName? prevScene
        {
            get;
            private set;
        }
        
        public bool CanLoadNextScene => _canLoadNextScene;
        private bool _canLoadNextScene = false;
        public bool CanChangeScene => _canChangeScene;
        private bool _canChangeScene = true;
        
        private bool _intermediateSceneLoaded = false;

        public event Action<SceneName> OnSceneUnloaded;
        public event Action<SceneName> BeforeSceneLoad;
        public event Action<SceneName> OnSceneLoaded;

        private SceneController()
        {
            currentScene = SceneName.MainMenu;
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += InvokeOnSceneLoaded;
            SceneManager.sceneUnloaded += InvokeOnSceneUnloaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= InvokeOnSceneLoaded;
            SceneManager.sceneUnloaded -= InvokeOnSceneUnloaded;
        }

        private void InvokeOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Log.Info("SceneController", $"Scene, loaded {scene.name}");
            if (scene.name != "Intermediate")
            {
                OnSceneLoaded?.Invoke((SceneName)scene.buildIndex);
            }
            else
            {
                _intermediateSceneLoaded = true;
            }
        }

        private void InvokeOnSceneUnloaded(Scene scene)
        {
            Log.Info("SceneController", $"Scene, Unloaded {scene.name}");
            if (scene.name != "Intermediate")
            {
                OnSceneUnloaded?.Invoke((SceneName)scene.buildIndex);
            }
        }

        public void LoadScene(SceneName scene, float waitTime = 0, bool forceLoad = true)
        {
            if (forceLoad == false && SceneManager.GetSceneByBuildIndex((int)scene).isLoaded) return;
            
            prevScene = currentScene;
            
            StartCoroutine(LoadSceneAsync(scene, waitTime, LoadSceneMode.Single));
        }

        public void ReloadScene(float waitTime = 0)
        {
            StartCoroutine(LoadSceneAsync(currentScene, waitTime, LoadSceneMode.Single));
        }

        private IEnumerator LoadSceneAsync(SceneName scene, float waitTime, LoadSceneMode mode)
        {
            if (waitTime > 0)
                yield return new WaitForSeconds(waitTime);
            
            _canChangeScene = false;
            BeforeSceneLoad?.Invoke(scene);
            
            _intermediateSceneLoaded = false;
            SceneManager.LoadScene((int)SceneName.Intermediate, LoadSceneMode.Single);

            yield return new WaitUntil(() => _intermediateSceneLoaded);
            
            SceneManager.LoadScene((int)scene, mode);

            currentScene = scene;
            _canChangeScene = true;
            _canLoadNextScene = false;

            yield return null;
        }
    }
}