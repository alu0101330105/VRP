using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {
  class RInsertInter : Neighborhood {
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
      int secondRoute = r.Next(0, newSolution.Paths.Count);
      int secondNode = r.Next(0, newSolution.Paths[secondRoute].Count);
      // insert firstNode in secondRoute before secondNode
      newSolution.Paths[secondRoute].Insert(secondNode, newSolution.Paths[firstRoute][firstNode]);
      newSolution.Paths[firstRoute].RemoveAt(firstNode);
      newSolution.Costs = new int[graph.CarNumber];
      for (int i = 0; i < graph.CarNumber; i++) {
        newSolution.Costs[i] = graph.CalculatePathValue(newSolution.Paths[i]);
      }
      return newSolution;
    }
  }
}