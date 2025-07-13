using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x02000675 RID: 1653
	public class ToggleableSurfaceItem : SurfaceItem
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000B37B1 File Offset: 0x000B19B1
		// (set) Token: 0x06002B69 RID: 11113 RVA: 0x000B37B9 File Offset: 0x000B19B9
		public bool IsOn { get; private set; }

		// Token: 0x06002B6A RID: 11114 RVA: 0x000B37C4 File Offset: 0x000B19C4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.ToggleableSurfaceItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000B37E3 File Offset: 0x000B19E3
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsOn)
			{
				this.SetIsOn(connection, true);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000B37FC File Offset: 0x000B19FC
		public void Toggle()
		{
			if (this.IsOn)
			{
				this.TurnOff(true);
				return;
			}
			this.TurnOn(true);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000B3818 File Offset: 0x000B1A18
		public void TurnOn(bool network = true)
		{
			if (this.IsOn)
			{
				return;
			}
			if (network)
			{
				this.SendIsOn(true);
				return;
			}
			this.IsOn = true;
			if (this.onTurnedOn != null)
			{
				this.onTurnedOn.Invoke();
			}
			if (this.onTurnOnOrOff != null)
			{
				this.onTurnOnOrOff.Invoke();
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000B3868 File Offset: 0x000B1A68
		public void TurnOff(bool network = true)
		{
			if (!this.IsOn)
			{
				return;
			}
			if (network)
			{
				this.SendIsOn(false);
				return;
			}
			this.IsOn = false;
			if (this.onTurnedOff != null)
			{
				this.onTurnedOff.Invoke();
			}
			if (this.onTurnOnOrOff != null)
			{
				this.onTurnOnOrOff.Invoke();
			}
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000B38B6 File Offset: 0x000B1AB6
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendIsOn(bool on)
		{
			this.RpcWriter___Server_SendIsOn_1140765316(on);
			this.RpcLogic___SendIsOn_1140765316(on);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000B38CC File Offset: 0x000B1ACC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetIsOn(NetworkConnection conn, bool on)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsOn_214505783(conn, on);
				this.RpcLogic___SetIsOn_214505783(conn, on);
			}
			else
			{
				this.RpcWriter___Target_SetIsOn_214505783(conn, on);
			}
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000B3904 File Offset: 0x000B1B04
		public override BuildableItemData GetBaseData()
		{
			return new ToggleableSurfaceItemData(base.GUID, base.ItemInstance, 0, base.ParentSurface.GUID.ToString(), this.RelativePosition, this.RelativeRotation, this.IsOn);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000B3958 File Offset: 0x000B1B58
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ToggleableSurfaceItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ToggleableSurfaceItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendIsOn_1140765316));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOn_214505783));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOn_214505783));
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000B39C1 File Offset: 0x000B1BC1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.ToggleableSurfaceItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.ToggleableSurfaceItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000B39DA File Offset: 0x000B1BDA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000B39E8 File Offset: 0x000B1BE8
		private void RpcWriter___Server_SendIsOn_1140765316(bool on)
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
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(on);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x000B3A8F File Offset: 0x000B1C8F
		private void RpcLogic___SendIsOn_1140765316(bool on)
		{
			base.HasChanged = true;
			this.SetIsOn(null, on);
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x000B3AA0 File Offset: 0x000B1CA0
		private void RpcReader___Server_SendIsOn_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendIsOn_1140765316(on);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000B3AE0 File Offset: 0x000B1CE0
		private void RpcWriter___Observers_SetIsOn_214505783(NetworkConnection conn, bool on)
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
			writer.WriteBoolean(on);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000B3B96 File Offset: 0x000B1D96
		private void RpcLogic___SetIsOn_214505783(NetworkConnection conn, bool on)
		{
			if (on)
			{
				this.TurnOn(false);
				return;
			}
			this.TurnOff(false);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000B3BAC File Offset: 0x000B1DAC
		private void RpcReader___Observers_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(null, on);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000B3BE8 File Offset: 0x000B1DE8
		private void RpcWriter___Target_SetIsOn_214505783(NetworkConnection conn, bool on)
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
			writer.WriteBoolean(on);
			base.SendTargetRpc(10U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x000B3CA0 File Offset: 0x000B1EA0
		private void RpcReader___Target_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(base.LocalConnection, on);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000B3CD8 File Offset: 0x000B1ED8
		protected override void dll()
		{
			base.Awake();
			switch (this.StartupAction)
			{
			case ToggleableSurfaceItem.EStartupAction.TurnOn:
				this.TurnOn(true);
				return;
			case ToggleableSurfaceItem.EStartupAction.TurnOff:
				this.TurnOff(true);
				return;
			case ToggleableSurfaceItem.EStartupAction.Toggle:
				this.Toggle();
				return;
			default:
				return;
			}
		}

		// Token: 0x04001F68 RID: 8040
		[Header("Settings")]
		public ToggleableSurfaceItem.EStartupAction StartupAction;

		// Token: 0x04001F69 RID: 8041
		public UnityEvent onTurnedOn;

		// Token: 0x04001F6A RID: 8042
		public UnityEvent onTurnedOff;

		// Token: 0x04001F6B RID: 8043
		public UnityEvent onTurnOnOrOff;

		// Token: 0x04001F6C RID: 8044
		private bool dll_Excuted;

		// Token: 0x04001F6D RID: 8045
		private bool dll_Excuted;

		// Token: 0x02000676 RID: 1654
		public enum EStartupAction
		{
			// Token: 0x04001F6F RID: 8047
			None,
			// Token: 0x04001F70 RID: 8048
			TurnOn,
			// Token: 0x04001F71 RID: 8049
			TurnOff,
			// Token: 0x04001F72 RID: 8050
			Toggle
		}
	}
}
