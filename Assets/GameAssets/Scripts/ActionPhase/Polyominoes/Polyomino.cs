using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameAssets.Scripts.ActionPhase
{
    public class Polyomino : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform cellsContainer;

        private Cell[,] _cells;
        
        private int[,] _cellsShape;
        
        private bool _isOnDrag;
        private bool _canBeDropped;
        
        public static event Action<Polyomino> OnPolyominoSuccessfullyPlaced;

        public void SetUp(int[,] cellsShape, Cell cellPrefab)
        {
            _cellsShape = cellsShape;

            SetContainerDimensions(cellsShape, cellPrefab);
            
            CreateCells(cellsShape, cellPrefab);
        }

        private void SetContainerDimensions(int[,] cellsShape, Cell cellPrefab)
        {
            var cellRect = cellPrefab.GetComponent<RectTransform>().rect;
            
            float width = cellsShape.GetLength(1) * cellRect.width;
            float height = cellsShape.GetLength(0) * cellRect.height;
            
            cellsContainer.sizeDelta = new Vector2(width, height);
            cellsContainer.pivot = new Vector2(0.5f, 0.5f);
            cellsContainer.localPosition = Vector3.zero;
        }


        private void CreateCells(int[,] cellsShape, Cell cellPrefab)
        {
            _cells = new Cell[cellsShape.GetLength(0), cellsShape.GetLength(1)];
            
            for (int r = 0; r < cellsShape.GetLength(0); r++)
            {
                for (int c = 0; c < cellsShape.GetLength(1); c++)
                {
                    if(cellsShape[r, c] == 0)
                        continue;
                    
                    var newCell = Instantiate(cellPrefab, cellsContainer);
                    var cellRectTransform = newCell.GetComponent<RectTransform>();
                    
                    cellRectTransform.anchoredPosition = new Vector3(c * cellRectTransform.rect.width, 
                        -r * cellRectTransform.rect.height, 0f);

                    _cells[r, c] = newCell;
                }
            }
        }
        
        #region Drag and Drop
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isOnDrag = true;
            _canBeDropped = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!_isOnDrag)
                return;
        
            cellsContainer.transform.position = new Vector3(eventData.position.x, eventData.position.y + 200f);

            _canBeDropped = CheckIfCanBeDropped();
        }

        private bool CheckIfCanBeDropped()
        {
            bool canBeDropped = ActionPhaseManager.Instance.board.HoverByPolyomino(cellsContainer, _cellsShape);
            
            return canBeDropped;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!_isOnDrag)
                return;
            
            _isOnDrag = false;

            if (!_canBeDropped)
            {
                cellsContainer.transform.localPosition = Vector3.zero;
                ActionPhaseManager.Instance.board.ClearHoveredCells();
            }
            
            ActionPhaseManager.Instance.board.ConfirmHoveredCellsAsPlacedPolyomino();
            
            OnPolyominoSuccessfullyPlaced?.Invoke(this);
        }

        #endregion
        
    }
}