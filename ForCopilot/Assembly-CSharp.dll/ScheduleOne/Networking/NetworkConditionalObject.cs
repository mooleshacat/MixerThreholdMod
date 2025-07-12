using System;
using FishNet;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Networking
{
	// Token: 0x02000578 RID: 1400
	public class NetworkConditionalObject : MonoBehaviour
	{
		// Token: 0x060021CC RID: 8652 RVA: 0x0008B668 File Offset: 0x00089868
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.Check));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Check));
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x0008B6B8 File Offset: 0x000898B8
		public void Check()
		{
			NetworkConditionalObject.ECondition econdition = this.condition;
			if (econdition != NetworkConditionalObject.ECondition.All && econdition == NetworkConditionalObject.ECondition.HostOnly && !InstanceFinder.IsHost)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x040019D9 RID: 6617
		public NetworkConditionalObject.ECondition condition;

		// Token: 0x02000579 RID: 1401
		public enum ECondition
		{
			// Token: 0x040019DB RID: 6619
			All,
			// Token: 0x040019DC RID: 6620
			HostOnly
		}
	}
}
