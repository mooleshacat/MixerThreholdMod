using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D3 RID: 211
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00013E1E File Offset: 0x0001201E
		// (set) Token: 0x0600036D RID: 877 RVA: 0x00013E26 File Offset: 0x00012026
		public UserLutModel.Settings settings
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

		// Token: 0x0600036E RID: 878 RVA: 0x00013E2F File Offset: 0x0001202F
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x0400044A RID: 1098
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x020000D4 RID: 212
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000370 RID: 880 RVA: 0x00013E50 File Offset: 0x00012050
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x0400044B RID: 1099
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x0400044C RID: 1100
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
