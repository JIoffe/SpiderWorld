using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Utils
{
    public static class MathUtils
    {
        public static bool Approximately(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.0001f;
        }

        public static bool Approximately(Vector3 a, Vector3 b)
        {
            return Approximately(a.x, b.x) &&
                Approximately(a.y, b.y) &&
                Approximately(a.z, b.z);
        }

        public static bool Coplanar(Vector3 a, Vector3 b)
        {
            return Approximately(a.x, b.x) ||
                Approximately(a.y, b.y) ||
                Approximately(a.z, b.z);
        }
    }
}