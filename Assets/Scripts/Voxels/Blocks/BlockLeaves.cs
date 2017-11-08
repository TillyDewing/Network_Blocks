using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class BlockLeaves : Block
{
    public BlockLeaves() : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 1;
        return tile;
    }

    public override bool IsSolid(Direction direction)
    {
        return false;
    }

    public override byte GetBlockHealth()
    {
        return 2;
    }
    public override ToolType GetToolType()
    {
        return ToolType.hands;
    }
}
