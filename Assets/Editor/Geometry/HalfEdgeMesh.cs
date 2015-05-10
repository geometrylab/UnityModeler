using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    class HE_Vertex
    {
        public HE_Edge edge_ = null;
        public Vector3 pos_ = new Vector3();

        public void GetNeighboringVertices(out List<HE_Vertex> outVertices)
        {
            outVertices = new List<HE_Vertex>();
            HE_Edge edge = edge_;
            bool bCircular = true;

            do
            {
                outVertices.Add(edge.next_.vert_);
                if (edge.pair_ == null)
                {
                    bCircular = false;
                    break;
                }
                edge = edge.pair_.next_;
            } while (edge != edge_);

            if (bCircular)
                return;

            edge = edge_.prev_;
            do
            {
                outVertices.Add(edge.vert_);
                if (edge.pair_ == null)
                    break;
                edge = edge.pair_.prev_;
            } while (edge != edge_);

            return;
        }
    }

    class HE_Edge
    {
        public HE_Vertex vert_ = null;
        public HE_Edge next_ = null;
        public HE_Edge prev_ = null;
        public HE_Edge pair_ = null;
        public HE_Face face_ = null;
    }

    class HE_Face
    {
        public HE_Edge edge_ = null;

        public int FillVertices(int offset, Vector3[] outVertices)
        {
            int idx = 0;
            HE_Edge edge = edge_;

            do
            {
                outVertices[offset+(idx++)] = edge.vert_.pos_;
                edge = edge.next_;
            } while (edge != edge_);

            return offset + idx;
        }

        public int GetVertexCount()
        {
            int count = 0;
            HE_Edge edge = edge_;
            do
            {
                edge = edge.next_;
                ++count;
            }while (edge != edge_);
            return count;
        }

        public HE_Edge FindEdge( Vector3 v0, Vector3 v1 )
        {
            HE_Edge edge = edge_;
            do
            {
                if (v0 == edge.vert_.pos_ && v1 == edge.next_.vert_.pos_)
                    return edge;
                edge = edge.next_;
            } while (edge != edge_);
            return null;
        }
    }

    class HalfEdgeMesh
    {
        public List<HE_Face> faces = new List<HE_Face>();

        public void AddFace(Vector3[] positions)
        {
            int length = positions.Length;

            HE_Vertex[] vertices = new HE_Vertex[length];
            HE_Edge[] edges = new HE_Edge[length];
            HE_Face face = new HE_Face();

            for (int i = 0; i < length; ++i)
            {
                vertices[i] = new HE_Vertex();
                edges[i] = new HE_Edge();
            }

            for( int i = 0; i < length; ++i )
            {
                vertices[i].pos_ = positions[i];
                vertices[i].edge_ = edges[i];
             
                edges[i].vert_ = vertices[i];
                edges[i].next_ = edges[(i+1)%length];
                edges[i].prev_ = edges[(i+length-1)%length];
                edges[i].face_ = face;               
            }

            face.edge_ = edges[0];
            faces.Add(face);
        }

        void SolvePair( HE_Face face )
        {
            HE_Edge edge = face.edge_;
            do
            {
                foreach (HE_Face f in faces)
                {
                    if (f == face)
                        continue;

                    HE_Edge pair = f.FindEdge(edge.next_.vert_.pos_,edge.vert_.pos_);
                    if (pair != null)
                    {
                        edge.pair_ = pair;
                        pair.pair_ = edge;
                        break;
                    }
                }
                edge = edge.next_;
            } while (edge != face.edge_);
        }

        public void SolveAllPairs()
        {
            foreach (HE_Face face in faces)
                SolvePair(face);
        }

        public void MakeMesh( out Mesh mesh )
        {
            mesh = new Mesh();

            int vertexCount = 0;
            int triangleCount = 0;

            foreach (HE_Face face in faces)
            {
                int vertexCountInFace = face.GetVertexCount();
                vertexCount += vertexCountInFace;
                triangleCount += vertexCountInFace-2;
            }

            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            int[] triangles = new int[triangleCount*3];

            int vertexOffset = 0;
            int triangleOffset = 0;

            foreach (HE_Face face in faces)
            {
                int nextVertexOffset = face.FillVertices(vertexOffset, vertices);

                for (int i = vertexOffset; i < nextVertexOffset-2; ++i)
                {
                    triangles[triangleOffset++] = vertexOffset;
                    triangles[triangleOffset++] = i+1;
                    triangles[triangleOffset++] = i+2;
                }

                Vector3 v0 = (vertices[vertexOffset] - vertices[vertexOffset+1]);
                Vector3 v1 = (vertices[vertexOffset+2] - vertices[vertexOffset+1]);
                v0.Normalize();
                v1.Normalize();
                Vector3 normal = Vector3.Cross(v1,v0);
                normal.Normalize();

                for (int i = vertexOffset; i < nextVertexOffset; ++i)
                {
                    normals[i] = normal;
                    uvs[i].x = 0;
                    uvs[i].y = 0;
                }

                vertexOffset = nextVertexOffset;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.normals = normals;
        }
    }
}
