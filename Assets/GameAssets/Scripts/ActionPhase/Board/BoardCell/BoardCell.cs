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
        
        private CellState _currrentState = CellState.Free;


        public void SetState(CellState state)
        {
            if(_currrentState == state)
                return;
            
            _currrentState = state;
            
            view.SetVisualState(state);
        }
    }
}