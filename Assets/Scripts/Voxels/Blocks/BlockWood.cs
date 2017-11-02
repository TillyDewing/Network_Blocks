﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[Serializable]
public class BlockWood : Block
{
    public BlockWood() : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        switch (direction)
        {
            case Direction.up:
                tile.x = 2;
                tile.y = 1;
                return tile;
            case Direction.down:
                tile.x = 2;
                tile.y = 1;
                return tile;
        }
        tile.x = 1;
        tile.y = 1;
        return tile;
    }

    public override byte GetBlockHealth()
    {
        return 6;
    }
    public override ToolType GetToolType()
    {
        return ToolType.axe;
    }
}
