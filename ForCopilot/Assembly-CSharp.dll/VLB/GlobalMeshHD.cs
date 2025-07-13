using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000117 RID: 279
	public static class GlobalMeshHD
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x00017888 File Offset: 0x00015A88
		public static Mesh Get()
		{
			if (GlobalMeshHD.ms_Mesh == null)
			{
				GlobalMeshHD.Destroy();
				GlobalMeshHD.ms_Mesh = MeshGenerator.GenerateConeZ_Radii_DoubleCaps(1f, 1f, 1f, Config.Instance.sharedMeshSides, true);
				GlobalMeshHD.ms_Mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			}
			return GlobalMeshHD.ms_Mesh;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000178DF File Offset: 0x00015ADF
		public static void Destroy()
		{
			if (GlobalMeshHD.ms_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(GlobalMeshHD.ms_Mesh);
				GlobalMeshHD.ms_Mesh = null;
			}
		}

		// Token: 0x04000606 RID: 1542
		private static Mesh ms_Mesh;
	}
}
