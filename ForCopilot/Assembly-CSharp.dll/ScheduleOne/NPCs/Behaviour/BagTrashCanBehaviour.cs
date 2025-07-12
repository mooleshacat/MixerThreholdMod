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
using ScheduleOne.ObjectScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200052A RID: 1322
	public class BagTrashCanBehaviour : Behaviour
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0007C393 File Offset: 0x0007A593
		// (set) Token: 0x06001DEF RID: 7663 RVA: 0x0007C39B File Offset: 0x0007A59B
		public TrashContainerItem TargetTrashCan { get; private set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x0007C3A4 File Offset: 0x0007A5A4
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0007C3B1 File Offset: 0x0007A5B1
		public void SetTargetTrashCan(TrashContainerItem trashCan)
		{
			this.TargetTrashCan = trashCan;
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0007C3BA File Offset: 0x0007A5BA
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0007C3C8 File Offset: 0x0007A5C8
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0007C3D6 File Offset: 0x0007A5D6
		private void StartAction()
		{
			if (base.Npc.Avatar.CurrentEquippable != null)
			{
				base.Npc.SetEquippable_Return(string.Empty);
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0007C401 File Offset: 0x0007A601
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0007C425 File Offset: 0x0007A625
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0007C434 File Offset: 0x0007A634
		private void StopAllActions()
		{
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			base.Npc.SetAnimationBool("PatSoil", false);
			base.Npc.SetCrouched_Networked(false);
			if (this.actionCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.actionCoroutine);
				this.actionCoroutine = null;
			}
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0007C4A0 File Offset: 0x0007A6A0
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

		// Token: 0x06001DFA RID: 7674 RVA: 0x0007C4FD File Offset: 0x0007A6FD
		private void GoToTarget()
		{
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			base.SetDestination(NavMeshUtility.GetAccessPoint(this.TargetTrashCan, base.Npc).position, true);
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0007C52D File Offset: 0x0007A72D
		[ObserversRpc(RunLocally = true)]
		private void PerformAction()
		{
			this.RpcWriter___Observers_PerformAction_2166136261();
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0007C53B File Offset: 0x0007A73B
		private bool IsAtDestination()
		{
			return Vector3.Distance(base.Npc.transform.position, this.TargetTrashCan.transform.position) <= 2f;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0007C56C File Offset: 0x0007A76C
		private bool AreActionConditionsMet(bool checkAccess)
		{
			if (this.TargetTrashCan == null)
			{
				return false;
			}
			if (this.TargetTrashCan.Container.NormalizedTrashLevel == 0f)
			{
				return false;
			}
			if (checkAccess)
			{
				Transform accessPoint = NavMeshUtility.GetAccessPoint(this.TargetTrashCan, base.Npc);
				if (accessPoint == null)
				{
					return false;
				}
				if (!base.Npc.Movement.CanGetTo(accessPoint.position, 2f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0007C5E2 File Offset: 0x0007A7E2
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Action|21_0()
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
			yield return new WaitForSeconds(0.4f);
			base.Npc.SetAnimationBool("PatSoil", true);
			base.Npc.SetCrouched_Networked(true);
			if (this.onPerfomAction != null)
			{
				this.onPerfomAction.Invoke();
			}
			yield return new WaitForSeconds(3f);
			if (InstanceFinder.IsServer && this.AreActionConditionsMet(false))
			{
				this.TargetTrashCan.Container.BagTrash();
				if (this.onPerfomDone != null)
				{
					this.onPerfomDone.Invoke();
				}
			}
			base.Npc.SetAnimationBool("PatSoil", false);
			yield return new WaitForSeconds(0.2f);
			this.actionCoroutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0007C5F1 File Offset: 0x0007A7F1
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_PerformAction_2166136261));
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0007C621 File Offset: 0x0007A821
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0007C63A File Offset: 0x0007A83A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0007C648 File Offset: 0x0007A848
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

		// Token: 0x06001E04 RID: 7684 RVA: 0x0007C6F1 File Offset: 0x0007A8F1
		private void RpcLogic___PerformAction_2166136261()
		{
			if (this.actionCoroutine != null)
			{
				return;
			}
			this.actionCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PerformAction>g__Action|21_0());
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0007C714 File Offset: 0x0007A914
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

		// Token: 0x06001E06 RID: 7686 RVA: 0x0007C73E File Offset: 0x0007A93E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400180D RID: 6157
		public const float ACTION_MAX_DISTANCE = 2f;

		// Token: 0x0400180E RID: 6158
		public const float BAG_TIME = 3f;

		// Token: 0x04001810 RID: 6160
		private Coroutine actionCoroutine;

		// Token: 0x04001811 RID: 6161
		public UnityEvent onPerfomAction;

		// Token: 0x04001812 RID: 6162
		public UnityEvent onPerfomDone;

		// Token: 0x04001813 RID: 6163
		private bool dll_Excuted;

		// Token: 0x04001814 RID: 6164
		private bool dll_Excuted;
	}
}
