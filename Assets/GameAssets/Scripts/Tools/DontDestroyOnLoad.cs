using UnityEngine;

namespace GameAssets.Scripts.Tools
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public void OnEnable()
        {
            DontDestroyOnLoad(this);
        }   
    }
}