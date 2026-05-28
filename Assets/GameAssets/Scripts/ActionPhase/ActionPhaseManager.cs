using System.Collections;
using GameAssets.Scripts.ActionPhase.Score;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class ActionPhaseManager : Singleton<ActionPhaseManager>, IManageable
    {
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
        
        [Space(10)]
        [Header("Score configuration")]
        [SerializeField] ScoreView scoreView;
        public ScoreController scoreController;
        
        [FormerlySerializedAs("cellSize")]
        [Space(10)]
        [Header("Common game configuration")]
        public Vector2 blockSize = Vector2.one;
        public Vector2 BlockSize => blockSize;


        //TODO: Delete
        public void Start()
        {
            StartCoroutine(LoadAssets());
        }
        
        
        public bool initialized { get; set; }
        public IEnumerator LoadAssets()
        {
            board = new Board.Builder()
                .Build(boardView, boardData);
            
            desk = new Desk.Builder()
                .Build(deskView, deskData);
            
            scoreController = new ScoreController.Builder()
                .Build(scoreView);
            
            yield break;
        }

        public void UnloadData()
        {
            throw new System.NotImplementedException();
        }
    }
}