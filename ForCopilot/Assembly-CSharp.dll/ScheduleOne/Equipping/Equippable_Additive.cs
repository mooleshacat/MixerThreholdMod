using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200095A RID: 2394
	public class Equippable_Additive : Equippable_Pourable
	{
		// Token: 0x06004091 RID: 16529 RVA: 0x00110F94 File Offset: 0x0010F194
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.additiveDef = (this.itemInstance.Definition as AdditiveDefinition);
			this.InteractionLabel = "Apply " + this.additiveDef.Name;
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x00110FCE File Offset: 0x0010F1CE
		protected override void StartPourTask(Pot pot)
		{
			new ApplyAdditiveToPot(pot, this.itemInstance, this.PourablePrefab);
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x00110FE4 File Offset: 0x0010F1E4
		protected override bool CanPour(Pot pot, out string reason)
		{
			if (pot.SoilLevel < pot.SoilCapacity)
			{
				reason = "No soil";
				return false;
			}
			if (pot.Plant == null)
			{
				reason = "No plant";
				return false;
			}
			if (pot.GetAdditive(this.additiveDef.AdditivePrefab.AdditiveName) != null)
			{
				reason = "Already contains " + this.additiveDef.AdditivePrefab.AdditiveName;
				return false;
			}
			return base.CanPour(pot, out reason);
		}

		// Token: 0x04002DFD RID: 11773
		private AdditiveDefinition additiveDef;
	}
}
