using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000547 RID: 1351
	public class StartCauldronBehaviour : Behaviour
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x000817F8 File Offset: 0x0007F9F8
		// (set) Token: 0x06001F68 RID: 8040 RVA: 0x00081800 File Offset: 0x0007FA00
		public Cauldron Station { get; protected set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001F69 RID: 8041 RVA: 0x00081809 File Offset: 0x0007FA09
		// (set) Token: 0x06001F6A RID: 8042 RVA: 0x00081811 File Offset: 0x0007FA11
		public bool WorkInProgress { get; protected set; }

		// Token: 0x06001F6B RID: 8043 RVA: 0x0008181A File Offset: 0x0007FA1A
		protected override void Begin()
		{
			base.Begin();
			this.StartWork();
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x00081828 File Offset: 0x0007FA28
		protected override void Resume()
		{
			base.Resume();
			this.StartWork();
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x00081838 File Offset: 0x0007FA38
		protected override void Pause()
		{
			base.Pause();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x00081898 File Offset: 0x0007FA98
		protected override void End()
		{
			base.End();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000818F8 File Offset: 0x0007FAF8
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.WorkInProgress)
			{
				if (this.IsStationReady(this.Station))
				{
					if (base.Npc.Movement.IsMoving)
					{
						return;
					}
					if (this.IsAtStation())
					{
						this.BeginCauldron();
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

		// Token: 0x06001F71 RID: 8049 RVA: 0x0008195C File Offset: 0x0007FB5C
		private void StartWork()
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

		// Token: 0x06001F72 RID: 8050 RVA: 0x000819B8 File Offset: 0x0007FBB8
		public void AssignStation(Cauldron station)
		{
			if (this.Station == station)
			{
				return;
			}
			if (this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				Console.Log("Clearing NPC user from previous rack: " + this.Station.name, null);
				this.Station.SetNPCUser(null);
			}
			this.Station = station;
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x00081A2D File Offset: 0x0007FC2D
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(this.Station.StandPoint.position, 0.5f);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x00081A54 File Offset: 0x0007FC54
		public void GoToStation()
		{
			base.SetDestination(this.Station.StandPoint.position, true);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00081A70 File Offset: 0x0007FC70
		[ObserversRpc(RunLocally = true)]
		public void BeginCauldron()
		{
			this.RpcWriter___Observers_BeginCauldron_2166136261();
			this.RpcLogic___BeginCauldron_2166136261();
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x00081A8C File Offset: 0x0007FC8C
		private void StopCauldron()
		{
			if (this.workRoutine != null)
			{
				base.StopCoroutine(this.workRoutine);
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
			this.WorkInProgress = false;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x00081AF4 File Offset: 0x0007FCF4
		public bool IsStationReady(Cauldron station)
		{
			return !(station == null) && station.GetState() == Cauldron.EState.Ready && (!((IUsable)station).IsInUse || (!(station.PlayerUserObject != null) && !(station.NPCUserObject != base.Npc.NetworkObject))) && base.Npc.Movement.CanGetTo(station.StandPoint.position, 1f);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00081B6C File Offset: 0x0007FD6C
		[CompilerGenerated]
		private IEnumerator <BeginCauldron>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetBool("UseChemistryStation", true);
			float packageTime = 15f;
			for (float i = 0f; i < packageTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.Station.LinkOrigin.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			base.Npc.Avatar.Anim.SetBool("UseChemistryStation", false);
			if (InstanceFinder.IsServer)
			{
				EQuality quality = this.Station.RemoveIngredients();
				this.Station.StartCookOperation(null, this.Station.CookTime, quality);
			}
			this.WorkInProgress = false;
			this.workRoutine = null;
			yield break;
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00081B7B File Offset: 0x0007FD7B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginCauldron_2166136261));
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00081BAB File Offset: 0x0007FDAB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00081BC4 File Offset: 0x0007FDC4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00081BD4 File Offset: 0x0007FDD4
		private void RpcWriter___Observers_BeginCauldron_2166136261()
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

		// Token: 0x06001F7E RID: 8062 RVA: 0x00081C80 File Offset: 0x0007FE80
		public void RpcLogic___BeginCauldron_2166136261()
		{
			if (this.WorkInProgress)
			{
				return;
			}
			if (this.Station == null)
			{
				return;
			}
			this.WorkInProgress = true;
			base.Npc.Movement.FaceDirection(this.Station.StandPoint.forward, 0.5f);
			this.workRoutine = base.StartCoroutine(this.<BeginCauldron>g__Package|20_0());
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x00081CE4 File Offset: 0x0007FEE4
		private void RpcReader___Observers_BeginCauldron_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginCauldron_2166136261();
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x00081D0E File Offset: 0x0007FF0E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018A6 RID: 6310
		public const float START_CAULDRON_TIME = 15f;

		// Token: 0x040018A9 RID: 6313
		private Coroutine workRoutine;

		// Token: 0x040018AA RID: 6314
		private bool dll_Excuted;

		// Token: 0x040018AB RID: 6315
		private bool dll_Excuted;
	}
}
