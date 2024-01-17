using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeComparer : IComparer<Node>
{
    public int Compare(Node m, Node n) {
        return m.value.CompareTo(n.value);
    }

}
