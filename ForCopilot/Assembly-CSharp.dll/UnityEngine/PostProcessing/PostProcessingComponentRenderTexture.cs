using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DC RID: 220
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000396 RID: 918 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Prepare(Material material)
		{
		}
	}
}
