using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapGen : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;




    // Start is called before the first frame update
    void Start()
    {

        //Tilemap tm = GetComponentInChildren<Tilemap>();
        //tm.SetTile(new Vector3Int(0, 0, 0), (Tile)Resources.Load("Sprites/image_17", typeof(Tile)));
        //tm.SetTile(new Vector3Int(0, 1, 0), (Tile)Resources.Load("image_43", typeof(Tile)));
        //tm.RefreshAllTiles();

        var map = CellularAutomata.Generate();
        Debug.Log("Map generated");
        CellularAutomata.PrintMap(map);
        Debug.Log(Cell.printChars[map[0, 0]]);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
