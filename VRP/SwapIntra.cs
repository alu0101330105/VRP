using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  class SwapIntra : LocalSearch {
    /*
      procedure SwapIntraRute(Solution)
      For each car in the problem
        For each node in the car's solution
          For each node in the car's solution
            If the nodes are not the same
              swapCost = deltaCost(node1, node2)
              If swapCost < bestSwapCost
                bestSwapCost[car] = swapCost
                bestSwapNode1[car] = node1
                bestSwapNode2[car] = node2
              end;
            end;
          end;
        end;
        If a bettser cost was found
          Swap the nodes
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

      for (int i = 0; i < graph.CarNumber; i++) {
        for (int j = 1; j < solution.Paths[i].Count - 1; j++) {
          for (int k = j + 1; k < solution.Paths[i].Count - 1; k++) {
            // if the swap produces a lower cost than the current best
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
              bestSecondNode = k;
              updated = true;
            }
          }
        }
        if (updated) {
          Node node1 = newSolution.Paths[i][bestFirstNode];
          Node node2 = newSolution.Paths[i][bestSecondNode];
          newSolution.Paths[i][bestFirstNode] = node2;
          newSolution.Paths[i][bestSecondNode] = node1;
          updated = false;
          bestCost = int.MaxValue;
        }
      }
      return newSolution;
    }
  }
}