using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegionRoaming
{
    public class Region : MonoBehaviour
    {
        public List<Vector3> Vertices = new List<Vector3>() { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1) };
    }
}
