		using UnityEngine;
using System.Collections;

namespace FFMProject
{
    class PlaceSubdividedMeshTool : BaseTool
    {
        public override void Start()
        {
            base.Start();
            Debug.Log("PlaceSubdivisionMeshTool.Start()");
        }

        public override void End()
        {
            Debug.Log("PlaceSubdivisionMeshTool.End()");
        }

        public override void OnLMBDown(ModelEngine engine)
        {
            RaycastHit hit;
            if (engine.Raycast(out hit))
            {
                PlaceSubdividedObj(hit.point, hit.normal, engine.material);
            }
        }

        public override void OnLMBUp(ModelEngine engine)
        {
        }

        public override void OnMouseDrag(ModelEngine engine)
        {
        }

        public override void OnMouseMove(ModelEngine engine)
        {
        }

        public override void Display(ModelEngine engine)
        {
        }

        private void PlaceSubdividedObj(Vector3 pos, Vector3 normal, Material material)
        {
            HalfEdgeMesh halfedge_mesh = new HalfEdgeMesh();
            const float size = 2.0f;

            var vertices = new Vector3[3];

            vertices[0] = new Vector3(0, 0, 0) * size;
            vertices[1] = new Vector3(1, 1, 0) * size;
            vertices[2] = new Vector3(1, 0, 0) * size;
            halfedge_mesh.AddFace(vertices);
            vertices[0] = new Vector3(0, 0, 0) * size;
            vertices[1] = new Vector3(0, 1, 0) * size;
            vertices[2] = new Vector3(1, 1, 0) * size;
            halfedge_mesh.AddFace(vertices);

            vertices[0] = new Vector3(1, 1, 0) * size;
            vertices[1] = new Vector3(1, 0, 1) * size;
            vertices[2] = new Vector3(1, 0, 0) * size;
            halfedge_mesh.AddFace(vertices);
            vertices[0] = new Vector3(1, 1, 0) * size;
            vertices[1] = new Vector3(1, 1, 1) * size;
            vertices[2] = new Vector3(1, 0, 1) * size;
            halfedge_mesh.AddFace(vertices);

            vertices[0] = new Vector3(1, 1, 1) * size;
            vertices[1] = new Vector3(0, 1, 1) * size;
            vertices[2] = new Vector3(1, 0, 1) * size;
            halfedge_mesh.AddFace(vertices);
            vertices[0] = new Vector3(1, 0, 1) * size;
            vertices[1] = new Vector3(0, 1, 1) * size;
            vertices[2] = new Vector3(0, 0, 1) * size;
            halfedge_mesh.AddFace(vertices);

            vertices[0] = new Vector3(0, 1, 1) * size;
            vertices[1] = new Vector3(0, 0, 0) * size;
            vertices[2] = new Vector3(0, 0, 1) * size;
            halfedge_mesh.AddFace(vertices);
            vertices[0] = new Vector3(0, 1, 1) * size;
            vertices[1] = new Vector3(0, 1, 0) * size;
            vertices[2] = new Vector3(0, 0, 0) * size;
            halfedge_mesh.AddFace(vertices);

            vertices[0] = new Vector3(0, 1, 0) * size;
            vertices[1] = new Vector3(0, 1, 1) * size;
            vertices[2] = new Vector3(1, 1, 1) * size;
            halfedge_mesh.AddFace(vertices);
            vertices[0] = new Vector3(0, 1, 0) * size;
            vertices[1] = new Vector3(1, 1, 1) * size;
            vertices[2] = new Vector3(1, 1, 0) * size;
            halfedge_mesh.AddFace(vertices);

            vertices[0] = new Vector3(0, 0, 0) * size;
            vertices[1] = new Vector3(1, 0, 1) * size;
            vertices[2] = new Vector3(0, 0, 1) * size;
            halfedge_mesh.AddFace(vertices);
            vertices[0] = new Vector3(0, 0, 0) * size;
            vertices[1] = new Vector3(1, 0, 0) * size;
            vertices[2] = new Vector3(1, 0, 1) * size;
            halfedge_mesh.AddFace(vertices);

            halfedge_mesh.SolveAllPairs();

            GameObject subdividedMesh = new GameObject("subdividedMesh", typeof(MeshFilter), typeof(MeshRenderer));
            subdividedMesh.transform.position = pos + normal * 0.02f;

            int count = (int)(Random.value * 3.0f) + 1;
            for (int i = 0; i < count; ++i)
                halfedge_mesh = LoopSubdivision.Subdivide(halfedge_mesh);

            Mesh mesh;
            halfedge_mesh.MakeMesh(out mesh);

            MeshFilter mesh_filter = subdividedMesh.GetComponent<MeshFilter>();
            mesh_filter.mesh = mesh;

            MeshRenderer mesh_renderer = subdividedMesh.GetComponent<MeshRenderer>();
            mesh_renderer.material = material;
        }
    }
}
