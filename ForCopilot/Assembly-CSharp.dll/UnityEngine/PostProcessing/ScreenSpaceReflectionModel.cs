using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000CC RID: 204
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00013D1B File Offset: 0x00011F1B
		// (set) Token: 0x06000368 RID: 872 RVA: 0x00013D23 File Offset: 0x00011F23
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00013D2C File Offset: 0x00011F2C
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x04000433 RID: 1075
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x020000CD RID: 205
		public enum SSRResolution
		{
			// Token: 0x04000435 RID: 1077
			High,
			// Token: 0x04000436 RID: 1078
			Low = 2
		}

		// Token: 0x020000CE RID: 206
		public enum SSRReflectionBlendType
		{
			// Token: 0x04000438 RID: 1080
			PhysicallyBased,
			// Token: 0x04000439 RID: 1081
			Additive
		}

		// Token: 0x020000CF RID: 207
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x0400043A RID: 1082
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x0400043B RID: 1083
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x0400043C RID: 1084
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x0400043D RID: 1085
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x020000D0 RID: 208
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x0400043E RID: 1086
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x0400043F RID: 1087
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x04000440 RID: 1088
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x04000441 RID: 1089
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x04000442 RID: 1090
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x04000443 RID: 1091
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x04000444 RID: 1092
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x04000445 RID: 1093
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x020000D1 RID: 209
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04000446 RID: 1094
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x020000D2 RID: 210
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000078 RID: 120
			// (get) Token: 0x0600036B RID: 875 RVA: 0x00013D4C File Offset: 0x00011F4C
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04000447 RID: 1095
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04000448 RID: 1096
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04000449 RID: 1097
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
