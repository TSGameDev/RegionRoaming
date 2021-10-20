using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace RegionRoaming.Mathematics
{
    public static class RegionMathematics
    {
        public static ICollection<Triangle> Triangulate(List<Vector3> points)
        {
            var pointsIn2D = points.Select(p => new Vector2(p.x, p.z)).ToList();
            return Triangulate(pointsIn2D);
        }

        public static Vertex ToVertex(this Vector2 vec2)
        {
            return new Vertex(vec2.x, vec2.y);
        }

        public static Vector2 ToVector2 (this Vertex v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        public static float TriArea(this Triangle t)
        {
            var point1 = t.GetVertex(0).ToVector2();
            var point2 = t.GetVertex(1).ToVector2();
            var point3 = t.GetVertex(2).ToVector2();

            Vector3 V = Vector3.Cross(point1 - point2, point1 - point3);
            return V.magnitude * 0.5f;
        }

        public static List<Vector2> ToVertexList(this Triangle t)
        {
            var ls = new List<Vector2>();
            for (int i = 0; i < 3; i++)
            {
                ls.Add(t.GetVertex(i).ToVector2());
            }
            return ls;
        }

        public static ICollection<Triangle> Triangulate(IEnumerable<Vector2> points)
        {
            var poly = new Polygon();
            poly.Add(new Contour(points.Select(p => p.ToVertex())));
            var mesh = poly.Triangulate();
            return mesh.Triangles;
        }
    }
}
