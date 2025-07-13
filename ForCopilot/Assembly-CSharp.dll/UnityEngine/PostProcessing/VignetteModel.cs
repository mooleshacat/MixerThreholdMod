using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D5 RID: 213
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00013E7A File Offset: 0x0001207A
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00013E82 File Offset: 0x00012082
		public VignetteModel.Settings settings
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

		// Token: 0x06000373 RID: 883 RVA: 0x00013E8B File Offset: 0x0001208B
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x0400044D RID: 1101
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x020000D6 RID: 214
		public enum Mode
		{
			// Token: 0x0400044F RID: 1103
			Classic,
			// Token: 0x04000450 RID: 1104
			Masked
		}

		// Token: 0x020000D7 RID: 215
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000375 RID: 885 RVA: 0x00013EAC File Offset: 0x000120AC
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x04000451 RID: 1105
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x04000452 RID: 1106
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x04000453 RID: 1107
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x04000454 RID: 1108
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x04000455 RID: 1109
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x04000456 RID: 1110
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04000457 RID: 1111
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04000458 RID: 1112
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04000459 RID: 1113
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
