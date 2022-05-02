using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VRP {

  class Solution {
    public List<List<Node>> Paths { get; set; }
    public int[] Costs { get; set; }

    public Solution(List<List<Node>> paths, int[] costs) {
      Paths = paths;
      Costs = costs;
    }

    public Solution() {
      Paths = new List<List<Node>>();
      Costs = new int[] {};
    }
  }
}