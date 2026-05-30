using System;
using GameAssets.Scripts.Managers.Audio;
using GameAssets.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class Polyomino : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [FormerlySerializedAs("cellsContainer")] [SerializeField] Transform blockContainer;
        public Transform BlockContainerTransform => blockContainer;
        
        [SerializeField] BoxCollider2D boxCollider;
        [SerializeField] SortingGroup sortingGroup;

        private int _initialLayerOrder;
        

        public int[,] blocksShape { private set; get; }
        
        private Block[,] _blocks;
        private uint _blocksCount;
        
        public Block.BlockColorData blocksColorData { private set; get; }
        
        private Vector2 _hoverExtraDistance;
        
        private Camera _mainCam;
        
        
        private bool _isOnDrag;
        private bool _canBeDropped;
        
        public static event Action<Polyomino> OnPolyominoSuccessfullyPlaced;

        public void SetUp(int[,] blocksShape, Block.BlockColorData blockColorData)
        {
            this.blocksShape = blocksShape;
            this.blocksColorData = blockColorData;
            _initialLayerOrder = sortingGroup.sortingOrder;

            SetColliderDimensions(blocksShape);
            
            CreateBlocks(blocksShape, blockColorData);
            
            if (Camera.main != null && Camera.main.GetComponent<PhysicsRaycaster>() == null)
                Log.Error("Polyomino", "Main camera doesn't have a PhysicsRaycaster component, which is required for the drag and drop system to work");
            else
                _mainCam = Camera.main;
        }

        private void SetColliderDimensions(int[,] blocksShape)
        {
            var blockRect = ActionPhaseManager.Instance.BlockSize;
            
            float width = blocksShape.GetLength(1) * blockRect.x;
            float height = blocksShape.GetLength(0) * blockRect.y;
            
            boxCollider.size = new Vector2(width, height);
        }


        private void CreateBlocks(int[,] blocksShape, Block.BlockColorData blockColorData)
        {
            _blocks = new Block[blocksShape.GetLength(0), blocksShape.GetLength(1)];
            _blocksCount = 0;
            
            var blockSize = ActionPhaseManager.Instance.BlockSize;
            Vector2 polyominoCenter = new Vector2(blockSize.x * blocksShape.GetLength(1) / 2f, 
                blockSize.y * blocksShape.GetLength(0) / 2f);
            Vector2 blockCenter = new Vector2(blockSize.x / 2f, blockSize.y / 2f);
            
            for (int r = 0; r < blocksShape.GetLength(0); r++)
            {
                for (int c = 0; c < blocksShape.GetLength(1); c++)
                {
                    if(blocksShape[r, c] == 0)
                        continue;
                    
                    var newBlock = ActionPhaseManager.Instance.blockFactory.CreateBlock(blockContainer, blockColorData);
                    var blockTransform = newBlock.GetComponent<Transform>();
                    
                    blockTransform.localPosition = new Vector3(c * blockSize.x - polyominoCenter.x + blockCenter.x, 
                        -r * blockSize.y + polyominoCenter.y - blockCenter.y, 0f);

                    _blocks[r, c] = newBlock;
                    _blocksCount++;
                }
            }
        }

        public void SetHoverExtraDistance(Vector2 hoverExtraDistance)
        {
            _hoverExtraDistance = hoverExtraDistance;
        }
        
        public Vector3 GetTopLeftBlockPosition()
        {
            Vector3 topLeftBlockPosition = new Vector3();
            var blockSize = ActionPhaseManager.Instance.BlockSize;
            
            Vector2 polyominoCenter = blockContainer.position;
            Vector2 blockCenter = new Vector2(blockSize.x / 2f, blockSize.y / 2f);
            
            topLeftBlockPosition = new Vector3(polyominoCenter.x - blocksShape.GetLength(1) * blockSize.x / 2f + blockCenter.x, 
                polyominoCenter.y + blocksShape.GetLength(0) * blockSize.y / 2f - blockCenter.y, 0f);
            
            return topLeftBlockPosition;
        }
        
        #region Drag and Drop
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isOnDrag = true;
            _canBeDropped = false;
            
            AudioManager.Instance.PlaySound("DragPolyomino");
            
            SetSortingGroupOrder(sortingGroup.sortingOrder + 1);
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

            blockContainer.position = worldPos + (Vector3)_hoverExtraDistance;
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
                blockContainer.transform.localPosition = Vector3.zero;
                ActionPhaseManager.Instance.board.ClearHoveredCells();
                
                AudioManager.Instance.PlaySound("ReturnPolyomino");
                
                return;
            }
            
            ActionPhaseManager.Instance.board.PlacePolyominoInLastHoveredPos(this);
            
            AudioManager.Instance.PlaySound("PlacePolyomino");
            
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

        public uint GetBlocksCount()
        {
            return _blocksCount;
        }
    }
}