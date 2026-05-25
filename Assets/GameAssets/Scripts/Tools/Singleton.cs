using UnityEngine;

namespace GameAssets.Scripts.Tools
{
    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0)
                        _instance = objs[0];
                    if (objs.Length > 1)
                    {
                        Log.Error("Singleton", "There is more than one " + typeof(T).Name + " in the scene.");
                    }
                }
          
                return _instance;
            }
        }
    }
}