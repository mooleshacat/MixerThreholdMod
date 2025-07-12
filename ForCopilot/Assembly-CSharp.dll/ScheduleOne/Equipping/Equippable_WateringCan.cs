using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerTasks.Tasks;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000958 RID: 2392
	public class Equippable_WateringCan : Equippable_Pourable
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x00110E1B File Offset: 0x0010F01B
		// (set) Token: 0x06004087 RID: 16519 RVA: 0x00110E23 File Offset: 0x0010F023
		public override string InteractionLabel { get; set; } = "Pour water";

		// Token: 0x06004088 RID: 16520 RVA: 0x00110E2C File Offset: 0x0010F02C
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.WCInstance = (item as WateringCanInstance);
			this.UpdateVisuals();
			item.onDataChanged = (Action)Delegate.Combine(item.onDataChanged, new Action(this.UpdateVisuals));
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00110E69 File Offset: 0x0010F069
		public override void Unequip()
		{
			base.Unequip();
			if (this.WCInstance != null)
			{
				WateringCanInstance wcinstance = this.WCInstance;
				wcinstance.onDataChanged = (Action)Delegate.Remove(wcinstance.onDataChanged, new Action(this.UpdateVisuals));
			}
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x00110EA0 File Offset: 0x0010F0A0
		private void UpdateVisuals()
		{
			if (this.WCInstance == null)
			{
				return;
			}
			this.Visuals.SetFillLevel(this.WCInstance.CurrentFillAmount / 15f);
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00110EC8 File Offset: 0x0010F0C8
		protected override bool CanPour(Pot pot, out string reason)
		{
			if (pot.SoilLevel < pot.SoilCapacity)
			{
				reason = "No soil";
				return false;
			}
			if (pot.NormalizedWaterLevel >= 0.975f)
			{
				reason = string.Empty;
				return false;
			}
			if ((this.itemInstance as WateringCanInstance).CurrentFillAmount <= 0f)
			{
				reason = "Watering can empty";
				return false;
			}
			return base.CanPour(pot, out reason);
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00110F2A File Offset: 0x0010F12A
		protected override void StartPourTask(Pot pot)
		{
			new PourWaterTask(pot, this.itemInstance, this.PourablePrefab);
		}

		// Token: 0x04002DF8 RID: 11768
		public WateringCanVisuals Visuals;

		// Token: 0x04002DF9 RID: 11769
		private WateringCanInstance WCInstance;
	}
}
