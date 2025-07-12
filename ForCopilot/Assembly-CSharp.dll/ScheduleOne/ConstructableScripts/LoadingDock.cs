using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x0200096A RID: 2410
	public class LoadingDock : Constructable_GridBased
	{
		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x00112DDA File Offset: 0x00110FDA
		public bool isOccupied
		{
			get
			{
				return this.vehicleDetector.vehicles.Count > 0;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06004104 RID: 16644 RVA: 0x00112DEF File Offset: 0x00110FEF
		// (set) Token: 0x06004105 RID: 16645 RVA: 0x00112DF7 File Offset: 0x00110FF7
		public LandVehicle reservant { get; protected set; }

		// Token: 0x06004106 RID: 16646 RVA: 0x00112E00 File Offset: 0x00111000
		private void Start()
		{
			this.reservationBlocker.gameObject.SetActive(false);
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x00112E14 File Offset: 0x00111014
		protected virtual void Update()
		{
			if (this.vehicleDetector.vehicles.Count > 0 && !this.vehicleDetector.closestVehicle.isOccupied)
			{
				this.wallsOpen = true;
			}
			else
			{
				this.wallsOpen = false;
			}
			bool isOccupied = this.isOccupied;
			if (this.vehicleDetector.closestVehicle != null)
			{
				if (this.currentOccupant != this.vehicleDetector.closestVehicle && this.currentOccupant != null)
				{
					this.currentOccupant = this.vehicleDetector.closestVehicle;
					return;
				}
			}
			else if (this.currentOccupant != null)
			{
				this.currentOccupant = null;
			}
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x00112EC0 File Offset: 0x001110C0
		protected virtual void LateUpdate()
		{
			if (this.isOccupied)
			{
				MeshRenderer[] array = this.redLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.redLightMat_On;
				}
				array = this.greenLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.greenLightMat_Off;
				}
			}
			else
			{
				MeshRenderer[] array = this.redLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.redLightMat_Off;
				}
				array = this.greenLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.greenLightMat_On;
				}
			}
			float max = 0.387487f;
			float min = -0.35f;
			if (this.wallsOpen)
			{
				foreach (Transform transform in this.sideWalls)
				{
					transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, Mathf.Clamp(transform.transform.localPosition.y - Time.deltaTime, min, max), transform.transform.localPosition.z);
				}
				return;
			}
			foreach (Transform transform2 in this.sideWalls)
			{
				transform2.transform.localPosition = new Vector3(transform2.transform.localPosition.x, Mathf.Clamp(transform2.transform.localPosition.y + Time.deltaTime, min, max), transform2.transform.localPosition.z);
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00113054 File Offset: 0x00111254
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.reservant != null)
			{
				reason = "Reserved for dealer";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00113074 File Offset: 0x00111274
		public override void DestroyConstructable(bool callOnServer = true)
		{
			if (this.isOccupied && this.vehicleDetector.closestVehicle != null)
			{
				this.vehicleDetector.closestVehicle.Rb.isKinematic = false;
			}
			base.DestroyConstructable(callOnServer);
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x001130B0 File Offset: 0x001112B0
		public void SetReservant(LandVehicle _res)
		{
			if (this.reservant != null)
			{
				Collider[] componentsInChildren = this.reservant.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Physics.IgnoreCollision(componentsInChildren[i], this.reservationBlocker, false);
				}
			}
			this.reservant = _res;
			if (this.reservant != null)
			{
				this.gateAnim.Play("LoadingDock_Gate_Close");
			}
			else
			{
				this.gateAnim.Play("LoadingDock_Gate_Open");
			}
			if (this.reservant != null)
			{
				Collider[] componentsInChildren2 = this.reservant.GetComponentsInChildren<Collider>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Physics.IgnoreCollision(componentsInChildren2[j], this.reservationBlocker, true);
				}
			}
			this.reservationBlocker.gameObject.SetActive(this.reservant != null);
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00113180 File Offset: 0x00111380
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x00113199 File Offset: 0x00111399
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x001131B2 File Offset: 0x001113B2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x001131C0 File Offset: 0x001113C0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002E68 RID: 11880
		[Header("References")]
		[SerializeField]
		protected VehicleDetector vehicleDetector;

		// Token: 0x04002E69 RID: 11881
		[SerializeField]
		protected MeshRenderer[] redLightMeshes;

		// Token: 0x04002E6A RID: 11882
		[SerializeField]
		protected MeshRenderer[] greenLightMeshes;

		// Token: 0x04002E6B RID: 11883
		[SerializeField]
		protected Transform[] sideWalls;

		// Token: 0x04002E6C RID: 11884
		[SerializeField]
		protected Animation gateAnim;

		// Token: 0x04002E6D RID: 11885
		[SerializeField]
		protected Collider reservationBlocker;

		// Token: 0x04002E6E RID: 11886
		public Transform vehiclePosition;

		// Token: 0x04002E6F RID: 11887
		[Header("Materials")]
		[SerializeField]
		protected Material redLightMat_On;

		// Token: 0x04002E70 RID: 11888
		[SerializeField]
		protected Material redLightMat_Off;

		// Token: 0x04002E71 RID: 11889
		[SerializeField]
		protected Material greenLightMat_On;

		// Token: 0x04002E72 RID: 11890
		[SerializeField]
		protected Material greenLightMat_Off;

		// Token: 0x04002E73 RID: 11891
		private bool wallsOpen;

		// Token: 0x04002E74 RID: 11892
		private LandVehicle currentOccupant;

		// Token: 0x04002E76 RID: 11894
		private bool dll_Excuted;

		// Token: 0x04002E77 RID: 11895
		private bool dll_Excuted;
	}
}
