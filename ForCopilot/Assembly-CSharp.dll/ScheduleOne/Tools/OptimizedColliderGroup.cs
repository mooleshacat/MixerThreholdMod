using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A5 RID: 2213
	public class OptimizedColliderGroup : MonoBehaviour
	{
		// Token: 0x06003C12 RID: 15378 RVA: 0x000FD570 File Offset: 0x000FB770
		private void OnEnable()
		{
			this.sqrColliderEnableMaxDistance = this.ColliderEnableMaxDistance * this.ColliderEnableMaxDistance;
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				this.RegisterEvent();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.RegisterEvent));
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x000FD5BE File Offset: 0x000FB7BE
		private void OnDestroy()
		{
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				PlayerSingleton<PlayerMovement>.Instance.DeregisterMovementEvent(new Action(this.Refresh));
			}
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x000FD5DD File Offset: 0x000FB7DD
		private void RegisterEvent()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.RegisterEvent));
			PlayerSingleton<PlayerMovement>.Instance.RegisterMovementEvent(5, new Action(this.Refresh));
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x000FD616 File Offset: 0x000FB816
		[Button]
		public void GetColliders()
		{
			this.Colliders = base.GetComponentsInChildren<Collider>();
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x000045B1 File Offset: 0x000027B1
		public void Start()
		{
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x000FD624 File Offset: 0x000FB824
		private void Refresh()
		{
			if (Player.Local == null || Player.Local.Avatar == null)
			{
				return;
			}
			float sqrMagnitude = (Player.Local.Avatar.CenterPoint - base.transform.position).sqrMagnitude;
			this.SetCollidersEnabled(sqrMagnitude < this.sqrColliderEnableMaxDistance);
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x000FD688 File Offset: 0x000FB888
		private void SetCollidersEnabled(bool enabled)
		{
			if (this.collidersEnabled == enabled)
			{
				return;
			}
			this.collidersEnabled = enabled;
			foreach (Collider collider in this.Colliders)
			{
				if (!(collider == null))
				{
					collider.enabled = enabled;
				}
			}
		}

		// Token: 0x04002AE6 RID: 10982
		public const int UPDATE_DISTANCE = 5;

		// Token: 0x04002AE7 RID: 10983
		public Collider[] Colliders;

		// Token: 0x04002AE8 RID: 10984
		public float ColliderEnableMaxDistance = 30f;

		// Token: 0x04002AE9 RID: 10985
		private float sqrColliderEnableMaxDistance;

		// Token: 0x04002AEA RID: 10986
		private bool collidersEnabled = true;
	}
}
