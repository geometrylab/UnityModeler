using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    class HeightManipulator
    {
        HeightManipulator( Plane plane, Vector3 pivot )
        {
            floorPlane_ = plane;
            pivot_ = pivot;
        }

        private float UpdateHeight( ModelEngine engine )
        {
            return 0;
        }

        private Vector3 pivot_;
        private Plane floorPlane_;
        private Plane helperPlane_;
    }
}