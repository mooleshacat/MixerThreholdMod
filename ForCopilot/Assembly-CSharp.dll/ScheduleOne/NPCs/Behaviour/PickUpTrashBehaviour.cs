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
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200053D RID: 1341
	public class PickUpTrashBehaviour : Behaviour
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x0007F88A File Offset: 0x0007DA8A
		// (set) Token: 0x06001EF4 RID: 7924 RVA: 0x0007F892 File Offset: 0x0007DA92
		public TrashItem TargetTrash { get; private set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x0007C3A4 File Offset: 0x0007A5A4
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0007F89B File Offset: 0x0007DA9B
		public void SetTargetTrash(TrashItem trash)
		{
			this.TargetTrash = trash;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0007F8A4 File Offset: 0x0007DAA4
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0007F8B2 File Offset: 0x0007DAB2
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0007F8C0 File Offset: 0x0007DAC0
		private void StartAction()
		{
			if (base.Npc.Avatar.CurrentEquippable == null || base.Npc.Avatar.CurrentEquippable.AssetPath != "Tools/TrashGrabber/TrashGrabber_AvatarEquippable")
			{
				base.Npc.SetEquippable_Return("Tools/TrashGrabber/TrashGrabber_AvatarEquippable");
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0007F917 File Offset: 0x0007DB17
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0007F925 File Offset: 0x0007DB25
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0007F934 File Offset: 0x0007DB34
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

		// Token: 0x06001EFE RID: 7934 RVA: 0x0007F984 File Offset: 0x0007DB84
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				if (this.Cleaner.DEBUG)
				{
					Console.Log("Waiting for movement to finish", null);
				}
				return;
			}
			if (this.actionCoroutine != null)
			{
				if (this.Cleaner.DEBUG)
				{
					Console.Log("Waiting for action to finish", null);
				}
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

		// Token: 0x06001EFF RID: 7935 RVA: 0x0007FA14 File Offset: 0x0007DC14
		private void GoToTarget()
		{
			if (this.Cleaner.DEBUG)
			{
				Console.Log("Going to target", null);
			}
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			base.SetDestination(this.TargetTrash.transform.position, true);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0007FA61 File Offset: 0x0007DC61
		[ObserversRpc(RunLocally = true)]
		private void PerformAction()
		{
			this.RpcWriter___Observers_PerformAction_2166136261();
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0007FA6F File Offset: 0x0007DC6F
		private bool IsAtDestination()
		{
			return Vector3.Distance(base.Npc.transform.position, this.TargetTrash.transform.position) <= 2f;
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0007FAA0 File Offset: 0x0007DCA0
		private bool AreActionConditionsMet(bool checkAccess)
		{
			return !(this.TargetTrash == null) && !this.TargetTrash.Draggable.IsBeingDragged && (!checkAccess || base.Npc.Movement.CanGetTo(this.TargetTrash.transform.position, 2f));
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0007FAFE File Offset: 0x0007DCFE
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Action|20_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				this.actionCoroutine = null;
				base.Disable_Networked(null);
				yield break;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.FacePoint(this.TargetTrash.transform.position, 0.5f);
			}
			yield return new WaitForSeconds(0.3f);
			if (this.onPerfomAction != null)
			{
				this.onPerfomAction.Invoke();
			}
			yield return new WaitForSeconds(0.4f);
			if (InstanceFinder.IsServer)
			{
				this.Cleaner.trashGrabberInstance.AddTrash(this.TargetTrash.ID, 1);
				if (this.TargetTrash != null)
				{
					this.TargetTrash.DestroyTrash();
				}
			}
			yield return new WaitForSeconds(0.2f);
			this.actionCoroutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0007FB0D File Offset: 0x0007DD0D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_PerformAction_2166136261));
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0007FB3D File Offset: 0x0007DD3D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0007FB56 File Offset: 0x0007DD56
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0007FB64 File Offset: 0x0007DD64
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

		// Token: 0x06001F09 RID: 7945 RVA: 0x0007FC0D File Offset: 0x0007DE0D
		private void RpcLogic___PerformAction_2166136261()
		{
			if (this.Cleaner.DEBUG)
			{
				Console.Log("Picking up trash", null);
			}
			if (this.actionCoroutine != null)
			{
				return;
			}
			this.actionCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PerformAction>g__Action|20_0());
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0007FC48 File Offset: 0x0007DE48
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

		// Token: 0x06001F0B RID: 7947 RVA: 0x0007FC72 File Offset: 0x0007DE72
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400186E RID: 6254
		public const float ACTION_MAX_DISTANCE = 2f;

		// Token: 0x0400186F RID: 6255
		public const string EQUIPPABLE_ASSET_PATH = "Tools/TrashGrabber/TrashGrabber_AvatarEquippable";

		// Token: 0x04001871 RID: 6257
		private Coroutine actionCoroutine;

		// Token: 0x04001872 RID: 6258
		public UnityEvent onPerfomAction;

		// Token: 0x04001873 RID: 6259
		private bool dll_Excuted;

		// Token: 0x04001874 RID: 6260
		private bool dll_Excuted;
	}
}
