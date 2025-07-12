using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D9 RID: 217
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06000387 RID: 903 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000388 RID: 904
		public abstract bool active { get; }

		// Token: 0x06000389 RID: 905 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void OnEnable()
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void OnDisable()
		{
		}

		// Token: 0x0600038B RID: 907
		public abstract PostProcessingModel GetModel();

		// Token: 0x04000477 RID: 1143
		public PostProcessingContext context;
	}
}
