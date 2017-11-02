using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockIron : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 1;
        tile.y = 3;
        return tile;
    }

    public override byte GetBlockHealth()
    {
        return 13;
    }
    public override ToolType GetToolType()
    {
        return ToolType.pickAxe;
    }
}
