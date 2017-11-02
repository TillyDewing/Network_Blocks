using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BlockStone : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    public override byte GetBlockHealth()
    {
        return 10;
    }
    public override ToolType GetToolType()
    {
        return ToolType.pickAxe;
    }
}
