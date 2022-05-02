using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  class Gvns : LocalSearch{

    /*
      procedure GVNS(Solution)
      Set the order of Neighborhoods
      K <- Neighborhoods.Count
      Repeat untill i = K
        Shake: Generate a random solution using Neighborhood[i]
        LocalSearch using VND
        If solution Local optimum is better than the best solution
          Replace the best solution with the new solution
          Reset i = 0
        Else
          i++
        end;
      end;
    */
    public override Solution Solve(Solution solution, Graph graph) {
      Neighborhood[] Nk = new Neighborhood[5] {
        new RInsertInter(),
        new RInsertIntra(),
        new RSwapInter(),
        new RSwapIntra(),
        new RTwoOpt(),
      };
      int bestCost = int.MaxValue;

      Solution bestSolution = new Solution();
      bestSolution.Paths = new List<List<Node>>();
      foreach (List<Node> path in solution.Paths) {
        bestSolution.Paths.Add(new List<Node>());
        foreach (Node node in path) {
          bestSolution.Paths[bestSolution.Paths.Count - 1].Add(node);
        }
      }
      bestSolution.Costs = (int[])solution.Costs.Clone();



      int i = 0;
      while (i < Nk.Length) {
        Solution shakeSolution = Nk[i].Random(solution, graph);
        Solution newSolution = VND(shakeSolution, graph);
        if (newSolution.Costs.Sum() < bestCost) {
          bestCost = newSolution.Costs.Sum();
          bestSolution = newSolution;
          i = 0;
          continue;
        }
        i++;
      }
      return bestSolution;
    }

    /*
      procedure VND(Solution)
      Set the order of LocalSearch
      K <- LocalSearch.Count
      Repeat untill i = K
        LocalSearch using LocalSearch[i]
        If solution Local optimum is better than the best solution
          Replace the best solution with the new solution
          Reset i = 0
        Else
          i++
        end;
      end;
    */
    private Solution VND(Solution solution, Graph graph) {
      LocalSearch[] Lk = new LocalSearch[5] {
        new InsertInter(),
        new InsertIntra(),
        new SwapInter(),
        new SwapIntra(),
        new TwoOpt(),
      };
      int bestCost = int.MaxValue;

      Solution bestSolution = new Solution();
      bestSolution.Paths = new List<List<Node>>();
      foreach (List<Node> path in solution.Paths) {
        bestSolution.Paths.Add(new List<Node>());
        foreach (Node node in path) {
          bestSolution.Paths[bestSolution.Paths.Count - 1].Add(node);
        }
      }
      bestSolution.Costs = (int[])solution.Costs.Clone();

      int i = 0;
      while (i < Lk.Length) {
        Solution newSolution = Lk[i].Solve(solution, graph);
        if (newSolution.Costs.Sum() < bestCost) {
          bestCost = newSolution.Costs.Sum();
          bestSolution = newSolution;
          i = 0;
          continue;
        }
        i++;
      }
      return bestSolution;
    }
  }
}