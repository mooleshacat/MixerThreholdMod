using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000C6 RID: 198
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00013BEF File Offset: 0x00011DEF
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00013BF7 File Offset: 0x00011DF7
		public FogModel.Settings settings
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

		// Token: 0x0600035A RID: 858 RVA: 0x00013C00 File Offset: 0x00011E00
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04000428 RID: 1064
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x020000C7 RID: 199
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000072 RID: 114
			// (get) Token: 0x0600035C RID: 860 RVA: 0x00013C20 File Offset: 0x00011E20
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04000429 RID: 1065
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
