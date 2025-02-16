// New class needed for Kruskal's Algorithm
using System.Xml.Linq;

public class DisjointSet
{
    private Dictionary<int, int> parent;
    private Dictionary<int, int> rank;

    public DisjointSet()
    {
        parent = new Dictionary<int, int>();
        rank = new Dictionary<int, int>();
    }

    public void MakeSet(int vertex)
    {
        if (!parent.ContainsKey(vertex))
        {
            parent[vertex] = vertex;
            rank[vertex] = 0;
        }
    }

    public int Find(int vertex)
    {
        if (parent[vertex] != vertex)
        {
            parent[vertex] = Find(parent[vertex]); // Path compression
        }
        return parent[vertex];
    }

    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX != rootY)
        {
            if (rank[rootX] < rank[rootY])
            {
                parent[rootX] = rootY;
            }
            else if (rank[rootX] > rank[rootY])
            {
                parent[rootY] = rootX;
            }
            else
            {
                parent[rootY] = rootX;
                rank[rootX]++;
            }
        }
    }
}
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


// Add these methods to your existing Graph class
public class Graph
{
    // ... (previous code remains the same)
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

    // Question 3: Kruskal's Algorithm Implementation
    public (List<Edge> mst, TimeSpan executionTime) KruskalMST()
    {
        List<Edge> minimumSpanningTree = new List<Edge>();
        DisjointSet disjointSet = new DisjointSet();

        // Create a copy of edges for sorting
        List<Edge> sortedEdges = new List<Edge>(edges);

        // Initialize sets for each vertex
        foreach (var node in nodes.Values)
        {
            disjointSet.MakeSet(node.Label);
        }

        // Start timing
        DateTime startTime = DateTime.Now;

        // Sort edges by weight
        sortedEdges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

        // Process each edge
        foreach (var edge in sortedEdges)
        {
            int fromRoot = disjointSet.Find(edge.From.Label);
            int toRoot = disjointSet.Find(edge.To.Label);

            if (fromRoot != toRoot)
            {
                minimumSpanningTree.Add(edge);
                disjointSet.Union(fromRoot, toRoot);
            }
        }

        // End timing
        DateTime endTime = DateTime.Now;
        TimeSpan executionTime = endTime - startTime;

        return (minimumSpanningTree, executionTime);
    }

    // Question 3: Helper method to print MST
    public void PrintMST(List<Edge> mst)
    {
        double totalWeight = 0;
        Console.WriteLine("\nMinimum Spanning Tree edges:");
        foreach (var edge in mst)
        {
            Console.WriteLine($"Edge: {edge.From.Label} -- {edge.To.Label}, Weight: {edge.Weight}");
            totalWeight += edge.Weight;
        }
        Console.WriteLine($"Total MST Weight: {totalWeight}");
    }
}

// Main program to run the algorithm 10 times
class Program
{
    static void Main(string[] args)
    {
        Graph graph = new Graph();
        graph.ReadGraphFromFile("Graph.txt");

        List<TimeSpan> runTimes = new List<TimeSpan>();

        Console.WriteLine("Running Kruskal's Algorithm 10 times...\n");

        for (int i = 0; i < 10; i++)
        {
            var (mst, executionTime) = graph.KruskalMST();
            runTimes.Add(executionTime);

            Console.WriteLine($"Run {i + 1} execution time: {executionTime.TotalMilliseconds} ms");

            // Only print MST details for first run
            if (i == 0)
            {
                graph.PrintMST(mst);
            }
        }

        // Calculate and display statistics
        double averageTime = runTimes.Average(t => t.TotalMilliseconds);
        Console.WriteLine($"\nAverage execution time: {averageTime} ms");
    }
}
