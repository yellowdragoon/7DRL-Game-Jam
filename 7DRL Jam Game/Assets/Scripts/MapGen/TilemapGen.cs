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

    [SerializeField] private int wallPad = 25;

    [SerializeField] private NavMeshManager navManager;
    [SerializeField] private EnemySpawner spawner;

    [SerializeField] private float maxEnemyDensity = 0.01f;

    private Rooms roomGenerator = new Rooms();

    // Start is called before the first frame update
    void Start()
    {
        SetupRooms();
    }



    private void Update()
    {
        if (Input.GetButtonDown("Debug Reset")) // left alt key
        {
            //Debug.Log("reset");
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy); // should this be done in SetupRooms? then SetupRooms is like a nice restart method
            }
            SetupRooms();
        }
    }

    private void SetupRooms()
    {
        var map = roomGenerator.Generate();

        Rooms.Node randomLeaf = roomGenerator.leaves[Random.Range(0, roomGenerator.leaves.Count-1)];
        Rooms.Node startLeaf = roomGenerator.GetFurthestLeaf(randomLeaf);
        Rooms.Node endLeaf = roomGenerator.GetFurthestLeaf(startLeaf); // if ending room needs to be boss, can instead look for largest room then find start from there?

        // Place the player at a starting point
        var startCoords = floorTilemap.CellToLocalInterpolated(startLeaf.room.center);
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = startCoords;

        // Place the end room [just change tiles for now]
        var r = endLeaf.room;
        for (int i=r.x; i<r.x+r.width; i++)
        {
            for (int j=r.y; j<r.y+r.height; j++)
            {
                map[i, j] = Cell.Type.Corridor;
            }
        }

        // Generate things like enemies here
        foreach (var currentLeaf in roomGenerator.leaves)
        {
            //int roomSize = currentLeaf.room.width * currentLeaf.room.height;
            //int numEnemies = Random.Range(0, Mathf.CeilToInt(roomSize * maxEnemyDensity));
            int numEnemies = Random.Range(0, 5);
            for (int j = 0; j < numEnemies+1; j++)
            {
                Vector3 position = new Vector3(Random.Range(currentLeaf.x + 1, currentLeaf.x + currentLeaf.w - 1), Random.Range(currentLeaf.y + 1, currentLeaf.y + currentLeaf.h - 1), 0);
                spawner.spawnEnemy(position);
            }

        }

        // Tilemaps
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        wallTopsTilemap.ClearAllTiles();

        FillWalls(-wallPad, -wallPad, map.GetLength(0)+wallPad, 0);
        FillWalls(-wallPad, map.GetLength(0), map.GetLength(0)+wallPad, map.GetLength(1)+wallPad);
        FillWalls(-wallPad, 0, 0, map.GetLength(1));
        FillWalls(map.GetLength(0), 0, map.GetLength(0)+wallPad, map.GetLength(1));

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


    void FillWalls(int x0, int y0, int x1, int y1)
    {
        for (int i = x0; i < x1; i++)
        {
            for (int j = y0; j < y1; j++)
            {
                wallTilemap.SetTile(new Vector3Int(i, j, 0), wallTile);
                wallTopsTilemap.SetTile(new Vector3Int(i, j, 0), wallTopsTile);
            }
        }
    }
}
