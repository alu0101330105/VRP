using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VRP {
  // TODO divide each local search into a class
  // TODO Implement GVNS


  class Grasp {
    public Solution Solve(Graph graph, LocalSearch localSearch) {
      int maxIterations = 1000;
      int maxNoImprovementIterations = 100;
      int noImprovementIterations = 0;
      // for maxIterations iterations
      // for noImprovementIterations iterations without improvement
      //  Construct a solution
      //  use local search to improve the solution
      //  if the solution is better than the best solution so far, replace it.
      Solution solution = ConstructSolution(graph);
      for (int i = 0; i < maxIterations; i++) {
        Solution newSolution = ConstructSolution(graph);
        Solution improvedSolution = localSearch.Solve(newSolution, graph);
        if (improvedSolution.Costs.Sum() < solution.Costs.Sum()) {
          solution = improvedSolution;
          // Console.WriteLine($"{i} [{noImprovementIterations}] {solution.Costs[0]} {solution.Costs[1]} {{ {solution.Costs.Sum()} }}");
          noImprovementIterations = 0;
        } else {
          noImprovementIterations++;
        }
        if (noImprovementIterations == maxNoImprovementIterations) {
          break;
        }
      }
      return solution;
    }

    /*
      procedure Greedy_Randomized_construction(Seed)
      Solution <- Void
      Initialize the set of candidate elements;
      Evaluate the incremental costs of the candidate elements;
      while there exists at least one candidate do
        For each car in the problem
          If it is the first iteration
            Select the first candidate element
          Else
            Build the restricted candidate list (5 nodes) using greedy algorithm
            Select an element s from the RCL at random;
            Solution <- Solution U {s};
            Update the set of candidate elements;
          end;
        end;
      end;
      return Solution;
      end;
    */
    // Grasp use my greedy procedure
    private Solution ConstructSolution(Graph graph) {
      int[,] distanceMatrix = graph.DistanceMatrix;
      Random random = new Random();
      List<int> available = new List<int>();
      for (int i = 0; i < graph.Nodes.Count; i++) {
        available.Add(i);
      }

      Solution result = new Solution();
      for (int i = 0; i < graph.CarNumber; i++) {
        result.Paths.Add(new List<Node>());
        result.Paths[i].Add(graph.Nodes[0]);
      }
      result.Costs = new int[graph.CarNumber];
      Node actualNode = graph.Nodes[0];
      available.Remove(0);

      while (available.Count > 0) {
        for (int i = 0; i < graph.CarNumber; i++) {
          if (available.Count == 0) {
            break;
          }
          actualNode = result.Paths[i].Last();
          List<Node> rcl = GetNext(2, available, actualNode, graph);
          if (rcl.Count() == 0) {
            break;
          }
          int randomId = random.Next(0, rcl.Count);
          Node nextNode = rcl[randomId];
          int distance = distanceMatrix[actualNode.Id, nextNode.Id];
          for (int j = 0; j < available.Count; j++) {
            if (available[j] == nextNode.Id) {
              available.RemoveAt(j);
              break;
            }
          }
          result.Paths[i].Add(nextNode);
          result.Costs[i] += distance;
        }
      }

      for (int i = 0; i < graph.CarNumber; i++) {
        result.Paths[i].Add(graph.Nodes[0]);
        result.Costs[i] += distanceMatrix[result.Paths[i].Last().Id, graph.Nodes[0].Id];
      }
      return result;
    }

    // Get the closest n nodes to last from the candidate list using graph distance matrix
    private List<Node> GetNext(int n, List<int> available, Node last, Graph graph) {
      List<Node> result = new List<Node>();
      for (int i = 0; i < graph.Nodes.Count; i++) {
        if (available.Any((x) => x == i)) {
          result.Add(graph.Nodes[i]);
        }
      }
      result.Sort((x, y) => graph.DistanceMatrix[last.Id, x.Id] - graph.DistanceMatrix[last.Id, y.Id]);
      return result.Take(n).ToList();
    }
  }
}
