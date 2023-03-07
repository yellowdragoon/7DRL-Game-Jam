using System.Collections;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CellularAutomata
{
    public enum Cell
    {
        Wall,
        Floor
    }

    Dictionary<Cell, char> cellPrintChars = new Dictionary<Cell, char>
    {
        {Cell.Wall, '#'},
        {Cell.Floor, '.'}
    };

    const int sizeX = 80;
    const int sizeY = 100;
    const double startChance = 0.30; // chance of wall

    Cell[,] map;

    public static void Main()
    {
        var c = new CellularAutomata();
        Debug.Log("in cellular automata");
        c.Fill();
        c.PrintMap();

        for (int i = 0; i < 4; i++)
        {
            //c.Step((prev, countFloor1, countFloor2) => ((prev==Cell.Wall ? countFloor1 <= 5 : countFloor1 <= 4) ? Cell.Wall : Cell.Floor));
            c.Step((prev, countFloor1, countFloor2) => (countFloor1 <= 4 || countFloor2 >= 19 ? Cell.Wall : Cell.Floor));
            c.PrintMap();
        }
        for (int i = 0; i < 3; i++)
            c.Step((prev, countFloor1, countFloor2) => (countFloor1 < 5 ? Cell.Wall : Cell.Floor));
        c.PrintMap();
    }


    public CellularAutomata()
    {
        map = new Cell[sizeX, sizeY];

    }

    public void Fill()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                map[i, j] = Random.value < startChance ? Cell.Wall : Cell.Floor;
            }
        }
    }

    // rule of form (prev, countFloor) -> new
    public void Step(Func<Cell, int, int, Cell> rule)
    {
        Cell[,] tmp = new Cell[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                int countFloor1 = 0; // count neighbouring floors - so stuff outside borders will be considered walls by default
                for (int ii = Mathf.Max(i - 1, 0); ii < Mathf.Min(i + 2, sizeX); ii++)
                {
                    for (int jj = Mathf.Max(j - 1, 0); jj < Mathf.Min(j + 2, sizeY); jj++)
                    {
                        if (ii != i || jj != j)
                            countFloor1 += map[ii, jj] == Cell.Floor ? 1 : 0;
                    }
                }
                int countFloor2 = 0;
                for (int ii = Mathf.Max(i - 2, 0); ii < Mathf.Min(i + 3, sizeX); ii++)
                {
                    for (int jj = Mathf.Max(j - 2, 0); jj < Mathf.Min(j + 3, sizeY); jj++)
                    {
                        if ((ii != i || jj != j) && !(Math.Abs(j-jj) == 2 && Math.Abs(i-ii) == 2))
                            countFloor2 += map[ii, jj] == Cell.Floor ? 1 : 0;
                    }
                }
                //Debug.Log($"i {i} j {j}, countFloor1 {countFloor1}");
                tmp[i, j] = rule(map[i, j], countFloor1, countFloor2);
                //if (map[i, j] == Cell.Wall)
                //    tmp[i, j] = countFloor <= 3 ? Cell.Wall : Cell.Floor;
                //else
                //    tmp[i, j] = countFloor >= 7 ? Cell.Wall : Cell.Floor;
            }
        }
        map = tmp;
    }

    public void PrintMap()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                sb.Append(cellPrintChars[map[i, j]]);
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
        //Console.Write(sb.ToString());
    }
}