using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000368 RID: 872
	public class StartLabOvenTask : Task
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x000565F9 File Offset: 0x000547F9
		// (set) Token: 0x060013A2 RID: 5026 RVA: 0x00056601 File Offset: 0x00054801
		public LabOven Oven { get; private set; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x0005660A File Offset: 0x0005480A
		// (set) Token: 0x060013A4 RID: 5028 RVA: 0x00056612 File Offset: 0x00054812
		public StartLabOvenTask.EStep CurrentStep { get; protected set; }

		// Token: 0x060013A5 RID: 5029 RVA: 0x0005661C File Offset: 0x0005481C
		public StartLabOvenTask(LabOven oven)
		{
			this.Oven = oven;
			oven.ResetPourableContainer();
			this.stationItem = oven.CreateStationItems(1)[0];
			this.stationItem.ActivateModule<PourableModule>();
			this.pourableModule = this.stationItem.GetModule<PourableModule>();
			ConfigurableJoint componentInChildren = this.stationItem.GetComponentInChildren<ConfigurableJoint>();
			if (componentInChildren != null)
			{
				UnityEngine.Object.Destroy(componentInChildren);
			}
			Rigidbody componentInChildren2 = this.stationItem.GetComponentInChildren<Rigidbody>();
			if (componentInChildren2 != null)
			{
				UnityEngine.Object.Destroy(componentInChildren2);
			}
			Draggable componentInChildren3 = this.stationItem.GetComponentInChildren<Draggable>();
			if (componentInChildren3 != null)
			{
				componentInChildren3.ClickableEnabled = false;
			}
			this.ingredient = this.Oven.IngredientSlot.ItemInstance.GetCopy(1);
			this.Oven.IngredientSlot.ItemInstance.ChangeQuantity(-1);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			oven.Door.SetInteractable(true);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00056716 File Offset: 0x00054916
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			base.CurrentInstruction = StartLabOvenTask.GetStepInstruction(this.CurrentStep);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00056738 File Offset: 0x00054938
		public override void Success()
		{
			string id = (this.ingredient.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().Product.ID;
			EQuality ingredientQuality = EQuality.Standard;
			if (this.ingredient is QualityItemInstance)
			{
				ingredientQuality = (this.ingredient as QualityItemInstance).Quality;
			}
			this.Oven.SendCookOperation(new OvenCookOperation(this.ingredient.ID, ingredientQuality, 1, id));
			base.Success();
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000567B0 File Offset: 0x000549B0
		public override void StopTask()
		{
			base.StopTask();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Oven.IngredientSlot.AddItem(this.ingredient, false);
				this.Oven.LiquidMesh.gameObject.SetActive(false);
			}
			this.stationItem.Destroy();
			if (this.pourRoutine != null)
			{
				this.Oven.PourAnimation.Stop();
				this.Oven.StopCoroutine(this.pourRoutine);
			}
			this.Oven.ClearDecals();
			this.Oven.Door.SetPosition(0f);
			this.Oven.Door.SetInteractable(false);
			this.Oven.WireTray.SetPosition(0f);
			this.Oven.Button.SetInteractable(false);
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(this.Oven, true, true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Oven.CameraPosition_Default.position, this.Oven.CameraPosition_Default.rotation, 0.2f, false);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x000568DC File Offset: 0x00054ADC
		private void CheckProgress()
		{
			switch (this.CurrentStep)
			{
			case StartLabOvenTask.EStep.OpenDoor:
				this.CheckStep_OpenDoor();
				return;
			case StartLabOvenTask.EStep.Pour:
				this.CheckStep_Pour();
				return;
			case StartLabOvenTask.EStep.CloseDoor:
				this.CheckStep_CloseDoor();
				return;
			case StartLabOvenTask.EStep.PressButton:
				this.CheckStep_PressButton();
				return;
			default:
				return;
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00056924 File Offset: 0x00054B24
		private void ProgressStep()
		{
			if (this.CurrentStep == StartLabOvenTask.EStep.PressButton)
			{
				this.Success();
				return;
			}
			StartLabOvenTask.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == StartLabOvenTask.EStep.Pour)
			{
				this.Oven.WireTray.SetPosition(1f);
			}
			if (this.CurrentStep == StartLabOvenTask.EStep.CloseDoor)
			{
				this.Oven.Door.SetInteractable(true);
			}
			if (this.CurrentStep == StartLabOvenTask.EStep.Pour)
			{
				this.pourRoutine = this.Oven.StartCoroutine(this.PlayPourAnimation());
			}
			if (this.CurrentStep == StartLabOvenTask.EStep.PressButton)
			{
				this.Oven.Button.SetInteractable(true);
			}
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000569C4 File Offset: 0x00054BC4
		private void CheckStep_OpenDoor()
		{
			if (this.Oven.Door.TargetPosition > 0.9f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(1f);
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00056A14 File Offset: 0x00054C14
		private void CheckStep_Pour()
		{
			if (this.pourAnimDone)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00056A24 File Offset: 0x00054C24
		private void CheckStep_CloseDoor()
		{
			if (this.Oven.Door.TargetPosition < 0.05f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(0f);
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00056A74 File Offset: 0x00054C74
		private void CheckStep_PressButton()
		{
			if (this.Oven.Button.Pressed)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00056A8E File Offset: 0x00054C8E
		private IEnumerator PlayPourAnimation()
		{
			this.Oven.SetLiquidColor(this.stationItem.GetModule<CookableModule>().LiquidColor);
			this.Oven.PourAnimation.Play();
			yield return new WaitForSeconds(0.6f);
			float pourTime = 1f;
			for (float i = 0f; i < pourTime; i += Time.deltaTime)
			{
				this.pourableModule.LiquidContainer.SetLiquidLevel(1f - i / pourTime, false);
				yield return null;
			}
			this.pourableModule.LiquidContainer.SetLiquidLevel(0f, false);
			this.pourAnimDone = true;
			yield break;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00056A9D File Offset: 0x00054C9D
		public static string GetStepInstruction(StartLabOvenTask.EStep step)
		{
			switch (step)
			{
			case StartLabOvenTask.EStep.OpenDoor:
				return "Open oven door";
			case StartLabOvenTask.EStep.Pour:
				return "Pour liquid into tray";
			case StartLabOvenTask.EStep.CloseDoor:
				return "Close oven door";
			case StartLabOvenTask.EStep.PressButton:
				return "Start oven";
			default:
				return string.Empty;
			}
		}

		// Token: 0x040012B9 RID: 4793
		private ItemInstance ingredient;

		// Token: 0x040012BA RID: 4794
		private Coroutine pourRoutine;

		// Token: 0x040012BB RID: 4795
		private StationItem stationItem;

		// Token: 0x040012BC RID: 4796
		private PourableModule pourableModule;

		// Token: 0x040012BD RID: 4797
		private bool pourAnimDone;

		// Token: 0x02000369 RID: 873
		public enum EStep
		{
			// Token: 0x040012BF RID: 4799
			OpenDoor,
			// Token: 0x040012C0 RID: 4800
			Pour,
			// Token: 0x040012C1 RID: 4801
			CloseDoor,
			// Token: 0x040012C2 RID: 4802
			PressButton
		}
	}
}
