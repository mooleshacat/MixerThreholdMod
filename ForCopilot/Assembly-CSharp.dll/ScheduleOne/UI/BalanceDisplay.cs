using System;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009FB RID: 2555
	public class BalanceDisplay : MonoBehaviour
	{
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060044BD RID: 17597 RVA: 0x0012084D File Offset: 0x0011EA4D
		// (set) Token: 0x060044BE RID: 17598 RVA: 0x00120855 File Offset: 0x0011EA55
		public bool active { get; protected set; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x0012085E File Offset: 0x0011EA5E
		// (set) Token: 0x060044C0 RID: 17600 RVA: 0x00120866 File Offset: 0x0011EA66
		public float timeSinceActiveSet { get; protected set; }

		// Token: 0x060044C1 RID: 17601 RVA: 0x00120870 File Offset: 0x0011EA70
		protected virtual void Update()
		{
			this.timeSinceActiveSet += Time.deltaTime;
			if (this.timeSinceActiveSet > 3f)
			{
				this.active = false;
			}
			if (this.Group != null)
			{
				this.Group.alpha = Mathf.MoveTowards(this.Group.alpha, this.active ? 1f : 0f, Time.deltaTime / 0.25f);
			}
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x001208EB File Offset: 0x0011EAEB
		public void SetBalance(float balance)
		{
			this.BalanceLabel.text = MoneyManager.FormatAmount(balance, false, false);
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x00120900 File Offset: 0x0011EB00
		public void Show()
		{
			this.active = true;
			this.timeSinceActiveSet = 0f;
		}

		// Token: 0x04003192 RID: 12690
		public const float RESIDUAL_TIME = 3f;

		// Token: 0x04003193 RID: 12691
		public const float FADE_TIME = 0.25f;

		// Token: 0x04003194 RID: 12692
		[Header("References")]
		public CanvasGroup Group;

		// Token: 0x04003195 RID: 12693
		public TextMeshProUGUI BalanceLabel;
	}
}
