using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameAssets.Scripts.ActionPhase
{
    public class Desk
    {
        private readonly DeskModel _deskModel;
        private readonly DeskView _deskView;
        public Vector2 PolyominoHoverExtraDistance => _deskModel.data.polyominoHoverExtraDistance;
        
        private readonly List<Polyomino> _usablePolyominoes = new List<Polyomino>();


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

        ~Desk()
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

            for (int i = 0; i < usablePolyominoesPositions; i++)
            {
                var newPolyomino = _deskView.InstantiatePolyominoInUsablePosition(i);
                newPolyomino.SetHoverExtraDistance(PolyominoHoverExtraDistance);
                
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
                    ActionPhaseManager.Instance.board.CheckIfPolyominoCanBePlacedInAllGrid(usablePolyomino.blocksShape);
                
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
            _deskView.DestroyGameObject(polyomino.gameObject);
            _usablePolyominoes.Remove(polyomino);
        }

        #endregion
    }
}