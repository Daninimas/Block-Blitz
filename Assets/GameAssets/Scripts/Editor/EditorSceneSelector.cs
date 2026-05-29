using UnityEditor;
using UnityEditor.SceneManagement;

namespace GameAssets.Scripts.Editor
{
    public class EditorSceneSelector
    {
#if UNITY_EDITOR
        [MenuItem("Scenes/1- MainMenu")]
        public static void LoadLogin() { OpenScene("Assets/GameAssets/Scenes/MainMenu.unity"); }
        [MenuItem("Scenes/2- ActionPhase")]
        public static void LoadActionPhase() { OpenScene("Assets/GameAssets/Scenes/ActionPhase.unity"); }
        
        private static void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }
#endif
    }
}