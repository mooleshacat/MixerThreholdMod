using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000153 RID: 339
	public static class ShaderProperties
	{
		// Token: 0x04000730 RID: 1840
		public static readonly int ConeRadius = Shader.PropertyToID("_ConeRadius");

		// Token: 0x04000731 RID: 1841
		public static readonly int ConeGeomProps = Shader.PropertyToID("_ConeGeomProps");

		// Token: 0x04000732 RID: 1842
		public static readonly int ColorFlat = Shader.PropertyToID("_ColorFlat");

		// Token: 0x04000733 RID: 1843
		public static readonly int DistanceFallOff = Shader.PropertyToID("_DistanceFallOff");

		// Token: 0x04000734 RID: 1844
		public static readonly int NoiseVelocityAndScale = Shader.PropertyToID("_NoiseVelocityAndScale");

		// Token: 0x04000735 RID: 1845
		public static readonly int NoiseParam = Shader.PropertyToID("_NoiseParam");

		// Token: 0x04000736 RID: 1846
		public static readonly int ColorGradientMatrix = Shader.PropertyToID("_ColorGradientMatrix");

		// Token: 0x04000737 RID: 1847
		public static readonly int LocalToWorldMatrix = Shader.PropertyToID("_LocalToWorldMatrix");

		// Token: 0x04000738 RID: 1848
		public static readonly int WorldToLocalMatrix = Shader.PropertyToID("_WorldToLocalMatrix");

		// Token: 0x04000739 RID: 1849
		public static readonly int BlendSrcFactor = Shader.PropertyToID("_BlendSrcFactor");

		// Token: 0x0400073A RID: 1850
		public static readonly int BlendDstFactor = Shader.PropertyToID("_BlendDstFactor");

		// Token: 0x0400073B RID: 1851
		public static readonly int ZTest = Shader.PropertyToID("_ZTest");

		// Token: 0x0400073C RID: 1852
		public static readonly int ParticlesTintColor = Shader.PropertyToID("_TintColor");

		// Token: 0x0400073D RID: 1853
		public static readonly int HDRPExposureWeight = Shader.PropertyToID("_HDRPExposureWeight");

		// Token: 0x0400073E RID: 1854
		public static readonly int GlobalUsesReversedZBuffer = Shader.PropertyToID("_VLB_UsesReversedZBuffer");

		// Token: 0x0400073F RID: 1855
		public static readonly int GlobalNoiseTex3D = Shader.PropertyToID("_VLB_NoiseTex3D");

		// Token: 0x04000740 RID: 1856
		public static readonly int GlobalNoiseCustomTime = Shader.PropertyToID("_VLB_NoiseCustomTime");

		// Token: 0x04000741 RID: 1857
		public static readonly int GlobalDitheringFactor = Shader.PropertyToID("_VLB_DitheringFactor");

		// Token: 0x04000742 RID: 1858
		public static readonly int GlobalDitheringNoiseTex = Shader.PropertyToID("_VLB_DitheringNoiseTex");

		// Token: 0x02000154 RID: 340
		public static class SD
		{
			// Token: 0x04000743 RID: 1859
			public static readonly int FadeOutFactor = Shader.PropertyToID("_FadeOutFactor");

			// Token: 0x04000744 RID: 1860
			public static readonly int ConeSlopeCosSin = Shader.PropertyToID("_ConeSlopeCosSin");

			// Token: 0x04000745 RID: 1861
			public static readonly int AlphaInside = Shader.PropertyToID("_AlphaInside");

			// Token: 0x04000746 RID: 1862
			public static readonly int AlphaOutside = Shader.PropertyToID("_AlphaOutside");

			// Token: 0x04000747 RID: 1863
			public static readonly int AttenuationLerpLinearQuad = Shader.PropertyToID("_AttenuationLerpLinearQuad");

			// Token: 0x04000748 RID: 1864
			public static readonly int DistanceCamClipping = Shader.PropertyToID("_DistanceCamClipping");

			// Token: 0x04000749 RID: 1865
			public static readonly int FresnelPow = Shader.PropertyToID("_FresnelPow");

			// Token: 0x0400074A RID: 1866
			public static readonly int GlareBehind = Shader.PropertyToID("_GlareBehind");

			// Token: 0x0400074B RID: 1867
			public static readonly int GlareFrontal = Shader.PropertyToID("_GlareFrontal");

			// Token: 0x0400074C RID: 1868
			public static readonly int DrawCap = Shader.PropertyToID("_DrawCap");

			// Token: 0x0400074D RID: 1869
			public static readonly int DepthBlendDistance = Shader.PropertyToID("_DepthBlendDistance");

			// Token: 0x0400074E RID: 1870
			public static readonly int CameraParams = Shader.PropertyToID("_CameraParams");

			// Token: 0x0400074F RID: 1871
			public static readonly int DynamicOcclusionClippingPlaneWS = Shader.PropertyToID("_DynamicOcclusionClippingPlaneWS");

			// Token: 0x04000750 RID: 1872
			public static readonly int DynamicOcclusionClippingPlaneProps = Shader.PropertyToID("_DynamicOcclusionClippingPlaneProps");

			// Token: 0x04000751 RID: 1873
			public static readonly int DynamicOcclusionDepthTexture = Shader.PropertyToID("_DynamicOcclusionDepthTexture");

			// Token: 0x04000752 RID: 1874
			public static readonly int DynamicOcclusionDepthProps = Shader.PropertyToID("_DynamicOcclusionDepthProps");

			// Token: 0x04000753 RID: 1875
			public static readonly int LocalForwardDirection = Shader.PropertyToID("_LocalForwardDirection");

			// Token: 0x04000754 RID: 1876
			public static readonly int TiltVector = Shader.PropertyToID("_TiltVector");

			// Token: 0x04000755 RID: 1877
			public static readonly int AdditionalClippingPlaneWS = Shader.PropertyToID("_AdditionalClippingPlaneWS");
		}

		// Token: 0x02000155 RID: 341
		public static class HD
		{
			// Token: 0x04000756 RID: 1878
			public static readonly int Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04000757 RID: 1879
			public static readonly int SideSoftness = Shader.PropertyToID("_SideSoftness");

			// Token: 0x04000758 RID: 1880
			public static readonly int CameraForwardOS = Shader.PropertyToID("_CameraForwardOS");

			// Token: 0x04000759 RID: 1881
			public static readonly int CameraForwardWS = Shader.PropertyToID("_CameraForwardWS");

			// Token: 0x0400075A RID: 1882
			public static readonly int TransformScale = Shader.PropertyToID("_TransformScale");

			// Token: 0x0400075B RID: 1883
			public static readonly int ShadowDepthTexture = Shader.PropertyToID("_ShadowDepthTexture");

			// Token: 0x0400075C RID: 1884
			public static readonly int ShadowProps = Shader.PropertyToID("_ShadowProps");

			// Token: 0x0400075D RID: 1885
			public static readonly int Jittering = Shader.PropertyToID("_Jittering");

			// Token: 0x0400075E RID: 1886
			public static readonly int CookieTexture = Shader.PropertyToID("_CookieTexture");

			// Token: 0x0400075F RID: 1887
			public static readonly int CookieProperties = Shader.PropertyToID("_CookieProperties");

			// Token: 0x04000760 RID: 1888
			public static readonly int CookiePosAndScale = Shader.PropertyToID("_CookiePosAndScale");

			// Token: 0x04000761 RID: 1889
			public static readonly int GlobalCameraBlendingDistance = Shader.PropertyToID("_VLB_CameraBlendingDistance");

			// Token: 0x04000762 RID: 1890
			public static readonly int GlobalJitteringNoiseTex = Shader.PropertyToID("_VLB_JitteringNoiseTex");
		}
	}
}
