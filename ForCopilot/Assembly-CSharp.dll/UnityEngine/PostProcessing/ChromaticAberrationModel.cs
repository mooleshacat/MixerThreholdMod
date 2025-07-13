using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000B1 RID: 177
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000134E6 File Offset: 0x000116E6
		// (set) Token: 0x06000334 RID: 820 RVA: 0x000134EE File Offset: 0x000116EE
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x06000335 RID: 821 RVA: 0x000134F7 File Offset: 0x000116F7
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x040003D4 RID: 980
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x020000B2 RID: 178
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000337 RID: 823 RVA: 0x00013518 File Offset: 0x00011718
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x040003D5 RID: 981
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x040003D6 RID: 982
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
