using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    [CreateAssetMenu(fileName = "PolyominoDirectory", menuName = "ScriptableObjects/PolyominoDirectory", order = 0)]
    public class PolyominoDirectory : ScriptableObject
    {
        public int PolyominoShapesCount => polyominoShapes.Length;
        
        private readonly int[][,] polyominoShapes =
        {
            new int[,] // 0
            {
                { 0, 0, 1 },
                { 1, 1, 1 }
            },
            new int[,] // 1
            {
                { 1, 0, 0 },
                { 1, 1, 1 }
            },
            
            new int[,] // 2
            {
                { 0, 1 },
                { 1, 1 }
            },
            new int[,] // 3
            {
                { 1, 0 },
                { 1, 1 }
            },
            
            new int[,] // 4
            {
                { 0, 0, 1 }, 
                { 0, 0, 1 },
                { 1, 1, 1 }
            },
            new int[,] // 5
            {
                { 1, 0, 0 }, 
                { 1, 0, 0 },
                { 1, 1, 1 }
            },
            
            new int[,] // 6
            {
                { 0, 1, 0 },
                { 1, 1, 1 }
            },
            new int[,] // 7
            {
                { 1, 0 },
                { 1, 1 },
                { 1, 0 }
            },
            new int[,] // 8
            {
                { 0, 1 },
                { 1, 1 },
                { 0, 1 }
            },
            
            new int[,] // 9
            {
                { 1 }, 
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            },
            new int[,] // 10
            {
                { 1 }, 
                { 1 },
                { 1 },
                { 1 }
            },
            new int[,] // 11
            {
                { 1 }, 
                { 1 },
                { 1 }
            },
            
            new int[,] // 12
            {
                { 1, 1 }, 
                { 1, 1 }
            },
            new int[,] // 13
            {
                { 1, 1 }, 
                { 1, 1 }, 
                { 1, 1 }
            },
            new int[,] // 14
            {
                { 1, 1 },
                { 1, 1 }, 
                { 1, 1 }, 
                { 1, 1 }
            },
            new int[,] // 15
            {
                { 1, 1 , 1},
                { 1, 1 , 1}, 
                { 1, 1 , 1}
            },
            
            new int[,] // 16
            {
                { 1, 1, 0 }, 
                { 0, 1, 1 }
            },
            new int[,] // 17
            {
                { 0, 1, 1 }, 
                { 1, 1, 0 }
            },
            
            new int[,] // 18
            {
                { 1, 1 }
            },
            new int[,] // 19
            {
                { 1, 1, 1, 1 }
            },
            new int[,] // 20
            {
                { 1, 1, 1, 1, 1 }
            },
            new int[,] // 21
            {
                { 1 }
            },
            
            new int[,] // 22
            {
                { 1, 0 },
                { 0, 1 }
            },
            new int[,] // 23
            {
                { 0, 1 },
                { 1, 0 }
            },
            
            
            new int[,] // 24
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            },
            new int[,] // 25
            {
                { 0, 0, 1 },
                { 0, 1, 0 },
                { 1, 0, 0 }
            }
        };
        
        
        public int[,] GetPolyominoShape(int figureIndex)
        {
            if(figureIndex < 0 || figureIndex >= polyominoShapes.Length)
            {
                Log.Error("PolyominoFactory", $"Trying to get a polyomino shape with invalid index. figureIndex: {figureIndex}");
                return null;
            }
            
            return polyominoShapes[figureIndex];
        }
    }
}