
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDataHolder", menuName = "Block/BlockDataHolder")]
public class BlockDataHolder : ScriptableObject
{
    public List<BlockData> blockDataList;

    public BlockData GetBlockData(BlockType type)
    {
        return blockDataList.Find(w => w.wallType == type);
    }
}


public enum BlockType
{
    Glass,
    Wood,
    Rock
}

[System.Serializable]
public struct BlockData
{
    public BlockType wallType;
    public float breakThreshold;    // Minimum velocity to break
    public float impactMultiplier;
    public float maxHealth;
}
