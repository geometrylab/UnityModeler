using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    enum E4Phases
    {
        PlaceFirstSpot,
        DrawBase,
        RaiseHeight
    }

    class BoxTool : BaseTool
    {
        public override void Start()
        {
            base.Start();
        }

        public override void End()
        {
            base.End();
        }

        public override void OnLMBDown(ModelEngine engine)
        {
            if (phase_ == E4Phases.PlaceFirstSpot)
            {
                RaycastHit hit;
                if (engine.Raycast(out hit))
                {
                    phase_ = E4Phases.DrawBase;
                    base_[0] = base_[1] = hit.point;
                    plane_ = new Plane(hit.point, hit.point+new Vector3(0,0,1), hit.point+new Vector3(1,0,0));
                }
            }
            else if (phase_ == E4Phases.RaiseHeight)
            {
                phase_ = E4Phases.PlaceFirstSpot;
            }
        }

        public override void OnLMBUp(ModelEngine engine)
        {
            if( phase_ == E4Phases.DrawBase )
            {
                phase_ = E4Phases.RaiseHeight;
				heightManipulator_ = new HeightManipulator(plane_,base_[1]);
				height_ = 0;
            }
        }

        public override void OnMouseDrag(ModelEngine engine)
        {
            if (phase_ == E4Phases.DrawBase )
            {   
                float t;
                Ray ray = engine.ViewRay();
                if (plane_.Raycast(ray, out t))
                    base_[1] = cursor_ = ray.origin + ray.direction * t;

                UpdateModel();
            }
        }

        public override void OnMouseMove(ModelEngine engine)
        {
			if (phase_ == E4Phases.PlaceFirstSpot) 
			{
				engine.HitPos(out cursor_, cursor_);
			}
			else if (phase_ == E4Phases.RaiseHeight)
            {
				height_ = Mathf.Clamp( heightManipulator_.UpdateHeight(engine), 0, 100.0f );
                UpdateModel();
            }
        }

        public override void Display(ModelEngine engine)        
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(cursor_, 0.25f);

            if (phase_ == E4Phases.DrawBase)
			{
				Vector3[] vs = { base_ [0], 
                                 new Vector3 (base_ [0].x, base_ [0].y, base_ [1].z),
                                 base_ [1],
                                 new Vector3 (base_ [1].x, base_ [0].y, base_ [0].z) };
				Gizmos.color = Color.red;
				Gizmos.DrawLine (vs[0], vs[1]);
				Gizmos.DrawLine (vs[1], vs[2]);
				Gizmos.DrawLine (vs[2], vs[3]);
				Gizmos.DrawLine (vs[3], vs[0]);
			} 
			else if (phase_ == E4Phases.RaiseHeight) 
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine (base_[1], base_[1]+plane_.normal*height_);
			}
        }

        private void UpdateModel()
        {
			Model model = ModelEngine.the.model;

			Vector3 v0 = new Vector3( Mathf.Min(base_[0].x,base_[1].x), base_[0].y, Mathf.Min(base_[0].z,base_[1].z) );
			Vector3 v1 = new Vector3( Mathf.Max(base_[0].x,base_[1].x), base_[0].y + height_, Mathf.Max(base_[0].z,base_[1].z) );

			List<SVertex> vlist = new List<SVertex>();
			vlist.Add (new SVertex (v0));
			vlist.Add (new SVertex (new Vector3 (v1.x, v0.y, v0.z)));
			vlist.Add (new SVertex (new Vector3 (v1.x, v1.y, v0.z)));
			vlist.Add (new SVertex (new Vector3 (v0.x, v1.y, v0.z)));
			model.AddPolygon(new Polygon(vlist));

			ModelEngine.the.UpdateAll();
        }

        E4Phases phase_ = E4Phases.PlaceFirstSpot;
        Vector3[] base_ = new Vector3[2];
        Vector3 cursor_ = new Vector3(0, 0, 0);
        float height_ = 0;
        Plane plane_;
		HeightManipulator heightManipulator_ = null;
    }
}