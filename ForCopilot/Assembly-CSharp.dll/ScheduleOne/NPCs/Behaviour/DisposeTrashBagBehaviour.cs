using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200052E RID: 1326
	public class DisposeTrashBagBehaviour : Behaviour
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x0007CEB0 File Offset: 0x0007B0B0
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x0007CEB8 File Offset: 0x0007B0B8
		public TrashBag TargetBag { get; private set; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x0007C3A4 File Offset: 0x0007A5A4
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0007CEC1 File Offset: 0x0007B0C1
		public void SetTargetBag(TrashBag bag)
		{
			this.TargetBag = bag;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0007CECA File Offset: 0x0007B0CA
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0007CED8 File Offset: 0x0007B0D8
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000045B1 File Offset: 0x000027B1
		private void StartAction()
		{
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0007CEE6 File Offset: 0x0007B0E6
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0007CEF4 File Offset: 0x0007B0F4
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0007CF04 File Offset: 0x0007B104
		private void StopAllActions()
		{
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			if (this.grabRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.grabRoutine);
				this.grabRoutine = null;
			}
			if (this.dropRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.dropRoutine);
				this.dropRoutine = null;
			}
			if (base.Npc.Avatar.CurrentEquippable != null && base.Npc.Avatar.CurrentEquippable.AssetPath == this.TRASH_BAG_ASSET_PATH)
			{
				base.Npc.SetEquippable_Return(string.Empty);
			}
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0007CFBC File Offset: 0x0007B1BC
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				return;
			}
			if (this.grabRoutine != null)
			{
				return;
			}
			if (this.dropRoutine != null)
			{
				return;
			}
			if (!this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				return;
			}
			if (this.heldTrash == null)
			{
				if (this.IsAtDestination())
				{
					this.GrabTrash();
					return;
				}
				this.GoToTarget();
				return;
			}
			else
			{
				if (this.IsAtDestination())
				{
					this.DropTrash();
					return;
				}
				this.GoToTarget();
				return;
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0007D040 File Offset: 0x0007B240
		private void GoToTarget()
		{
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			if (this.heldTrash == null)
			{
				base.SetDestination(this.TargetBag.transform.position, true);
				return;
			}
			base.SetDestination(this.Cleaner.AssignedProperty.DisposalArea.StandPoint.position, true);
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0007D09F File Offset: 0x0007B29F
		[ObserversRpc(RunLocally = true)]
		private void GrabTrash()
		{
			this.RpcWriter___Observers_GrabTrash_2166136261();
			this.RpcLogic___GrabTrash_2166136261();
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0007D0AD File Offset: 0x0007B2AD
		[ObserversRpc(RunLocally = true)]
		private void DropTrash()
		{
			this.RpcWriter___Observers_DropTrash_2166136261();
			this.RpcLogic___DropTrash_2166136261();
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0007D0BC File Offset: 0x0007B2BC
		private bool IsAtDestination()
		{
			if (this.heldTrash == null)
			{
				return Vector3.Distance(base.Npc.transform.position, this.TargetBag.transform.position) <= 2f;
			}
			return Vector3.Distance(base.Npc.transform.position, this.Cleaner.AssignedProperty.DisposalArea.StandPoint.position) <= 2f;
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0007D13C File Offset: 0x0007B33C
		private bool AreActionConditionsMet(bool checkAccess)
		{
			if (this.heldTrash == null)
			{
				if (this.TargetBag == null)
				{
					return false;
				}
				if (this.TargetBag.Draggable.IsBeingDragged)
				{
					return false;
				}
				if (checkAccess && !base.Npc.Movement.CanGetTo(this.TargetBag.transform.position, 2f))
				{
					return false;
				}
			}
			else if (checkAccess && !base.Npc.Movement.CanGetTo(this.Cleaner.AssignedProperty.DisposalArea.StandPoint.position, 2f))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0007D1EB File Offset: 0x0007B3EB
		[CompilerGenerated]
		private IEnumerator <GrabTrash>g__Action|21_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				yield break;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.FacePoint(this.TargetBag.transform.position, 0.5f);
			}
			yield return new WaitForSeconds(0.3f);
			base.Npc.SetAnimationTrigger("GrabItem");
			if (InstanceFinder.IsServer)
			{
				if (!this.AreActionConditionsMet(false))
				{
					base.Disable_Networked(null);
					this.grabRoutine = null;
					yield break;
				}
				base.Npc.SetEquippable_Networked(null, this.TRASH_BAG_ASSET_PATH);
				this.heldTrash = this.TargetBag.Content;
				this.TargetBag.DestroyTrash();
			}
			yield return new WaitForSeconds(0.2f);
			this.grabRoutine = null;
			yield break;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0007D1FA File Offset: 0x0007B3FA
		[CompilerGenerated]
		private IEnumerator <DropTrash>g__Action|22_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				yield break;
			}
			base.Npc.Movement.FaceDirection(this.Cleaner.AssignedProperty.DisposalArea.StandPoint.forward, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (InstanceFinder.IsServer)
			{
				Transform trashDropPoint = this.Cleaner.AssignedProperty.DisposalArea.TrashDropPoint;
				NetworkSingleton<TrashManager>.Instance.CreateTrashBag("trashbag", trashDropPoint.position, UnityEngine.Random.rotation, this.heldTrash.GetData(), default(Vector3), "", false);
				this.heldTrash = null;
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
			yield return new WaitForSeconds(0.2f);
			this.dropRoutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0007D20C File Offset: 0x0007B40C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_GrabTrash_2166136261));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_DropTrash_2166136261));
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0007D25E File Offset: 0x0007B45E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0007D277 File Offset: 0x0007B477
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0007D288 File Offset: 0x0007B488
		private void RpcWriter___Observers_GrabTrash_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0007D331 File Offset: 0x0007B531
		private void RpcLogic___GrabTrash_2166136261()
		{
			if (this.grabRoutine != null)
			{
				return;
			}
			this.grabRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<GrabTrash>g__Action|21_0());
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0007D354 File Offset: 0x0007B554
		private void RpcReader___Observers_GrabTrash_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___GrabTrash_2166136261();
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0007D380 File Offset: 0x0007B580
		private void RpcWriter___Observers_DropTrash_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0007D429 File Offset: 0x0007B629
		private void RpcLogic___DropTrash_2166136261()
		{
			if (this.dropRoutine != null)
			{
				return;
			}
			this.dropRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<DropTrash>g__Action|22_0());
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0007D44C File Offset: 0x0007B64C
		private void RpcReader___Observers_DropTrash_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DropTrash_2166136261();
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0007D476 File Offset: 0x0007B676
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001823 RID: 6179
		public string TRASH_BAG_ASSET_PATH = "Avatar/Equippables/TrashBag";

		// Token: 0x04001824 RID: 6180
		public const float GRAB_MAX_DISTANCE = 2f;

		// Token: 0x04001826 RID: 6182
		private TrashContent heldTrash;

		// Token: 0x04001827 RID: 6183
		private Coroutine grabRoutine;

		// Token: 0x04001828 RID: 6184
		private Coroutine dropRoutine;

		// Token: 0x04001829 RID: 6185
		private bool dll_Excuted;

		// Token: 0x0400182A RID: 6186
		private bool dll_Excuted;
	}
}
