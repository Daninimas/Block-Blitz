using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    [CreateAssetMenu(fileName = "DeskData", menuName = "ScriptableObjects/DeskData", order = 0)]
    public class DeskData : ScriptableObject
    {
        public Polyomino polyominoPrefab;
        public Cell cellPrefab;
        public Vector2 polyominoHoverExtraDistance = Vector2.zero;
        
        public readonly int[][,] cellsShapes =
        {
            new int[,]
            {
                { 0, 0, 1 }, 
                { 0, 0, 1 },
                { 1, 1, 1 }
            },
            new int[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 }
            },
            new int[,]
            {
                { 1 }, 
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            },
            new int[,]
            {
                { 1, 1 }, 
                { 1, 1 }
            },
            new int[,]
            {
                { 1, 1, 0 }, 
                { 0, 1, 1 }
            },
            new int[,]
            {
                { 1, 1 }
            },
            new int[,]
            {
                { 1 }
            }
            
        };
    }
}