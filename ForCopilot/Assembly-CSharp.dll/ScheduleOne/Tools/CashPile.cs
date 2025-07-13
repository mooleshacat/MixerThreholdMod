using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000883 RID: 2179
	public class CashPile : MonoBehaviour
	{
		// Token: 0x06003B99 RID: 15257 RVA: 0x000FC650 File Offset: 0x000FA850
		private void Awake()
		{
			this.CashInstances = new Transform[this.Container.childCount];
			for (int i = 0; i < this.CashInstances.Length; i++)
			{
				this.CashInstances[i] = this.Container.GetChild(i);
				this.CashInstances[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x000FC6B0 File Offset: 0x000FA8B0
		public void SetDisplayedAmount(float amount)
		{
			int num = Mathf.FloorToInt(amount / 100000f * (float)this.CashInstances.Length);
			for (int i = 0; i < this.CashInstances.Length; i++)
			{
				this.CashInstances[i].gameObject.SetActive(i < num);
			}
		}

		// Token: 0x04002A93 RID: 10899
		public const float MAX_AMOUNT = 100000f;

		// Token: 0x04002A94 RID: 10900
		public Transform Container;

		// Token: 0x04002A95 RID: 10901
		private Transform[] CashInstances;
	}
}
