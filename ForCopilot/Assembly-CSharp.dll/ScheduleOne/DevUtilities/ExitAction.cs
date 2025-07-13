using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200074E RID: 1870
	public class ExitAction
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x000D1613 File Offset: 0x000CF813
		// (set) Token: 0x0600323C RID: 12860 RVA: 0x000D161B File Offset: 0x000CF81B
		public bool Used
		{
			get
			{
				return this.used;
			}
			set
			{
				if (value)
				{
					this.Use();
				}
			}
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000D1626 File Offset: 0x000CF826
		public void Use()
		{
			this.used = true;
		}

		// Token: 0x0400237F RID: 9087
		public ExitType exitType;

		// Token: 0x04002380 RID: 9088
		private bool used;
	}
}
