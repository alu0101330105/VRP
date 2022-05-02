using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace VRP {
  class Node {
    public string Name { get; private set; }
    public int Id { get; private set; }

    public Node(string name) {
      Name = name;
      Id = int.Parse(name);
    }

    public override string ToString() {
      return Name;
    }
  }
}
