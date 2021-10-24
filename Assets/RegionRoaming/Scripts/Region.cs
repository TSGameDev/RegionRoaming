using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Topology;
using RegionRoaming.Mathematics;
using System.Linq;

namespace RegionRoaming
{
    public class Region : MonoBehaviour
    {
        #region Variables

        public List<Vector3> Vertices = new List<Vector3>()
        { new Vector3(5, 0, 0), new Vector3(-5, 0, 0), new Vector3(0, 0, 5) };

        private List<Triangle> triangles;
        private double areaSum;

        #endregion

        /// <summary>
        /// Returns a random Vector3 position in the sense of a 2D plane from within the region. (The Y parameter is always 0, using PickRandomRaycastLocation if casting to a ground is required).
        /// </summary>
        /// <returns>Returns a random Vector3 without a Y parameter</returns>
        public Vector3 PickRandomLocation()
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            return new Vector3(randomPos.x, 0f, randomPos.y);
        }

        /// <summary>
        /// Returns a random Vector3 position within the region using raycast to get the correct height of a terrain.
        /// </summary>
        /// <param name="terrainLayerInt">The int for the layer the terrain is on</param>
        /// <param name="maxTerrainHeight">The max heigh of the terrain within the region</param>
        /// <returns>A random vector3</returns>
        public Vector3 PickRandomRaycastLocation(int terrainLayerInt, float maxTerrainHeight)
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            LayerMask layer = 1 << terrainLayerInt;
            if(Physics.Raycast(new Vector3(randomPos.x, maxTerrainHeight, randomPos.y), transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity, layer))
            {
                return hit.point;
            }

            throw new System.Exception("Ray didn't hit");
        }

        /// <summary>
        /// Returns a random Vector3 position within the region and maxheight for flying AI. The min height will be the terrain height
        /// </summary>
        /// <returns></returns>
        public Vector3 PickRandomFlightLocation(LayerMask terrainLayer, float maxHeight)
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            Vector3 randomLocation = new Vector3(randomPos.x, 0f, randomPos.y);
            RaycastHit hit;
            Physics.Raycast(randomLocation, Vector3.down, out hit, 100f, terrainLayer);

            if (hit.point == null)
                Physics.Raycast(randomLocation, Vector3.up, out hit, 100f, terrainLayer);

            if (hit.point != null)
            {
                float newY = Random.Range(hit.point.y, maxHeight);
                return new Vector3(hit.point.x, newY, hit.point.z);
            }
            else
                throw new System.Exception("Hit.point is null or maxheight is less than terrain height");
        }

        //Picks a random triangle within the region using area-bias
        private Triangle PickRandomTriangle()
        {
            RegionInistalisation();
            var range = Random.Range(0f, (float)areaSum);
            for (int i = 0; i < triangles.Count; i++)
            {
                if (range < triangles[i].TriArea())
                    return triangles[i];
                range -= triangles[i].TriArea();
            }
            throw new System.Exception("Should not get here.");
        }

        //picks a random point within the passed in triangle
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

        /// <summary>
        /// A function that performs tasks to inistalisate the region for randompoints to work. NO NEED TO CALL IN USER SCRIPTS
        /// </summary>
        public void RegionInistalisation()
        {
            //makes triangles a new list, called the triangulate function on the vertices and stores every triangle.
            triangles = new List<Triangle>();
            triangles.AddRange(RegionMathematics.Triangulate(Vertices));
            //makes area sum equal to 0. Calculates each triangles area and adds that to areasum
            areaSum = 0f;
            triangles.ForEach(x => areaSum += x.TriArea());
        }
    }
}


