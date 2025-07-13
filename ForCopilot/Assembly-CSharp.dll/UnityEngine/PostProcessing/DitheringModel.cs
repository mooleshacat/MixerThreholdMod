using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000C1 RID: 193
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00013AEA File Offset: 0x00011CEA
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00013AF2 File Offset: 0x00011CF2
		public DitheringModel.Settings settings
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

		// Token: 0x06000350 RID: 848 RVA: 0x00013AFB File Offset: 0x00011CFB
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x04000418 RID: 1048
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x020000C2 RID: 194
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006E RID: 110
			// (get) Token: 0x06000352 RID: 850 RVA: 0x00013B1C File Offset: 0x00011D1C
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
