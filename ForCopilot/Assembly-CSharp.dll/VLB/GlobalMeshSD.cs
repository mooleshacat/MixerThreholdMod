using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000149 RID: 329
	public static class GlobalMeshSD
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x0001BFDC File Offset: 0x0001A1DC
		public static Mesh Get()
		{
			bool sd_requiresDoubleSidedMesh = Config.Instance.SD_requiresDoubleSidedMesh;
			if (GlobalMeshSD.ms_Mesh == null || GlobalMeshSD.ms_DoubleSided != sd_requiresDoubleSidedMesh)
			{
				GlobalMeshSD.Destroy();
				GlobalMeshSD.ms_Mesh = MeshGenerator.GenerateConeZ_Radii(1f, 1f, 1f, Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, true, sd_requiresDoubleSidedMesh);
				GlobalMeshSD.ms_Mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				GlobalMeshSD.ms_DoubleSided = sd_requiresDoubleSidedMesh;
			}
			return GlobalMeshSD.ms_Mesh;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001C057 File Offset: 0x0001A257
		public static void Destroy()
		{
			if (GlobalMeshSD.ms_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(GlobalMeshSD.ms_Mesh);
				GlobalMeshSD.ms_Mesh = null;
			}
		}

		// Token: 0x040006E0 RID: 1760
		private static Mesh ms_Mesh;

		// Token: 0x040006E1 RID: 1761
		private static bool ms_DoubleSided;
	}
}
