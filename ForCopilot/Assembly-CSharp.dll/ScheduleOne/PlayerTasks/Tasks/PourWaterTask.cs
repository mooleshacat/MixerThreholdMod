using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000373 RID: 883
	public class PourWaterTask : PourOntoTargetTask
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override bool UseCoverage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00014B5A File Offset: 0x00012D5A
		protected override bool FailOnEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00053CFF File Offset: 0x00051EFF
		protected override Pot.ECameraPosition CameraPosition
		{
			get
			{
				return Pot.ECameraPosition.BirdsEye;
			}
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0005846C File Offset: 0x0005666C
		public PourWaterTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			base.CurrentInstruction = "Pour water over target";
			this.removeItemAfterInitialPour = false;
			this.pourable.GetComponent<FunctionalWateringCan>().Setup(_itemInstance as WateringCanInstance);
			this.pourable.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			this.pot.SoilCover.ConfigureAppearance(Color.black, 0.6f);
			if (NetworkSingleton<GameManager>.Instance.IsTutorial && !PourWaterTask.hintShown)
			{
				PourWaterTask.hintShown = true;
				Singleton<HintDisplay>.Instance.ShowHint_20s("While dragging an item, press <Input_Left> or <Input_Right> to rotate it.");
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00058510 File Offset: 0x00056710
		public override void StopTask()
		{
			this.pot.PushWaterDataToServer();
			base.StopTask();
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00058524 File Offset: 0x00056724
		public override void TargetReached()
		{
			this.pot.ChangeWaterAmount(0.2f * this.pot.WaterCapacity);
			this.pot.PushWaterDataToServer();
			if (this.pot.NormalizedWaterLevel >= 0.975f)
			{
				this.Success();
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("WateredPotsCount");
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("WateredPotsCount", (value + 1f).ToString(), true);
			}
			base.TargetReached();
		}

		// Token: 0x040012F3 RID: 4851
		public const float NORMALIZED_FILL_PER_TARGET = 0.2f;

		// Token: 0x040012F4 RID: 4852
		public static bool hintShown;
	}
}
