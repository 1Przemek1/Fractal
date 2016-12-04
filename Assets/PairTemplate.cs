using UnityEngine;
using System.Collections;

public class PairTemplate<T,K>  {

    public PairTemplate()
    {

    }

    public PairTemplate(T first, K second)
    {
        this.first = first;
        this.second = second;
    }

    public T first { get; set; }
    public K second { get; set; }
}
