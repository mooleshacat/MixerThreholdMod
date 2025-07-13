using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008AF RID: 2223
	public class SafeBalanceActivationZone : MonoBehaviour
	{
		// Token: 0x06003C3E RID: 15422 RVA: 0x000FDDB2 File Offset: 0x000FBFB2
		private void Awake()
		{
			this.colliders = base.GetComponentsInChildren<Collider>();
			base.InvokeRepeating("UpdateCollider", 0f, 1f);
			base.InvokeRepeating("Activate", 0f, 0.25f);
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x000FDDEC File Offset: 0x000FBFEC
		private void UpdateCollider()
		{
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			Collider[] array = this.colliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = (num < 30f);
			}
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x000FDE32 File Offset: 0x000FC032
		private void Activate()
		{
			this.active = true;
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x000FDE3C File Offset: 0x000FC03C
		private void OnTriggerStay(Collider other)
		{
			if (!this.active)
			{
				return;
			}
			this.active = true;
			if (this.exclude.Contains(other))
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				Singleton<HUD>.Instance.SafeBalanceDisplay.SetBalance(this.Safe.GetCash());
				Singleton<HUD>.Instance.SafeBalanceDisplay.Show();
				return;
			}
			this.exclude.Add(other);
		}

		// Token: 0x04002B02 RID: 11010
		public const float ActivationDistance = 30f;

		// Token: 0x04002B03 RID: 11011
		public Safe Safe;

		// Token: 0x04002B04 RID: 11012
		private List<Collider> exclude = new List<Collider>();

		// Token: 0x04002B05 RID: 11013
		private Collider[] colliders;

		// Token: 0x04002B06 RID: 11014
		private bool active;
	}
}
