using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapGen : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;

    // Start is called before the first frame update
    void Start()
    { 

        var map = CellularAutomata.Generate();
        Debug.Log("Map generated");
        CellularAutomata.PrintMap(map);
        Debug.Log(Cell.printChars[map[0, 0]]);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if(map[i, j] == Cell.Type.Floor)
                { 
                    tileMap.SetTile(new Vector3Int(i, j, 0), floorTile);
                }
                else
                {
                    tileMap.SetTile(new Vector3Int(i, j, 0), wallTile);
                }
                
            }
        }
    }


}
