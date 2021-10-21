using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace RegionRoaming.Mathematics
{
    public static class RegionMathematics
    {
        /// <summary>
        /// Triangulates a set of Vector3 points on a 2D plane
        /// </summary>
        /// <param name="points">A List of Vector3s that make up a region on a 2D plane</param>
        /// <returns>A collection of Triangles</returns>
        public static ICollection<Triangle> Triangulate(List<Vector3> points)
        {
            //ICollection is just a collection of non-generic things.
            //.Select project each element of a sequence to a new form
            var pointsIn2D = points.Select(p => new Vector2(p.x, p.z)).ToList();
            return Triangulate(pointsIn2D);
        }

        /// <summary>
        /// Creates a new Vertex from a given Vector2
        /// </summary>
        /// <param name="vec2">The Vector2 to become a Vertex</param>
        /// <returns>A Vertex</returns>
        public static Vertex ToVertex(this Vector2 vec2)
        {
            return new Vertex(vec2.x, vec2.y);
        }

        /// <summary>
        /// Creates a new Vector2 from a given Vertex
        /// </summary>
        /// <param name="v">The Vertex to become a Vector2</param>
        /// <returns>A Vector2</returns>
        public static Vector2 ToVector2 (this Vertex v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        /// <summary>
        /// Takes in a Triangle and uses Vector Cross Product to calcualte its area
        /// </summary>
        /// <param name="t">The Triangle to calculate area from</param>
        /// <returns>A float of Triangle area</returns>
        public static float TriArea(this Triangle t)
        {
            var point1 = t.GetVertex(0).ToVector2();
            var point2 = t.GetVertex(1).ToVector2();
            var point3 = t.GetVertex(2).ToVector2();

            Vector3 V = Vector3.Cross(point1 - point2, point1 - point3);
            return V.magnitude * 0.5f;
        }

        /// <summary>
        /// Converts a Triangles points, vertexs, to a list of Vector2's
        /// </summary>
        /// <param name="t">A Triangle</param>
        /// <returns>A list of Vector2's</returns>
        public static List<Vector2> ToVector2List(this Triangle t)
        {
            var ls = new List<Vector2>();
            for (int i = 0; i < 3; i++)
            {
                ls.Add(t.GetVertex(i).ToVector2());
            }
            return ls;
        }

        /// <summary>
        /// Triangulates an IEnumerable of Vector2s
        /// </summary>
        /// <param name="points">An IEnumerable of Vector2s</param>
        /// <returns>A Collection of Triangles</returns>
        public static ICollection<Triangle> Triangulate(IEnumerable<Vector2> points)
        {
            var poly = new Polygon();
            poly.Add(new Contour(points.Select(p => p.ToVertex())));
            var mesh = poly.Triangulate();
            return mesh.Triangles;
        }
    }
}
