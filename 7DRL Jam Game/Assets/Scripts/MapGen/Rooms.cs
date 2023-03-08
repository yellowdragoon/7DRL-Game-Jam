using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MapGen
{
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;

    [SerializeField] private int minWidth = 20;
    [SerializeField] private int minHeight = 20;

    [SerializeField] private int levels = 3;

    [SerializeField] private double horizontalChance = 0.50;
    [SerializeField] private double splitMin = 0.40;
    [SerializeField] private double splitMax = 0.60;


    private class Node
    {
        public bool splitHorizontally;
        public Node left;
        public Node right;
        public bool isLeaf;
        public int x0, x1, y0, y1;

    }

    public override Cell.Type[,] Generate()
    {
        // 0 is wall
        var map = new int[width, height];
        Fill(map, 0);

        Node root = Build(map, 0, 0, 0, width, height);


        var result = new Cell.Type[width, height];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                result[i, j] = map[i, j] == 1 ? Cell.Type.Floor : Cell.Type.Wall;
            }
        }
        return result;
    }

    Node Build(int[,] map, int depth, int x0, int y0, int x1, int y1)
    {
        var root = new Node { x0=x0, y0=y0, x1=x1, y1=y1 };
        if (x1-x0 < minWidth || y1-y0 < minHeight)
        {
            return null;
        }

        if (depth == levels)
        {
            root.isLeaf = true;
            var rw = Random.Range(minWidth, x1 - x0);
            var rh = Random.Range(minHeight, y1 - y0);

            var rx = Random.Range(x0, x1 - rw - 1);
            var ry = Random.Range(y0, y1 - rh - 1);

            Debug.Log($"{rw} {rh} {rx} {ry}");
            Fill(map, 1, rx, ry, rx+rw, ry+rh);
            return root;
        }

        if (Random.value < horizontalChance)
        {
            root.splitHorizontally = true;
            var p = Lerp(Random.value, splitMin, splitMax);
            var mid = Lerp(p, x0, x1);
            root.left = Build(map, depth + 1, x0, y0, mid, y1);
            root.right = Build(map, depth + 1, mid, y0, x1, y1);
        }
        else
        {
            root.splitHorizontally = false;
            var p = Lerp(Random.value, splitMin, splitMax);
            var mid = Lerp(p, y0, y1);
            root.left = Build(map, depth + 1, x0, y0, x1, mid);
            root.right = Build(map, depth + 1, x0, mid, x1, y1);
        }
        return root;
    }

    void Fill(int[,] m, int val)
    {
        Fill(m, val, 0, 0, m.GetLength(0), m.GetLength(1));
    }
    void Fill(int[,] m, int val, int x0, int y0, int x1, int y1)
    {
        for (int i = x0; i < x1; i++)
        {
            for (int j = y0; j < y1; j++)
            {
                m[i, j] = val;
            }
        }
    }

    double Lerp(double t, double min, double max)
    {
        return min + t * (max - min);
    }
    int Lerp(double t, int min, int max)
    {
        return min + (int) (t * (max - min));
    }
}
