using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    public class Polygon : PolygonData
    {
		public Polygon() {}
        public Polygon(List<SVertex> vertices, List<SEdge> edges)
        {
            vertices_ = vertices;
            edges_ = edges;
        }

        public Polygon(List<SVertex> vertices)
        {
            vertices_ = vertices;
            for (int i = 0; i < vertices.Count; ++i)
                edges_.Add(new SEdge(i, (i+1)%vertices.Count));
        }
    }
}