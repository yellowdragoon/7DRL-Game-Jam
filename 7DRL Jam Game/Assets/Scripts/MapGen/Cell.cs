using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Cell
{
    public enum Type
    {
        Wall,
        Floor,
        Corridor
    }

    public static Dictionary<Type, char> printChars = new Dictionary<Type, char>
    {
        {Type.Wall, '#'},
        {Type.Floor, '.'}
    };

}