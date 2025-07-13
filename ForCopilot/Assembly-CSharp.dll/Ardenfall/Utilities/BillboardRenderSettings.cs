using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall.Utilities
{
	// Token: 0x02000236 RID: 566
	[CreateAssetMenu(menuName = "Ardenfall/Foliage/Billboard Render Settings")]
	public class BillboardRenderSettings : ScriptableObject
	{
		// Token: 0x04000D52 RID: 3410
		public List<BillboardRenderSettings.BillboardTexture> billboardTextures;

		// Token: 0x04000D53 RID: 3411
		public Shader billboardShader;

		// Token: 0x02000237 RID: 567
		[Serializable]
		public class BillboardTexture
		{
			// Token: 0x06000C15 RID: 3093 RVA: 0x00037C44 File Offset: 0x00035E44
			public TextureFormat GetFormat()
			{
				Vector4 vector = default(Vector4);
				foreach (BillboardRenderSettings.BakePass bakePass in this.bakePasses)
				{
					if (bakePass.r)
					{
						vector.x += 1f;
					}
					if (bakePass.g)
					{
						vector.y += 1f;
					}
					if (bakePass.b)
					{
						vector.z += 1f;
					}
					if (bakePass.a)
					{
						vector.w += 1f;
					}
				}
				if (vector.x > 1f || vector.y > 1f || vector.z > 1f || vector.w > 1f)
				{
					Debug.LogError("Multiple bake passes in the same texture channel detected");
				}
				if (vector.w >= 1f)
				{
					return TextureFormat.RGBA32;
				}
				if (vector.z >= 1f)
				{
					return TextureFormat.RGB24;
				}
				if (vector.y >= 1f)
				{
					return TextureFormat.RG16;
				}
				return TextureFormat.R8;
			}

			// Token: 0x04000D54 RID: 3412
			public string textureId = "_MainTex";

			// Token: 0x04000D55 RID: 3413
			public bool powerOfTwo = true;

			// Token: 0x04000D56 RID: 3414
			public bool alphaIsTransparency = true;

			// Token: 0x04000D57 RID: 3415
			public List<BillboardRenderSettings.BakePass> bakePasses;
		}

		// Token: 0x02000238 RID: 568
		[Serializable]
		public class BakePass
		{
			// Token: 0x04000D58 RID: 3416
			public Shader customShader;

			// Token: 0x04000D59 RID: 3417
			public MaterialOverrides materialOverrides;

			// Token: 0x04000D5A RID: 3418
			public bool r = true;

			// Token: 0x04000D5B RID: 3419
			public bool g = true;

			// Token: 0x04000D5C RID: 3420
			public bool b = true;

			// Token: 0x04000D5D RID: 3421
			public bool a = true;
		}
	}
}
