using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000743 RID: 1859
	[Serializable]
	public class GraphicsSettings
	{
		// Token: 0x04002355 RID: 9045
		public GraphicsSettings.EGraphicsQuality GraphicsQuality;

		// Token: 0x04002356 RID: 9046
		public GraphicsSettings.EAntiAliasingMode AntiAliasingMode;

		// Token: 0x04002357 RID: 9047
		public float FOV;

		// Token: 0x04002358 RID: 9048
		public bool SSAO;

		// Token: 0x04002359 RID: 9049
		public bool GodRays;

		// Token: 0x02000744 RID: 1860
		public enum EAntiAliasingMode
		{
			// Token: 0x0400235B RID: 9051
			Off,
			// Token: 0x0400235C RID: 9052
			FXAA,
			// Token: 0x0400235D RID: 9053
			SMAA
		}

		// Token: 0x02000745 RID: 1861
		public enum EGraphicsQuality
		{
			// Token: 0x0400235F RID: 9055
			Low,
			// Token: 0x04002360 RID: 9056
			Medium,
			// Token: 0x04002361 RID: 9057
			High,
			// Token: 0x04002362 RID: 9058
			Ultra
		}
	}
}
