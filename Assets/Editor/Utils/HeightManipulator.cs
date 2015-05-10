using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    class HeightManipulator
    {
        public HeightManipulator( Plane plane, Vector3 pivot )
        {
            floorPlane_ = plane;
            pivot_ = pivot;
        }

        public float UpdateHeight( ModelEngine engine )
        {
			Matrix4x4 w2c_tm = engine.camera.worldToCameraMatrix;
			Vector3 [] vThirdDirs = { w2c_tm.GetRow (0), w2c_tm.GetRow (1) };
			int nThirdIdx = 0;

			float fDotNearestZero = 1.0f;
			for( int i = 0; i < vThirdDirs.Length; ++i )
			{
				float fDot = Vector3.Dot(floorPlane_.normal, vThirdDirs[i]);
				if( Mathf.Abs (fDot) < fDotNearestZero )
				{
					fDotNearestZero = fDot;
					nThirdIdx = i;
				}
			}

			Vector3 v0 = pivot_;
			Vector3 v1 = pivot_ + floorPlane_.normal;
			Vector2 v2 = pivot_ + vThirdDirs [nThirdIdx];

			helperPlane_ = new Plane (v0, v1, v2);

			Ray ray = engine.ViewRay ();
			float t;
			if (helperPlane_.Raycast (ray, out t)) 
			{
				return floorPlane_.GetDistanceToPoint(ray.origin + ray.direction * t);
			}

            return 0;
        }

        private Vector3 pivot_;
        private Plane floorPlane_;
        private Plane helperPlane_;
    }
}