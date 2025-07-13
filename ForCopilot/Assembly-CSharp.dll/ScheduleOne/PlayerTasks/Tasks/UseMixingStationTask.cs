using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.StationFramework;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000374 RID: 884
	public class UseMixingStationTask : Task
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x000585A5 File Offset: 0x000567A5
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x000585AD File Offset: 0x000567AD
		public MixingStation Station { get; private set; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x000585B6 File Offset: 0x000567B6
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x000585BE File Offset: 0x000567BE
		public UseMixingStationTask.EStep CurrentStep { get; private set; }

		// Token: 0x06001400 RID: 5120 RVA: 0x000585C7 File Offset: 0x000567C7
		public static string GetStepDescription(UseMixingStationTask.EStep step)
		{
			if (step == UseMixingStationTask.EStep.CombineIngredients)
			{
				return "Combine ingredients in bowl";
			}
			if (step != UseMixingStationTask.EStep.StartMixing)
			{
				return "Unknown step";
			}
			return "Start mixing machine";
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000585E4 File Offset: 0x000567E4
		public UseMixingStationTask(MixingStation station)
		{
			UseMixingStationTask.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.station = station;
			base..ctor();
			CS$<>8__locals1.<>4__this = this;
			this.Station = CS$<>8__locals1.station;
			this.Station.onStartButtonClicked.AddListener(new UnityAction(this.StartButtonPressed));
			this.ClickDetectionRadius = 0.012f;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Station.CameraPosition_CombineIngredients.position, this.Station.CameraPosition_CombineIngredients.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.TaskName);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.removedIngredients = new ItemInstance[2];
			int mixQuantity = CS$<>8__locals1.station.GetMixQuantity();
			this.removedIngredients[0] = CS$<>8__locals1.station.ProductSlot.ItemInstance.GetCopy(mixQuantity);
			this.removedIngredients[1] = CS$<>8__locals1.station.MixerSlot.ItemInstance.GetCopy(mixQuantity);
			CS$<>8__locals1.station.ProductSlot.ChangeQuantity(-mixQuantity, false);
			CS$<>8__locals1.station.MixerSlot.ChangeQuantity(-mixQuantity, false);
			base.EnableMultiDragging(CS$<>8__locals1.station.ItemContainer, 0.12f);
			int num = 0;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			for (int i = 0; i < mixQuantity; i++)
			{
				this.<.ctor>g__SetupIngredient|15_0(this.removedIngredients[1].Definition as StorableItemDefinition, num, true, ref CS$<>8__locals1);
				num++;
			}
			for (int j = 0; j < mixQuantity; j++)
			{
				this.<.ctor>g__SetupIngredient|15_0(this.removedIngredients[0].Definition as StorableItemDefinition, num, false, ref CS$<>8__locals1);
				num++;
			}
			if (this.Jug != null)
			{
				this.Jug.Pourable.LiquidCapacity_L = this.Jug.Fillable.LiquidCapacity_L;
				this.Jug.Pourable.DefaultLiquid_L = this.Jug.Fillable.GetTotalLiquidVolume();
				this.Jug.Pourable.SetLiquidLevel(this.Jug.Pourable.DefaultLiquid_L);
				this.Jug.Pourable.PourParticlesColor = this.Jug.Fillable.LiquidContainer.LiquidColor;
				this.Jug.Pourable.LiquidColor = this.Jug.Fillable.LiquidContainer.LiquidColor;
				this.Jug.Pourable.PourParticles[0].trigger.AddCollider(this.Station.BowlFillable.LiquidContainer.Collider);
				this.Jug.Fillable.FillableEnabled = false;
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x000588A4 File Offset: 0x00056AA4
		private Beaker CreateJug()
		{
			Beaker component = UnityEngine.Object.Instantiate<GameObject>(this.Station.JugPrefab, this.Station.ItemContainer).GetComponent<Beaker>();
			component.transform.position = this.Station.JugAlignment.position;
			component.transform.rotation = this.Station.JugAlignment.rotation;
			component.GetComponent<DraggableConstraint>().Container = this.Station.ItemContainer;
			component.ActivateModule<PourableModule>();
			return component;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00058923 File Offset: 0x00056B23
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			this.UpdateInstruction();
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00058938 File Offset: 0x00056B38
		private void UpdateInstruction()
		{
			base.CurrentInstruction = UseMixingStationTask.GetStepDescription(this.CurrentStep);
			if (this.CurrentStep == UseMixingStationTask.EStep.CombineIngredients)
			{
				int num = this.items.Count;
				if (this.Jug != null)
				{
					num++;
				}
				int combinedIngredients = this.GetCombinedIngredients();
				base.CurrentInstruction = string.Concat(new string[]
				{
					base.CurrentInstruction,
					" (",
					combinedIngredients.ToString(),
					"/",
					num.ToString(),
					")"
				});
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000589C9 File Offset: 0x00056BC9
		private void CheckProgress()
		{
			if (this.CurrentStep == UseMixingStationTask.EStep.CombineIngredients)
			{
				this.CheckStep_CombineIngredients();
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000589D9 File Offset: 0x00056BD9
		private void CheckStep_CombineIngredients()
		{
			if (this.GetCombinedIngredients() >= this.items.Count + ((this.Jug != null) ? 1 : 0))
			{
				this.ProgressStep();
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00058A08 File Offset: 0x00056C08
		private int GetCombinedIngredients()
		{
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].HasModule<IngredientModule>())
				{
					IngredientModule module = this.items[i].GetModule<IngredientModule>();
					bool flag = true;
					IngredientPiece[] pieces = module.Pieces;
					for (int j = 0; j < pieces.Length; j++)
					{
						if (pieces[j].CurrentLiquidContainer != this.Station.BowlFillable.LiquidContainer)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num++;
					}
				}
				else if (this.items[i].HasModule<PourableModule>() && this.items[i].GetModule<PourableModule>().NormalizedLiquidLevel <= 0.02f)
				{
					num++;
				}
			}
			if (this.Jug != null && this.Jug.Pourable.NormalizedLiquidLevel <= 0.02f)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00058AFC File Offset: 0x00056CFC
		private void ProgressStep()
		{
			UseMixingStationTask.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == UseMixingStationTask.EStep.StartMixing)
			{
				this.Station.SetStartButtonClickable(true);
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00058B2E File Offset: 0x00056D2E
		private void StartButtonPressed()
		{
			if (this.CurrentStep == UseMixingStationTask.EStep.StartMixing)
			{
				this.Success();
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00058B40 File Offset: 0x00056D40
		public override void Success()
		{
			ProductItemInstance productItemInstance = this.removedIngredients[0] as ProductItemInstance;
			string id = this.removedIngredients[1].Definition.ID;
			this.CreateTrash();
			Singleton<MixingStationCanvas>.Instance.StartMixOperation(new MixOperation(productItemInstance.ID, productItemInstance.Quality, id, productItemInstance.Quantity));
			base.Success();
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00058B9C File Offset: 0x00056D9C
		private void CreateTrash()
		{
			BoxCollider trashSpawnVolume = this.Station.TrashSpawnVolume;
			for (int i = 0; i < Mathf.CeilToInt((float)this.mixerItems.Count / 2f); i++)
			{
				if (!(this.mixerItems[0].TrashPrefab == null))
				{
					Vector3 posiiton = trashSpawnVolume.transform.TransformPoint(new Vector3(UnityEngine.Random.Range(-trashSpawnVolume.size.x / 2f, trashSpawnVolume.size.x / 2f), 0f, UnityEngine.Random.Range(-trashSpawnVolume.size.z / 2f, trashSpawnVolume.size.z / 2f)));
					Vector3 vector = trashSpawnVolume.transform.forward;
					vector = Quaternion.Euler(0f, UnityEngine.Random.Range(-45f, 45f), 0f) * vector;
					float d = UnityEngine.Random.Range(0.25f, 0.4f);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.mixerItems[0].TrashPrefab.ID, posiiton, UnityEngine.Random.rotation, vector * d, "", false);
				}
			}
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00058CD8 File Offset: 0x00056ED8
		public override void StopTask()
		{
			this.Station.onStartButtonClicked.RemoveListener(new UnityAction(this.StartButtonPressed));
			this.Station.BowlFillable.ResetContents();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Station.ProductSlot.AddItem(this.removedIngredients[0], false);
				this.Station.MixerSlot.AddItem(this.removedIngredients[1], false);
			}
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			foreach (StationItem stationItem in this.items)
			{
				stationItem.Destroy();
			}
			this.items.Clear();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.TaskName);
			this.Station.Open();
			if (this.Jug != null)
			{
				UnityEngine.Object.Destroy(this.Jug.gameObject);
			}
			base.StopTask();
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00058DE4 File Offset: 0x00056FE4
		[CompilerGenerated]
		private void <.ctor>g__SetupIngredient|15_0(StorableItemDefinition def, int index, bool mixer, ref UseMixingStationTask.<>c__DisplayClass15_0 A_4)
		{
			if (def.StationItem == null)
			{
				Console.LogError("Ingredient '" + def.Name + "' does not have a station item", null);
				return;
			}
			if (mixer)
			{
				this.mixerItems.Add(def.StationItem);
			}
			if (def.StationItem.HasModule<PourableModule>())
			{
				if (this.Jug == null)
				{
					this.Jug = this.CreateJug();
				}
				PourableModule module = def.StationItem.GetModule<PourableModule>();
				this.Jug.Fillable.AddLiquid(module.LiquidType, module.LiquidCapacity_L, module.LiquidColor);
				return;
			}
			StationItem stationItem = UnityEngine.Object.Instantiate<StationItem>(def.StationItem, A_4.station.ItemContainer);
			stationItem.transform.rotation = A_4.station.IngredientTransforms[this.items.Count].rotation;
			Vector3 eulerAngles = stationItem.transform.eulerAngles;
			eulerAngles.y = UnityEngine.Random.Range(0f, 360f);
			stationItem.transform.eulerAngles = eulerAngles;
			stationItem.transform.position = A_4.station.IngredientTransforms[this.items.Count].position;
			stationItem.Initialize(def);
			if (stationItem.HasModule<IngredientModule>())
			{
				stationItem.ActivateModule<IngredientModule>();
				foreach (IngredientPiece ingredientPiece in stationItem.GetModule<IngredientModule>().Pieces)
				{
					this.ingredientPieces.Add(ingredientPiece);
					ingredientPiece.DisableInteractionInLiquid = false;
				}
			}
			else
			{
				Console.LogError("Ingredient '" + def.Name + "' does not have an ingredient or pourable module", null);
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

		// Token: 0x040012F7 RID: 4855
		private List<StationItem> items = new List<StationItem>();

		// Token: 0x040012F8 RID: 4856
		private List<StationItem> mixerItems = new List<StationItem>();

		// Token: 0x040012F9 RID: 4857
		private List<IngredientPiece> ingredientPieces = new List<IngredientPiece>();

		// Token: 0x040012FA RID: 4858
		private ItemInstance[] removedIngredients;

		// Token: 0x040012FB RID: 4859
		private Beaker Jug;

		// Token: 0x02000375 RID: 885
		public enum EStep
		{
			// Token: 0x040012FD RID: 4861
			CombineIngredients,
			// Token: 0x040012FE RID: 4862
			StartMixing
		}
	}
}
