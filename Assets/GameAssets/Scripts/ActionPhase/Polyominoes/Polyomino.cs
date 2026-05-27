using System;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace GameAssets.Scripts.ActionPhase
{
    public class Polyomino : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Transform cellsContainer;
        public Transform CellsContainerTransform => cellsContainer;
        
        [SerializeField] BoxCollider2D boxCollider;
        [SerializeField] SortingGroup sortingGroup;

        private int _initialLayerOrder;
        

        public int[,] CellsShape { private set; get; }
        
        private Cell[,] _cells;
        private uint _cellsCount;
        
        private Vector2 _hoverExtraDistance;
        
        private Camera _mainCam;
        
        
        private bool _isOnDrag;
        private bool _canBeDropped;
        
        public static event Action<Polyomino> OnPolyominoSuccessfullyPlaced;

        public void SetUp(int[,] cellsShape, Cell cellPrefab, Vector2 hoverExtraDistance)
        {
            CellsShape = cellsShape;
            _hoverExtraDistance = hoverExtraDistance;
            _initialLayerOrder = sortingGroup.sortingOrder;

            SetColliderDimensions(cellsShape);
            
            CreateCells(cellsShape, cellPrefab);
            
            if (Camera.main != null && Camera.main.GetComponent<PhysicsRaycaster>() == null)
                Log.Error("Polyomino", "Main camera doesn't have a PhysicsRaycaster component, which is required for the drag and drop system to work");
            else
                _mainCam = Camera.main;
        }

        private void SetColliderDimensions(int[,] cellsShape)
        {
            var cellRect = ActionPhaseManager.Instance.CellSize;
            
            float width = cellsShape.GetLength(1) * cellRect.x;
            float height = cellsShape.GetLength(0) * cellRect.y;
            
            boxCollider.size = new Vector2(width, height);
        }


        private void CreateCells(int[,] cellsShape, Cell cellPrefab)
        {
            _cells = new Cell[cellsShape.GetLength(0), cellsShape.GetLength(1)];
            _cellsCount = 0;
            
            var cellSize = ActionPhaseManager.Instance.CellSize;
            Vector2 polyominoCenter = new Vector2(cellSize.x * cellsShape.GetLength(1) / 2f, 
                cellSize.y * cellsShape.GetLength(0) / 2f);
            Vector2 cellCenter = new Vector2(cellSize.x / 2f, cellSize.y / 2f);
            
            for (int r = 0; r < cellsShape.GetLength(0); r++)
            {
                for (int c = 0; c < cellsShape.GetLength(1); c++)
                {
                    if(cellsShape[r, c] == 0)
                        continue;
                    
                    var newCell = Instantiate(cellPrefab, cellsContainer);
                    var cellRectTransform = newCell.GetComponent<Transform>();
                    
                    cellRectTransform.localPosition = new Vector3(c * cellSize.x - polyominoCenter.x + cellCenter.x, 
                        -r * cellSize.y + polyominoCenter.y - cellCenter.y, 0f);

                    _cells[r, c] = newCell;
                    _cellsCount++;
                }
            }
        }

        /*public Cell GetCell(int r, int c)
        {
            if (_cells == null)
            {
                Log.Warning("Polyomino", "Trying to get a cell when the cells array is null");
                return null;
            }

            if (r < 0 || r >= _cells.GetLength(0) || c >= _cells.GetLength(1) || c < 0)
            {
                Log.Warning("Polyomino", $"Trying to get a cell with invalid indexes. r: {r}, c: {c}");
                return null;
            }
            
            return _cells[r, c];
        }*/
        
        public Vector3 GetTopLeftCellPosition()
        {
            Vector3 topLeftCellPosition = new Vector3();
            var cellSize = ActionPhaseManager.Instance.CellSize;
            
            Vector2 polyominoCenter = cellsContainer.position;
            Vector2 cellCenter = new Vector2(cellSize.x / 2f, cellSize.y / 2f);
            
            topLeftCellPosition = new Vector3(polyominoCenter.x - CellsShape.GetLength(1) * cellSize.x / 2f + cellCenter.x, 
                polyominoCenter.y + CellsShape.GetLength(0) * cellSize.y / 2f - cellCenter.y, 0f);
            
            return topLeftCellPosition;
        }
        
        #region Drag and Drop
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isOnDrag = true;
            _canBeDropped = false;
            
            SetSortingGroupOrder(2);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!_isOnDrag)
                return;

            SetInPointerPosition(eventData);

            _canBeDropped = CheckIfCanBeDropped();
        }

        private void SetInPointerPosition(PointerEventData eventData)
        {
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0;

            cellsContainer.position = worldPos + (Vector3)_hoverExtraDistance;
        }

        private bool CheckIfCanBeDropped()
        {
            bool canBeDropped = ActionPhaseManager.Instance.board.HoverByPolyomino(this);
            
            return canBeDropped;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!_isOnDrag)
                return;
            
            _isOnDrag = false;
            ResetSortingGroupOrder();

            if (!_canBeDropped)
            {
                cellsContainer.transform.localPosition = Vector3.zero;
                ActionPhaseManager.Instance.board.ClearHoveredCells();
                return;
            }
            
            ActionPhaseManager.Instance.board.PlacePolyominoInLastHoveredPos();
            
            OnPolyominoSuccessfullyPlaced?.Invoke(this);
        }

        #endregion

        #region Manage sorting group

        private void SetSortingGroupOrder(int order)
        {
            sortingGroup.sortingOrder = order;
        }

        private void ResetSortingGroupOrder()
        {
            sortingGroup.sortingOrder = _initialLayerOrder;
        }

        #endregion

        public uint GetCellsCount()
        {
            return _cellsCount;
        }
    }
}