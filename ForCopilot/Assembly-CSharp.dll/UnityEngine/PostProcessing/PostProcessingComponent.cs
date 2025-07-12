using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DA RID: 218
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00014B5D File Offset: 0x00012D5D
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00014B65 File Offset: 0x00012D65
		public T model { get; internal set; }

		// Token: 0x0600038F RID: 911 RVA: 0x00014B6E File Offset: 0x00012D6E
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00014B7E File Offset: 0x00012D7E
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
