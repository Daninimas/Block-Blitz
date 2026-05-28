using UnityEngine;

namespace GameAssets.Scripts.Managers.ScreenManager
{
    public class ScreenRootHook : MonoBehaviour
    {
        private void OnEnable()
        {
            ScreenManager.Instance.root = this.gameObject.transform;
        }
    }
}