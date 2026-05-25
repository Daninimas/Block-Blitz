using System.Collections;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class ActionPhaseManager : Singleton<ActionPhaseManager>, IManageable
    {
        [SerializeField] BoardCellsFactory boardCellsFactory;
        public Board board;
        [SerializeField] Vector2Int gridSize = Vector2Int.one;
        [SerializeField] Desk desk;
        
        
        //TODO: Delete
        public void Start()
        {
            StartCoroutine(LoadAssets());
        }
        
        
        public bool initialized { get; set; }
        public IEnumerator LoadAssets()
        {
            board.SetUp(gridSize, boardCellsFactory);
            desk.SetUp();
            
            yield break;
        }

        public void UnloadData()
        {
            throw new System.NotImplementedException();
        }
    }
}