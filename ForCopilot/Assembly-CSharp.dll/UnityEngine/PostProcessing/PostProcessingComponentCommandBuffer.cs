using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DB RID: 219
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000392 RID: 914
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x06000393 RID: 915
		public abstract string GetName();

		// Token: 0x06000394 RID: 916
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
