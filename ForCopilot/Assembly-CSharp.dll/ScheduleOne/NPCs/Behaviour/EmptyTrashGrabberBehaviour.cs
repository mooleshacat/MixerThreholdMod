using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000531 RID: 1329
	public class EmptyTrashGrabberBehaviour : Behaviour
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0007D715 File Offset: 0x0007B915
		// (set) Token: 0x06001E58 RID: 7768 RVA: 0x0007D71D File Offset: 0x0007B91D
		public TrashContainerItem TargetTrashCan { get; private set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0007C3A4 File Offset: 0x0007A5A4
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0007D726 File Offset: 0x0007B926
		public void SetTargetTrashCan(TrashContainerItem trashCan)
		{
			this.TargetTrashCan = trashCan;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0007D72F File Offset: 0x0007B92F
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0007D73D File Offset: 0x0007B93D
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0007D74C File Offset: 0x0007B94C
		private void StartAction()
		{
			if (base.Npc.Avatar.CurrentEquippable == null || base.Npc.Avatar.CurrentEquippable.AssetPath != "Tools/TrashGrabber/Bin_AvatarEquippable")
			{
				base.Npc.SetEquippable_Return("Tools/TrashGrabber/Bin_AvatarEquippable");
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0007D7A3 File Offset: 0x0007B9A3
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0007D7B1 File Offset: 0x0007B9B1
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0007D7C0 File Offset: 0x0007B9C0
		private void StopAllActions()
		{
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			if (this.actionCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.actionCoroutine);
				this.actionCoroutine = null;
			}
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0007D810 File Offset: 0x0007BA10
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
			if (this.actionCoroutine != null)
			{
				return;
			}
			if (!this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				return;
			}
			if (this.IsAtDestination())
			{
				this.PerformAction();
				return;
			}
			this.GoToTarget();
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0007D870 File Offset: 0x0007BA70
		private void GoToTarget()
		{
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			Transform accessPoint = NavMeshUtility.GetAccessPoint(this.TargetTrashCan, base.Npc);
			if (accessPoint == null)
			{
				base.Disable_Networked(null);
				return;
			}
			base.SetDestination(accessPoint.position, true);
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0007D8BE File Offset: 0x0007BABE
		[ObserversRpc(RunLocally = true)]
		private void PerformAction()
		{
			this.RpcWriter___Observers_PerformAction_2166136261();
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0007D8CC File Offset: 0x0007BACC
		private bool IsAtDestination()
		{
			return Vector3.Distance(base.Npc.transform.position, this.TargetTrashCan.transform.position) <= 2f;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0007D900 File Offset: 0x0007BB00
		private bool AreActionConditionsMet(bool checkAccess)
		{
			return !(this.TargetTrashCan == null) && this.TargetTrashCan.Container.NormalizedTrashLevel < 1f && this.Cleaner.trashGrabberInstance.GetTotalSize() != 0 && (!checkAccess || base.Npc.Movement.CanGetTo(this.TargetTrashCan.transform.position, 2f));
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0007D977 File Offset: 0x0007BB77
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Action|20_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				yield break;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.FacePoint(this.TargetTrashCan.transform.position, 0.5f);
			}
			yield return new WaitForSeconds(0.3f);
			if (this.onPerfomAction != null)
			{
				this.onPerfomAction.Invoke();
			}
			yield return new WaitForSeconds(0.4f);
			if (InstanceFinder.IsServer)
			{
				while (this.AreActionConditionsMet(false))
				{
					List<string> trashIDs = this.Cleaner.trashGrabberInstance.GetTrashIDs();
					string id = trashIDs[trashIDs.Count - 1];
					this.Cleaner.trashGrabberInstance.RemoveTrash(id, 1);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(id, this.TargetTrashCan.transform.position + Vector3.up * 1.5f, UnityEngine.Random.rotation, default(Vector3), "", false);
					yield return new WaitForSeconds(0.5f);
				}
			}
			yield return new WaitForSeconds(0.2f);
			this.actionCoroutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0007D986 File Offset: 0x0007BB86
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_PerformAction_2166136261));
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0007D9B6 File Offset: 0x0007BBB6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0007D9CF File Offset: 0x0007BBCF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0007D9E0 File Offset: 0x0007BBE0
		private void RpcWriter___Observers_PerformAction_2166136261()
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

		// Token: 0x06001E6D RID: 7789 RVA: 0x0007DA89 File Offset: 0x0007BC89
		private void RpcLogic___PerformAction_2166136261()
		{
			if (this.actionCoroutine != null)
			{
				return;
			}
			this.actionCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PerformAction>g__Action|20_0());
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0007DAAC File Offset: 0x0007BCAC
		private void RpcReader___Observers_PerformAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0007DAD6 File Offset: 0x0007BCD6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001831 RID: 6193
		public const float ACTION_MAX_DISTANCE = 2f;

		// Token: 0x04001832 RID: 6194
		public const string EQUIPPABLE_ASSET_PATH = "Tools/TrashGrabber/Bin_AvatarEquippable";

		// Token: 0x04001834 RID: 6196
		private Coroutine actionCoroutine;

		// Token: 0x04001835 RID: 6197
		public UnityEvent onPerfomAction;

		// Token: 0x04001836 RID: 6198
		private bool dll_Excuted;

		// Token: 0x04001837 RID: 6199
		private bool dll_Excuted;
	}
}
