
namespace GameAssets.Scripts.ActionPhase
{
    public enum CellState
    {
        Free,
        Used,
        Hovered
    }
    
    public class BoardCell
    {
        private BoardCellView _view;
        public CellState currentState { private set; get; }
        
        private Block.BlockColorData _usedBlockColorData;


        #region Construction
        
        private BoardCell(BoardCellView view)
        {
            _view = view;
            
            SetFree();
        }

        public class Builder
        {
            public BoardCell Build(BoardCellView view)
            {
                return new BoardCell(view);
            }
        }
        
        #endregion


        #region Cell states management
        
        public void SetFree()
        {
            currentState = CellState.Free;
            
            _view.SetEmptyVisuals();
        }

        public void SetUsed(Block.BlockColorData blocksColorData, float delay = 0f)
        {
            currentState = CellState.Used;
            
            //_usedBlockColorData = (Block.BlockColorData)blocksColorData.Clone();
            _usedBlockColorData = blocksColorData;
            
            if(delay == 0f)
                _view.SetUsedVisuals(_usedBlockColorData);
            else
                _view.SetUsedVisualsWithDelay(_usedBlockColorData, delay);
        }

        public void SetHovered()
        {
            currentState = CellState.Hovered;
            
            _view.SetHoveredVisuals();
        }

        public void SetHighlighted(Block.BlockColorData blocksColorData)
        {
            _view.SetHighlightedVisuals(blocksColorData);
        }

        public void SetUnhighlighted()
        {
            if (currentState == CellState.Free)
                _view.SetEmptyVisuals();
            else if (currentState == CellState.Used)
                _view.SetUsedVisuals(_usedBlockColorData);
        }

        #endregion

        /*public void UpdateVisualState(VisualCellState visualState, float setVisualStateDelay = 0f)
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
        }*/
    }
}