using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRP {

  abstract class Neighborhood {
    public abstract Solution Random(Solution solution, Graph graph);
  }
}