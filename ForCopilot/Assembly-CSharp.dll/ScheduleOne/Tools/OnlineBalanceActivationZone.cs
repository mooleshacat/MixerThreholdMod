using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A4 RID: 2212
	public class OnlineBalanceActivationZone : MonoBehaviour
	{
		// Token: 0x06003C0E RID: 15374 RVA: 0x000FD4B5 File Offset: 0x000FB6B5
		private void Awake()
		{
			this.collider = base.GetComponent<Collider>();
			base.InvokeRepeating("UpdateCollider", 0f, 1f);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x000FD4D8 File Offset: 0x000FB6D8
		private void UpdateCollider()
		{
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			this.collider.enabled = (num < 20f);
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x000FD50C File Offset: 0x000FB70C
		private void OnTriggerStay(Collider other)
		{
			if (this.exclude.Contains(other))
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				Singleton<HUD>.Instance.OnlineBalanceDisplay.Show();
				return;
			}
			this.exclude.Add(other);
		}

		// Token: 0x04002AE3 RID: 10979
		public const float ActivationDistance = 20f;

		// Token: 0x04002AE4 RID: 10980
		private List<Collider> exclude = new List<Collider>();

		// Token: 0x04002AE5 RID: 10981
		private Collider collider;
	}
}
