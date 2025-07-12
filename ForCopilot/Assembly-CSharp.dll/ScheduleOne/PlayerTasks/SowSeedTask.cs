using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000367 RID: 871
	public class SowSeedTask : Task
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00055F25 File Offset: 0x00054125
		// (set) Token: 0x0600139A RID: 5018 RVA: 0x00055F2D File Offset: 0x0005412D
		public override string TaskName { get; protected set; } = "Sow seed";

		// Token: 0x0600139B RID: 5019 RVA: 0x00055F38 File Offset: 0x00054138
		public SowSeedTask(Pot _pot, SeedDefinition def)
		{
			if (_pot == null)
			{
				Console.LogWarning("PourIntoPotTask: pot null", null);
				this.StopTask();
				return;
			}
			if (def == null)
			{
				Console.LogWarning("SowSeedTask: seed definition null", null);
				this.StopTask();
				return;
			}
			this.ClickDetectionEnabled = true;
			this.pot = _pot;
			this.pot.TaskBounds.gameObject.SetActive(true);
			this.definition = def;
			this.pot.SetPlayerUser(Player.Local.NetworkObject);
			base.CurrentInstruction = "Click cap to remove";
			this.pot.PositionCameraContainer();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.pot.CloseupPosition.position, this.pot.CloseupPosition.rotation, 0.25f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.seed = UnityEngine.Object.Instantiate<FunctionalSeed>(def.FunctionSeedPrefab, GameObject.Find("_Temp").transform).GetComponent<FunctionalSeed>();
			this.seed.transform.position = this.pot.SeedStartPoint.position;
			Vector3 forward = PlayerSingleton<PlayerCamera>.Instance.transform.position - this.seed.transform.position;
			forward.y = 0f;
			Vector3 position = this.pot.SeedStartPoint.position;
			Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
			this.seed.Vial.transform.position = position;
			this.seed.Vial.transform.rotation = rotation;
			this.seed.Vial.Rb.position = position;
			this.seed.Vial.Rb.rotation = rotation;
			Vector3 position2 = this.pot.SeedStartPoint.position + Vector3.down * 0.05337f;
			this.seed.SeedRigidbody.transform.position = position2;
			this.seed.SeedRigidbody.position = position2;
			FunctionalSeed functionalSeed = this.seed;
			functionalSeed.onSeedExitVial = (Action)Delegate.Combine(functionalSeed.onSeedExitVial, new Action(this.OnSeedExitVial));
			this.seed.Vial.Rb.isKinematic = false;
			this.seed.SeedRigidbody.isKinematic = false;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("pourable");
			this.pot.SetSoilState(Pot.ESoilState.Parted);
			SoilChunk[] soilChunks = this.pot.SoilChunks;
			for (int i = 0; i < soilChunks.Length; i++)
			{
				soilChunks[i].ClickableEnabled = false;
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00056214 File Offset: 0x00054414
		public override void Update()
		{
			base.Update();
			if (this.seedExitedVial && !this.seedReachedDestination && this.capRemoved)
			{
				this.seed.Vial.idleUpForce = 0f;
				if (this.seed.SeedRigidbody.velocity.magnitude < 0.08f)
				{
					this.weedSeedStationaryTime += Time.deltaTime;
				}
				else
				{
					this.weedSeedStationaryTime = 0f;
				}
				if (this.weedSeedStationaryTime > 0.2f && Vector3.Distance(this.seed.SeedCollider.transform.position, this.pot.SeedRestingPoint.position) < 0.1f)
				{
					this.OnSeedReachedDestination();
				}
			}
			if (!this.capRemoved)
			{
				if (this.seed.Cap.Removed)
				{
					this.capRemoved = true;
				}
			}
			else
			{
				base.CurrentInstruction = "Drop seed into hole";
			}
			this.seed.SeedBlocker.enabled = !this.capRemoved;
			if (this.seedReachedDestination)
			{
				int num = 0;
				SoilChunk[] soilChunks = this.pot.SoilChunks;
				for (int i = 0; i < soilChunks.Length; i++)
				{
					if (soilChunks[i].CurrentLerp > 0f)
					{
						num++;
					}
				}
				base.CurrentInstruction = string.Concat(new string[]
				{
					"Click soil chunks to bury seed (",
					num.ToString(),
					"/",
					this.pot.SoilChunks.Length.ToString(),
					")"
				});
				if (num == this.pot.SoilChunks.Length)
				{
					this.Success();
				}
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000563C4 File Offset: 0x000545C4
		public override void Success()
		{
			this.successfullyPlanted = true;
			PlayerSingleton<PlayerInventory>.Instance.RemoveAmountOfItem(this.definition.ID, 1U);
			NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.seed.TrashPrefab.ID, Player.Local.Avatar.CenterPoint, UnityEngine.Random.rotation, default(Vector3), "", false);
			this.pot.SendPlantSeed(this.definition.ID, 0f, -1f, -1f);
			this.pot.SetSoilState(Pot.ESoilState.Packed);
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("SownSeedsCount");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("SownSeedsCount", (value + 1f).ToString(), true);
			base.Success();
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00056494 File Offset: 0x00054694
		public override void StopTask()
		{
			base.StopTask();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.25f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			UnityEngine.Object.Destroy(this.seed.gameObject);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (!this.successfullyPlanted)
			{
				this.pot.SetSoilState(Pot.ESoilState.Flat);
			}
			foreach (SoilChunk soilChunk in this.pot.SoilChunks)
			{
				soilChunk.StopLerp();
				soilChunk.ClickableEnabled = false;
			}
			this.pot.TaskBounds.gameObject.SetActive(false);
			this.pot.SetPlayerUser(null);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00056560 File Offset: 0x00054760
		private void OnSeedExitVial()
		{
			this.seedExitedVial = true;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0005656C File Offset: 0x0005476C
		private void OnSeedReachedDestination()
		{
			this.seedReachedDestination = true;
			this.seed.SeedCollider.GetComponent<Rigidbody>().isKinematic = true;
			this.seed.SeedCollider.GetComponent<Draggable>().enabled = false;
			this.seed.Vial.gameObject.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(50f, 0.25f);
			SoilChunk[] soilChunks = this.pot.SoilChunks;
			for (int i = 0; i < soilChunks.Length; i++)
			{
				soilChunks[i].ClickableEnabled = true;
			}
		}

		// Token: 0x040012AF RID: 4783
		protected Pot pot;

		// Token: 0x040012B0 RID: 4784
		protected SeedDefinition definition;

		// Token: 0x040012B1 RID: 4785
		protected FunctionalSeed seed;

		// Token: 0x040012B2 RID: 4786
		private bool seedExitedVial;

		// Token: 0x040012B3 RID: 4787
		private bool seedReachedDestination;

		// Token: 0x040012B4 RID: 4788
		private bool successfullyPlanted;

		// Token: 0x040012B5 RID: 4789
		private float weedSeedStationaryTime;

		// Token: 0x040012B6 RID: 4790
		private bool capRemoved;
	}
}
