using UnityEngine;
using System.Collections.Generic;

namespace FFMProject
{
    class MeshCompiler
    {
		public static void Compile( Model model, Mesh mesh )
		{
			int iCount = model.GetPolygonCount ();
			for( int i = 0; i < iCount; ++i )
			{
				PolygonDecomposer decomposer;
				decomposer.Triangulate(model.GetPolygon(i),mesh);
			}
		}
    }
}