using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Collections
{
    public class Tuple<T1, T2>
    {
        public Tuple(T1 a, T2 b)
        {
            First = a;
            Second = b;
        }

        public T1 First { get; private set; }
        public T2 Second { get; private set; }
    }
}