using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {
  class RInsertIntra : Neighborhood {
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
      int firstRoute = r.Next(0, newSolution.Paths.Count);
      int firstNode = r.Next(0, newSolution.Paths[firstRoute].Count);
      int secondNode = r.Next(0, newSolution.Paths[firstRoute].Count);
      // insert firstNode before secondNode
      Node node1 = newSolution.Paths[firstRoute][firstNode];
      Node node2 = newSolution.Paths[firstRoute][secondNode];
      newSolution.Paths[firstRoute][secondNode] = node1;
      newSolution.Paths[firstRoute][firstNode] = node2;
      newSolution.Costs = (int[])solution.Costs.Clone();
      newSolution.Costs[firstRoute] = graph.CalculatePathValue(newSolution.Paths[firstRoute]);
      return newSolution;
    }
  }
}