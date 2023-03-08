using System.Collections;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CellularAutomata : MapGen
{
    [SerializeField] private int width = 80;
    [SerializeField] private int height = 100;
    [SerializeField] private double startChance = 0.30; // chance of wall

    Cell.Type[,] map;

    void Awake()
    {
        map = new Cell.Type[width, height];
    }

    public override Cell.Type[,] Generate()
    {
        //Debug.Log("in cellular automata");
        Fill();
        //PrintMap(c.map);

        for (int i = 0; i < 4; i++)
        {
            //c.Step((prev, countFloor1, countFloor2) => ((prev==Cell.Wall ? countFloor1 <= 5 : countFloor1 <= 4) ? Cell.Wall : Cell.Floor));
            Step((prev, countFloor1, countFloor2) => (countFloor1 <= 4 || countFloor2 >= 19 ? Cell.Type.Wall : Cell.Type.Floor));
            //PrintMap(c.map);
        }
        for (int i = 0; i < 3; i++)
            Step((prev, countFloor1, countFloor2) => (countFloor1 < 5 ? Cell.Type.Wall : Cell.Type.Floor));
        //PrintMap(c.map);

        return map;
    }

    void Fill()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Debug.Log($"{i} {j} {width} {height}");
                map[i, j] = Random.value < startChance ? Cell.Type.Wall : Cell.Type.Floor;
            }
        }
    }

    // rule of form (prev, countFloor) -> new
    void Step(Func<Cell.Type, int, int, Cell.Type> rule)
    {
        Cell.Type[,] tmp = new Cell.Type[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // count neighbouring floors - so stuff outside borders will be considered walls by default
                int countFloor1 = 0;
                int countFloor2 = 0;
                for (int ii = -2; ii <= 2; ii++)
                {
                    for (int jj = -2; jj <= 2; jj++)
                    {
                        if ((ii == 0 && jj == 0) || i + ii < 0 || i + ii >= width || j + jj >= height || j + jj < 0) 
                        {
                            continue;
                        }

                        int isFloor = map[i + ii, j + jj] == Cell.Type.Floor ? 1 : 0;
                        if (Math.Max(Math.Abs(ii), Math.Abs(jj)) == 1) // in 3x3 square
                            countFloor1 += isFloor;
                        if (!(Math.Abs(jj) == 2 && Math.Abs(ii) == 2)) // not the diagonally outermost cells of 5x5
                            countFloor2 += isFloor;
                    }
                }

                tmp[i, j] = rule(map[i, j], countFloor1, countFloor2);
            }
        }
        map = tmp;
    }
}