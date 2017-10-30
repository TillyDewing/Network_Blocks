using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWoodPlanks : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 3;
        tile.y = 1;
        return tile;
    }
}
