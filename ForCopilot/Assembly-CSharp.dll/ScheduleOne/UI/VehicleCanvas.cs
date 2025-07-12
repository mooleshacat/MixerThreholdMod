using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A95 RID: 2709
	public class VehicleCanvas : Singleton<VehicleCanvas>
	{
		// Token: 0x060048C0 RID: 18624 RVA: 0x00131338 File Offset: 0x0012F538
		protected override void Start()
		{
			base.Start();
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Subscribe));
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x00131360 File Offset: 0x0012F560
		private void Subscribe()
		{
			Player local = Player.Local;
			local.onEnterVehicle = (Player.VehicleEvent)Delegate.Combine(local.onEnterVehicle, new Player.VehicleEvent(this.VehicleEntered));
			Player local2 = Player.Local;
			local2.onExitVehicle = (Player.VehicleTransformEvent)Delegate.Combine(local2.onExitVehicle, new Player.VehicleTransformEvent(this.VehicleExited));
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x001313B9 File Offset: 0x0012F5B9
		private void Update()
		{
			if (Player.Local == null)
			{
				return;
			}
			if (Player.Local.CurrentVehicle != null)
			{
				this.Canvas.enabled = !Singleton<GameplayMenu>.Instance.IsOpen;
			}
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x001313F3 File Offset: 0x0012F5F3
		private void LateUpdate()
		{
			if (this.currentVehicle != null)
			{
				this.UpdateSpeedText();
			}
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x00131409 File Offset: 0x0012F609
		private void VehicleEntered(LandVehicle veh)
		{
			this.currentVehicle = veh;
			this.UpdateSpeedText();
			this.Canvas.enabled = true;
			this.DriverPromptsContainer.SetActive(this.currentVehicle.localPlayerIsDriver);
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x0013143A File Offset: 0x0012F63A
		private void VehicleExited(LandVehicle veh, Transform exitPoint)
		{
			this.Canvas.enabled = false;
			this.currentVehicle = null;
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x00131450 File Offset: 0x0012F650
		private void UpdateSpeedText()
		{
			if (this.SpeedText == null)
			{
				return;
			}
			if (Singleton<Settings>.Instance.unitType == Settings.UnitType.Metric)
			{
				this.SpeedText.text = Mathf.Abs(this.currentVehicle.VelocityCalculator.Velocity.magnitude * 3.6f * 1.4f).ToString("0") + " km/h";
				return;
			}
			this.SpeedText.text = Mathf.Abs(this.currentVehicle.VelocityCalculator.Velocity.magnitude * 2.23694f * 1.4f).ToString("0") + " mph";
		}

		// Token: 0x04003568 RID: 13672
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003569 RID: 13673
		public TextMeshProUGUI SpeedText;

		// Token: 0x0400356A RID: 13674
		public GameObject DriverPromptsContainer;

		// Token: 0x0400356B RID: 13675
		private LandVehicle currentVehicle;
	}
}
