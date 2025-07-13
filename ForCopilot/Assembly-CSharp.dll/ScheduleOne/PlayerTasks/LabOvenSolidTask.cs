using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000363 RID: 867
	public class LabOvenSolidTask : Task
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x000552F3 File Offset: 0x000534F3
		// (set) Token: 0x0600137F RID: 4991 RVA: 0x000552FB File Offset: 0x000534FB
		public LabOven Oven { get; private set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00055304 File Offset: 0x00053504
		// (set) Token: 0x06001381 RID: 4993 RVA: 0x0005530C File Offset: 0x0005350C
		public LabOvenSolidTask.EStep CurrentStep { get; protected set; }

		// Token: 0x06001382 RID: 4994 RVA: 0x00055318 File Offset: 0x00053518
		public LabOvenSolidTask(LabOven oven)
		{
			this.Oven = oven;
			this.ingredientQuantity = Mathf.Min(this.Oven.IngredientSlot.Quantity, 10);
			this.stationItems = oven.CreateStationItems(this.ingredientQuantity);
			this.stationDraggables = new Draggable[this.stationItems.Length];
			for (int i = 0; i < this.stationItems.Length; i++)
			{
				this.stationDraggables[i] = this.stationItems[i].GetComponentInChildren<Draggable>();
			}
			this.ingredient = this.Oven.IngredientSlot.ItemInstance.GetCopy(this.ingredientQuantity);
			this.Oven.IngredientSlot.ChangeQuantity(-this.ingredientQuantity, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Oven.CameraPosition_PlaceItems.position, this.Oven.CameraPosition_PlaceItems.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			base.EnableMultiDragging(oven.ItemContainer, 0.12f);
			oven.Door.SetInteractable(true);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00055448 File Offset: 0x00053648
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			base.CurrentInstruction = LabOvenSolidTask.GetStepInstruction(this.CurrentStep);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00055468 File Offset: 0x00053668
		public override void Success()
		{
			string id = (this.ingredient.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().Product.ID;
			EQuality ingredientQuality = EQuality.Standard;
			if (this.ingredient is QualityItemInstance)
			{
				ingredientQuality = (this.ingredient as QualityItemInstance).Quality;
			}
			this.Oven.SendCookOperation(new OvenCookOperation(this.ingredient.ID, ingredientQuality, this.ingredientQuantity, id));
			base.Success();
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000554E4 File Offset: 0x000536E4
		public override void StopTask()
		{
			base.StopTask();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Oven.IngredientSlot.AddItem(this.ingredient, false);
				this.Oven.LiquidMesh.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.stationItems.Length; i++)
			{
				this.stationItems[i].Destroy();
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

		// Token: 0x06001386 RID: 4998 RVA: 0x000555FC File Offset: 0x000537FC
		private void CheckProgress()
		{
			switch (this.CurrentStep)
			{
			case LabOvenSolidTask.EStep.OpenDoor:
				this.CheckStep_OpenDoor();
				return;
			case LabOvenSolidTask.EStep.PlaceItems:
				this.CheckStep_PlaceItems();
				return;
			case LabOvenSolidTask.EStep.CloseDoor:
				this.CheckStep_CloseDoor();
				return;
			case LabOvenSolidTask.EStep.PressButton:
				this.CheckStep_PressButton();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00055644 File Offset: 0x00053844
		private void ProgressStep()
		{
			if (this.CurrentStep == LabOvenSolidTask.EStep.PressButton)
			{
				this.Success();
				return;
			}
			LabOvenSolidTask.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == LabOvenSolidTask.EStep.PlaceItems)
			{
				this.Oven.WireTray.SetPosition(1f);
			}
			if (this.CurrentStep == LabOvenSolidTask.EStep.CloseDoor)
			{
				this.Oven.Door.SetInteractable(true);
				for (int i = 0; i < this.stationDraggables.Length; i++)
				{
					this.stationDraggables[i].ClickableEnabled = false;
					UnityEngine.Object.Destroy(this.stationDraggables[i].Rb);
					this.stationItems[i].transform.SetParent(this.Oven.SquareTray);
				}
			}
			if (this.CurrentStep == LabOvenSolidTask.EStep.PressButton)
			{
				this.Oven.Button.SetInteractable(true);
			}
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00055714 File Offset: 0x00053914
		private void CheckStep_OpenDoor()
		{
			if (this.Oven.Door.TargetPosition > 0.9f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(1f);
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00055764 File Offset: 0x00053964
		private void CheckStep_PlaceItems()
		{
			for (int i = 0; i < this.stationDraggables.Length; i++)
			{
				if (this.stationDraggables[i].IsHeld)
				{
					return;
				}
				if (this.stationDraggables[i].Rb.velocity.magnitude > 0.02f)
				{
					return;
				}
				if (!this.Oven.TrayDetectionArea.bounds.Contains(this.stationDraggables[i].transform.position))
				{
					return;
				}
			}
			this.ProgressStep();
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000557EC File Offset: 0x000539EC
		private void CheckStep_CloseDoor()
		{
			if (this.Oven.Door.TargetPosition < 0.05f)
			{
				this.ProgressStep();
				this.Oven.Door.SetInteractable(false);
				this.Oven.Door.SetPosition(0f);
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005583C File Offset: 0x00053A3C
		private void CheckStep_PressButton()
		{
			if (this.Oven.Button.Pressed)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00055856 File Offset: 0x00053A56
		public static string GetStepInstruction(LabOvenSolidTask.EStep step)
		{
			switch (step)
			{
			case LabOvenSolidTask.EStep.OpenDoor:
				return "Open oven door";
			case LabOvenSolidTask.EStep.PlaceItems:
				return "Place items onto tray";
			case LabOvenSolidTask.EStep.CloseDoor:
				return "Close oven door";
			case LabOvenSolidTask.EStep.PressButton:
				return "Start oven";
			default:
				return string.Empty;
			}
		}

		// Token: 0x0400129D RID: 4765
		private ItemInstance ingredient;

		// Token: 0x0400129E RID: 4766
		private int ingredientQuantity = 1;

		// Token: 0x0400129F RID: 4767
		private StationItem[] stationItems;

		// Token: 0x040012A0 RID: 4768
		private Draggable[] stationDraggables;

		// Token: 0x02000364 RID: 868
		public enum EStep
		{
			// Token: 0x040012A2 RID: 4770
			OpenDoor,
			// Token: 0x040012A3 RID: 4771
			PlaceItems,
			// Token: 0x040012A4 RID: 4772
			CloseDoor,
			// Token: 0x040012A5 RID: 4773
			PressButton
		}
	}
}
