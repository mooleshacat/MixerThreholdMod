using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000A8 RID: 168
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000132C2 File Offset: 0x000114C2
		// (set) Token: 0x06000322 RID: 802 RVA: 0x000132CA File Offset: 0x000114CA
		public BloomModel.Settings settings
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

		// Token: 0x06000323 RID: 803 RVA: 0x000132D3 File Offset: 0x000114D3
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x040003B4 RID: 948
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x020000A9 RID: 169
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000326 RID: 806 RVA: 0x00013301 File Offset: 0x00011501
			// (set) Token: 0x06000325 RID: 805 RVA: 0x000132F3 File Offset: 0x000114F3
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000327 RID: 807 RVA: 0x00013310 File Offset: 0x00011510
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x040003B5 RID: 949
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x040003B6 RID: 950
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x040003B7 RID: 951
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x040003B8 RID: 952
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x040003B9 RID: 953
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x020000AA RID: 170
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x17000057 RID: 87
			// (get) Token: 0x06000328 RID: 808 RVA: 0x00013360 File Offset: 0x00011560
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x040003BA RID: 954
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x040003BB RID: 955
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x020000AB RID: 171
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000058 RID: 88
			// (get) Token: 0x06000329 RID: 809 RVA: 0x0001338C File Offset: 0x0001158C
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x040003BC RID: 956
			public BloomModel.BloomSettings bloom;

			// Token: 0x040003BD RID: 957
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
