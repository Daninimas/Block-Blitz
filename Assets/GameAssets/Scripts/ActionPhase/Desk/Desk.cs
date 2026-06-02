using System;
using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameAssets.Scripts.ActionPhase
{
    public class Desk : IController
    {
        private readonly DeskModel _deskModel;
        private readonly DeskView _deskView;
        public Vector2 PolyominoHoverExtraDistance => _deskModel.data.polyominoHoverExtraDistance;
        
        private readonly List<Polyomino> _usablePolyominoes = new List<Polyomino>();

        public event Action OnUnableToPlaceMorePolyominoes;


        #region Event subscription

        private void SubscribeEvents()
        {
            UnsubscribeEvents();
            
            Polyomino.OnPolyominoSuccessfullyPlaced += OnPolyominoPlaced;
        }

        private void UnsubscribeEvents()
        {
            Polyomino.OnPolyominoSuccessfullyPlaced -= OnPolyominoPlaced;
        }

        #endregion

        #region Construction
        
        Desk(DeskView view, DeskModel deskModel)
        {
            _deskView = view;
            _deskModel = deskModel;
            
            CreateUsablePolyominoes();

            SubscribeEvents();
        }

        public class Builder
        {
            public Desk Build(DeskView view, DeskData deskData)
            {
                return new Desk(view, new DeskModel(deskData));
            }
        }
        
        #endregion

        #region Destruction

        public void Destroy()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void CreateUsablePolyominoes()
        {
            if (_usablePolyominoes.Count != 0)
            {
                Log.Warning("Desk", "Trying to create usable polyominoes when there are some to USE!");
                return;
            }

            int usablePolyominoesPositions = _deskView.UsablePolyominoesPositions;
            
            var usablePolyominoes = TryToGetUsablePolyominoesShapes(usablePolyominoesPositions, 
                _deskModel.data.maxRetriesToFindUsablePolyominoesShapes);

            for (int i = 0; i < usablePolyominoes.Length; i++)
            {
                var newPolyomino = _deskView.InstantiatePolyominoInUsablePosition(usablePolyominoes[i], i);
                newPolyomino.SetHoverExtraDistance(PolyominoHoverExtraDistance);
                
                _usablePolyominoes.Add(newPolyomino);
            }
        }

        private int[][,] TryToGetUsablePolyominoesShapes(int usableShapesNeeded, int maxRetries)
        {
            int[][,] validPolyominoesShapes = new int[usableShapesNeeded][,];
            int currentUsablePolyominoesFound = 0;
            int currentRetries = 0;
            List<int> checkedPolyominoIndexes = new List<int>();
            var apManager = ActionPhaseManager.Instance;
            
            bool forceRemainingPolyominoes = false; // This forces to add the remaining polyominoes when we reached the max retries

            while (currentUsablePolyominoesFound < usableShapesNeeded)
            {
                int shapeIndex = Random.Range(0, apManager.polyominoDirectory.PolyominoShapesCount);
                if (checkedPolyominoIndexes.Contains(shapeIndex) && !forceRemainingPolyominoes)
                    continue;
                
                checkedPolyominoIndexes.Add(shapeIndex);
                
                var polyominoShape = apManager.polyominoDirectory.GetPolyominoShape(shapeIndex);
                
                if (forceRemainingPolyominoes || apManager.board.CheckIfPolyominoCanBePlacedInAllGrid(polyominoShape))
                {
                    validPolyominoesShapes[currentUsablePolyominoesFound] = polyominoShape;
                    currentUsablePolyominoesFound++;
                }
                else
                {
                    Log.Trace("Desk", $"Polyomino shape with index {shapeIndex} is not usable.");
                    
                    currentRetries++;
                    
                    if (currentRetries >= maxRetries)
                    {
                        Log.Warning("Desk", $"Couldn't find enough usable polyominoes shapes after {maxRetries} retries. " +
                                              $"Usable shapes found: {currentUsablePolyominoesFound}/{usableShapesNeeded}");
                        
                        forceRemainingPolyominoes = true;
                    }
                }
            }

            return validPolyominoesShapes;
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
                    ActionPhaseManager.Instance.board.CheckIfPolyominoCanBePlacedInAllGrid(usablePolyomino.blocksShape);
                
                if (canPlaceAnyPolyomino)
                    break;
            }

            if (!canPlaceAnyPolyomino)
            {
                // Game over
                OnUnableToPlaceMorePolyominoes?.Invoke();
            }
        }

        private void DestroyUsablePolyomino(Polyomino polyomino)
        {
            _deskView.DestroyGameObject(polyomino.gameObject);
            _usablePolyominoes.Remove(polyomino);
        }

        #endregion
    }
}