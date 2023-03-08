using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MapGen
{
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;


    [SerializeField] private int roomPad = 1;

    [SerializeField] private int minRoomWidth = 9;
    [SerializeField] private int minRoomHeight = 9;

    [SerializeField] private int minPartitionWidth = 20;
    [SerializeField] private int minPartitionHeight = 20;


    [SerializeField] private double splitOrientationForceThreshold = 1.25;
    [SerializeField] private double horizontalChance = 0.50;



    public override Cell.Type[,] Generate()
    {
        // 0 is wall
        var map = new int[width, height];
        Fill(map, 0);

        Node root = new Node(0, 0, width, height);
        Split(root);
        BuildRoom(map, root);


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

    private class Node
    {
        public Node left;
        public Node right;
        public int x, y, w, h;
        public Node(int _x, int _y, int _w, int _h)
        {
            x = _x;
            y = _y;
            w = _w;
            h = _h;
        }
    }

    private void Split(Node root)
    {
        // randomly choose split direction unless the area is too thin/wide
        bool splitHorizontally = Random.value < horizontalChance;
        if (root.w > root.h && (double)root.w / root.h > splitOrientationForceThreshold)
            splitHorizontally = false;
        else if (root.h > root.w && (double)root.h / root.w > splitOrientationForceThreshold)
            splitHorizontally = true;

        if (splitHorizontally)
        {
            var max = root.h - minPartitionHeight;
            if (max <= minPartitionHeight) return; // too small to split
            var split = Random.Range(minPartitionHeight, max);
            root.left = new Node(root.x, root.y, root.w, split);
            root.right = new Node(root.x, root.y+split, root.w, root.h-split);
        }
        else
        {
            var max = root.w - minPartitionWidth;
            if (max <= minPartitionWidth) return; // too small to split
            var split = Random.Range(minPartitionWidth, max);
            root.left = new Node(root.x, root.y, split, root.h);
            root.right = new Node(root.x + split, root.y, root.w-split, root.h);
        }
        Split(root.left);
        Split(root.right);
    }

    private void BuildRoom(int[,] map, Node root)
    {
        if (root.left != null)
            BuildRoom(map, root.left);
        if (root.right != null)
            BuildRoom(map, root.right);

        if (root.left == null && root.right == null)
        {
            var w = Random.Range(minRoomWidth, root.w - roomPad * 2);
            var h = Random.Range(minRoomHeight, root.h - roomPad * 2);
            var x = root.x + Random.Range(roomPad, root.w - roomPad - w);
            var y = root.y + Random.Range(roomPad, root.h - roomPad - h);

            Fill(map, 1, x, y, x + w, y + h);
        }
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
