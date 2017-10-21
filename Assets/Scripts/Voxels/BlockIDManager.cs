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
}
