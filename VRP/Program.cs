using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace VRP {
  class Program {
    static void Main(string[] args) {
      // read graph from file
      Graph graph = new Graph();
      graph.ReadFromFile("input.txt");
      // solve the problem
      Solution result = new Greedy().Solve(graph);
      // print result
      Console.WriteLine("Result Greed:");
      for (int i = 0; i < result.Paths.Count; i++) {
        foreach (Node node in result.Paths[i]) {
          Console.Write(node.Name + " -> ");
        }
        Console.Write($"{{Cost: {result.Costs[i]} }}\n");
      }
      Console.WriteLine($"Total Cost: {result.Costs.Sum()}");

      LocalSearch localSearch = new SwapIntra();
      Solution result2 = new Grasp().Solve(graph, localSearch);
      Console.WriteLine("Result Grasp:");
      for (int i = 0; i < result2.Paths.Count; i++) {
        foreach (Node node in result2.Paths[i]) {
          Console.Write(node.Name + " -> ");
        }
        Console.Write($"{{Cost: {result2.Costs[i]} }}\n");
      }
      Console.WriteLine("Total cost: " + result2.Costs.Sum());

      LocalSearch localSearch2 = new Gvns();
      Solution result3 = new Grasp().Solve(graph, localSearch2);
      Console.WriteLine("Result Gvns:");
      for (int i = 0; i < result3.Paths.Count; i++) {
        foreach (Node node in result3.Paths[i]) {
          Console.Write(node.Name + " -> ");
        }
        Console.Write($"{{Cost: {result3.Costs[i]} }}\n");
      }
      Console.WriteLine("Total cost: " + result3.Costs.Sum());
    }
  }
}