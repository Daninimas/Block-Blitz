using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class PolyominoFactory : MonoBehaviour
    {
        [SerializeField] private Polyomino polyominoPrefab;
        
        [SerializeField] private readonly int[][,] polyominoShapes =
        {
            new int[,]
            {
                { 0, 0, 1 },
                { 1, 1, 1 }
            },
            new int[,]
            {
                { 1, 0, 0 },
                { 1, 1, 1 }
            },
            
            new int[,]
            {
                { 0, 1 },
                { 1, 1 }
            },
            new int[,]
            {
                { 1, 0 },
                { 1, 1 }
            },
            
            new int[,]
            {
                { 0, 0, 1 }, 
                { 0, 0, 1 },
                { 1, 1, 1 }
            },
            new int[,]
            {
                { 1, 0, 0 }, 
                { 1, 0, 0 },
                { 1, 1, 1 }
            },
            
            new int[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 }
            },
            new int[,]
            {
                { 1, 0 },
                { 1, 1 },
                { 1, 0 }
            },
            new int[,]
            {
                { 0, 1 },
                { 1, 1 },
                { 0, 1 }
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
                { 1 }, 
                { 1 },
                { 1 },
                { 1 }
            },
            new int[,]
            {
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
                { 1, 1 }, 
                { 1, 1 }, 
                { 1, 1 }
            },
            new int[,]
            {
                { 1, 1 },
                { 1, 1 }, 
                { 1, 1 }, 
                { 1, 1 }
            },
            new int[,]
            {
                { 1, 1 , 1},
                { 1, 1 , 1}, 
                { 1, 1 , 1}
            },
            
            new int[,]
            {
                { 1, 1, 0 }, 
                { 0, 1, 1 }
            },
            new int[,]
            {
                { 0, 1, 1 }, 
                { 1, 1, 0 }
            },
            
            new int[,]
            {
                { 1, 1 }
            },
            new int[,]
            {
                { 1, 1, 1, 1 }
            },
            new int[,]
            {
                { 1, 1, 1, 1, 1 }
            },
            new int[,]
            {
                { 1 }
            },
            
            new int[,]
            {
                { 1, 0 },
                { 0, 1 }
            },
            new int[,]
            {
                { 0, 1 },
                { 1, 0 }
            },
            
            
            new int[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            },
            new int[,]
            {
                { 0, 0, 1 },
                { 0, 1, 0 },
                { 1, 0, 0 }
            }
        };


        public Polyomino CreateRandomPolyomino(Transform parent)
        {
            int figureIndex = Random.Range(0, polyominoShapes.Length);

            int[,] shape = GetPolyominoShape(figureIndex);
            var cellsColorData = ActionPhaseManager.Instance.blockColorsDirectory.GetRandomBlockColor();
            
            var newPolyomino = Instantiate(polyominoPrefab, parent);
            
            newPolyomino.SetUp(shape, cellsColorData);
                
            return newPolyomino;
        }

        private int[,] GetPolyominoShape(int figureIndex)
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