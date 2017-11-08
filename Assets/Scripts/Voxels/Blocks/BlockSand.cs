using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BlockSand : Block
{
    public BlockSand() : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 1;
        tile.y = 2;
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
