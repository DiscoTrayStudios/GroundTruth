using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public (int, int) position;
    public float value;

    public Node((int, int) ps, float v)
    {
        this.position = ps;
        this.value = v;
    }

}
