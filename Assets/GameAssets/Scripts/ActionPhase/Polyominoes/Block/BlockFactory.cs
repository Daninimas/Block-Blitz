using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class BlockFactory : MonoBehaviour
    {
        [FormerlySerializedAs("cellPrefab")] public Block blockPrefab;

        public Block CreateBlock(Transform parent, Block.BlockColorData blockColorData, 
            Block.BlockColorData highlightBlockColorData)
        {
            Block block = Instantiate(blockPrefab, parent);
            block.SetUp(blockColorData, highlightBlockColorData);
            return block;
        }
    }
}