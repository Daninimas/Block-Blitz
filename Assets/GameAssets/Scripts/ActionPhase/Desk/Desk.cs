using System;
using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

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


        #region Event subscription

        private void OnEnable()
        {
            Polyomino.OnPolyominoSuccessfullyPlaced += OnPolyominoPlaced;
        }

        private void OnDisable()
        {
            Polyomino.OnPolyominoSuccessfullyPlaced -= OnPolyominoPlaced;
        }

        #endregion


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
                
                var newPolyomino = Instantiate(polyominoPrefab, pos);
                newPolyomino.SetUp(_cellsShapes[figureIndex], cellPrefab);
                
                _usablePolyominoes.Add(newPolyomino);
            }
        }

        #region Placed polyomino  management

        private void OnPolyominoPlaced(Polyomino polyomino)
        {
            DestroyUsablePolyomino(polyomino);

            if (_usablePolyominoes.Count == 0)
            {
                CreateUsablePolyominoes();
            }

            CheckGameOver();
        }

        private void CheckGameOver()
        {
            bool canPlaceAnyPolyomino = false;
            foreach (var usablePolyomino in _usablePolyominoes)
            {
                canPlaceAnyPolyomino = 
                    ActionPhaseManager.Instance.board.CheckIfPolyominoCanBePlacedInAllGrid(usablePolyomino.CellsShape);
                
                if (canPlaceAnyPolyomino)
                    break;
            }

            if (!canPlaceAnyPolyomino)
            {
                Log.Trace("------------------ GAME OVER ------------------");
            }
        }

        private void DestroyUsablePolyomino(Polyomino polyomino)
        {
            Destroy(polyomino.gameObject);
            _usablePolyominoes.Remove(polyomino);
        }

        #endregion
    }
}