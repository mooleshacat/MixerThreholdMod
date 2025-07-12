using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using UnityEngine;

namespace ScheduleOne.Market
{
	// Token: 0x02000597 RID: 1431
	public class Merchant : MonoBehaviour
	{
		// Token: 0x06002296 RID: 8854 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x0008ED20 File Offset: 0x0008CF20
		public void Hovered()
		{
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.openTime, this.closeTime))
			{
				this.intObj.SetMessage("Browse " + this.shopName);
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			this.intObj.SetMessage("Closed");
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Interacted()
		{
		}

		// Token: 0x04001A4E RID: 6734
		[Header("Settings")]
		[SerializeField]
		protected string shopName = "Store";

		// Token: 0x04001A4F RID: 6735
		[SerializeField]
		protected int openTime = 600;

		// Token: 0x04001A50 RID: 6736
		[SerializeField]
		protected int closeTime = 1800;

		// Token: 0x04001A51 RID: 6737
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;
	}
}
