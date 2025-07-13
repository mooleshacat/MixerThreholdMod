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
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200053B RID: 1339
	public class PackagingStationBehaviour : Behaviour
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x0007F2A8 File Offset: 0x0007D4A8
		// (set) Token: 0x06001ED4 RID: 7892 RVA: 0x0007F2B0 File Offset: 0x0007D4B0
		public PackagingStation Station { get; protected set; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x0007F2B9 File Offset: 0x0007D4B9
		// (set) Token: 0x06001ED6 RID: 7894 RVA: 0x0007F2C1 File Offset: 0x0007D4C1
		public bool PackagingInProgress { get; protected set; }

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0007F2CA File Offset: 0x0007D4CA
		protected override void Begin()
		{
			base.Begin();
			this.StartPackaging();
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0007F2D8 File Offset: 0x0007D4D8
		protected override void Resume()
		{
			base.Resume();
			this.StartPackaging();
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0007F2E6 File Offset: 0x0007D4E6
		protected override void Pause()
		{
			base.Pause();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0007F2FC File Offset: 0x0007D4FC
		protected override void End()
		{
			base.End();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0007F35C File Offset: 0x0007D55C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.PackagingInProgress)
			{
				if (this.IsStationReady(this.Station))
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

		// Token: 0x06001EDD RID: 7901 RVA: 0x0007F3C0 File Offset: 0x0007D5C0
		private void StartPackaging()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsStationReady(this.Station))
			{
				Console.LogWarning(base.Npc.fullName + " has no station to work with", null);
				base.Disable_Networked(null);
				return;
			}
			this.Station.SetNPCUser(base.Npc.NetworkObject);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0007F41C File Offset: 0x0007D61C
		public void AssignStation(PackagingStation station)
		{
			this.Station = station;
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0007F425 File Offset: 0x0007D625
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(this.Station.StandPoint.position, 0.5f);
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0007F44C File Offset: 0x0007D64C
		public void GoToStation()
		{
			base.SetDestination(this.Station.StandPoint.position, true);
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0007F468 File Offset: 0x0007D668
		[ObserversRpc(RunLocally = true)]
		public void BeginPackaging()
		{
			this.RpcWriter___Observers_BeginPackaging_2166136261();
			this.RpcLogic___BeginPackaging_2166136261();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0007F484 File Offset: 0x0007D684
		private void StopPackaging()
		{
			if (this.packagingRoutine != null)
			{
				base.StopCoroutine(this.packagingRoutine);
			}
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", false);
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
			this.PackagingInProgress = false;
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0007F508 File Offset: 0x0007D708
		public bool IsStationReady(PackagingStation station)
		{
			return !(station == null) && station.GetState(PackagingStation.EMode.Package) == PackagingStation.EState.CanBegin && (!((IUsable)station).IsInUse || !(station.NPCUserObject != base.Npc.NetworkObject)) && base.Npc.Movement.CanGetTo(station.StandPoint.position, 1f);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0007F572 File Offset: 0x0007D772
		[CompilerGenerated]
		private IEnumerator <BeginPackaging>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", true);
			float packageTime = 5f / ((base.Npc as Packager).PackagingSpeedMultiplier * this.Station.PackagerEmployeeSpeedMultiplier);
			for (float i = 0f; i < packageTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.Station.Container.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", false);
			if (InstanceFinder.IsServer)
			{
				this.Station.PackSingleInstance();
			}
			Console.Log("Packaging done!", null);
			this.PackagingInProgress = false;
			this.packagingRoutine = null;
			yield break;
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0007F581 File Offset: 0x0007D781
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginPackaging_2166136261));
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0007F5B1 File Offset: 0x0007D7B1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0007F5CA File Offset: 0x0007D7CA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0007F5D8 File Offset: 0x0007D7D8
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

		// Token: 0x06001EEA RID: 7914 RVA: 0x0007F684 File Offset: 0x0007D884
		public void RpcLogic___BeginPackaging_2166136261()
		{
			if (this.PackagingInProgress)
			{
				return;
			}
			if (this.Station == null)
			{
				return;
			}
			this.PackagingInProgress = true;
			base.Npc.Movement.FaceDirection(this.Station.StandPoint.forward, 0.5f);
			this.packagingRoutine = base.StartCoroutine(this.<BeginPackaging>g__Package|20_0());
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0007F6E8 File Offset: 0x0007D8E8
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

		// Token: 0x06001EEC RID: 7916 RVA: 0x0007F712 File Offset: 0x0007D912
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001863 RID: 6243
		public const float BASE_PACKAGING_TIME = 5f;

		// Token: 0x04001866 RID: 6246
		private Coroutine packagingRoutine;

		// Token: 0x04001867 RID: 6247
		private bool dll_Excuted;

		// Token: 0x04001868 RID: 6248
		private bool dll_Excuted;
	}
}
