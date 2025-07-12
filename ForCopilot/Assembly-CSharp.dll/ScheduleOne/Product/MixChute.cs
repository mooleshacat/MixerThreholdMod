using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200094D RID: 2381
	public class MixChute : MonoBehaviour
	{
		// Token: 0x0600404C RID: 16460 RVA: 0x0010FE7C File Offset: 0x0010E07C
		private void Update()
		{
			this.UpdateDoor();
			this.IntObj.gameObject.SetActive(!NetworkSingleton<ProductManager>.Instance.IsMixComplete);
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0010FEA4 File Offset: 0x0010E0A4
		private void UpdateDoor()
		{
			bool flag = false;
			if (NetworkSingleton<ProductManager>.Instance.IsMixComplete && NetworkSingleton<ProductManager>.Instance.CurrentMixOperation != null)
			{
				flag = true;
			}
			else if (Singleton<CreateMixInterface>.Instance.IsOpen)
			{
				flag = true;
			}
			if (flag != this.isDoorOpen)
			{
				this.SetDoorOpen(flag);
			}
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0010FEF0 File Offset: 0x0010E0F0
		public void Hovered()
		{
			if (!NetworkSingleton<ProductManager>.Instance.IsMixComplete)
			{
				if (NetworkSingleton<ProductManager>.Instance.IsMixingInProgress)
				{
					this.IntObj.SetMessage("Mix will be ready tomorrow");
					this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Label);
					return;
				}
				this.IntObj.SetMessage("Create new mix");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0010FF4E File Offset: 0x0010E14E
		public void Interacted()
		{
			if (!NetworkSingleton<ProductManager>.Instance.IsMixComplete && !NetworkSingleton<ProductManager>.Instance.IsMixingInProgress)
			{
				Singleton<CreateMixInterface>.Instance.Open();
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0010FF72 File Offset: 0x0010E172
		public void SetDoorOpen(bool isOpen)
		{
			this.isDoorOpen = isOpen;
			this.DoorAnim.Play(this.isDoorOpen ? "Cabin flap open" : "Cabin flap close");
		}

		// Token: 0x04002DB5 RID: 11701
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04002DB6 RID: 11702
		public Animation DoorAnim;

		// Token: 0x04002DB7 RID: 11703
		private bool isDoorOpen;
	}
}
