using System.Collections.Generic;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameAssets.Scripts.ActionPhase
{
    public class Polyomino : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform cellsContainer;
        [SerializeField] Image raycastImage; // TODO: Cambiar al view

        private Cell[,] _cells;
        
        private int[,] _cellsShape;
        
        private bool _isOnDrag;
        private bool _canBeDropped;

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
            
            raycastImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!_isOnDrag)
                return;
        
            cellsContainer.transform.position = new Vector3(eventData.position.x, eventData.position.y + 200f);

            _canBeDropped = CheckIfCanBeDropped(eventData);
        }

        private bool CheckIfCanBeDropped(PointerEventData onDragEnventData)
        {
            /*PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            //Set the Pointer Event Position to that of the game object
            pointerEventData.position = cellsContainer.transform.position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and Polyomino center position
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var hit in results)
            {
                
                RectTransform rect = hit.gameObject.GetComponent<RectTransform>();
                if (rect == null)
                    continue;

                if (hit.gameObject.TryGetComponent(out Board board))
                {
                    board.HoverByPolyomino(hit, _cellsShape);
                }
            }*/

            ActionPhaseManager.Instance.board.HoverByPolyomino(cellsContainer, _cellsShape);
            
            return false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!_isOnDrag)
                return;
            
            raycastImage.raycastTarget = true;
            
            cellsContainer.transform.localPosition = Vector3.zero;
        }

        #endregion
        
    }
}