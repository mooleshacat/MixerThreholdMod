using System;

namespace VLB
{
	// Token: 0x02000150 RID: 336
	public static class ShaderKeywords
	{
		// Token: 0x04000721 RID: 1825
		public const string AlphaAsBlack = "VLB_ALPHA_AS_BLACK";

		// Token: 0x04000722 RID: 1826
		public const string ColorGradientMatrixLow = "VLB_COLOR_GRADIENT_MATRIX_LOW";

		// Token: 0x04000723 RID: 1827
		public const string ColorGradientMatrixHigh = "VLB_COLOR_GRADIENT_MATRIX_HIGH";

		// Token: 0x04000724 RID: 1828
		public const string Noise3D = "VLB_NOISE_3D";

		// Token: 0x02000151 RID: 337
		public static class SD
		{
			// Token: 0x04000725 RID: 1829
			public const string DepthBlend = "VLB_DEPTH_BLEND";

			// Token: 0x04000726 RID: 1830
			public const string OcclusionClippingPlane = "VLB_OCCLUSION_CLIPPING_PLANE";

			// Token: 0x04000727 RID: 1831
			public const string OcclusionDepthTexture = "VLB_OCCLUSION_DEPTH_TEXTURE";

			// Token: 0x04000728 RID: 1832
			public const string MeshSkewing = "VLB_MESH_SKEWING";

			// Token: 0x04000729 RID: 1833
			public const string ShaderAccuracyHigh = "VLB_SHADER_ACCURACY_HIGH";
		}

		// Token: 0x02000152 RID: 338
		public static class HD
		{
			// Token: 0x06000673 RID: 1651 RVA: 0x0001D166 File Offset: 0x0001B366
			public static string GetRaymarchingQuality(int id)
			{
				return "VLB_RAYMARCHING_QUALITY_" + id.ToString();
			}

			// Token: 0x0400072A RID: 1834
			public const string AttenuationLinear = "VLB_ATTENUATION_LINEAR";

			// Token: 0x0400072B RID: 1835
			public const string AttenuationQuad = "VLB_ATTENUATION_QUAD";

			// Token: 0x0400072C RID: 1836
			public const string Shadow = "VLB_SHADOW";

			// Token: 0x0400072D RID: 1837
			public const string CookieSingleChannel = "VLB_COOKIE_1CHANNEL";

			// Token: 0x0400072E RID: 1838
			public const string CookieRGBA = "VLB_COOKIE_RGBA";

			// Token: 0x0400072F RID: 1839
			public const string RaymarchingStepCount = "VLB_RAYMARCHING_STEP_COUNT";
		}
	}
}
