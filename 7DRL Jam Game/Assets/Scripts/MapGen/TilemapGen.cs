using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapGen : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase corridorTile;

    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap wallTopsTilemap;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase wallTopsTile;

    [SerializeField] private Component generator;
    [SerializeField] private NavMeshManager navManager;

    // Start is called before the first frame update
    void Start()
    {
        SetupTilemaps();
    }



    private void Update()
    {
        if (Input.GetButtonDown("Debug Reset")) // left alt key
        {
            //Debug.Log("reset");
            SetupTilemaps();
        }
    }

    private void SetupTilemaps()
    {
        var map = ((MapGen)generator).Generate();

        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        wallTopsTilemap.ClearAllTiles();

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == Cell.Type.Floor)
                    floorTilemap.SetTile(new Vector3Int(i, j, 0), floorTile);
                else if (map[i, j] == Cell.Type.Corridor)
                    floorTilemap.SetTile(new Vector3Int(i, j, 0), corridorTile);
                else
                {
                    wallTilemap.SetTile(new Vector3Int(i, j, 0), wallTile);
                    wallTopsTilemap.SetTile(new Vector3Int(i, j, 0), wallTopsTile);
                }

            }
        }

        floorTilemap.RefreshAllTiles();
        wallTilemap.RefreshAllTiles();
        wallTopsTilemap.RefreshAllTiles();
        navManager.updateNavMesh();
    }

}
