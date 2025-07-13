using System;
using ScheduleOne.Equipping;
using ScheduleOne.PlayerTasks.Tasks;

namespace ScheduleOne.ObjectScripts.Soil
{
	// Token: 0x02000C55 RID: 3157
	public class Equippable_Soil : Equippable_Pourable
	{
		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x060058F8 RID: 22776 RVA: 0x00177F86 File Offset: 0x00176186
		// (set) Token: 0x060058F9 RID: 22777 RVA: 0x00177F8E File Offset: 0x0017618E
		public override string InteractionLabel { get; set; } = "Pour soil";

		// Token: 0x060058FA RID: 22778 RVA: 0x00177F98 File Offset: 0x00176198
		protected override bool CanPour(Pot pot, out string reason)
		{
			if (pot.SoilLevel >= pot.SoilCapacity)
			{
				reason = "Pot already full";
				return false;
			}
			if (!string.IsNullOrEmpty(pot.SoilID) && pot.SoilID != this.itemInstance.ID)
			{
				reason = "Soil type mismatch";
				return false;
			}
			return base.CanPour(pot, out reason);
		}

		// Token: 0x060058FB RID: 22779 RVA: 0x00177FF2 File Offset: 0x001761F2
		protected override void StartPourTask(Pot pot)
		{
			new PourSoilTask(pot, this.itemInstance, this.PourablePrefab);
		}
	}
}
