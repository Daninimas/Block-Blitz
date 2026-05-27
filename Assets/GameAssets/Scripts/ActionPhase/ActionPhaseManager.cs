using System.Collections;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class ActionPhaseManager : Singleton<ActionPhaseManager>, IManageable
    {
        public BoardCellsFactory boardCellsFactory;
        
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
        
        
        public Vector2 cellSize = Vector2.one;
        public Vector2 CellSize => cellSize;


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
            
            yield break;
        }

        public void UnloadData()
        {
            throw new System.NotImplementedException();
        }
    }
}