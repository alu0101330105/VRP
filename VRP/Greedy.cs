using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace VRP {
  class Greedy : Method {
    /// <summary>
    /// The function finds the closest unvisited node to each car, visits it, and then returns to the
    /// starting node when all nodes are visited
    /// </summary>
    /// <param name="Graph">The graph object that contains the nodes and the distance matrix.</param>
    /// <returns>
    /// The followed path for each car and the cost of each of those paths
    /// </returns>
    public override Solution Solve(Graph graph) {
      int n = graph.Nodes.Count;
      int[,] distanceMatrix = graph.DistanceMatrix;
      bool[] visited = new bool[n];
      int[] carPosition = new int[graph.CarNumber];
      List<List<Node>> visitedNodesPerCar = new List<List<Node>>();
      for (int i = 0; i < graph.CarNumber; i++) {
        visitedNodesPerCar.Add(new List<Node>());
      }
      bool Starting = true;
      // while there are still unvisited nodes
      while (visited.Count(x => !x) > 0) {
        int[] closestNode = new int[graph.CarNumber];

        if (!Starting) {
          // find the closest node to each car
          for (int i = 0; i < graph.CarNumber; i++) {
            int minDistance = int.MaxValue;
            for (int j = 0; j < n; j++) {
              if (!visited[j] && distanceMatrix[carPosition[i], j] < minDistance) {
                // if node is actually the closest node to any other car
                if (closestNode.Any(x => x == j)) {
                  continue;
                }
                minDistance = distanceMatrix[carPosition[i], j];
                closestNode[i] = j;
              }
            }
          }
        } else {
          // if it is the first iteration, the closest node to each car is the starting node
          for (int i = 0; i < graph.CarNumber; i++) {
            closestNode[i] = 0;
          }
          Starting = false;
        }
        // visit the closest node to each car
        for (int i = 0; i < graph.CarNumber; i++) {
          visited[closestNode[i]] = true;
          visitedNodesPerCar[i].Add(graph.Nodes[closestNode[i]]);
          carPosition[i] = closestNode[i];
        }
      }
      // return to the starting node for each car
      for (int i = 0; i < graph.CarNumber; i++) {
        visitedNodesPerCar[i].Add(visitedNodesPerCar[i][0]);
      }
      int[] pathValue = CalculatePathValue(visitedNodesPerCar, graph);
      return new Solution(visitedNodesPerCar, pathValue);
    }

    // calculates cost of the path of each car using the edges in the graph
    private int[] CalculatePathValue(List<List<Node>> visitedNodesPerCar, Graph graph) {
      List<int> result = new List<int>();
      foreach (dynamic car in visitedNodesPerCar) {
        result.Add(graph.CalculatePathValue(car));
      }
      return result.ToArray();
    }
  }
}