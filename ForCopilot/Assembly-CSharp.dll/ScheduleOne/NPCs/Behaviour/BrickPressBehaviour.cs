using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200052C RID: 1324
	public class BrickPressBehaviour : Behaviour
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001E0D RID: 7693 RVA: 0x0007C8CE File Offset: 0x0007AACE
		// (set) Token: 0x06001E0E RID: 7694 RVA: 0x0007C8D6 File Offset: 0x0007AAD6
		public BrickPress Press { get; protected set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001E0F RID: 7695 RVA: 0x0007C8DF File Offset: 0x0007AADF
		// (set) Token: 0x06001E10 RID: 7696 RVA: 0x0007C8E7 File Offset: 0x0007AAE7
		public bool PackagingInProgress { get; protected set; }

		// Token: 0x06001E11 RID: 7697 RVA: 0x0007C8F0 File Offset: 0x0007AAF0
		protected override void Begin()
		{
			base.Begin();
			this.StartPackaging();
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0007C8FE File Offset: 0x0007AAFE
		protected override void Resume()
		{
			base.Resume();
			this.StartPackaging();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0007C90C File Offset: 0x0007AB0C
		protected override void Pause()
		{
			base.Pause();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x0007C924 File Offset: 0x0007AB24
		protected override void End()
		{
			base.End();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
			if (InstanceFinder.IsServer && this.Press != null && this.Press.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Press.SetNPCUser(null);
			}
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0007C984 File Offset: 0x0007AB84
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.PackagingInProgress)
			{
				if (this.IsStationReady(this.Press))
				{
					if (base.Npc.Movement.IsMoving)
					{
						return;
					}
					if (this.IsAtStation())
					{
						this.BeginPackaging();
						return;
					}
					this.GoToStation();
					return;
				}
				else
				{
					base.Disable_Networked(null);
				}
			}
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0007C9E8 File Offset: 0x0007ABE8
		private void StartPackaging()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsStationReady(this.Press))
			{
				Console.LogWarning(base.Npc.fullName + " has no station to work with", null);
				base.Disable_Networked(null);
				return;
			}
			this.Press.SetNPCUser(base.Npc.NetworkObject);
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0007CA44 File Offset: 0x0007AC44
		public void AssignStation(BrickPress press)
		{
			this.Press = press;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0007CA4D File Offset: 0x0007AC4D
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(this.Press.StandPoint.position, 0.5f);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0007CA74 File Offset: 0x0007AC74
		public void GoToStation()
		{
			base.SetDestination(this.Press.StandPoint.position, true);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0007CA90 File Offset: 0x0007AC90
		[ObserversRpc(RunLocally = true)]
		public void BeginPackaging()
		{
			this.RpcWriter___Observers_BeginPackaging_2166136261();
			this.RpcLogic___BeginPackaging_2166136261();
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0007CAA9 File Offset: 0x0007ACA9
		private void StopPackaging()
		{
			if (this.packagingRoutine != null)
			{
				base.StopCoroutine(this.packagingRoutine);
			}
			this.PackagingInProgress = false;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0007CAC8 File Offset: 0x0007ACC8
		public bool IsStationReady(BrickPress press)
		{
			return !(press == null) && press.GetState() == PackagingStation.EState.CanBegin && (!((IUsable)press).IsInUse || !(press.NPCUserObject != base.Npc.NetworkObject)) && base.Npc.Movement.CanGetTo(press.StandPoint.position, 1f);
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0007CB31 File Offset: 0x0007AD31
		[CompilerGenerated]
		private IEnumerator <BeginPackaging>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", true);
			float packageTime = 15f / (base.Npc as Packager).PackagingSpeedMultiplier;
			for (float i = 0f; i < packageTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.Press.uiPoint.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", false);
			yield return new WaitForSeconds(0.2f);
			base.Npc.Avatar.Anim.SetTrigger("GrabItem");
			this.Press.PlayPressAnim();
			yield return new WaitForSeconds(1f);
			ProductItemInstance product;
			if (InstanceFinder.IsServer && this.Press.HasSufficientProduct(out product))
			{
				this.Press.CompletePress(product);
			}
			this.PackagingInProgress = false;
			this.packagingRoutine = null;
			yield break;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0007CB40 File Offset: 0x0007AD40
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginPackaging_2166136261));
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0007CB70 File Offset: 0x0007AD70
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0007CB89 File Offset: 0x0007AD89
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0007CB98 File Offset: 0x0007AD98
		private void RpcWriter___Observers_BeginPackaging_2166136261()
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

		// Token: 0x06001E24 RID: 7716 RVA: 0x0007CC44 File Offset: 0x0007AE44
		public void RpcLogic___BeginPackaging_2166136261()
		{
			if (this.PackagingInProgress)
			{
				return;
			}
			if (this.Press == null)
			{
				return;
			}
			this.PackagingInProgress = true;
			base.Npc.Movement.FaceDirection(this.Press.StandPoint.forward, 0.5f);
			this.packagingRoutine = base.StartCoroutine(this.<BeginPackaging>g__Package|20_0());
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0007CCA8 File Offset: 0x0007AEA8
		private void RpcReader___Observers_BeginPackaging_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginPackaging_2166136261();
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0007CCD2 File Offset: 0x0007AED2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001818 RID: 6168
		public const float BASE_PACKAGING_TIME = 15f;

		// Token: 0x0400181B RID: 6171
		private Coroutine packagingRoutine;

		// Token: 0x0400181C RID: 6172
		private bool dll_Excuted;

		// Token: 0x0400181D RID: 6173
		private bool dll_Excuted;
	}
}
