using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Topology;
using RegionRoaming.Mathematics;
using System.Linq;

namespace RegionRoaming
{
    public class Region : MonoBehaviour
    {
        public List<Vector3> Vertices = new List<Vector3>()
        { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1) };

        private List<Triangle> triangles;
        private double areaSum;

        private void Awake()
        {
            triangles = new List<Triangle>();
            triangles.AddRange(RegionMathematics.Triangulate(Vertices));
            areaSum = 0f;
            triangles.ForEach(x => areaSum += x.TriArea());

            foreach(var tri in triangles)
            {
                var points = tri.ToVertexList().Select(points => new Vector3(points.x, 0f, points.y)).ToList();
                points.Add(points[0]);
            }
        }

        public Vector3 PickRandomLocation()
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            return new Vector3(randomPos.x, 0f, randomPos.y);
        }

        private Triangle PickRandomTriangle()
        {
            var range = Random.Range(0f, (float)areaSum);
            for (int i = 0; i < triangles.Count; i++)
            {
                if (range < triangles[i].TriArea())
                    return triangles[i];
                range -= triangles[i].TriArea();
            }
            throw new System.Exception("Should not get here.");
        }

        private Vector2 RandomWithinTriangle(Triangle t)
        {
            var range1 = Mathf.Sqrt(Random.Range(0f, 1f));
            var range2 = Random.Range(0f, 1f);
            var m1 = 1 - range1;
            var m2 = range1 * (1 - range2);
            var m3 = range2 * range1;

            var point1 = t.GetVertex(0).ToVector2();
            var point2 = t.GetVertex(1).ToVector2();
            var point3 = t.GetVertex(2).ToVector2();

            return (m1 * point1) + (m2 * point2) + (m3 * point3);
        }
    }
}
