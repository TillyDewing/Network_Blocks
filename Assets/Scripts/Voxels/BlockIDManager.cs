//William Dewing 2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockIDManager
{
    public static Block GetBlock(byte id)
    {
        switch (id)
        {
            case 0:
                return new BlockAir();
            case 1:
                return new Block();  //Stone
            case 2:
                return new BlockGrass();
            case 3:
                return new BlockDirt();
            case 4:
                return new BlockWood();
            case 5:
                return new BlockLeaves();
        }

        return new Block();
    }
    public static byte GetID(Block block)
    {
        if (block.GetType() == typeof(BlockAir))
        {
            return 0;
        }
        if (block.GetType() == typeof(Block))
        {
            return 1;
        }
        if (block.GetType() == typeof(BlockGrass))
        {
            return 2;
        }
        if (block.GetType() == typeof(BlockDirt))
        {
            return 3;
        }
        if (block.GetType() == typeof(BlockWood))
        {
            return 4;
        }
        if (block.GetType() == typeof(BlockLeaves))
        {
            return 5;
        }

        return 0;
    }
}
