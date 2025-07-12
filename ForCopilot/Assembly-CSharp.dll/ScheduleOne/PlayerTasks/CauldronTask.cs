using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200035B RID: 859
	public class CauldronTask : Task
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x00053E2B File Offset: 0x0005202B
		// (set) Token: 0x0600134C RID: 4940 RVA: 0x00053E33 File Offset: 0x00052033
		public Cauldron Cauldron { get; private set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x00053E3C File Offset: 0x0005203C
		// (set) Token: 0x0600134E RID: 4942 RVA: 0x00053E44 File Offset: 0x00052044
		public CauldronTask.EStep CurrentStep { get; private set; }

		// Token: 0x0600134F RID: 4943 RVA: 0x00053E4D File Offset: 0x0005204D
		public static string GetStepDescription(CauldronTask.EStep step)
		{
			if (step == CauldronTask.EStep.CombineIngredients)
			{
				return "Combine leaves and gasoline in cauldron";
			}
			if (step != CauldronTask.EStep.StartMixing)
			{
				return "Unknown step";
			}
			return "Start cauldron";
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00053E6C File Offset: 0x0005206C
		public CauldronTask(Cauldron caudron)
		{
			this.Cauldron = caudron;
			this.Cauldron.onStartButtonClicked.AddListener(new UnityAction(this.StartButtonPressed));
			this.Cauldron.OverheadLight.enabled = true;
			this.ClickDetectionRadius = 0.012f;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Cauldron.CameraPosition_CombineIngredients.position, this.Cauldron.CameraPosition_CombineIngredients.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.TaskName);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			base.EnableMultiDragging(this.Cauldron.ItemContainer, 0.15f);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			this.Gasoline = UnityEngine.Object.Instantiate<StationItem>(this.Cauldron.GasolinePrefab, caudron.ItemContainer);
			this.Gasoline.transform.rotation = caudron.GasolineSpawnPoint.rotation;
			this.Gasoline.transform.position = caudron.GasolineSpawnPoint.position;
			this.Gasoline.transform.localScale = Vector3.one * 1.5f;
			this.Gasoline.ActivateModule<PourableModule>();
			this.Gasoline.GetComponentInChildren<Rigidbody>().rotation = caudron.GasolineSpawnPoint.rotation;
			this.CocaLeaves = new StationItem[20];
			for (int i = 0; i < this.CocaLeaves.Length; i++)
			{
				this.CocaLeaves[i] = UnityEngine.Object.Instantiate<StationItem>(this.Cauldron.CocaLeafPrefab, caudron.ItemContainer);
				this.CocaLeaves[i].transform.rotation = caudron.LeafSpawns[i].rotation;
				this.CocaLeaves[i].transform.position = caudron.LeafSpawns[i].position;
				this.CocaLeaves[i].ActivateModule<IngredientModule>();
				this.CocaLeaves[i].transform.localScale = Vector3.one * 0.85f;
				this.CocaLeaves[i].GetModule<IngredientModule>().Pieces[0].transform.SetParent(caudron.ItemContainer);
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0005409C File Offset: 0x0005229C
		public override void Success()
		{
			EQuality quality = this.Cauldron.RemoveIngredients();
			this.Cauldron.SendCookOperation(this.Cauldron.CookTime, quality);
			this.Cauldron.CreateTrash(new List<StationItem>
			{
				this.Gasoline
			});
			base.Success();
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x000540F0 File Offset: 0x000522F0
		public override void StopTask()
		{
			this.Cauldron.OverheadLight.enabled = false;
			this.Cauldron.onStartButtonClicked.RemoveListener(new UnityAction(this.StartButtonPressed));
			this.Cauldron.StartButtonClickable.ClickableEnabled = false;
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.TaskName);
			this.Cauldron.Open();
			foreach (StationItem stationItem in this.CocaLeaves)
			{
				UnityEngine.Object.Destroy(stationItem.GetModule<IngredientModule>().Pieces[0].gameObject);
				stationItem.Destroy();
			}
			this.Gasoline.Destroy();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Cauldron.CauldronFillable.ResetContents();
			}
			base.StopTask();
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x000541BE File Offset: 0x000523BE
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			this.UpdateInstruction();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x000541D2 File Offset: 0x000523D2
		private void CheckProgress()
		{
			if (this.CurrentStep == CauldronTask.EStep.CombineIngredients)
			{
				this.CheckStep_CombineIngredients();
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000541E4 File Offset: 0x000523E4
		private void CheckStep_CombineIngredients()
		{
			if (this.Gasoline.GetModule<PourableModule>().LiquidLevel > 0.01f)
			{
				return;
			}
			StationItem[] cocaLeaves = this.CocaLeaves;
			for (int i = 0; i < cocaLeaves.Length; i++)
			{
				if (cocaLeaves[i].GetModule<IngredientModule>().Pieces[0].CurrentLiquidContainer == null)
				{
					return;
				}
			}
			this.StartMixing();
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00054244 File Offset: 0x00052444
		private void StartMixing()
		{
			this.CurrentStep = CauldronTask.EStep.StartMixing;
			bool isHeld = this.Gasoline.GetModule<PourableModule>().Draggable.IsHeld;
			this.Gasoline.GetModule<PourableModule>().Draggable.ClickableEnabled = false;
			if (isHeld)
			{
				this.Gasoline.GetModule<PourableModule>().Draggable.Rb.AddForce(this.Cauldron.transform.right * 10f, 2);
			}
			StationItem[] cocaLeaves = this.CocaLeaves;
			for (int i = 0; i < cocaLeaves.Length; i++)
			{
				cocaLeaves[i].GetModule<IngredientModule>().Pieces[0].GetComponent<Draggable>().ClickableEnabled = false;
			}
			this.Cauldron.StartButtonClickable.ClickableEnabled = true;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Cauldron.CameraPosition_StartMachine.position, this.Cauldron.CameraPosition_StartMachine.rotation, 0.2f, false);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0005432A File Offset: 0x0005252A
		private void UpdateInstruction()
		{
			base.CurrentInstruction = CauldronTask.GetStepDescription(this.CurrentStep);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005433D File Offset: 0x0005253D
		private void StartButtonPressed()
		{
			if (this.CurrentStep == CauldronTask.EStep.StartMixing)
			{
				this.Success();
			}
		}

		// Token: 0x0400127A RID: 4730
		private StationItem[] CocaLeaves;

		// Token: 0x0400127B RID: 4731
		private StationItem Gasoline;

		// Token: 0x0400127C RID: 4732
		private Draggable Tub;

		// Token: 0x0200035C RID: 860
		public enum EStep
		{
			// Token: 0x0400127E RID: 4734
			CombineIngredients,
			// Token: 0x0400127F RID: 4735
			StartMixing
		}
	}
}
