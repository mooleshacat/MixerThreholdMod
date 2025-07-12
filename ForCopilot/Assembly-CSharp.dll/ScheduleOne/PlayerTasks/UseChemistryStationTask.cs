using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200036F RID: 879
	public class UseChemistryStationTask : Task
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x000570FA File Offset: 0x000552FA
		// (set) Token: 0x060013CE RID: 5070 RVA: 0x00057102 File Offset: 0x00055302
		public ChemistryStation.EStep CurrentStep { get; private set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x0005710B File Offset: 0x0005530B
		// (set) Token: 0x060013D0 RID: 5072 RVA: 0x00057113 File Offset: 0x00055313
		public ChemistryStation Station { get; private set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x0005711C File Offset: 0x0005531C
		// (set) Token: 0x060013D2 RID: 5074 RVA: 0x00057124 File Offset: 0x00055324
		public StationRecipe Recipe { get; private set; }

		// Token: 0x060013D3 RID: 5075 RVA: 0x00057130 File Offset: 0x00055330
		public static string GetStepDescription(ChemistryStation.EStep step)
		{
			switch (step)
			{
			case ChemistryStation.EStep.CombineIngredients:
				return "Combine ingredients in beaker";
			case ChemistryStation.EStep.Stir:
				return "Stir mixture";
			case ChemistryStation.EStep.LowerBoilingFlask:
				return "Lower boiling flask";
			case ChemistryStation.EStep.PourIntoBoilingFlask:
				return "Pour mixture into boiling flask";
			case ChemistryStation.EStep.RaiseBoilingFlask:
				return "Raise boiling flask above burner";
			case ChemistryStation.EStep.StartHeat:
				return "Start burner";
			case ChemistryStation.EStep.Cook:
				return "Wait for the mixture to finish cooking";
			case ChemistryStation.EStep.LowerBoilingFlaskAgain:
				return "Lower boiling flask";
			case ChemistryStation.EStep.PourThroughFilter:
				return "Pour mixture through filter";
			default:
				return "Unknown step";
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000571A4 File Offset: 0x000553A4
		public UseChemistryStationTask(ChemistryStation station, StationRecipe recipe)
		{
			this.Station = station;
			this.Recipe = recipe;
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.beaker = station.CreateBeaker();
			station.StaticBeaker.gameObject.SetActive(false);
			base.EnableMultiDragging(station.ItemContainer, 0.1f);
			this.RemovedIngredients = new ItemInstance[station.IngredientSlots.Length];
			for (int i = 0; i < recipe.Ingredients.Count; i++)
			{
				StorableItemDefinition storableItemDefinition = null;
				foreach (ItemDefinition itemDefinition in recipe.Ingredients[i].Items)
				{
					StorableItemDefinition storableItemDefinition2 = itemDefinition as StorableItemDefinition;
					for (int j = 0; j < station.IngredientSlots.Length; j++)
					{
						if (station.IngredientSlots[j].ItemInstance != null && station.IngredientSlots[j].ItemInstance.Definition.ID == storableItemDefinition2.ID)
						{
							storableItemDefinition = storableItemDefinition2;
							this.RemovedIngredients[j] = station.IngredientSlots[j].ItemInstance.GetCopy(recipe.Ingredients[i].Quantity);
							station.IngredientSlots[j].ChangeQuantity(-recipe.Ingredients[i].Quantity, false);
							break;
						}
					}
				}
				if (storableItemDefinition.StationItem == null)
				{
					Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have a station item", null);
				}
				else
				{
					StationItem stationItem = UnityEngine.Object.Instantiate<StationItem>(storableItemDefinition.StationItem, station.ItemContainer);
					stationItem.transform.position = station.IngredientTransforms[i].position;
					stationItem.Initialize(storableItemDefinition);
					stationItem.transform.rotation = station.IngredientTransforms[i].rotation;
					if (stationItem.HasModule<IngredientModule>())
					{
						stationItem.ActivateModule<IngredientModule>();
						foreach (IngredientPiece ingredientPiece in stationItem.GetModule<IngredientModule>().Pieces)
						{
							this.ingredientPieces.Add(ingredientPiece);
							ingredientPiece.GetComponent<Draggable>().CanBeMultiDragged = true;
						}
					}
					else if (stationItem.HasModule<PourableModule>())
					{
						stationItem.ActivateModule<PourableModule>();
						Draggable componentInChildren = stationItem.GetComponentInChildren<Draggable>();
						if (componentInChildren != null)
						{
							componentInChildren.CanBeMultiDragged = false;
						}
					}
					else
					{
						Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have an ingredient or pourable module", null);
					}
					foreach (Draggable draggable in stationItem.GetComponentsInChildren<Draggable>())
					{
						draggable.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
						DraggableConstraint component = draggable.gameObject.GetComponent<DraggableConstraint>();
						if (component != null)
						{
							component.ProportionalZClamp = true;
						}
					}
					this.items.Add(stationItem);
				}
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000574B4 File Offset: 0x000556B4
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			this.UpdateInstruction();
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000574C8 File Offset: 0x000556C8
		private void UpdateInstruction()
		{
			base.CurrentInstruction = UseChemistryStationTask.GetStepDescription(this.CurrentStep);
			if (this.CurrentStep == ChemistryStation.EStep.Stir)
			{
				base.CurrentInstruction = base.CurrentInstruction + " (" + Mathf.RoundToInt(this.stirProgress * 100f).ToString() + "%)";
			}
			if (this.CurrentStep == ChemistryStation.EStep.StartHeat)
			{
				base.CurrentInstruction = base.CurrentInstruction + " (" + Mathf.RoundToInt(this.timeInTemperatureRange / 2f * 100f).ToString() + "%)";
			}
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00057568 File Offset: 0x00055768
		private void CheckProgress()
		{
			switch (this.CurrentStep)
			{
			case ChemistryStation.EStep.CombineIngredients:
				this.CheckStep_CombineIngredients();
				return;
			case ChemistryStation.EStep.Stir:
				this.CheckStep_StirMixture();
				return;
			case ChemistryStation.EStep.LowerBoilingFlask:
				this.CheckStep_LowerBoilingFlask();
				return;
			case ChemistryStation.EStep.PourIntoBoilingFlask:
				this.CheckStep_PourIntoBoilingFlask();
				return;
			case ChemistryStation.EStep.RaiseBoilingFlask:
				this.CheckStep_RaiseBoilingFlask();
				return;
			case ChemistryStation.EStep.StartHeat:
				this.CheckStep_StartHeat();
				return;
			default:
				return;
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x000575C4 File Offset: 0x000557C4
		private void ProgressStep()
		{
			ChemistryStation.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == ChemistryStation.EStep.Stir)
			{
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Station.CameraPosition_Stirring.position, this.Station.CameraPosition_Stirring.rotation, 0.2f, false);
				this.stirringRod = this.Station.CreateStirringRod();
				this.Station.StaticStirringRod.gameObject.SetActive(false);
			}
			if (this.CurrentStep == ChemistryStation.EStep.LowerBoilingFlask)
			{
				if (this.stirringRod != null)
				{
					this.stirringRod.Destroy();
				}
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Station.CameraPosition_Default.position, this.Station.CameraPosition_Default.rotation, 0.2f, false);
				this.Station.LabStand.SetInteractable(true);
			}
			if (this.CurrentStep == ChemistryStation.EStep.PourIntoBoilingFlask)
			{
				this.beaker.SetStatic(false);
				this.beaker.ActivateModule<PourableModule>();
				this.beaker.Fillable.enabled = false;
				PourableModule module = this.beaker.GetModule<PourableModule>();
				module.SetLiquidLevel(module.LiquidContainer.CurrentLiquidLevel);
				module.LiquidColor = module.LiquidContainer.LiquidVolume.liquidColor1;
				module.PourParticlesColor = module.LiquidColor;
			}
			if (this.CurrentStep == ChemistryStation.EStep.RaiseBoilingFlask)
			{
				this.Station.LabStand.SetInteractable(true);
			}
			if (this.CurrentStep == ChemistryStation.EStep.StartHeat)
			{
				this.Station.Burner.SetInteractable(true);
				this.Station.BoilingFlask.SetCanvasVisible(true);
				this.Station.BoilingFlask.SetRecipe(this.Recipe);
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00057770 File Offset: 0x00055970
		private void CheckStep_CombineIngredients()
		{
			bool flag = true;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].HasModule<PourableModule>())
				{
					if (this.items[i].GetModule<PourableModule>().NormalizedLiquidLevel > 0.05f)
					{
						flag = false;
						break;
					}
				}
				else if (this.items[i].HasModule<IngredientModule>())
				{
					IngredientPiece[] pieces = this.items[i].GetModule<IngredientModule>().Pieces;
					for (int j = 0; j < pieces.Length; j++)
					{
						if (pieces[j].CurrentLiquidContainer == null)
						{
							flag = false;
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00057824 File Offset: 0x00055A24
		private void CheckStep_StirMixture()
		{
			float num = this.stirringRod.CurrentStirringSpeed * Time.deltaTime / 1.5f;
			if (num > 0f)
			{
				this.stirProgress = Mathf.Clamp(this.stirProgress + num, 0f, 1f);
				foreach (IngredientPiece ingredientPiece in this.ingredientPieces)
				{
					ingredientPiece.DissolveAmount(num, num > 0.001f);
				}
			}
			if (this.stirProgress >= 1f)
			{
				this.ProgressStep();
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000578D0 File Offset: 0x00055AD0
		private void CheckStep_LowerBoilingFlask()
		{
			if (this.Station.LabStand.CurrentPosition <= this.Station.LabStand.FunnelThreshold)
			{
				this.Station.LabStand.SetPosition(0f);
				this.Station.LabStand.SetInteractable(false);
				this.ProgressStep();
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0005792B File Offset: 0x00055B2B
		private void CheckStep_PourIntoBoilingFlask()
		{
			if (this.beaker.Pourable.NormalizedLiquidLevel <= 0.01f)
			{
				this.beaker.Pourable.LiquidContainer.SetLiquidLevel(0f, false);
				this.ProgressStep();
			}
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00057968 File Offset: 0x00055B68
		private void CheckStep_RaiseBoilingFlask()
		{
			if (this.Station.LabStand.CurrentPosition >= 0.95f)
			{
				this.Station.LabStand.SetPosition(1f);
				this.Station.LabStand.SetInteractable(false);
				this.ProgressStep();
			}
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x000579B8 File Offset: 0x00055BB8
		private void CheckStep_StartHeat()
		{
			if (this.Station.BoilingFlask.OverheatScale >= 1f)
			{
				this.Fail();
				NetworkSingleton<CombatManager>.Instance.CreateExplosion(this.Station.ExplosionPoint.transform.position, ExplosionData.DefaultSmall);
				Player.Local.Health.TakeDamage(100f, true, true);
			}
			if (this.Station.BoilingFlask.IsTemperatureInRange)
			{
				this.timeInTemperatureRange += Time.deltaTime;
			}
			else
			{
				this.timeInTemperatureRange = Mathf.Clamp(this.timeInTemperatureRange - Time.deltaTime, 0f, 2f);
			}
			if (this.timeInTemperatureRange >= 2f)
			{
				this.ProgressStep();
				this.Station.BoilingFlask.SetCanvasVisible(false);
				this.Station.Burner.SetInteractable(false);
				this.Success();
			}
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x00057AA0 File Offset: 0x00055CA0
		public override void Success()
		{
			EQuality productQuality = this.Recipe.CalculateQuality(this.RemovedIngredients.ToList<ItemInstance>());
			ChemistryCookOperation op = new ChemistryCookOperation(this.Recipe, productQuality, this.Station.BoilingFlask.LiquidContainer.LiquidVolume.liquidColor1, this.Station.BoilingFlask.LiquidContainer.CurrentLiquidLevel, 0);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Chemical_Operations_Started", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Chemical_Operations_Started") + 1f).ToString(), true);
			this.Station.CreateTrash(this.items);
			this.Station.SendCookOperation(op);
			base.Success();
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00057B54 File Offset: 0x00055D54
		public override void StopTask()
		{
			base.StopTask();
			if (this.Outcome != Task.EOutcome.Success && this.Outcome != Task.EOutcome.Fail)
			{
				for (int i = 0; i < this.RemovedIngredients.Length; i++)
				{
					if (this.RemovedIngredients[i] != null)
					{
						this.Station.IngredientSlots[i].AddItem(this.RemovedIngredients[i], false);
					}
				}
				this.Station.ResetStation();
			}
			if (this.Outcome == Task.EOutcome.Fail)
			{
				this.Station.ResetStation();
			}
			Singleton<ChemistryStationCanvas>.Instance.Open(this.Station);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			this.beaker.Destroy();
			this.Station.StaticBeaker.gameObject.SetActive(true);
			this.Station.StaticFunnel.gameObject.SetActive(true);
			this.Station.StaticStirringRod.gameObject.SetActive(true);
			this.Station.LabStand.SetPosition(1f);
			this.Station.LabStand.SetInteractable(false);
			this.Station.Burner.SetInteractable(false);
			this.Station.BoilingFlask.SetCanvasVisible(false);
			if (this.stirringRod != null)
			{
				this.stirringRod.Destroy();
			}
			foreach (StationItem stationItem in this.items)
			{
				stationItem.Destroy();
			}
			this.items.Clear();
		}

		// Token: 0x040012D9 RID: 4825
		public const float STIR_TIME = 1.5f;

		// Token: 0x040012DA RID: 4826
		public const float TEMPERATURE_TIME = 2f;

		// Token: 0x040012DE RID: 4830
		private Beaker beaker;

		// Token: 0x040012DF RID: 4831
		private StirringRod stirringRod;

		// Token: 0x040012E0 RID: 4832
		private List<StationItem> items = new List<StationItem>();

		// Token: 0x040012E1 RID: 4833
		private List<IngredientPiece> ingredientPieces = new List<IngredientPiece>();

		// Token: 0x040012E2 RID: 4834
		private float stirProgress;

		// Token: 0x040012E3 RID: 4835
		private float timeInTemperatureRange;

		// Token: 0x040012E4 RID: 4836
		private ItemInstance[] RemovedIngredients;
	}
}
