using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FFMProject
{
    public delegate void DrawDelegate();
	public delegate ModelData ModelCreator();

	public struct SVertex
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
	
	public struct SEdge
	{
		public SEdge(int i0, int i1)
		{
			i0_ = i0;
			i1_ = i1;
		}
		
		public int i0_;
		public int i1_;
	}

	public class PolygonData
	{
		protected List<SVertex> vertices_ = new List<SVertex> ();
		protected List<SEdge> edges_ = new List<SEdge> ();
		protected Plane plane_ = new Plane();

		public int GetEdgeCount()  { return edges_.Count; }
		public int GetVertexCount(){ return vertices_.Count; }
	}

	public class ModelData
	{
		protected List<PolygonData> polygons_ = new List<PolygonData>();

		public int GetPolygonCount() { return polygons_.Count; }
		public PolygonData GetPolygonData( int idx ) { return polygons_[idx]; }
	}
}

public class EditableObject : MonoBehaviour
{
    public event FFMProject.DrawDelegate DrawEventHandlers;
	public FFMProject.ModelCreator ModelCreator;
    public MeshFilter meshfilter { get 
		{   
			if( meshfilter_ == null )
			{
				meshfilter_ = GetComponent<MeshFilter>();
				if (meshfilter_ == null)
					meshfilter_ = gameObject.AddComponent<MeshFilter>();
			}
			return meshfilter_; 
		} 
	}
	public FFMProject.ModelData model { 
		get
		{ 
			if( model_ == null )
				model_ = ModelCreator();
			return model_; 
		} 
	}

	void Start()
    {
	}
	
    void OnDrawGizmosSelected()
    {
        if( DrawEventHandlers != null )
            DrawEventHandlers();
    }

    private MeshFilter meshfilter_ = null;
	private FFMProject.ModelData model_ = null;
}
