using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    [CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData", order = 0)]
    public class BoardData : ScriptableObject
    {
        public Vector2Int gridSize = Vector2Int.one;
        
        [Header("Animation data")]
        public float useCellsAnimationStartDelay;
        public float useCellsAnimationInterDelay;
    }
}