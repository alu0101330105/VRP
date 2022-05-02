using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  abstract class LocalSearch {
    public abstract Solution Solve(Solution solution, Graph graph);
  }
}