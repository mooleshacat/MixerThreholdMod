using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000890 RID: 2192
	public class EditionConditionalObject : MonoBehaviour
	{
		// Token: 0x06003BCC RID: 15308 RVA: 0x000FCDC1 File Offset: 0x000FAFC1
		private void Awake()
		{
			if (this.type == EditionConditionalObject.EType.ActiveInDemo)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04002AB8 RID: 10936
		public EditionConditionalObject.EType type;

		// Token: 0x02000891 RID: 2193
		public enum EType
		{
			// Token: 0x04002ABA RID: 10938
			ActiveInDemo,
			// Token: 0x04002ABB RID: 10939
			ActiveInFullGame
		}
	}
}
