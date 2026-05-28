using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;

namespace GameAssets.Scripts.Managers.SceneInitializer
{
    [Serializable]
    public class SceneManagers
    {
        public string managersSceneId;
        public MonoBehaviour[] managers;
        internal Action OnLoaded;
        internal Action OnUnloaded;
    }

    [DefaultExecutionOrder(0)]
    public class ManagersLoader : Singleton<ManagersLoader>
    {
        #region serialized private variables

        [SerializeField] private SceneManagers[] scenesManagers;

        [Space(10)]

        #endregion

        #region private variables

        private Coroutine _initializeRoutine;

        #endregion
        
        

        #region Event subscription
        private void OnEnable()
        {
            SceneController.SceneController.Instance.OnSceneLoaded += ClearNullComponents;
        }
        
        private void OnDisable()
        {
            SceneController.SceneController.Instance.OnSceneLoaded += ClearNullComponents;
        }
        #endregion
        
        
        #region API

        /// <summary>
        /// Load the IManagers systems, if you need to upload a manager, use the attach method
        /// </summary>
        /// <param name="managersSceneId">The scene Id of the manager that are going to be loaded</param>
        /// <param name="OnLoaded">Event launched when the managers are loaded</param>
        public void InitializeSceneManagers(string managersSceneId, Action OnLoaded)
        {
            SceneManagers sceneManagers = scenesManagers.FirstOrDefault(p => p.managersSceneId == managersSceneId);

            if (sceneManagers == null)
            {
                Log.Error("ManagersLoader", $"The scene manager you are trying to load does not exist. Id: {managersSceneId}");
                return;
            }

            sceneManagers.OnLoaded = OnLoaded;
            IManageable[] availableManagers = CheckThatMonoBehavioursAreManagers(sceneManagers);
            StartManagersLoad(availableManagers, sceneManagers.OnLoaded);
        }


        public void UnloadSceneManagers(string managersSceneId, Action OnUnloaded)
        {
            SceneManagers data = scenesManagers.FirstOrDefault(p => p.managersSceneId == managersSceneId);
            if (data == null)
            {
                Log.Error("ManagersLoader", $"The scene managers you are trying to unload does not exist. Id: {managersSceneId}");
                return;
            }

            data.OnUnloaded = OnUnloaded;
            UnloadManagers(data);
        }

        public void AttachSceneManagers(SceneManagers data)
        {
            scenesManagers ??= Array.Empty<SceneManagers>();

            var sceneManagers = scenesManagers.ToList();
            sceneManagers.Add(data);
            scenesManagers = sceneManagers.ToArray();
        }

        public void DetachSceneManagers(SceneManagers data)
        {
            scenesManagers ??= Array.Empty<SceneManagers>();

            if (!scenesManagers.Contains(data))
            {
                Log.Error("Game", "The item you are trying to unbind does not exist in the system collection");
                return;
            }

            var sceneManagers = scenesManagers.ToList();
            sceneManagers.Remove(data);
            scenesManagers = sceneManagers.ToArray();
        }

        #endregion
        
        
        #region System

        private void ClearNullComponents(SceneController.SceneController.SceneName scene)
        {
            var data = scenesManagers.ToList();
            for (var index = 0; index < data.Count; index++)
            {
                var dataManager = data[index];

                for (int i = 0; i < dataManager.managers.Length; i++)
                {
                    var monoBehaviourData = dataManager.managers[i];
                    if (monoBehaviourData == null)
                    {
                        data.RemoveAt(index);
                        break;
                    }
                }
            }

            scenesManagers = data.ToArray();
        }

        private IManageable[] CheckThatMonoBehavioursAreManagers(SceneManagers sceneManagers)
        {
            List<IManageable> availableManagers = new List<IManageable>();

            foreach (var data in sceneManagers.managers)
            {
                IManageable manager = data.GetComponent<IManageable>();
                if (manager != null)
                {
                    availableManagers.Add(manager);
                }
            }

            if (availableManagers.Count != sceneManagers.managers.Length)
            {
                List<IManageable> conflicts =
                    sceneManagers.managers.GroupBy(x => x.GetComponent<IManageable>() == null) as List<IManageable>;

                conflicts?.ForEach(x =>
                    Log.Error("ManagersLoader",
                        $"Conflict detected, {x.GetType()} dont contain definition IManageable"));
                return null;
            }

            return availableManagers.ToArray();
        }

        private void StartManagersLoad(IManageable[] availableManagers, Action OnLoaded)
        {
            if (_initializeRoutine != null)
            {
                StopCoroutine(_initializeRoutine);
                _initializeRoutine = StartCoroutine(InitializeManagers(availableManagers, OnLoaded));
            }
            else _initializeRoutine = StartCoroutine(InitializeManagers(availableManagers, OnLoaded));
        }

        private void UnloadManagers(SceneManagers sceneManagers)
        {
            IManageable[] availableManagers = CheckThatMonoBehavioursAreManagers(sceneManagers);

            for (int i = 0; i < availableManagers.Length; i++)
            {
                if (!availableManagers[i].initialized)
                {
                    Log.Error("ManagersLoader", availableManagers.GetType().FullName + " is NOT LOADED");
                    break;
                }

                availableManagers[i].initialized = false;
                availableManagers[i].UnloadData();
            }

            if (availableManagers.Count(x => x.initialized) == availableManagers.Length)
                sceneManagers.OnUnloaded?.Invoke();
        }


        /// <summary>
        /// Use a routine to initialize the assets load one by one in order
        /// </summary>
        private IEnumerator InitializeManagers(IManageable[] managers, Action onLoaded)
        {
            Log.Info("ManagersLoader", $"Loading managers... [{managers.Length}]");
            foreach (var manager in managers)
            {
                if (manager.initialized) continue;

                Log.Info("ManagersLoader", $"Loading manager... {manager.GetType()}");

                yield return manager.LoadAssets();

                Log.Info("ManagersLoader", $"Manager loaded... {manager.GetType()}");
            }

            if (managers.Any(x => !x.initialized))
                Log.Error("ManagersLoader", "At least one manager has not been initialized");

            onLoaded?.Invoke();
            _initializeRoutine = null;
        }

        #endregion
    }
}