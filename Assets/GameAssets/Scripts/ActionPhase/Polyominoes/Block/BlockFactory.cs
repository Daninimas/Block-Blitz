using UnityEngine;
using UnityEngine.Serialization;

namespace GameAssets.Scripts.ActionPhase
{
    public class BlockFactory : MonoBehaviour
    {
        [FormerlySerializedAs("cellPrefab")] public Block blockPrefab;

        public Block CreateBlock(Transform parent, Block.BlockColorData blockColorData)
        {
            Block block = Instantiate(blockPrefab, parent);
            block.SetBlockColors(blockColorData);
            return block;
        }
    }
}