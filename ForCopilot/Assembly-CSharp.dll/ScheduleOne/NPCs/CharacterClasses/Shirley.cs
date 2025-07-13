using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200051D RID: 1309
	public class Shirley : Supplier
	{
		// Token: 0x06001CDA RID: 7386 RVA: 0x00077D30 File Offset: 0x00075F30
		protected override void DeaddropConfirmed(List<PhoneShopInterface.CartEntry> cart, float totalPrice)
		{
			base.DeaddropConfirmed(cart, totalPrice);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ShirleyDeaddropOrders", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("ShirleyDeaddropOrders") + 1f).ToString(), true);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x00077D72 File Offset: 0x00075F72
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x00077D8B File Offset: 0x00075F8B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ShirleyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x00077DA4 File Offset: 0x00075FA4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x00077DB2 File Offset: 0x00075FB2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017B4 RID: 6068
		private bool dll_Excuted;

		// Token: 0x040017B5 RID: 6069
		private bool dll_Excuted;
	}
}
