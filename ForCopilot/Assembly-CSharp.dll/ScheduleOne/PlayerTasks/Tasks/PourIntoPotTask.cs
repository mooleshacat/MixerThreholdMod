using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000370 RID: 880
	public class PourIntoPotTask : Task
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x00057CF0 File Offset: 0x00055EF0
		// (set) Token: 0x060013E2 RID: 5090 RVA: 0x00057CF8 File Offset: 0x00055EF8
		public override string TaskName { get; protected set; } = "Pour";

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00057D01 File Offset: 0x00055F01
		protected virtual bool UseCoverage { get; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00057D09 File Offset: 0x00055F09
		protected virtual bool FailOnEmpty { get; } = 1;

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00057D11 File Offset: 0x00055F11
		protected virtual Pot.ECameraPosition CameraPosition { get; } = 1;

		// Token: 0x060013E6 RID: 5094 RVA: 0x00057D1C File Offset: 0x00055F1C
		public PourIntoPotTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab)
		{
			if (_pot == null)
			{
				Console.LogWarning("PourIntoPotTask: pot null", null);
				this.StopTask();
				return;
			}
			if (_pourablePrefab == null)
			{
				Console.LogWarning("PourIntoPotTask: pourablePrefab null", null);
				this.StopTask();
				return;
			}
			this.ClickDetectionEnabled = true;
			this.item = _itemInstance;
			this.pot = _pot;
			if (this.pot.Plant != null)
			{
				this.pot.Plant.SetVisible(false);
			}
			this.pot.SetPlayerUser(Player.Local.NetworkObject);
			this.pot.PositionCameraContainer();
			Transform cameraPosition = this.pot.GetCameraPosition(this.CameraPosition);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(cameraPosition.position, cameraPosition.rotation, 0.25f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.pourable = UnityEngine.Object.Instantiate<GameObject>(_pourablePrefab.gameObject, NetworkSingleton<GameManager>.Instance.Temp).GetComponent<Pourable>();
			this.pourable.transform.position = this.pot.PourableStartPoint.position;
			this.pourable.Rb.position = this.pot.PourableStartPoint.position;
			this.pourable.Origin = this.pot.PourableStartPoint.position;
			this.pourable.MaxDistanceFromOrigin = 0.5f;
			this.pourable.LocationRestrictionEnabled = true;
			this.pourable.TargetPot = _pot;
			Pourable pourable = this.pourable;
			pourable.onInitialPour = (Action)Delegate.Combine(pourable.onInitialPour, new Action(this.OnInitialPour));
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.position - this.pourable.transform.position;
			this.pourable.transform.rotation = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z), Vector3.up);
			this.pourable.Rb.rotation = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z), Vector3.up);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("pourable");
			if (this.UseCoverage)
			{
				this.pot.SoilCover.Reset();
				this.pot.SoilCover.gameObject.SetActive(true);
				this.pot.SoilCover.onSufficientCoverage.AddListener(new UnityAction(this.FullyCovered));
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00057FE7 File Offset: 0x000561E7
		public override void Update()
		{
			base.Update();
			if (this.FailOnEmpty && this.pourable.currentQuantity <= 0f)
			{
				this.Fail();
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00058010 File Offset: 0x00056210
		public override void StopTask()
		{
			base.StopTask();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			UnityEngine.Object.Destroy(this.pourable.gameObject);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.UseCoverage)
			{
				this.pot.SoilCover.onSufficientCoverage.RemoveListener(new UnityAction(this.FullyCovered));
				this.pot.SoilCover.gameObject.SetActive(false);
			}
			if (this.pot.Plant != null)
			{
				this.pot.Plant.SetVisible(true);
			}
			this.pot.SetPlayerUser(null);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000580ED File Offset: 0x000562ED
		private void OnInitialPour()
		{
			if (this.removeItemAfterInitialPour)
			{
				this.RemoveItem();
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00058100 File Offset: 0x00056300
		protected void RemoveItem()
		{
			PlayerSingleton<PlayerInventory>.Instance.RemoveAmountOfItem(this.item.ID, 1U);
			if (this.pourable.TrashItem != null)
			{
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.pourable.TrashItem.ID, Player.Local.Avatar.transform.position + Vector3.up * 0.3f, UnityEngine.Random.rotation, default(Vector3), "", false);
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void FullyCovered()
		{
		}

		// Token: 0x040012E6 RID: 4838
		protected Pot pot;

		// Token: 0x040012E7 RID: 4839
		protected ItemInstance item;

		// Token: 0x040012E8 RID: 4840
		protected Pourable pourable;

		// Token: 0x040012EC RID: 4844
		protected bool removeItemAfterInitialPour;
	}
}
