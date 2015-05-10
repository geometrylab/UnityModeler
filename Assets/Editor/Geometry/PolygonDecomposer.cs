using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    class PolygonDecomposer
    {
        public void Triangulate(Polygon polygon, out List<Polygon> triangles)
        {
            triangles = new List<Polygon>();
        }

        public void Triangulate(Polygon polygon, out Mesh mesh)
        {
            mesh = new Mesh();
        }

        public void Decompose2Convexes(Polygon polygon)
        {
        }
    }
}