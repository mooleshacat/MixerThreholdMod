using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001DD RID: 477
	[RequireComponent(typeof(Camera))]
	public class RenderCloudCubemap : MonoBehaviour
	{
		// Token: 0x04000B5A RID: 2906
		public const string kDefaultFilenamePrefix = "CloudCubemap";

		// Token: 0x04000B5B RID: 2907
		[Tooltip("Filename of the final output cubemap asset. It will be written to the same directory as the current scene.")]
		public string filenamePrefix = "CloudCubemap";

		// Token: 0x04000B5C RID: 2908
		[Tooltip("Resolution of each face of the cubemap.")]
		public int faceWidth = 1024;

		// Token: 0x04000B5D RID: 2909
		[Tooltip("Format for the exported cubemap. RGBColor (Additive texture), RGBAColor (Color with alpha channel), RGBANormal (Normal lighting data encoded).")]
		public RenderCloudCubemap.CubemapTextureFormat textureFormat = RenderCloudCubemap.CubemapTextureFormat.RGBALit;

		// Token: 0x04000B5E RID: 2910
		public bool exportFaces;

		// Token: 0x020001DE RID: 478
		public enum CubemapTextureFormat
		{
			// Token: 0x04000B60 RID: 2912
			RGBColor,
			// Token: 0x04000B61 RID: 2913
			RGBAColor,
			// Token: 0x04000B62 RID: 2914
			RGBALit
		}
	}
}
