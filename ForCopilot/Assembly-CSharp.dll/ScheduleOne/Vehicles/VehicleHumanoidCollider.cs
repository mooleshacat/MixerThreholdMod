using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000812 RID: 2066
	public class VehicleHumanoidCollider : MonoBehaviour
	{
		// Token: 0x06003834 RID: 14388 RVA: 0x000ED0D2 File Offset: 0x000EB2D2
		private void Start()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Ignore Raycast"));
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x000ED0E9 File Offset: 0x000EB2E9
		private void OnCollisionStay(Collision collision)
		{
			Debug.Log("Collision Stay: " + collision.collider.gameObject.name);
		}

		// Token: 0x04002802 RID: 10242
		public LandVehicle vehicle;
	}
}
