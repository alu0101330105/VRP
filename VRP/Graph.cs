using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VRP {
  class Graph {
    public List<Node> Nodes;
    public List<Edge> Edges;
    public int   CarNumber;
    public int[,] DistanceMatrix;

    // class to represent a weighted graph from a file containing the distance matrix
    public Graph() {
      Nodes = new List<Node>();
      Edges = new List<Edge>();
        CarNumber = 0;
      DistanceMatrix = new int[0, 0];
    }

    /// <summary>
    /// It reads the input file and creates a distance matrix, a list of nodes, and a list of edges
    /// </summary>
    /// <param name="inputFile">the file path of the input file</param>
    public void ReadFromFile(string inputFile) {
      string[] lines = File.ReadAllLines(inputFile);
      int n = int.Parse(lines[0].Split('\t')[1]) + 1;
        CarNumber = int.Parse(lines[1].Split('\t')[1]);
      int[,] matrix = new int[n, n];
      for (int i = 0; i < n; i++) {
        string[] line = lines[i + 3].Split('\t');
        for (int j = 0; j < n; j++) {
          matrix[i, j] = int.Parse(line[j]);
        }
      }
      DistanceMatrix = matrix;
      Nodes = new List<Node>();
      Edges = new List<Edge>();
      for (int i = 0; i < n; i++) {
        Nodes.Add(new Node(i.ToString()));
      }
      for (int i = 0; i < n; i++) {
        for (int j = i + 1; j < n; j++) {
          Edges.Add(new Edge(Nodes[i], Nodes[j], matrix[i, j]));
        }
      }
    }

    /// <summary>
    /// It prints out the nodes and edges of the graph
    /// </summary>
    public void Print() {
      Console.WriteLine("Nodes:");
      foreach (Node node in Nodes) {
        Console.WriteLine(node.ToString());
      }
      Console.WriteLine("Edges:");
      foreach (Edge edge in Edges) {
        Console.WriteLine(edge.ToString());
      }
    }

    /// <summary>
    /// It takes a list of nodes and returns the sum of the distances between each node in the list
    /// </summary>
    /// <param name="path">The path to calculate the value of.</param>
    /// <returns>
    /// The total distance of the path.
    /// </returns>
    public int CalculatePathValue(List<Node> path) {
      int value = 0;
      for (int i = 0; i < path.Count - 1; i++) {
        value += DistanceMatrix[path[i].Id, path[i + 1].Id];
      }
      return value;
    }
  }
}