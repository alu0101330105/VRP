using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {
  class RTwoOpt : Neighborhood {
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
      int secondNode = r.Next(firstNode, newSolution.Paths[firstRoute].Count);
      // insert firstNode before secondNode
      newSolution.Paths[firstRoute] = twoOptSwap(newSolution.Paths[firstRoute], firstNode, secondNode);
      newSolution.Costs = (int[])solution.Costs.Clone();
      newSolution.Costs[firstRoute] = graph.CalculatePathValue(newSolution.Paths[firstRoute]);
      return newSolution;
    }

    /*
      procedure 2optSwap(route, i, k) {
        1. take route[0] to route[i-1] and add them in order to new_route
        2. take route[i] to route[k] and add them in reverse order to new_route
        3. take route[k+1] to end and add them in order to new_route
        return new_route;
      }
    */
    private List<Node> twoOptSwap(List<Node> route, int i, int k) {
      // use addRange and reverse to make it faster
      List<Node> newRoute = new List<Node>();
      newRoute.AddRange(route.GetRange(0, i));
      List<Node> subRoute = route.GetRange(i, k - i + 1);
      subRoute.Reverse();
      newRoute.AddRange(subRoute);
      newRoute.AddRange(route.GetRange(k + 1, route.Count - k - 1));
      return newRoute;
    }


  }
}