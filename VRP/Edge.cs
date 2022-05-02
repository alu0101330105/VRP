using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VRP {
  class Edge {
    public Node From { get; set; }
    public Node To { get; set; }
    public int Weight { get; set; }

    public Edge(Node from, Node to, int weight) {
      From = from;
      To = to;
      Weight = weight;
    }

    public override string ToString() {
      return string.Format("{0} -> {1} ({2})", From, To, Weight);
    }
  }
}