using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Defines a point in world space (essestialy vector3 int)
[Serializable]
public struct WorldPos
{
    public int x, y, z;
    public WorldPos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        if (GetHashCode() == obj.GetHashCode())
        {
            return true;
        }
        return false;
    }
    //Returns unique hash for world pos
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 47;
            hash = hash * 227 + x.GetHashCode();
            hash = hash * 227 + y.GetHashCode();
            hash = hash * 227 + z.GetHashCode();
            return hash;
        }
    }
}
