using JI.Unity.SpiderWorld.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Graph
{
    public class PathNode
    {
        public Coords3D Coords { get; set; }

        /// <summary>
        /// Approximate distance from target
        /// </summary>
        public int HeuristicScore { get; set; }
        /// <summary>
        /// Approximate distance from starting point
        /// </summary>
        public int GScore { get; set; }

        /// <summary>
        /// Combination of H and G scores
        /// </summary>
        public int Score { get; set; }

        public PathNode GetLeft(PathNode start, PathNode goal)
        {
            return GetNeighbor(start, goal, Coords.Left());
        }

        public PathNode GetRight(PathNode start, PathNode goal)
        {
            return GetNeighbor(start, goal, Coords.Right());
        }

        public PathNode GetForward(PathNode start, PathNode goal)
        {
            return GetNeighbor(start, goal, Coords.Forward());
        }

        public PathNode GetBack(PathNode start, PathNode goal)
        {
            return GetNeighbor(start, goal, Coords.Back());
        }

        private PathNode GetNeighbor(PathNode start, PathNode goal, Coords3D coords)
        {
            var h = Mathf.Abs(coords.X - goal.Coords.X) + Mathf.Abs(coords.Y - goal.Coords.Y);
            var g = Mathf.Abs(coords.X - start.Coords.X) + Mathf.Abs(coords.Y - start.Coords.Y);

            return new PathNode
            {
                Coords = Coords.Left(),
                GScore = g,
                HeuristicScore = h,
                Score = g + h
            };
        }
        public override bool Equals(object obj)
        {
            if(obj is PathNode)
            {
                var node = obj as PathNode;
                return Coords.Equals(node.Coords);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Coords.GetHashCode();
        }
    }
}