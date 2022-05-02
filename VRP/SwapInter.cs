using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  class SwapInter : LocalSearch {
    /*
      procedure SwapInterRute(Solution)
      For each car in the problem
        For each other car2 in the problem
          For each node in the car's solution
            For each node in the car2 solution
              If the nodes are not the same
                swapCost = deltaCost(node1, node2)
                If swapCost < bestSwapCost
                  bestSwapCost[car] = swapCost
                  bestSwapNode1[car] = node1
                  bestSwapNode2[car2] = node2
                end;
              end;
            end;
          end;
        If a bettser cost was found
          Swap the nodes
        end;
        end;
      end;
      return newSolution;
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

      for (int car1 = 0; car1 < graph.CarNumber; car1++) {
        for (int car2 = car1 + 1; car2 < graph.CarNumber; car2++) {
          for (int j = 1; j < solution.Paths[car1].Count - 1; j++) {
            for (int k = 1; k < solution.Paths[car2].Count - 1; k++) {
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
                bestSecondNode = k;
                updated = true;
              }
            }
          }
          if (updated) {
          Node node1 = newSolution.Paths[car1][bestFirstNode];
          Node node2 = newSolution.Paths[car2][bestSecondNode];
          newSolution.Paths[car1][bestFirstNode] = node2;
          newSolution.Paths[car2][bestSecondNode] = node1;
          updated = false;
          bestCost = int.MaxValue;
          }
        }
      }
      return newSolution;
    }
  }
}