using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  class TwoOpt : LocalSearch {

    /*
      procedure 2opt(Solution)
      For each car in the problem
        For each node in the car's solution
          For each node in the car's solution
            If the nodes are not the same
              twoOptSwap the nodes
              If the solution improves
                Replace the old solution with the new one
              Else
                Restore the old solution
              end;
            end;
          end;
        end;
      end;
    */
    public override Solution Solve(Solution solution, Graph graph) {
      int bestCost = int.MaxValue;
      int bestFirstNode = -1;
      int bestSecondNode = -1;
      bool updated = false;

      Solution newSolution = new Solution();
      newSolution.Paths = new List<List<Node>>();
      // copy solution paths
      for (int i = 0; i < graph.CarNumber; i++) {
        newSolution.Paths.Add(new List<Node>());
        for (int j = 0; j < solution.Paths[i].Count; j++) {
          newSolution.Paths[i].Add(solution.Paths[i][j]);
        }
      }
      newSolution.Costs = new int[graph.CarNumber];
      for (int i = 0; i < graph.CarNumber; i++) {
        newSolution.Costs[i] = solution.Costs[i];
      }

      for (int i = 0; i < graph.CarNumber; i++) {
        for (int j = 1; j < solution.Paths[i].Count - 1; j++) {
          for (int k = j + 1; k < solution.Paths[i].Count - 1; k++) {
            if (j != k) {
              int delta = twoOtpCost(solution.Paths[i], j, k, graph);
              if (solution.Costs[i] + delta < bestCost) {
                bestCost = solution.Costs[i] + delta;
                bestFirstNode = j;
                bestSecondNode = k;
                updated = true;
              }
            }
          }
        }
        if (updated) {
          newSolution.Paths[i] = twoOptSwap(newSolution.Paths[i], bestFirstNode, bestSecondNode);
          newSolution.Costs[i] = bestCost;
          updated = false;
          bestCost = int.MaxValue;
        }
      }
      return solution;
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

    private int twoOtpCost(List<Node> route, int firstIndex, int secondIndex, Graph graph) {
      int[,] distanceMatrix = graph.DistanceMatrix;
      int diference = 0;
      int prevValue = route[firstIndex - 1].Id;
      int nextValue = route[secondIndex + 1].Id;
      diference -= distanceMatrix[prevValue, route[firstIndex].Id];
      diference -= distanceMatrix[route[secondIndex].Id, nextValue];
      for (int i = firstIndex; i < secondIndex; i++) {
        diference += distanceMatrix[route[i].Id, route[i + 1].Id];
      }
      diference += distanceMatrix[prevValue, route[secondIndex].Id];
      diference += distanceMatrix[route[firstIndex].Id, nextValue];
      for (int i = firstIndex; i < secondIndex; i++) {
        diference += distanceMatrix[route[i + 1].Id, route[i].Id];
      }
      return diference;
    }


    // Calculate the costs of the solution
    private int[] CalculateCosts(List<List<Node>> paths, Graph graph) {
      List<int> costs = new List<int>();
      foreach (List<Node> path in paths) {
        costs.Add(graph.CalculatePathValue(path));
      }
      return costs.ToArray();
    }
  }
}