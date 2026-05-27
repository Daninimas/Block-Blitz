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

    public enum VisualCellState
    {
        Normal,
        Used,
        Highlighted,
        Hovered
    }
    
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private BoardCellView view;

        public CellState CurrentState { private set; get; }


        public void Initialize()
        {
            CurrentState = CellState.Free;
             view.SetVisualState(VisualCellState.Normal);
        }


        public void SetState(CellState state)
        {
            if(state == CurrentState)
                return;
            
            CurrentState = state;
            
            VisualCellState visualState = VisualCellState.Normal;

            switch (state)
            {
                case CellState.Free:
                    visualState = VisualCellState.Normal;
                    break;
                case CellState.Used:
                    visualState = VisualCellState.Used;
                    break;
                case CellState.Hovered:
                    visualState = VisualCellState.Hovered;
                    break;
            }
            
            view.SetVisualState(visualState);
        }

        public void SetVisualState(VisualCellState visualState)
        {
            view.SetVisualState(visualState);
        }
    }
}