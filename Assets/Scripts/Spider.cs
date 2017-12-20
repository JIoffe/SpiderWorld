using JI.Unity.SpiderWorld.Collections;
using JI.Unity.SpiderWorld.Graph;
using JI.Unity.SpiderWorld.Structs;
using JI.Unity.SpiderWorld.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JI.Unity.SpiderWorld.Characters
{
    public class Spider : MonoBehaviour
    {
        /// <summary>
        /// Speed in M/S
        /// </summary>
        public float movementSpeed = 0.3f;

        /// <summary>
        /// The size in meters between units in the grid used for path finding
        /// </summary>
        public float gridPrecision = 0.06f;
        public GameObject waypointPrefab;

        private readonly YieldInstruction NextFrame = new WaitForEndOfFrame();
        private Animator Animator { get; set; }
        private SphereCollider SphereCollider { get; set; }
        private float ColliderDiameter { get; set; }

        private bool renderPath = false;

        // Use this for initialization
        void Start()
        {
            Animator = GetComponentInChildren<Animator>();
            SphereCollider = GetComponentInChildren<SphereCollider>();
            if (SphereCollider != null)
                ColliderDiameter = SphereCollider.radius * 2f;
        }

        public void TogglePathRender()
        {
            renderPath = !renderPath;
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void OnMovementRequested()
        {
            Debug.Log("Got movement request");
            StopAllCoroutines();

            //Defaults to pointing towards player view

            var player = Camera.main.transform;
            //Process waypoints

            RaycastHit hitInfo;
            if (!Physics.Raycast(player.position, player.forward, out hitInfo, 50f, (1 << 8)))
                return;

            StartCoroutine(ComputeWaypoints(new Tuple<Vector3, Vector3>(hitInfo.point, hitInfo.normal)));
        }

        IEnumerator ComputeWaypoints(Tuple<Vector3, Vector3> target)
        {
            var pos = transform.position;

            var yRay = target.First - pos;
            yRay.y = 0;
            yRay.Normalize();

            var points = new List<Tuple<Vector3, Vector3>>();

            if (!MathUtils.Coplanar(target.First, pos))
            {
                for (var t = 0f; t < 1f; t += gridPrecision)
                {
                    var origin = Vector3.Lerp(target.First, pos, t);

                    RaycastHit hitInfo;
                    if (target.First.y > pos.y)
                    {
                        if (Physics.Raycast(origin, yRay, out hitInfo, 50f, (1 << 8)))
                        {
                            points.Add(new Tuple<Vector3, Vector3>(hitInfo.point, hitInfo.normal));
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(origin, -yRay, out hitInfo, 50f, (1 << 8)))
                        {
                            points.Add(new Tuple<Vector3, Vector3>(hitInfo.point, hitInfo.normal));
                        }
                    }
                    if (Physics.Raycast(origin, Vector3.down, out hitInfo, 50f, (1 << 8)))
                    {
                        points.Add(new Tuple<Vector3, Vector3>(hitInfo.point, hitInfo.normal));
                    }

                    //if (Physics.Raycast(origin, Vector3.up, out hitInfo, 50f, (1 << 8)))
                    //{
                    //    points.Add(new Tuple<Vector3, Vector3>(hitInfo.point, hitInfo.normal));
                    //}

                    yield return null;
                }
            }

            points.Add(target);

            Debug.Log("Found " + points.Count + " possible waypoints");

            //Parse the point candidates into an actual path
            var path = new List<Tuple<Vector3, Vector3>>();

            //First determine the closest point to the spider
            var p = GetNearestPoint(pos, points);
            points.RemoveAll(point => MathUtils.Approximately(point.First, p.First));
            do
            {
                //Debug.Log("Adding " + p + " to path");
                path.Add(p);
                p = GetNearestPoint(p.First, points);
                points.RemoveAll(point => MathUtils.Approximately(point.First, p.First));
                yield return null;
            }
            while (p != null && p != target);

            path.Add(target);

            if(renderPath)
                RenderPoints(path.Select(waypoint => waypoint.First));

            StartCoroutine(Coroutine_TravelPath(path));

            yield break;
        }

        IEnumerator Coroutine_TravelPath(IEnumerable<Tuple<Vector3, Vector3>> path)
        {
            if (Animator != null)
                Animator.SetBool("IsWalking", true);

            foreach(var waypoint in path)
            {
                var point = waypoint.First;

                var start = transform.position;
                var distance = Vector3.Distance(start, point);

                //Rate is a function of distance and movement speed
                var rate = movementSpeed / distance;
                var t = 0f;

                var forward = (point - start).normalized;
                var up = waypoint.Second;
                var targetRotation = Quaternion.LookRotation(forward, up);
                var startRotation = transform.rotation;

                while(t < 1f)
                {
                    transform.position = Vector3.Lerp(start, point, t);
                    transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

                    t += rate * Time.deltaTime;
                    yield return NextFrame;
                }
            }

            if (Animator != null)
                Animator.SetBool("IsWalking", false);

            yield break;
        }
        private void RenderPoints(IEnumerable<Vector3> points)
        {
            foreach (var point in points)
            {
                Instantiate(waypointPrefab, point, Quaternion.identity);
            }
        }

        private Tuple<Vector3, Vector3> GetNearestPoint(Vector3 p, IEnumerable<Tuple<Vector3, Vector3>> waypoints)
        {
            var minDistance = double.MaxValue;
            Tuple<Vector3, Vector3> nearestPoint = null;

            foreach(var waypoint in waypoints)
            {
                var point = waypoint.First;

                if (point == p || MathUtils.Approximately(point, p))
                    continue;
                
                var d = Vector3.Distance(point, p);
                if (d < minDistance)
                {
                    minDistance = d;
                    nearestPoint = waypoint;
                }
            }
            return nearestPoint;
        }
    }
}