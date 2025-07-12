using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000C5A RID: 3162
	public class CashStackVisuals : MonoBehaviour
	{
		// Token: 0x06005913 RID: 22803 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x00178518 File Offset: 0x00176718
		public void ShowAmount(float amount)
		{
			this.Visuals_Over100.SetActive(amount >= 100f);
			this.Visuals_Under100.SetActive(amount < 100f);
			if (amount >= 100f)
			{
				int num = Mathf.RoundToInt(amount / 100f);
				for (int i = 0; i < this.Bills.Length; i++)
				{
					this.Bills[i].SetActive(num > i);
				}
				return;
			}
			int num2 = Mathf.Clamp(Mathf.RoundToInt(amount / 10f), 0, 10);
			for (int j = 0; j < this.Notes.Length; j++)
			{
				this.Notes[j].SetActive(num2 > j);
			}
		}

		// Token: 0x04004137 RID: 16695
		public const float MAX_AMOUNT = 1000f;

		// Token: 0x04004138 RID: 16696
		[Header("References")]
		public GameObject Visuals_Under100;

		// Token: 0x04004139 RID: 16697
		public GameObject[] Notes;

		// Token: 0x0400413A RID: 16698
		public GameObject Visuals_Over100;

		// Token: 0x0400413B RID: 16699
		public GameObject[] Bills;
	}
}
