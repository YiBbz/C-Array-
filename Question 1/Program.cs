using System;
using System.Collections.Generic;

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
}

class Program
{
    static void Main(string[] args)
    {
        Graph graph = new Graph();

        // Adding nodes
        graph.AddNode(1);
        graph.AddNode(2);
        graph.AddNode(3);

        // Adding edges
        graph.AddEdge(1, 2, 5.0);
        graph.AddEdge(1, 3, 10.0);
        graph.AddEdge(2, 3, 2.5);

        // Display the graph
        Console.WriteLine(graph);
    }
}
