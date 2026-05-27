using System.Collections;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class ActionPhaseManager : Singleton<ActionPhaseManager>, IManageable
    {
        [SerializeField] BoardCellsFactory boardCellsFactory;
        public Board board;
        [SerializeField] Vector2Int gridSize = Vector2Int.one;
        [SerializeField] Vector2 cellSize = Vector2.one;
        
        [Space(10)]
        [Header("Desk configuration")]
        [SerializeField] DeskView deskView;
        [SerializeField] DeskData deskData;
        public Desk desk;
        
        
        public Vector2 CellSize => cellSize;


        //TODO: Delete
        public void Start()
        {
            StartCoroutine(LoadAssets());
        }
        
        
        public bool initialized { get; set; }
        public IEnumerator LoadAssets()
        {
            board.SetUp(gridSize, boardCellsFactory);
            
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