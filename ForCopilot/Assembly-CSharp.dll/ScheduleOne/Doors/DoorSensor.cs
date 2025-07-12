using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C6 RID: 1734
	[RequireComponent(typeof(Rigidbody))]
	public class DoorSensor : MonoBehaviour
	{
		// Token: 0x06002FC8 RID: 12232 RVA: 0x000C8E91 File Offset: 0x000C7091
		private void Awake()
		{
			this.collider = base.GetComponent<Collider>();
			base.InvokeRepeating("UpdateCollider", 0f, 1f);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000C8EB4 File Offset: 0x000C70B4
		private void UpdateCollider()
		{
			if (PlayerSingleton<PlayerCamera>.Instance == null)
			{
				return;
			}
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			if (InstanceFinder.IsServer)
			{
				Player.GetClosestPlayer(base.transform.position, out num, null);
			}
			this.collider.enabled = (num < 30f);
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000C8F20 File Offset: 0x000C7120
		private void OnTriggerStay(Collider other)
		{
			if (this.exclude.Contains(other))
			{
				return;
			}
			NPC componentInParent = other.GetComponentInParent<NPC>();
			if (componentInParent != null && componentInParent.IsConscious && !componentInParent.Avatar.Ragdolled && componentInParent.CanOpenDoors)
			{
				this.Door.NPCVicinityDetected(this.DetectorSide);
				return;
			}
			if (other.GetComponentInParent<Player>() != null)
			{
				this.Door.PlayerVicinityDetected(this.DetectorSide);
				return;
			}
			this.exclude.Add(other);
		}

		// Token: 0x04002197 RID: 8599
		public const float ActivationDistance = 30f;

		// Token: 0x04002198 RID: 8600
		public EDoorSide DetectorSide = EDoorSide.Exterior;

		// Token: 0x04002199 RID: 8601
		public DoorController Door;

		// Token: 0x0400219A RID: 8602
		private List<Collider> exclude = new List<Collider>();

		// Token: 0x0400219B RID: 8603
		private Collider collider;
	}
}
