using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class MapGen : MonoBehaviour
{
    public abstract Cell.Type[,] Generate();

    public static void PrintMap(Cell.Type[,] m)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < m.GetLength(0); i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                sb.Append(Cell.printChars[m[i, j]]);
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }
}
