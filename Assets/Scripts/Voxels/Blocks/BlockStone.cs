using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BlockStone : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 5;
        tile.y = 0;
        return tile;
    }
}
