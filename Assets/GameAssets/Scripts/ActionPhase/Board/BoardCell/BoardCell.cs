using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public enum CellState
    {
        Free,
        Used,
        Hovered
    }
    
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private BoardCellView view;

        public CellState CurrentState { private set; get; } = CellState.Free;


        public void SetState(CellState state)
        {
            if(CurrentState == state)
                return;
            
            CurrentState = state;
            
            view.SetVisualState(state);
        }
    }
}