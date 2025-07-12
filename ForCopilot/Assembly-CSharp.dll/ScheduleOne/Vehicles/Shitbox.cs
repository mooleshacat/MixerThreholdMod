using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200080B RID: 2059
	public class Shitbox : LandVehicle
	{
		// Token: 0x06003816 RID: 14358 RVA: 0x000EC755 File Offset: 0x000EA955
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x000EC76E File Offset: 0x000EA96E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x000EC787 File Offset: 0x000EA987
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x000EC795 File Offset: 0x000EA995
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040027DE RID: 10206
		public LoanSharkCarVisuals LoanSharkVisuals;

		// Token: 0x040027DF RID: 10207
		private bool dll_Excuted;

		// Token: 0x040027E0 RID: 10208
		private bool dll_Excuted;

		// Token: 0x0200080C RID: 2060
		[Serializable]
		public class LoanSharkVisualsData : SaveData
		{
			// Token: 0x040027E1 RID: 10209
			public bool Enabled;

			// Token: 0x040027E2 RID: 10210
			public bool NoteVisible;
		}
	}
}
