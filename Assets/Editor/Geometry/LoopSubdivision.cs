using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    class LoopSubdivision
    {
        public static HalfEdgeMesh Subdivide(HalfEdgeMesh mesh)
        {
            HalfEdgeMesh nextLevelMesh = new HalfEdgeMesh();

            foreach( HE_Face face in mesh.faces )
            {
                Vector3[] next_edge_positions = new Vector3[3];
                Vector3[] next_vert_positions = new Vector3[3];

                {
                    int count = 0;
                    HE_Edge edge = face.edge_;
                    do
                    {
                        if (edge.pair_ != null)
                        {
                            next_edge_positions[count] =
                                edge.vert_.pos_ * 0.375f +
                                edge.next_.vert_.pos_ * 0.375f +
                                edge.prev_.vert_.pos_ * 0.125f +
                                edge.pair_.prev_.vert_.pos_ * 0.125f;
                        }
                        else
                        {
                            next_edge_positions[count] =
                                edge.vert_.pos_ * 0.5f +
                                edge.next_.vert_.pos_ * 0.5f;
                        }

                        {
                            List<HE_Vertex> neighbors;
                            edge.vert_.GetNeighboringVertices(out neighbors);
                            int n = neighbors.Count;
                            float beta = n > 3 ? 3.0f / (8.0f * n) : 3.0f / 16.0f;
                            next_vert_positions[count] = (1 - n * beta) * edge.vert_.pos_;
                            for (int k = 0; k < n; ++k)
                                next_vert_positions[count] += neighbors[k].pos_ * beta;
                        }

                        ++count;
                        edge = edge.next_;
                    } while (!edge.Equals(face.edge_) && count < 3);
                }

                {
                    for( int k = 0; k < 3; ++k )
                    {
                        int prev_k = (k + 2) % 3;

                        Vector3[] next_face = {
                            next_vert_positions[k],
                            next_edge_positions[k],
                            next_edge_positions[prev_k] };

                        nextLevelMesh.AddFace(next_face);
                    }

                    Vector3[] next_face_second = { next_edge_positions[0], next_edge_positions[1], next_edge_positions[2] };
                    nextLevelMesh.AddFace(next_face_second);
                }
            }

            nextLevelMesh.SolveAllPairs();
            return nextLevelMesh;
        }
    }
}
