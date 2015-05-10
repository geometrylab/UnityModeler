using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    public enum EPolygonOp
    {
        None,
        Subtract,
        Split,
        Union
    }

    public class Model : ModelData
	{
        public Polygon FindPolygon(Ray ray)
        {
            return null;
        }

		public void AddPolygon (Polygon polygon, EPolygonOp op = EPolygonOp.None)
		{
			if (op == EPolygonOp.None) 
			{
				polygons_.Add (polygon);
			}
        }

		public Polygon GetPolygon(int idx)
		{ 
			return (Polygon)polygons_[idx];
		}

		public void Clear()
		{
			polygons_.Clear ();
		}
    }
}
