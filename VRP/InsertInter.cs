using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  class InsertInter : LocalSearch {
    /*
      procedure inserInter(Solution)
      For each car in the problem
        For each other car in the problem
          For each node in the car's solution
            Calculate the cost of inserting the node in places in the other car's solution
            If the current cost is better tha the best cost so far
              If the insertion does not surpass the limit of nodes in the solution
                BestCost <- CurrentCost
                BestNode <- CurrentNode
              end;
            end;
          end;
          If a there was a better cost inserting any node
            Insert the best node in the others car's solution
          end;
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

      for (int car1 = 0; car1 < graph.CarNumber; car1++) {
        for (int car2 = 0; car2 < graph.CarNumber; car2++) {
          if (car1 != car2) {
            for (int j = 1; j < solution.Paths[car1].Count - 1; j++) {
              for (int k = 1; k < solution.Paths[car2].Count - 1; k++) {
                if (j != k) {
                  int newCost = solution.Costs.Sum();
                  newCost -= graph.DistanceMatrix[solution.Paths[car1][j - 1].Id, solution.Paths[car1][j].Id];
                  newCost -= graph.DistanceMatrix[solution.Paths[car1][j].Id, solution.Paths[car1][j + 1].Id];
                  newCost -= graph.DistanceMatrix[solution.Paths[car2][k].Id, solution.Paths[car2][k + 1].Id];
                  newCost += graph.DistanceMatrix[solution.Paths[car2][k - 1].Id, solution.Paths[car2][k].Id];
                  newCost += graph.DistanceMatrix[solution.Paths[car2][k].Id, solution.Paths[car2][k + 1].Id];
                  newCost -= graph.DistanceMatrix[solution.Paths[car2][k - 1].Id, solution.Paths[car2][k].Id];
                  newCost += graph.DistanceMatrix[solution.Paths[car2][k].Id, solution.Paths[car2][k + 1].Id];
                  newCost += graph.DistanceMatrix[solution.Paths[car2][k].Id, solution.Paths[car2][k].Id];

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
              newSolution.Paths[car2].Insert(bestInsertPlace, solution.Paths[car1][bestFirstNode]);
              newSolution.Paths[car1].RemoveAt(bestFirstNode);
              newSolution.Costs = CalculateCosts(newSolution.Paths, graph);
              updated = false;
              bestCost = int.MaxValue;
            }
          }
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