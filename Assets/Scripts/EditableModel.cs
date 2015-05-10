using UnityEngine;
using System.Collections;

namespace FFMProject
{
    public delegate void DrawDelegate();
}

public class EditableModel : MonoBehaviour
{
    public event FFMProject.DrawDelegate DrawEventHandlers;
    public MeshFilter mesh { get { return mesh_; } }

	void Start()
    {
        mesh_ = GetComponent<MeshFilter>();
        if (mesh_ == null)
            mesh_ = gameObject.AddComponent<MeshFilter>();
	}
	
	void Update()
    {
	}

    void OnDrawGizmosSelected()
    {
        if( DrawEventHandlers != null )
            DrawEventHandlers();
    }

    private MeshFilter mesh_;
}
