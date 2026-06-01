using System;
using System.Collections;
using GameAssets.Scripts.ActionPhase.Score;
using GameAssets.Scripts.Managers.Audio;
using GameAssets.Scripts.Managers.ScreenManager;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using GameAssets.Scripts.UI.Screens;
using GameAssets.Scripts.UI.Screens.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class ActionPhaseManager : Singleton<ActionPhaseManager>, IManageable
    {
        public ScoreController scoreController;
        
        [Header("Factories")]
        public BoardCellsFactory boardCellsFactory;
        [FormerlySerializedAs("cellFactory")] public BlockFactory blockFactory;
        public PolyominoFactory polyominoFactory;
        
        [Space(10)]
        [Header("Board configuration")]
        [SerializeField] BoardView boardView;
        [SerializeField] BoardData boardData;
        public Board board;
        
        [Space(10)]
        [Header("Desk configuration")]
        [SerializeField] DeskView deskView;
        [SerializeField] DeskData deskData;
        public Desk desk;
        
        private HUDScreen _hudScreen;
        
        [FormerlySerializedAs("cellSize")]
        [Space(10)]
        [Header("Common game configuration")]
        public Vector2 blockSize = Vector2.one;
        public Vector2 BlockSize => blockSize;
        
        public bool initialized { get; set; }

        public event Action OnGameOver;
        
        
        #region Event subscription

        private void SubscribeEvents()
        {
            UnsubscribeEvents();

            desk.OnUnableToPlaceMorePolyominoes += ManageGameOver;
        }

        private void UnsubscribeEvents()
        {
            desk.OnUnableToPlaceMorePolyominoes -= ManageGameOver;
        }

        #endregion
        
        public IEnumerator LoadAssets()
        {
            // ------- WORLD ELEMENTS -------
            
            board = new Board.Builder()
                .Build(boardView, boardData);
            
            desk = new Desk.Builder()
                .Build(deskView, deskData);
            
            
            // ------- UI Elements -------
            
            _hudScreen = ScreenManager.Instance.Show<HUDScreen>();
            
            scoreController = new ScoreController.Builder()
                .Build(_hudScreen.scoreView);
            
            // ------- Finish initialization -------
            SubscribeEvents();
            AudioManager.Instance.PlayMusic("ActionPhaseMusic");
            
            ScreenManager.Instance.Hide<LoadScreen>();
            initialized = true;
            yield break;
        }

        public void UnloadData()
        {
            throw new System.NotImplementedException();
        }


        #region Game over

        private void ManageGameOver()
        {
            var screenData = new GameOverScreen.GameOverScreenData
            {
                finalScore = scoreController.CurrentScore
            };
            
            ScreenManager.Instance.Show<GameOverScreen>(screenData);
            
            OnGameOver?.Invoke();
        }

        #endregion
    }
}