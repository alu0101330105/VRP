using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  class InsertIntra : LocalSearch {
    /*
      procedure insertIntra(Solution)
      For each car in the problem
        For each node in the car's solution
          Calculate the cost of inserting the node in other places in the car's solution
          If the current cost is better tha the best cost so far
            BestCost <- CurrentCost
            BestNode <- CurrentNode
          end;
        end;
        If a there was a better cost inserting any node
          Insert the best node in the car's solution
        end;
      end;
      return Solution;
      end;
    */
    public override Solution Solve(Solution solution, Graph graph) {
      int bestCost = int.MaxValue;
      int bestFirstNode = -1;
      int bestInsertPlace = -1;
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
          for (int k = 1; k < solution.Paths[i].Count - 1; k++) {
            if (j != k) {
              int newCost = solution.Costs[i];
              newCost -= graph.DistanceMatrix[solution.Paths[i][j - 1].Id, solution.Paths[i][j].Id];
              newCost -= graph.DistanceMatrix[solution.Paths[i][j].Id, solution.Paths[i][j + 1].Id];
              newCost -= graph.DistanceMatrix[solution.Paths[i][k].Id, solution.Paths[i][k + 1].Id];
              newCost += graph.DistanceMatrix[solution.Paths[i][j - 1].Id, solution.Paths[i][k].Id];
              newCost += graph.DistanceMatrix[solution.Paths[i][j].Id, solution.Paths[i][k + 1].Id];

              if (solution.Paths[i][j + 1].Id == solution.Paths[i][k].Id) {
                newCost += graph.DistanceMatrix[solution.Paths[i][k].Id, solution.Paths[i][j].Id];
              } else {
                newCost -= graph.DistanceMatrix[solution.Paths[i][k - 1].Id, solution.Paths[i][k].Id];
                newCost += graph.DistanceMatrix[solution.Paths[i][k].Id, solution.Paths[i][j + 1].Id];
                newCost += graph.DistanceMatrix[solution.Paths[i][k].Id, solution.Paths[i][j].Id];
              }

              if (newCost < bestCost) {
                bestCost = newCost;
                bestFirstNode = j;
                bestInsertPlace = k;
                updated = true;
              }
            }
          }
        }
        if (updated) {
          newSolution.Paths[i].Insert(bestInsertPlace, solution.Paths[i][bestFirstNode]);
          if (bestFirstNode > bestInsertPlace) {
            newSolution.Paths[i].RemoveAt(bestFirstNode + 1);
          } else {
            newSolution.Paths[i].RemoveAt(bestFirstNode);
          }
          newSolution.Costs = CalculateCosts(newSolution.Paths, graph);
          updated = false;
          bestCost = int.MaxValue;
        }
      }
      return newSolution;
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