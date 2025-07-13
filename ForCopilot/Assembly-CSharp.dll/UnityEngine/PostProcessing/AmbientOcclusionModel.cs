using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200009D RID: 157
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00012EF4 File Offset: 0x000110F4
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00012EFC File Offset: 0x000110FC
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x06000315 RID: 789 RVA: 0x00012F05 File Offset: 0x00011105
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x0400038C RID: 908
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x0200009E RID: 158
		public enum SampleCount
		{
			// Token: 0x0400038E RID: 910
			Lowest = 3,
			// Token: 0x0400038F RID: 911
			Low = 6,
			// Token: 0x04000390 RID: 912
			Medium = 10,
			// Token: 0x04000391 RID: 913
			High = 16
		}

		// Token: 0x0200009F RID: 159
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x06000317 RID: 791 RVA: 0x00012F28 File Offset: 0x00011128
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x04000392 RID: 914
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x04000393 RID: 915
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04000394 RID: 916
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04000395 RID: 917
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x04000396 RID: 918
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x04000397 RID: 919
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x04000398 RID: 920
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
