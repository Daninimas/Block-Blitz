using UnityEngine;

namespace GameAssets.Scripts.ActionPhase.Score
{
    [CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData", order = 0)]
    public class ScoreData : ScriptableObject
    {
        [SerializeField]
        public int[] multipleLinesFactor = new int[] { 0, 2, 4, 6, 10, 15, 20 };
    }
}