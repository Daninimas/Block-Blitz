using System.Collections;
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

        private Coroutine _setVisualStateWithDelayCoroutine;


        public void Initialize()
        {
            CurrentState = CellState.Free;
             view.SetVisualState(VisualCellState.Normal);
        }


        public void SetState(CellState state, float setVisualStateDelay = 0f)
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
            
            UpdateVisualState(visualState, setVisualStateDelay);
        }

        public void UpdateVisualState(VisualCellState visualState, float setVisualStateDelay = 0f)
        {
            if(_setVisualStateWithDelayCoroutine != null)
                StopCoroutine(_setVisualStateWithDelayCoroutine);
            
            if(setVisualStateDelay == 0f)
            {
                view.SetVisualState(visualState);
            }
            else
            {
                _setVisualStateWithDelayCoroutine = StartCoroutine(SetVisualStateWithDelay(visualState, setVisualStateDelay));
            }
        }

        public IEnumerator SetVisualStateWithDelay(VisualCellState visualState, float setVisualStateDelay)
        {
            yield return new WaitForSeconds(setVisualStateDelay);
            
            view.SetVisualState(visualState);
            
            _setVisualStateWithDelayCoroutine = null;
        }
    }
}