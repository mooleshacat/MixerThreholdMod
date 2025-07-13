using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Water;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200035D RID: 861
	public class FillWateringCan : Task
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x0005434E File Offset: 0x0005254E
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x00054356 File Offset: 0x00052556
		public new string TaskName { get; protected set; } = "Fill watering can";

		// Token: 0x0600135B RID: 4955 RVA: 0x00054360 File Offset: 0x00052560
		public FillWateringCan(Tap _tap, WateringCanInstance _instance)
		{
			this.tap = _tap;
			this.instance = _instance;
			this.ClickDetectionEnabled = true;
			this.tap.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.tap.CameraPos.position, this.tap.CameraPos.rotation, 0.25f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			base.CurrentInstruction = "Click and hold tap to refill watering can";
			this.visuals = this.tap.CreateWateringCanModel_Local(this.instance.ID, true).GetComponent<WateringCanVisuals>();
			this.visuals.SetFillLevel(this.instance.CurrentFillAmount / 15f);
			this.visuals.FillSound.VolumeMultiplier = 0f;
			this.tap.SendWateringCanModel(this.instance.ID);
			this.tap.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.HandleClickStart));
			this.tap.HandleClickable.onClickEnd.AddListener(new UnityAction(this.HandleClickEnd));
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000544C4 File Offset: 0x000526C4
		public override void Update()
		{
			base.Update();
			if (this.tap.ActualFlowRate > 0f)
			{
				this.instance.ChangeFillAmount(this.tap.ActualFlowRate * Time.deltaTime);
				if (!this.visuals.FillSound.isPlaying && !this.audioPlayed)
				{
					this.visuals.FillSound.Play();
					this.audioPlayed = true;
				}
				this.visuals.FillSound.VolumeMultiplier = Mathf.MoveTowards(this.visuals.FillSound.VolumeMultiplier, 1f, Time.deltaTime * 4f);
			}
			else
			{
				this.audioPlayed = false;
				if (this.visuals.FillSound.isPlaying)
				{
					this.visuals.FillSound.VolumeMultiplier = Mathf.MoveTowards(this.visuals.FillSound.VolumeMultiplier, 0f, Time.deltaTime * 4f);
					if (this.visuals.FillSound.VolumeMultiplier <= 0f)
					{
						this.visuals.FillSound.Stop();
					}
				}
			}
			this.visuals.SetFillLevel(this.instance.CurrentFillAmount / 15f);
			if (this.instance.CurrentFillAmount >= 15f)
			{
				this.Success();
				return;
			}
			if (this.tap.ActualFlowRate > 0f && this.instance.CurrentFillAmount >= 15f)
			{
				this.visuals.SetOverflowParticles(true);
				return;
			}
			this.visuals.SetOverflowParticles(false);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00054658 File Offset: 0x00052858
		public override void StopTask()
		{
			this.tap.SetHeldOpen(false);
			this.tap.SetPlayerUser(null);
			this.tap.SendClearWateringCanModelModel();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.25f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			UnityEngine.Object.Destroy(this.visuals.gameObject);
			base.StopTask();
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000546DE File Offset: 0x000528DE
		private void HandleClickStart(RaycastHit hit)
		{
			this.tap.SetHeldOpen(true);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000546EC File Offset: 0x000528EC
		private void HandleClickEnd()
		{
			this.tap.SetHeldOpen(false);
		}

		// Token: 0x04001281 RID: 4737
		protected Tap tap;

		// Token: 0x04001282 RID: 4738
		protected WateringCanInstance instance;

		// Token: 0x04001283 RID: 4739
		protected WateringCanVisuals visuals;

		// Token: 0x04001284 RID: 4740
		private bool audioPlayed;
	}
}
