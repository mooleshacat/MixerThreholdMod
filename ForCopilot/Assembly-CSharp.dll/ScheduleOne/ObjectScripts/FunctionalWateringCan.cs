using System;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerTasks;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C45 RID: 3141
	public class FunctionalWateringCan : Pourable
	{
		// Token: 0x0600584F RID: 22607 RVA: 0x00175CE4 File Offset: 0x00173EE4
		public void Setup(WateringCanInstance instance)
		{
			this.itemInstance = instance;
			this.autoSetCurrentQuantity = false;
			this.currentQuantity = this.itemInstance.CurrentFillAmount;
			this.Visuals.SetFillLevel(this.itemInstance.CurrentFillAmount / 15f);
			base.Rb.isKinematic = false;
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x00175D38 File Offset: 0x00173F38
		protected override void PourAmount(float amount)
		{
			this.itemInstance.ChangeFillAmount(-amount);
			this.Visuals.SetFillLevel(this.itemInstance.CurrentFillAmount / 15f);
			base.PourAmount(amount);
		}

		// Token: 0x040040C9 RID: 16585
		public WateringCanVisuals Visuals;

		// Token: 0x040040CA RID: 16586
		private WateringCanInstance itemInstance;
	}
}
