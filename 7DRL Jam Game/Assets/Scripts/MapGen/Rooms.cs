using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// with help from http://www.rombdn.com/blog/2018/01/12/random-dungeon-bsp-unity/

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

    public List<Node> leaves;


    public override Cell.Type[,] Generate()
    {
        // 0 is wall
        var map = new int[width, height];
        var corridorMap = new int[width, height];
        Fill(map, 0);
        Fill(corridorMap, 0);

        leaves = new List<Node>();

        Node root = new Node(0, 0, width, height);
        Split(root);
        BuildRoom(map, corridorMap, root);

        var result = new Cell.Type[width, height]; // defaults to all walls
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Cell.Type cell;
                if (map[i, j] == 1)
                    cell = Cell.Type.Floor;
                else if (corridorMap[i, j] == 1)
                    cell = Cell.Type.Corridor;
                else
                    cell = Cell.Type.Wall;

                result[i, j] = cell;
            }
        }
        return result;
    }


    public class Node
    {
        public Node left;
        public Node right;
        public int x, y, w, h;
        public Vector2Int center;
        public RectInt room;
        public List<Node> connections = new List<Node>();
        public Node(int _x, int _y, int _w, int _h)
        {
            x = _x;
            y = _y;
            w = _w;
            h = _h;
            center = new Vector2Int(x + w / 2, y + h / 2);
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
            int max = root.h - minPartitionHeight;
            if (max <= minPartitionHeight) return; // too small to split
            int split = Random.Range(minPartitionHeight, max);
            root.left = new Node(root.x, root.y, root.w, split);
            root.right = new Node(root.x, root.y+split, root.w, root.h-split);
        }
        else
        {
            int max = root.w - minPartitionWidth;
            if (max <= minPartitionWidth) return; // too small to split
            int split = Random.Range(minPartitionWidth, max);
            root.left = new Node(root.x, root.y, split, root.h);
            root.right = new Node(root.x + split, root.y, root.w-split, root.h);
        }
        Split(root.left);
        Split(root.right);
    }


    private void BuildRoom(int[,] map, int[,] corridorMap, Node root)
    {
        if (root.left != null)
            BuildRoom(map, corridorMap, root.left);
        if (root.right != null)
            BuildRoom(map, corridorMap, root.right);

        if (root.left != null && root.right != null)
        {
            Connect(corridorMap, root.left, root.right);
        }

        if (root.left == null && root.right == null)
        {
            int w = Random.Range(minRoomWidth, root.w - roomPad * 2);
            int h = Random.Range(minRoomHeight, root.h - roomPad * 2);
            int x = root.x + Random.Range(roomPad, root.w - roomPad - w);
            int y = root.y + Random.Range(roomPad, root.h - roomPad - h);

            root.room = new RectInt(x, y, w, h);
            Fill(map, 1, x, y, x + w, y + h);
            leaves.Add(root);
        }
    }


    private void Connect(int[,] corridorMap, Node a, Node b)
    {
        //a = GetRandomLeaf(a);
        //b = GetRandomLeaf(b);
        a = GetCloseLeaf(a, b);
        b = GetCloseLeaf(b, a);

        Vector2Int lpoint = new Vector2Int(Random.Range(a.room.x + 1, a.room.xMax - 1), Random.Range(a.room.y + 1, a.room.yMax - 1));
        Vector2Int rpoint = new Vector2Int(Random.Range(b.room.x + 1, b.room.xMax - 1), Random.Range(b.room.y + 1, b.room.yMax - 1));

        if (Random.value < 0.5)
        { // go vertical then horizontal
            VerticalCorridor(corridorMap, lpoint.x, lpoint.y, rpoint.y);
            HorizontalCorridor(corridorMap, lpoint.x, rpoint.x, rpoint.y);
        } 
        else
        { // go horizontal then vertical
            HorizontalCorridor(corridorMap, lpoint.x, rpoint.x, rpoint.y);
            VerticalCorridor(corridorMap, lpoint.x, lpoint.y, rpoint.y);
        }

        a.connections.Add(b);
        b.connections.Add(a);
    }

    private void HorizontalCorridor(int[,] corridorMap, int x0, int x1, int y)
    {
        for (int i=Mathf.Min(x0, x1); i <= Mathf.Max(x0, x1); i++)
        {
            corridorMap[i, y] = 1;
        }
    }

    private void VerticalCorridor(int[,] corridorMap, int x, int y0, int y1)
    {
        for (int i = Mathf.Min(y0, y1); i <= Mathf.Max(y0, y1); i++)
        {
            corridorMap[x, i] = 1;
        }
    }

    private Node GetRandomLeaf(Node root)
    {
        if (root.left == null && root.right == null)
            return root; // must be a leaf
        if (root.left == null) return GetRandomLeaf(root.right);
        if (root.right == null) return GetRandomLeaf(root.left);
        return GetRandomLeaf(Random.value < 0.5 ? root.left : root.right);
    }

    private Node GetCloseLeaf(Node root, Node other)
    {
        if (root.left == null && root.right == null)
            return root;
        if (root.left == null) return GetCloseLeaf(root.right, other);
        if (root.right == null) return GetCloseLeaf(root.left, other);

        if (Vector2Int.Distance(root.left.center, other.center) < Vector2Int.Distance(root.right.center, other.center))
            return GetCloseLeaf(root.left, other);
        return GetCloseLeaf(root.right, other);
    }

    public Node GetFurthestLeaf(Node start)
    {
        var q = new Queue<Node>();
        var visited = new HashSet<Node>();
        q.Enqueue(start);
        Node last = start;

        while (q.Count > 0)
        {
            last = q.Dequeue();
            foreach (Node nei in last.connections)
            {
                if (!visited.Contains(nei))
                {
                    visited.Add(nei);
                    q.Enqueue(nei);
                }
            }
        }

        return last;
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
