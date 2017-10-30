using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockWater : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 4;
        tile.y = 0;
        return tile;
    }

    public override bool IsSolid(Direction direction)
    {
        return false;
    }

    //public override MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    //{
    //    meshData.useRenderDataForCol = false;
    //    meshData = base.Blockdata(chunk, x, y, z, meshData);
    //    meshData.useRenderDataForCol = true;
    //    return meshData;
    //}

    public override MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = false;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down) && chunk.GetBlock(x, y + 1, z).GetType() != typeof(BlockWater))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up) && chunk.GetBlock(x, y - 1, z).GetType() != typeof(BlockWater))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south) && chunk.GetBlock(x, y, z + 1).GetType() != typeof(BlockWater))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north) && chunk.GetBlock(x, y, z - 1).GetType() != typeof(BlockWater))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west) && chunk.GetBlock(x + 1, y, z).GetType() != typeof(BlockWater))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east) && chunk.GetBlock(x - 1, y, z).GetType() != typeof(BlockWater))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }

        meshData.useRenderDataForCol = true;
        return meshData;

    }

    protected override MeshData FaceDataUp
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.35f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.35f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.35f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.35f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected override MeshData FaceDataDown
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected override MeshData FaceDataNorth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.35f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.35f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected override MeshData FaceDataEast
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.35f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.35f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected override MeshData FaceDataSouth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.35f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.35f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected override MeshData FaceDataWest
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.35f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.35f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;
    }
}
