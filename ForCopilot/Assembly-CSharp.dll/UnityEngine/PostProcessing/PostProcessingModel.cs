using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DE RID: 222
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00014C1F File Offset: 0x00012E1F
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x00014C27 File Offset: 0x00012E27
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x060003A4 RID: 932
		public abstract void Reset();

		// Token: 0x060003A5 RID: 933 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void OnValidate()
		{
		}

		// Token: 0x0400047E RID: 1150
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
