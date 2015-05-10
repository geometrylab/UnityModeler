using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    struct SVertex
    {
        public SVertex( Vector3 pos )
        {
            pos_ = pos;
            uv_ = new Vector2(0, 0);
        }

        public SVertex(Vector3 pos, Vector2 uv)
        {
            pos_ = pos;
            uv_ = uv;
        }

        public Vector3 pos_;
        public Vector2 uv_;
    }

    struct SEdge
    {
        public SEdge(int i0, int i1)
        {
            i0_ = i0;
            i1_ = i1;
        }

        public int i0_;
        public int i1_;
    }

    class Polygon
    {
        private List<SVertex> vertices_;
        private List<SEdge> edges_;
        private List<HE_Face> faces_;
        private Plane plane_;

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

        int GetEdgeCount()  { return edges_.Count; }
        int GetVertexCount(){ return vertices_.Count; }
    }
}