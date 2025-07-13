using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Vehicles.Modification
{
	// Token: 0x02000826 RID: 2086
	public class VehicleModStation : MonoBehaviour
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x000EEBBD File Offset: 0x000ECDBD
		// (set) Token: 0x060038A5 RID: 14501 RVA: 0x000EEBC5 File Offset: 0x000ECDC5
		public LandVehicle currentVehicle { get; protected set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060038A6 RID: 14502 RVA: 0x000EEBCE File Offset: 0x000ECDCE
		public bool isOpen
		{
			get
			{
				return this.currentVehicle != null;
			}
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000EEBDC File Offset: 0x000ECDDC
		public void Open(LandVehicle vehicle)
		{
			this.orbitCam.Enable();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.currentVehicle = vehicle;
			vehicle.transform.rotation = this.vehiclePosition.rotation;
			vehicle.transform.position = this.vehiclePosition.position;
			vehicle.transform.position -= vehicle.transform.InverseTransformPoint(vehicle.boundingBox.transform.position);
			vehicle.transform.position += Vector3.up * vehicle.boundingBox.transform.localScale.y * 0.5f;
			Singleton<VehicleModMenu>.Instance.Open(this.currentVehicle);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000EECED File Offset: 0x000ECEED
		protected virtual void Update()
		{
			if (this.isOpen && GameInput.GetButtonDown(GameInput.ButtonCode.Escape))
			{
				this.Close();
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000EED06 File Offset: 0x000ECF06
		public void Close()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.orbitCam.Disable();
			Singleton<VehicleModMenu>.Instance.Close();
			this.currentVehicle = null;
		}

		// Token: 0x04002891 RID: 10385
		[Header("References")]
		[SerializeField]
		protected Transform vehiclePosition;

		// Token: 0x04002892 RID: 10386
		[SerializeField]
		protected OrbitCamera orbitCam;
	}
}
