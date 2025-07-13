using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.WorldspacePopup;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000521 RID: 1313
	public class CallPoliceBehaviour : Behaviour
	{
		// Token: 0x06001D57 RID: 7511 RVA: 0x00079F00 File Offset: 0x00078100
		protected override void Begin()
		{
			base.Begin();
			if (!this.IsTargetValid())
			{
				this.End();
				this.Disable();
				return;
			}
			if (this.ReportedCrime == null)
			{
				Console.LogError("CallPoliceBehaviour doesn't have a crime set, disabling.", null);
				this.Disable();
				this.End();
				return;
			}
			Console.Log("CallPoliceBehaviour started on player " + this.Target.PlayerName, null);
			this.currentCallTime = 0f;
			this.RefreshIcon();
			if (this.Target.Owner.IsLocalClient)
			{
				this.PhoneCallPopup.enabled = true;
			}
			this.CallSound.Play();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, this.PhonePrefab.AssetPath);
			}
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000045B1 File Offset: 0x000027B1
		public void SetData(NetworkObject player, Crime crime)
		{
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00079FBC File Offset: 0x000781BC
		protected override void Resume()
		{
			base.Resume();
			if (!this.IsTargetValid())
			{
				this.End();
				this.Disable();
				return;
			}
			this.currentCallTime = 0f;
			this.RefreshIcon();
			if (this.Target.Owner.IsLocalClient)
			{
				this.PhoneCallPopup.enabled = true;
			}
			this.CallSound.Play();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, this.PhonePrefab.AssetPath);
			}
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x0007A03C File Offset: 0x0007823C
		protected override void End()
		{
			base.End();
			this.currentCallTime = 0f;
			this.PhoneCallPopup.enabled = false;
			this.CallSound.Stop();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0007A08C File Offset: 0x0007828C
		protected override void Pause()
		{
			base.Pause();
			this.currentCallTime = 0f;
			this.PhoneCallPopup.enabled = false;
			this.CallSound.Stop();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x0007A0DC File Offset: 0x000782DC
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.currentCallTime += Time.deltaTime;
			this.RefreshIcon();
			base.Npc.Avatar.LookController.OverrideLookTarget(this.Target.EyePosition, 1, true);
			if (this.currentCallTime >= 4f && InstanceFinder.IsServer)
			{
				this.FinalizeCall();
			}
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0007A143 File Offset: 0x00078343
		private void RefreshIcon()
		{
			this.PhoneCallPopup.CurrentFillLevel = this.currentCallTime / 4f;
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x0007A15C File Offset: 0x0007835C
		[ObserversRpc(RunLocally = true)]
		private void FinalizeCall()
		{
			this.RpcWriter___Observers_FinalizeCall_2166136261();
			this.RpcLogic___FinalizeCall_2166136261();
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0007A175 File Offset: 0x00078375
		private bool IsTargetValid()
		{
			return !(this.Target == null) && this.Target.Health.IsAlive && !this.Target.IsArrested;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0007A1AB File Offset: 0x000783AB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_FinalizeCall_2166136261));
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x0007A1DB File Offset: 0x000783DB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0007A1F4 File Offset: 0x000783F4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0007A204 File Offset: 0x00078404
		private void RpcWriter___Observers_FinalizeCall_2166136261()
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

		// Token: 0x06001D65 RID: 7525 RVA: 0x0007A2B0 File Offset: 0x000784B0
		private void RpcLogic___FinalizeCall_2166136261()
		{
			if (!base.Active)
			{
				return;
			}
			if (!this.IsTargetValid())
			{
				this.End();
				this.Disable();
				return;
			}
			Debug.Log("Call finalized on player " + this.Target.PlayerName);
			this.Target.CrimeData.RecordLastKnownPosition(true);
			this.Target.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
			this.Target.CrimeData.AddCrime(this.ReportedCrime, 1);
			if (InstanceFinder.IsServer)
			{
				Singleton<LawManager>.Instance.PoliceCalled(this.Target, this.ReportedCrime);
			}
			this.End();
			this.Disable();
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x0007A358 File Offset: 0x00078558
		private void RpcReader___Observers_FinalizeCall_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___FinalizeCall_2166136261();
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0007A382 File Offset: 0x00078582
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017DE RID: 6110
		public const float CALL_POLICE_TIME = 4f;

		// Token: 0x040017DF RID: 6111
		[Header("References")]
		public WorldspacePopup PhoneCallPopup;

		// Token: 0x040017E0 RID: 6112
		public AvatarEquippable PhonePrefab;

		// Token: 0x040017E1 RID: 6113
		public AudioSourceController CallSound;

		// Token: 0x040017E2 RID: 6114
		private float currentCallTime;

		// Token: 0x040017E3 RID: 6115
		public Player Target;

		// Token: 0x040017E4 RID: 6116
		public Crime ReportedCrime;

		// Token: 0x040017E5 RID: 6117
		private bool dll_Excuted;

		// Token: 0x040017E6 RID: 6118
		private bool dll_Excuted;
	}
}
