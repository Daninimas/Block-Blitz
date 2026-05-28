using System;
using GameAssets.Scripts.Managers.SceneInitializer;
using GameAssets.Scripts.Tools;
using UnityEngine;


/// <summary>
/// Implement this interface to initialize the scene
/// </summary>

namespace GameAssets.Scripts.Managers.SceneController
{
    public abstract class SceneInitializerBase : Singleton<SceneInitializerBase>
    {
        [SerializeField] public SceneManagers managerData = new SceneManagers();
        public event Action OnManagersLoaded;


        protected virtual void OnEnable()
        {
            SceneController.Instance.OnSceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnDisable()
        {
            SceneController.Instance.OnSceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// For correct use, use first base.NameMethod when you override method
        /// </summary>
        protected virtual void OnSceneLoaded(SceneController.SceneName scene)
        {
            SceneController.Instance.OnSceneLoaded -= OnSceneLoaded;
            ManagersLoader.Instance.AttachSceneManagers(managerData);
            ManagersLoader.Instance.InitializeSceneManagers(managerData.managersSceneId, OnManagersLoaded);
        }
    }
}