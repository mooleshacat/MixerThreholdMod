using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200073C RID: 1852
	public class VehicleSelector : Singleton<VehicleSelector>
	{
		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x000D08EB File Offset: 0x000CEAEB
		// (set) Token: 0x06003204 RID: 12804 RVA: 0x000D08F3 File Offset: 0x000CEAF3
		public bool isSelecting { get; protected set; }

		// Token: 0x06003205 RID: 12805 RVA: 0x000D08FC File Offset: 0x000CEAFC
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 8);
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x000D0918 File Offset: 0x000CEB18
		protected virtual void Update()
		{
			if (this.isSelecting)
			{
				this.hoveredVehicle = this.GetHoveredVehicle();
				if (this.hoveredVehicle != null)
				{
					Singleton<HUD>.Instance.ShowRadialIndicator(1f);
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.hoveredVehicle != null && (this.vehicleFilter == null || this.vehicleFilter(this.hoveredVehicle)))
				{
					if (this.selectedVehicles.Contains(this.hoveredVehicle))
					{
						Console.Log("Deselected: " + this.hoveredVehicle.VehicleName, null);
						this.selectedVehicles.Remove(this.hoveredVehicle);
						return;
					}
					if (this.selectedVehicles.Count < this.selectionLimit)
					{
						this.selectedVehicles.Add(this.hoveredVehicle);
						if (this.selectedVehicles.Count >= this.selectionLimit && this.exitOnSelectionLimit)
						{
							this.StopSelecting();
						}
					}
				}
			}
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x000D0A1C File Offset: 0x000CEC1C
		protected virtual void LateUpdate()
		{
			if (this.isSelecting)
			{
				for (int i = 0; i < this.outlinedVehicles.Count; i++)
				{
					this.outlinedVehicles[i].HideOutline();
				}
				this.outlinedVehicles.Clear();
				for (int j = 0; j < this.selectedVehicles.Count; j++)
				{
					this.selectedVehicles[j].ShowOutline(BuildableItem.EOutlineColor.Blue);
					this.outlinedVehicles.Add(this.selectedVehicles[j]);
				}
				if (this.hoveredVehicle != null)
				{
					if (this.selectedVehicles.Contains(this.hoveredVehicle))
					{
						this.hoveredVehicle.ShowOutline(BuildableItem.EOutlineColor.LightBlue);
						return;
					}
					this.hoveredVehicle.ShowOutline(BuildableItem.EOutlineColor.White);
					this.outlinedVehicles.Add(this.hoveredVehicle);
				}
			}
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000D0AF0 File Offset: 0x000CECF0
		private LandVehicle GetHoveredVehicle()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0.1f))
			{
				LandVehicle componentInParent = raycastHit.collider.GetComponentInParent<LandVehicle>();
				if (componentInParent != null && (this.vehicleFilter == null || this.vehicleFilter(componentInParent)))
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000D0B4C File Offset: 0x000CED4C
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.isSelecting)
			{
				action.Used = true;
				this.StopSelecting();
			}
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x000D0B78 File Offset: 0x000CED78
		public void StartSelecting(string selectionTitle, ref List<LandVehicle> initialSelection, int _selectionLimit, bool _exitOnSelectionLimit, Func<LandVehicle, bool> filter = null)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.selectedVehicles = initialSelection;
			for (int i = 0; i < this.selectedVehicles.Count; i++)
			{
				this.selectedVehicles[i].ShowOutline(BuildableItem.EOutlineColor.White);
				this.outlinedVehicles.Add(this.selectedVehicles[i]);
			}
			this.selectionLimit = _selectionLimit;
			this.vehicleFilter = filter;
			Singleton<HUD>.Instance.ShowTopScreenText(selectionTitle);
			this.isSelecting = true;
			this.exitOnSelectionLimit = _exitOnSelectionLimit;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000D0C08 File Offset: 0x000CEE08
		public void StopSelecting()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.vehicleFilter = null;
			for (int i = 0; i < this.outlinedVehicles.Count; i++)
			{
				this.outlinedVehicles[i].HideOutline();
			}
			this.outlinedVehicles.Clear();
			if (this.onClose != null)
			{
				this.onClose();
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			this.isSelecting = false;
		}

		// Token: 0x04002335 RID: 9013
		[Header("Settings")]
		[SerializeField]
		protected float detectionRange = 5f;

		// Token: 0x04002336 RID: 9014
		[SerializeField]
		protected LayerMask detectionMask;

		// Token: 0x04002338 RID: 9016
		private List<LandVehicle> selectedVehicles = new List<LandVehicle>();

		// Token: 0x04002339 RID: 9017
		public Action onClose;

		// Token: 0x0400233A RID: 9018
		private int selectionLimit;

		// Token: 0x0400233B RID: 9019
		private bool exitOnSelectionLimit;

		// Token: 0x0400233C RID: 9020
		private LandVehicle hoveredVehicle;

		// Token: 0x0400233D RID: 9021
		private List<LandVehicle> outlinedVehicles = new List<LandVehicle>();

		// Token: 0x0400233E RID: 9022
		private Func<LandVehicle, bool> vehicleFilter;
	}
}
