using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Structs
{
    public class Coords3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Coords3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Coords3D(Vector3 position, float unitSize)
        {
            X = (int)(position.x / unitSize);
            Y = (int)(position.y / unitSize);
            Z = (int)(position.z / unitSize);
        }
        public override bool Equals(object obj)
        {
            if(obj is Coords3D)
            {
                var cmp = obj as Coords3D;
                return X == cmp.X && Y == cmp.Y && Z == cmp.Z;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (X + Y + Z).GetHashCode();
        }

        public Coords3D Right()
        {
            return new Coords3D
            {
                X = X + 1,
                Y = Y,
                Z = Z
            };
        }

        public Coords3D Left()
        {
            return new Coords3D
            {
                X = X - 1,
                Y = Y,
                Z = Z
            };
        }

        public Coords3D Forward()
        {
            return new Coords3D
            {
                X = X,
                Y = Y,
                Z = Z + 1
            };
        }

        public Coords3D Back()
        {
            return new Coords3D
            {
                X = X,
                Y = Y,
                Z = Z - 1
            };
        }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1} Z: {2}", X, Y, Z);
        }
    }
}