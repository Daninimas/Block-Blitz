using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    [CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData", order = 0)]
    public class BoardData : ScriptableObject
    {
        public Vector2Int gridSize = Vector2Int.one;
        
        [Header("Use cell animation data")]
        public float useCellsAnimationStartDelay;
        public float useCellsAnimationInterDelay;
        
        [Header("Game Over animation data")]
        public float gameOverAnimationStartDelay;
        public float maxColumnFillVelocity;
        public float minColumnFillVelocity;
    }
}