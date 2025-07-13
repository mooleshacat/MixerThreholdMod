using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000C8 RID: 200
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00013C3E File Offset: 0x00011E3E
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00013C46 File Offset: 0x00011E46
		public GrainModel.Settings settings
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

		// Token: 0x0600035F RID: 863 RVA: 0x00013C4F File Offset: 0x00011E4F
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x0400042A RID: 1066
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x020000C9 RID: 201
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000361 RID: 865 RVA: 0x00013C70 File Offset: 0x00011E70
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x0400042B RID: 1067
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x0400042C RID: 1068
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x0400042D RID: 1069
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x0400042E RID: 1070
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
