using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.GameTime;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200073B RID: 1851
	[RequireComponent(typeof(Rigidbody))]
	public class VehicleDetector : MonoBehaviour
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x000D05C6 File Offset: 0x000CE7C6
		// (set) Token: 0x060031F6 RID: 12790 RVA: 0x000D05CE File Offset: 0x000CE7CE
		public bool IgnoreNewDetections { get; protected set; }

		// Token: 0x060031F7 RID: 12791 RVA: 0x000D05D8 File Offset: 0x000CE7D8
		private void Awake()
		{
			Rigidbody rigidbody = base.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			this.detectionColliders = base.GetComponentsInChildren<Collider>();
			rigidbody.isKinematic = true;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000D0614 File Offset: 0x000CE814
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onTick = (Action)Delegate.Combine(instance.onTick, new Action(this.MinPass));
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000D063C File Offset: 0x000CE83C
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onTick = (Action)Delegate.Remove(instance.onTick, new Action(this.MinPass));
			}
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000D066C File Offset: 0x000CE86C
		private void OnTriggerEnter(Collider other)
		{
			if (this.IgnoreNewDetections)
			{
				return;
			}
			LandVehicle componentInParent = other.GetComponentInParent<LandVehicle>();
			if (componentInParent != null && other == componentInParent.boundingBox && !this.vehicles.Contains(componentInParent))
			{
				this.vehicles.Add(componentInParent);
				this.SortVehicles();
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x000D06C0 File Offset: 0x000CE8C0
		private void MinPass()
		{
			bool flag = false;
			for (int i = 0; i < NetworkSingleton<VehicleManager>.Instance.AllVehicles.Count; i++)
			{
				if (Vector3.SqrMagnitude(NetworkSingleton<VehicleManager>.Instance.AllVehicles[i].transform.position - base.transform.position) < 400f)
				{
					flag = true;
					break;
				}
			}
			if (flag != this.collidersEnabled)
			{
				this.collidersEnabled = flag;
				for (int j = 0; j < this.detectionColliders.Length; j++)
				{
					this.detectionColliders[j].enabled = this.collidersEnabled;
				}
			}
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000D075C File Offset: 0x000CE95C
		private void OnTriggerExit(Collider other)
		{
			if (this.ignoreExit)
			{
				return;
			}
			LandVehicle componentInParent = other.GetComponentInParent<LandVehicle>();
			if (componentInParent != null && other == componentInParent.boundingBox && this.vehicles.Contains(componentInParent))
			{
				this.vehicles.Remove(componentInParent);
				this.SortVehicles();
			}
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000D07B4 File Offset: 0x000CE9B4
		private void SortVehicles()
		{
			if (this.vehicles.Count > 1)
			{
				from x in this.vehicles
				orderby Vector3.Distance(base.transform.position, x.transform.position)
				select x;
			}
			if (this.vehicles.Count > 0)
			{
				this.closestVehicle = this.vehicles[0];
				return;
			}
			this.closestVehicle = null;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000D0810 File Offset: 0x000CEA10
		public void SetIgnoreNewCollisions(bool ignore)
		{
			this.IgnoreNewDetections = ignore;
			if (!ignore)
			{
				this.ignoreExit = true;
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].isTrigger)
					{
						componentsInChildren[i].enabled = false;
						componentsInChildren[i].enabled = true;
					}
				}
				this.ignoreExit = false;
			}
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x000D0868 File Offset: 0x000CEA68
		public bool AreAnyVehiclesOccupied()
		{
			for (int i = 0; i < this.vehicles.Count; i++)
			{
				if (this.vehicles[i].isOccupied)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x000D08A1 File Offset: 0x000CEAA1
		public void Clear()
		{
			this.vehicles.Clear();
			this.SortVehicles();
		}

		// Token: 0x0400232E RID: 9006
		public const float ACTIVATION_DISTANCE_SQ = 400f;

		// Token: 0x0400232F RID: 9007
		public List<LandVehicle> vehicles = new List<LandVehicle>();

		// Token: 0x04002330 RID: 9008
		public LandVehicle closestVehicle;

		// Token: 0x04002332 RID: 9010
		private bool ignoreExit;

		// Token: 0x04002333 RID: 9011
		private Collider[] detectionColliders;

		// Token: 0x04002334 RID: 9012
		private bool collidersEnabled = true;
	}
}
