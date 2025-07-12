using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000809 RID: 2057
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))]
	public class PlayerPusher : MonoBehaviour
	{
		// Token: 0x0600380B RID: 14347 RVA: 0x000EC54E File Offset: 0x000EA74E
		private void Awake()
		{
			this.veh = base.GetComponentInParent<LandVehicle>();
			this.collider = base.GetComponent<Collider>();
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Ignore Raycast"));
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000EC57D File Offset: 0x000EA77D
		private void FixedUpdate()
		{
			this.collider.enabled = !this.veh.Rb.isKinematic;
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000EC5A0 File Offset: 0x000EA7A0
		private void OnTriggerStay(Collider other)
		{
			if (this.veh.speed_Kmh < this.MinSpeedToPush)
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent == Player.Local && componentInParent.CurrentVehicle == null)
			{
				Vector3 normalized = Vector3.Project((componentInParent.transform.position - base.transform.position).normalized, base.transform.right).normalized;
				float d = this.MinPushForce + Mathf.Clamp((this.veh.speed_Kmh - this.MinSpeedToPush) / this.MaxPushSpeed, 0f, 1f) * (this.MaxPushSpeed - this.MinPushForce);
				PlayerSingleton<PlayerMovement>.Instance.Controller.Move(normalized * d * Time.fixedDeltaTime);
			}
		}

		// Token: 0x040027D5 RID: 10197
		private LandVehicle veh;

		// Token: 0x040027D6 RID: 10198
		[Header("Settings")]
		public float MinSpeedToPush = 3f;

		// Token: 0x040027D7 RID: 10199
		public float MaxPushSpeed = 20f;

		// Token: 0x040027D8 RID: 10200
		public float MinPushForce = 0.5f;

		// Token: 0x040027D9 RID: 10201
		public float MaxPushForce = 5f;

		// Token: 0x040027DA RID: 10202
		private Collider collider;
	}
}
