using System.Collections.Generic;
using System.Linq;
using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    [System.Serializable]
    public class NamedBlockColor
    {
        public string colorName;
        public Block.BlockColorData blockColorData;
    }
    
    
    [CreateAssetMenu(fileName = "BlockColorsDirectory", menuName = "ScriptableObjects/BlockColorsDirectory", order = 0)]
    public class BlockColorsDirectory : ScriptableObject
    {
        [SerializeField] private List<NamedBlockColor> blocksColors;
        [SerializeField] private Block.BlockColorData highlightedBlockColor;
        

        public Block.BlockColorData GetBlockColorByName(string colorName)
        {
            NamedBlockColor namedBlockColor = blocksColors.FirstOrDefault(blockColor => blockColor.colorName == colorName);
            
            if(namedBlockColor == null)
            {
                Log.Error("BlockColorsDirectory", $"No block color with name {colorName} found in BlockColorsDirectory.");
                return null;
            }
            
            return namedBlockColor.blockColorData;
        }
        
        public Block.BlockColorData GetRandomBlockColor()
        {
            return blocksColors[Random.Range(0, blocksColors.Count)].blockColorData;
        }

        public Block.BlockColorData GetHighlightedBlockColor()
        {
            return highlightedBlockColor;
        }
    }
}