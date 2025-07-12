using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A65 RID: 2661
	public class EyelidOverlay : Singleton<EyelidOverlay>
	{
		// Token: 0x06004799 RID: 18329 RVA: 0x0012CE1C File Offset: 0x0012B01C
		protected override void Awake()
		{
			base.Awake();
			this.OpenMultiplier.Initialize();
			this.SetOpen(1f);
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0012CE3C File Offset: 0x0012B03C
		private void Update()
		{
			if (Player.Local == null)
			{
				return;
			}
			if (this.AutoUpdate)
			{
				if (Player.Local.Energy.CurrentEnergy < 20f)
				{
					this.CurrentOpen = Mathf.Lerp(0.625f, 1f, Player.Local.Energy.CurrentEnergy / 20f);
				}
				else
				{
					this.CurrentOpen = 1f;
				}
			}
			this.SetOpen(this.CurrentOpen * this.OpenMultiplier.CurrentValue);
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0012CEC4 File Offset: 0x0012B0C4
		public void SetOpen(float openness)
		{
			this.CurrentOpen = openness;
			this.Upper.anchoredPosition = new Vector2(0f, Mathf.Lerp(this.Closed, this.Open, openness));
			this.Lower.anchoredPosition = new Vector2(0f, -Mathf.Lerp(this.Closed, this.Open, openness));
			this.Canvas.enabled = (openness < 1f);
		}

		// Token: 0x04003464 RID: 13412
		public const float MaxTiredOpenAmount = 0.625f;

		// Token: 0x04003465 RID: 13413
		public bool AutoUpdate = true;

		// Token: 0x04003466 RID: 13414
		[Header("Settings")]
		public float Open = 400f;

		// Token: 0x04003467 RID: 13415
		public float Closed = 30f;

		// Token: 0x04003468 RID: 13416
		[Header("References")]
		public RectTransform Upper;

		// Token: 0x04003469 RID: 13417
		public RectTransform Lower;

		// Token: 0x0400346A RID: 13418
		public Canvas Canvas;

		// Token: 0x0400346B RID: 13419
		[Range(0f, 1f)]
		public float CurrentOpen = 1f;

		// Token: 0x0400346C RID: 13420
		public FloatSmoother OpenMultiplier;
	}
}
