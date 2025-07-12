using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Money;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000975 RID: 2421
	public class CashPickup : ItemPickup
	{
		// Token: 0x0600417F RID: 16767 RVA: 0x00114A14 File Offset: 0x00112C14
		protected override void Hovered()
		{
			this.IntObj.SetMessage("Pick up " + MoneyManager.FormatAmount(this.Value, false, false));
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override bool CanPickup()
		{
			return true;
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00114A44 File Offset: 0x00112C44
		protected override void Pickup()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.Value, true, false);
			base.Pickup();
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00114A71 File Offset: 0x00112C71
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00114A8A File Offset: 0x00112C8A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ItemFramework.CashPickupAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x00114AA3 File Offset: 0x00112CA3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x00114AB1 File Offset: 0x00112CB1
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002EB1 RID: 11953
		public float Value = 10f;

		// Token: 0x04002EB2 RID: 11954
		private bool dll_Excuted;

		// Token: 0x04002EB3 RID: 11955
		private bool dll_Excuted;
	}
}
