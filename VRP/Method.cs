using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VRP {
  abstract class Method {
    public abstract Solution Solve(Graph graph);
  }
}