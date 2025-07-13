using System;
using System.Collections.Generic;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000807 RID: 2055
	[RequireComponent(typeof(Rigidbody))]
	public class ObstructionDetector : MonoBehaviour
	{
		// Token: 0x06003806 RID: 14342 RVA: 0x000EC1DB File Offset: 0x000EA3DB
		protected virtual void Awake()
		{
			this.vehicle = base.gameObject.GetComponentInParent<LandVehicle>();
			this.range = base.transform.Find("Collider").transform.localScale.z;
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000EC214 File Offset: 0x000EA414
		protected virtual void FixedUpdate()
		{
			this.closestObstructionDistance = float.MaxValue;
			for (int i = 0; i < this.vehicles.Count; i++)
			{
				if (Vector3.Distance(base.transform.position, this.vehicles[i].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.vehicles[i].transform.position);
				}
			}
			for (int j = 0; j < this.npcs.Count; j++)
			{
				if (Vector3.Distance(base.transform.position, this.npcs[j].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.npcs[j].transform.position);
				}
			}
			for (int k = 0; k < this.players.Count; k++)
			{
				if (Vector3.Distance(base.transform.position, this.players[k].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.players[k].transform.position);
				}
			}
			for (int l = 0; l < this.vehicleObstacles.Count; l++)
			{
				if (Vector3.Distance(base.transform.position, this.vehicleObstacles[l].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.vehicleObstacles[l].transform.position);
				}
			}
			this.vehicles.Clear();
			this.npcs.Clear();
			this.players.Clear();
			this.vehicleObstacles.Clear();
			float num = this.closestObstructionDistance;
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x000EC428 File Offset: 0x000EA628
		private void OnTriggerStay(Collider other)
		{
			LandVehicle componentInParent = other.GetComponentInParent<LandVehicle>();
			NPC componentInParent2 = other.GetComponentInParent<NPC>();
			PlayerMovement componentInParent3 = other.GetComponentInParent<PlayerMovement>();
			VehicleObstacle componentInParent4 = other.GetComponentInParent<VehicleObstacle>();
			if (componentInParent != null && componentInParent != this.vehicle && !this.vehicles.Contains(componentInParent))
			{
				this.vehicles.Add(componentInParent);
			}
			if (componentInParent2 != null && !this.npcs.Contains(componentInParent2))
			{
				this.npcs.Add(componentInParent2);
			}
			if (componentInParent3 != null && !this.players.Contains(componentInParent3))
			{
				this.players.Add(componentInParent3);
			}
			if (componentInParent4 != null && (componentInParent4.twoSided || Vector3.Angle(-componentInParent4.transform.forward, base.transform.forward) < 90f) && !this.vehicleObstacles.Contains(componentInParent4))
			{
				this.vehicleObstacles.Add(componentInParent4);
			}
		}

		// Token: 0x040027CB RID: 10187
		private LandVehicle vehicle;

		// Token: 0x040027CC RID: 10188
		public List<LandVehicle> vehicles = new List<LandVehicle>();

		// Token: 0x040027CD RID: 10189
		public List<NPC> npcs = new List<NPC>();

		// Token: 0x040027CE RID: 10190
		public List<PlayerMovement> players = new List<PlayerMovement>();

		// Token: 0x040027CF RID: 10191
		public List<VehicleObstacle> vehicleObstacles = new List<VehicleObstacle>();

		// Token: 0x040027D0 RID: 10192
		public float closestObstructionDistance;

		// Token: 0x040027D1 RID: 10193
		public float range;
	}
}
