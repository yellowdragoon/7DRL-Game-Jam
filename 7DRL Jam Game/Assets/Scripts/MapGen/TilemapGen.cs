using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapGen : MonoBehaviour
{
    private Tilemap tileMap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase corridorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Component generator;

    // Start is called before the first frame update
    void Start()
    {
        tileMap = GetComponentInChildren<Tilemap>();

        SetupTilemap();    
    }



    private void Update()
    {
        if (Input.GetButtonDown("Debug Reset"))
        {
            Debug.Log("reset");
            tileMap.ClearAllTiles();
            SetupTilemap();
        }
    }

    private void SetupTilemap()
    {
        var map = ((MapGen)generator).Generate();
        //Debug.Log("Map generated");
        //MapGen.PrintMap(map);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == Cell.Type.Floor)
                    tileMap.SetTile(new Vector3Int(i, j, 0), floorTile);
                else if (map[i, j] == Cell.Type.Corridor)
                    tileMap.SetTile(new Vector3Int(i, j, 0), corridorTile);
                else
                    tileMap.SetTile(new Vector3Int(i, j, 0), wallTile);

            }
        }
    }

}
