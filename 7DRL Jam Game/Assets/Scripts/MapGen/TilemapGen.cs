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
            SetupRooms();
        }
    }

    private void SetupRooms()
    {
        var map = roomGenerator.Generate();

        // Place the player at a starting point
        // TODO graph traversal to find starting point + end point
        Rooms.Node startLeaf = roomGenerator.leaves[Random.Range(0, roomGenerator.leaves.Count-1)];
        var startCoords = floorTilemap.CellToLocalInterpolated(startLeaf.room.center);
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = startCoords;

        // Generate things like enemies here
        for (int i = 0; i < roomGenerator.leaves.Count; i++)
        {
            Rooms.Node currentLeaf = roomGenerator.leaves[i];
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
