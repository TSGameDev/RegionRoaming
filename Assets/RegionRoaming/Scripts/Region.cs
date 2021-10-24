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
        private LayerMask layer;
        private LayerMask layerNoncase;

        #endregion

        /// <summary>
        /// Returns a random Vector3 within the region with a Y of 0. If using a navmesh for AI this will do.
        /// </summary>
        /// <returns>Returns a random Vector3 without a Y parameter</returns>
        public Vector3 PickRandomLocation()
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            return new Vector3(randomPos.x, 0f, randomPos.y);
        }

        /// <summary>
        /// Returns a random Vector3 position within the region at terrain height. Requires a Terrain/terrain layer to work.
        /// </summary>
        /// <returns>A random vector3</returns>
        public Vector3 PickRandomRaycastLocation()
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            return TerrainCast(randomPos);
        }

        /// <summary>
        /// Gets a random Vector3 within the region up to the maxFlyingHeight. If the Y isn't greater than minFlyingHeight, the vector3 becomes groundlevel. This Aids walking along the ground.
        /// </summary>
        /// <param name="minFlyingHeight">The min the Y point needs to be for the AI to begin flying</param>
        /// <param name="maxFlyingHeight">The max the Y point can be aka the max flying height of the AI</param>
        /// <returns>A Vector3 at terrain level for walking or between minHeight and maxHeight for flying</returns>
        public Vector3 PickRandomFlightLocation(float minFlyingHeight, float maxFlyingHeight)
        {
            var tri = PickRandomTriangle();
            var randomPos = RandomWithinTriangle(tri);
            Vector3 raycastHit = TerrainCast(randomPos);

            float newY = Random.Range(raycastHit.y, maxFlyingHeight);
            float heightDifference = newY - raycastHit.y;

            if(heightDifference < minFlyingHeight)
            {
                return raycastHit;
            }
            else
            {
                return new Vector3(raycastHit.x, newY, raycastHit.z);
            }
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

        //casts a ray from 1000y down to the terrain
        private Vector3 TerrainCast(Vector2 pos)
        {
            if (Physics.Raycast(new Vector3(pos.x, 1000f, pos.y), transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity, layer))
                return hit.point;
            else if (Physics.Raycast(new Vector3(pos.x, 1000f, pos.y), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerNoncase))
                return hit.point;
            else
                throw new System.Exception("No Terrain/terrain layer or region point not on a terrain object");
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
            layer = 1 << LayerMask.NameToLayer("Terrain");
            layerNoncase = 1 << LayerMask.NameToLayer("terrain");
        }
    }
}


