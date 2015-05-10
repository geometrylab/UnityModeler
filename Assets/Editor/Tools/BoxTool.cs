using UnityEngine;
using System.Collections;

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
            }
        }

        public override void OnMouseDrag(ModelEngine engine)
        {
            if (phase_ == E4Phases.DrawBase )
            {   
                float t;
                Ray ray = engine.ViewRay();
                if (plane_.Raycast(ray, out t))
                    base_[1] = ray.origin + ray.direction * t;

                UpdateModel();
            }
        }

        public override void OnMouseMove(ModelEngine engine)
        {
            engine.HitPos(out cursor_, cursor_);

            if (phase_ == E4Phases.RaiseHeight)
            {
                UpdateModel();
            }
        }

        public override void Display(ModelEngine engine)        
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(cursor_, 0.5f);

            if (phase_ == E4Phases.DrawBase)
            {
                Vector3[] vs = { base_[0], 
                                 new Vector3(base_[0].x,base_[0].y,base_[1].z),
                                 base_[1],
                                 new Vector3(base_[1].x,base_[0].y,base_[0].z) };
                Gizmos.color = Color.red;
                Gizmos.DrawLine(vs[0], vs[1]);
                Gizmos.DrawLine(vs[1], vs[2]);
                Gizmos.DrawLine(vs[2], vs[3]);
                Gizmos.DrawLine(vs[3], vs[0]);
            }
        }

        private void UpdateModel()
        {
        }

        E4Phases phase_ = E4Phases.PlaceFirstSpot;
        Vector3[] base_ = new Vector3[2];
        Vector3 cursor_ = new Vector3(0, 0, 0);
        int height_ = 0;
        Plane plane_;
    }
}