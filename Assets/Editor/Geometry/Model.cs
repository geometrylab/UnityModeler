using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    enum EAddOperation
    {
        eAddOp_Add,
        eAddOp_Subtract,
        eAddOp_Split,
        eAddOp_Union
    }

    class Model
    {
        private List<Polygon> polygons = new List<Polygon>();

        Polygon FindPolygon(Ray ray)
        {
            return null;
        }

        void AddPolygon(Polygon polygon, EAddOperation op)
        {
            polygons.Add(polygon);
        }
    }
}
