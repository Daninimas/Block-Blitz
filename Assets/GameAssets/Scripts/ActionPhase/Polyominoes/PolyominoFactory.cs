using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class PolyominoFactory : MonoBehaviour
    {
        [SerializeField] private Polyomino polyominoPrefab;
        
        [FormerlySerializedAs("polyominoCellsColors")] [SerializeField] private List<Block.BlockColorData> polyominoBlocksColors;
        
        [SerializeField] private readonly int[][,] polyominoShapes =
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


        public Polyomino CreateRandomPolyomino(Transform parent)
        {
            int figureIndex = Random.Range(0, polyominoShapes.Length);

            int[,] shape = GetPolyominoShape(figureIndex);
            var cellsColorData = GetRandomPolyominoCellsColor();
            
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
        
        private Block.BlockColorData GetPolyominoCellsColor(int colorIndex)
        {
            if(colorIndex < 0 || colorIndex >= polyominoBlocksColors.Count)
            {
                Log.Error("PolyominoFactory", $"Trying to get a polyomino cells color with invalid index. colorIndex: {colorIndex}");
                return null;
            }
            
            return polyominoBlocksColors[colorIndex];
        }
        
        public Block.BlockColorData GetRandomPolyominoCellsColor()
        {
            int colorIndex = Random.Range(0, polyominoBlocksColors.Count);
            return GetPolyominoCellsColor(colorIndex);
        }
    }
}