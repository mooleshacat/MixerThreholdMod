using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200063C RID: 1596
	public class PlayerVisualState : NetworkBehaviour
	{
		// Token: 0x06002944 RID: 10564 RVA: 0x000AA2F8 File Offset: 0x000A84F8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.PlayerVisualState_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000AA30C File Offset: 0x000A850C
		private void Update()
		{
			if (NetworkSingleton<CurfewManager>.InstanceExists && NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActiveWithTolerance && this.player.CurrentProperty == null && this.player.CurrentBusiness == null)
			{
				if (this.GetState("DisobeyingCurfew") == null)
				{
					this.ApplyState("DisobeyingCurfew", PlayerVisualState.EVisualState.DisobeyingCurfew, 0f);
				}
			}
			else if (this.GetState("DisobeyingCurfew") != null)
			{
				this.RemoveState("DisobeyingCurfew", 0f);
			}
			this.UpdateSuspiciousness();
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000AA398 File Offset: 0x000A8598
		[ServerRpc(RunLocally = true)]
		public void ApplyState(string label, PlayerVisualState.EVisualState state, float autoRemoveAfter = 0f)
		{
			this.RpcWriter___Server_ApplyState_868472085(label, state, autoRemoveAfter);
			this.RpcLogic___ApplyState_868472085(label, state, autoRemoveAfter);
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000AA3CC File Offset: 0x000A85CC
		[ServerRpc(RunLocally = true)]
		public void RemoveState(string label, float delay = 0f)
		{
			this.RpcWriter___Server_RemoveState_606697822(label, delay);
			this.RpcLogic___RemoveState_606697822(label, delay);
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x000AA3F8 File Offset: 0x000A85F8
		public PlayerVisualState.VisualState GetState(string label)
		{
			return this.visualStates.Find((PlayerVisualState.VisualState x) => x.label == label);
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000AA42C File Offset: 0x000A862C
		public void ClearStates()
		{
			PlayerVisualState.VisualState[] array = this.visualStates.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i].label == "Visible"))
				{
					this.RemoveState(array[i].label, 0f);
				}
			}
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000AA47C File Offset: 0x000A867C
		private void UpdateSuspiciousness()
		{
			this.Suspiciousness = 0f;
			if (this.player.Avatar.Anim.IsCrouched)
			{
				this.Suspiciousness += 0.3f;
			}
			if (this.player.Avatar.CurrentEquippable != null)
			{
				this.Suspiciousness += this.player.Avatar.CurrentEquippable.Suspiciousness;
			}
			if (this.player.VelocityCalculator.Velocity.magnitude > PlayerMovement.WalkSpeed)
			{
				this.Suspiciousness += 0.3f * Mathf.InverseLerp(PlayerMovement.WalkSpeed, PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier, this.player.VelocityCalculator.Velocity.magnitude);
			}
			this.Suspiciousness = Mathf.Clamp01(this.Suspiciousness);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000AA588 File Offset: 0x000A8788
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_ApplyState_868472085));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_RemoveState_606697822));
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000AA5D4 File Offset: 0x000A87D4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerVisualStateAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000AA5E7 File Offset: 0x000A87E7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000AA5F8 File Offset: 0x000A87F8
		private void RpcWriter___Server_ApplyState_868472085(string label, PlayerVisualState.EVisualState state, float autoRemoveAfter = 0f)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(label);
			writer.Write___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generated(state);
			writer.WriteSingle(autoRemoveAfter, 0);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000AA718 File Offset: 0x000A8918
		public void RpcLogic___ApplyState_868472085(string label, PlayerVisualState.EVisualState state, float autoRemoveAfter = 0f)
		{
			PlayerVisualState.VisualState visualState = this.GetState(label);
			if (visualState == null)
			{
				visualState = new PlayerVisualState.VisualState();
				visualState.label = label;
				this.visualStates.Add(visualState);
			}
			visualState.state = state;
			if (this.removalRoutinesDict.ContainsKey(label))
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.removalRoutinesDict[label]);
				this.removalRoutinesDict.Remove(label);
			}
			if (autoRemoveAfter > 0f)
			{
				this.RemoveState(label, autoRemoveAfter);
			}
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000AA794 File Offset: 0x000A8994
		private void RpcReader___Server_ApplyState_868472085(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string label = PooledReader0.ReadString();
			PlayerVisualState.EVisualState state = GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerVisualState/EVisualStateFishNet.Serializing.Generateds(PooledReader0);
			float autoRemoveAfter = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ApplyState_868472085(label, state, autoRemoveAfter);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000AA80C File Offset: 0x000A8A0C
		private void RpcWriter___Server_RemoveState_606697822(string label, float delay = 0f)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(label);
			writer.WriteSingle(delay, 0);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000AA920 File Offset: 0x000A8B20
		public void RpcLogic___RemoveState_606697822(string label, float delay = 0f)
		{
			PlayerVisualState.<>c__DisplayClass9_0 CS$<>8__locals1 = new PlayerVisualState.<>c__DisplayClass9_0();
			CS$<>8__locals1.delay = delay;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.label = label;
			CS$<>8__locals1.newState = this.GetState(CS$<>8__locals1.label);
			if (CS$<>8__locals1.newState == null)
			{
				return;
			}
			if (CS$<>8__locals1.delay > 0f)
			{
				if (this.removalRoutinesDict.ContainsKey(CS$<>8__locals1.label))
				{
					Singleton<CoroutineService>.Instance.StopCoroutine(this.removalRoutinesDict[CS$<>8__locals1.label]);
					this.removalRoutinesDict.Remove(CS$<>8__locals1.label);
				}
				this.removalRoutinesDict.Add(CS$<>8__locals1.label, Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<RemoveState>g__DelayedRemove|0()));
				return;
			}
			CS$<>8__locals1.<RemoveState>g__Destroy|1();
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000AA9D8 File Offset: 0x000A8BD8
		private void RpcReader___Server_RemoveState_606697822(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string label = PooledReader0.ReadString();
			float delay = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RemoveState_606697822(label, delay);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000AAA3D File Offset: 0x000A8C3D
		private void dll()
		{
			this.player = base.GetComponent<Player>();
			this.player.Health.onDie.AddListener(delegate()
			{
				this.ClearStates();
			});
			this.ApplyState("Visible", PlayerVisualState.EVisualState.Visible, 0f);
		}

		// Token: 0x04001DCD RID: 7629
		public float Suspiciousness;

		// Token: 0x04001DCE RID: 7630
		public List<PlayerVisualState.VisualState> visualStates = new List<PlayerVisualState.VisualState>();

		// Token: 0x04001DCF RID: 7631
		private Player player;

		// Token: 0x04001DD0 RID: 7632
		private Dictionary<string, Coroutine> removalRoutinesDict = new Dictionary<string, Coroutine>();

		// Token: 0x04001DD1 RID: 7633
		private bool dll_Excuted;

		// Token: 0x04001DD2 RID: 7634
		private bool dll_Excuted;

		// Token: 0x0200063D RID: 1597
		public enum EVisualState
		{
			// Token: 0x04001DD4 RID: 7636
			Visible,
			// Token: 0x04001DD5 RID: 7637
			Suspicious,
			// Token: 0x04001DD6 RID: 7638
			DisobeyingCurfew,
			// Token: 0x04001DD7 RID: 7639
			Vandalizing,
			// Token: 0x04001DD8 RID: 7640
			PettyCrime,
			// Token: 0x04001DD9 RID: 7641
			DrugDealing,
			// Token: 0x04001DDA RID: 7642
			SearchedFor,
			// Token: 0x04001DDB RID: 7643
			Wanted,
			// Token: 0x04001DDC RID: 7644
			Pickpocketing,
			// Token: 0x04001DDD RID: 7645
			DischargingWeapon,
			// Token: 0x04001DDE RID: 7646
			Brandishing
		}

		// Token: 0x0200063E RID: 1598
		[Serializable]
		public class VisualState
		{
			// Token: 0x04001DDF RID: 7647
			public PlayerVisualState.EVisualState state;

			// Token: 0x04001DE0 RID: 7648
			public string label;

			// Token: 0x04001DE1 RID: 7649
			public Action stateDestroyed;
		}
	}
}
