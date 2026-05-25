using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class Desk : MonoBehaviour
    {
        [SerializeField] private Transform[] usablePolyominoesPositions;
        [SerializeField] private Polyomino polyominoPrefab;
        [SerializeField] private Cell cellPrefab; // TODO: Refactor a otro sitio
        
        private List<Polyomino> _usablePolyominoes = new List<Polyomino>();
        
        private readonly int[][,] _cellsShapes =
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


        public void SetUp()
        {
            CreateUsablePolyominoes();
        }

        private void CreateUsablePolyominoes()
        {
            if (_usablePolyominoes.Count != 0)
            {
                Log.Warning("Desk", "Trying to create usable polyominoes when there are some to USE!");
                return;
            }

            foreach (var pos in usablePolyominoesPositions)
            {
                int figureIndex = Random.Range(0, _cellsShapes.Length - 1);
                
                var newPolymino = Instantiate(polyominoPrefab, pos);
                newPolymino.SetUp(_cellsShapes[figureIndex], cellPrefab);
                
                _usablePolyominoes.Add(newPolymino);
            }
        }
    }
}