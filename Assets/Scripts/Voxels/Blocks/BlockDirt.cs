using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class BlockDirt : Block
{
    public BlockDirt() : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 1;
        tile.y = 0;
        return tile;
    }

    public override byte GetBlockHealth()
    {
        return 5;
    }
    public override ToolType GetToolType()
    {
        return ToolType.shovel;
    }
}
