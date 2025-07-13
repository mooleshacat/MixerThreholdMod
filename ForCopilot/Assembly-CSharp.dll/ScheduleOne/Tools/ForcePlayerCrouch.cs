using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000899 RID: 2201
	public class ForcePlayerCrouch : MonoBehaviour
	{
		// Token: 0x06003BEA RID: 15338 RVA: 0x000FD0B0 File Offset: 0x000FB2B0
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				Player componentInParent = other.gameObject.GetComponentInParent<Player>();
				if (componentInParent != null && componentInParent.IsOwner && !PlayerSingleton<PlayerMovement>.Instance.isCrouched)
				{
					PlayerSingleton<PlayerMovement>.Instance.SetCrouched(true);
				}
			}
		}
	}
}
