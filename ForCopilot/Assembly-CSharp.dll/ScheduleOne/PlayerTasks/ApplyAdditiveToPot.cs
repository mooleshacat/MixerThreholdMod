using System;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks.Tasks;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200035A RID: 858
	public class ApplyAdditiveToPot : PourIntoPotTask
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override bool UseCoverage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x00053CFF File Offset: 0x00051EFF
		protected override Pot.ECameraPosition CameraPosition
		{
			get
			{
				return Pot.ECameraPosition.BirdsEye;
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00053D04 File Offset: 0x00051F04
		public ApplyAdditiveToPot(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			this.def = (_itemInstance.Definition as AdditiveDefinition);
			base.CurrentInstruction = "Cover soil with " + this.def.AdditivePrefab.AdditiveName + " (0%)";
			this.removeItemAfterInitialPour = false;
			this.pot.SoilCover.ConfigureAppearance((this.pourable as PourableAdditive).LiquidColor, 0.3f);
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00053D7C File Offset: 0x00051F7C
		public override void Update()
		{
			base.Update();
			int num = Mathf.FloorToInt(this.pot.SoilCover.GetNormalizedProgress() * 100f);
			base.CurrentInstruction = string.Concat(new string[]
			{
				"Cover soil with ",
				this.def.AdditivePrefab.AdditiveName,
				" (",
				num.ToString(),
				"%)"
			});
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00053DF1 File Offset: 0x00051FF1
		protected override void FullyCovered()
		{
			base.FullyCovered();
			this.pot.SendAdditive((this.pourable as PourableAdditive).AdditiveDefinition.AdditivePrefab.AssetPath, true);
			base.RemoveItem();
			this.Success();
		}

		// Token: 0x04001277 RID: 4727
		private AdditiveDefinition def;
	}
}
