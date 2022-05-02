using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {
  class RSwapIntra : Neighborhood {
    public override Solution Random(Solution solution, Graph graph) {
      Solution newSolution = new Solution();
      newSolution.Paths = new List<List<Node>>();
      foreach (List<Node> path in solution.Paths) {
        newSolution.Paths.Add(new List<Node>(path));
        foreach (Node node in path) {
          newSolution.Paths[newSolution.Paths.Count - 1].Add(node);
        }
      }
      Random r = new Random();
      int firstRoute = r.Next(0, solution.Paths.Count);
      int firstNode = r.Next(0, solution.Paths[firstRoute].Count);
      int secondNode = r.Next(0, solution.Paths[firstRoute].Count);
      // Swap firstNode in firstRoute with secondNode in secondRoute
      Node node1 = solution.Paths[firstRoute][firstNode];
      Node node2 = solution.Paths[firstRoute][secondNode];
      newSolution.Paths[firstRoute][firstNode] = node2;
      newSolution.Paths[firstRoute][secondNode] = node1;
      newSolution.Costs = new int[graph.CarNumber];
      for (int i = 0; i < graph.CarNumber; i++) {
        newSolution.Costs[i] = graph.CalculatePathValue(newSolution.Paths[i]);
      }
      return newSolution;
    }
  }
}