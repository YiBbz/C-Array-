using System;
using System.Collections.Generic;
using System.IO;

public class Node
{
    public int Label { get; private set; }

    public Node(int label)
    {
        Label = label;
    }
}

public class Edge
{
    public Node From { get; private set; }
    public Node To { get; private set; }
    public double Weight { get; private set; }

    public Edge(Node from, Node to, double weight)
    {
        From = from;
        To = to;
        Weight = weight;
    }
}

public class Graph
{
    private Dictionary<int, Node> nodes;
    private List<Edge> edges;

    public Graph()
    {
        nodes = new Dictionary<int, Node>();
        edges = new List<Edge>();
    }

    public void AddNode(int label)
    {
        if (!nodes.ContainsKey(label))
        {
            nodes[label] = new Node(label);
        }
    }

    public void AddEdge(int fromLabel, int toLabel, double weight)
    {
        if (!nodes.ContainsKey(fromLabel) || !nodes.ContainsKey(toLabel))
        {
            throw new ArgumentException("Both nodes must exist in the graph.");
        }

        Node fromNode = nodes[fromLabel];
        Node toNode = nodes[toLabel];

        Edge edge = new Edge(fromNode, toNode, weight);
        edges.Add(edge);
    }

    public List<Edge> GetEdges()
    {
        return edges;
    }

    public List<Node> GetNodes()
    {
        return new List<Node>(nodes.Values);
    }

    public override string ToString()
    {
        var result = "Graph:\n";
        foreach (var edge in edges)
        {
            result += $"{edge.From.Label} --({edge.Weight})--> {edge.To.Label}\n";
        }
        return result;
    }

    // Question 2: Depth-First Traversal
    public void DepthFirstTraversal(int startLabel)
    {
        if (!nodes.ContainsKey(startLabel))
            throw new ArgumentException("Start node does not exist in the graph.");

        HashSet<int> visited = new HashSet<int>();
        DepthFirstTraversalHelper(nodes[startLabel], visited);
    }

    private void DepthFirstTraversalHelper(Node node, HashSet<int> visited)
    {
        if (visited.Contains(node.Label))
            return;

        visited.Add(node.Label);
        Console.WriteLine(node.Label);

        foreach (var edge in edges)
        {
            if (edge.From.Label == node.Label && !visited.Contains(edge.To.Label))
            {
                DepthFirstTraversalHelper(edge.To, visited);
            }
            else if (edge.To.Label == node.Label && !visited.Contains(edge.From.Label))
            {
                DepthFirstTraversalHelper(edge.From, visited);
            }
        }
    }

    // Question 2: Breadth-First Traversal
    public void BreadthFirstTraversal(int startLabel)
    {
        if (!nodes.ContainsKey(startLabel))
            throw new ArgumentException("Start node does not exist in the graph.");

        HashSet<int> visited = new HashSet<int>();
        Queue<Node> queue = new Queue<Node>();

        visited.Add(startLabel);
        queue.Enqueue(nodes[startLabel]);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            Console.WriteLine(current.Label);

            foreach (var edge in edges)
            {
                if (edge.From.Label == current.Label && !visited.Contains(edge.To.Label))
                {
                    visited.Add(edge.To.Label);
                    queue.Enqueue(edge.To);
                }
                else if (edge.To.Label == current.Label && !visited.Contains(edge.From.Label))
                {
                    visited.Add(edge.From.Label);
                    queue.Enqueue(edge.From);
                }
            }
        }
    }

    // Question 2: Method to read graph from a text file
    public void ReadGraphFromFile(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            int numberOfNodes = int.Parse(sr.ReadLine());
            int numberOfEdges = int.Parse(sr.ReadLine());

            for (int i = 0; i < numberOfNodes; i++)
            {
                AddNode(i + 1); // Assuming nodes are labeled from 1 to numberOfNodes
            }

            for (int i = 0; i < numberOfEdges; i++)
            {
                var line = sr.ReadLine();
                var parts = line.Split(' ');

                int fromLabel = int.Parse(parts[0]);
                int toLabel = int.Parse(parts[1]);
                double weight = double.Parse(parts[2]);

                AddEdge(fromLabel, toLabel, weight);
            }
        }
    }
}

