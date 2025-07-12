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
	// Token: 0x02000673 RID: 1651
	public class ToggleableItem : GridItem
	{
		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000B3266 File Offset: 0x000B1466
		// (set) Token: 0x06002B52 RID: 11090 RVA: 0x000B326E File Offset: 0x000B146E
		public bool IsOn { get; private set; }

		// Token: 0x06002B53 RID: 11091 RVA: 0x000B3278 File Offset: 0x000B1478
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.ToggleableItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000B3297 File Offset: 0x000B1497
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsOn)
			{
				this.SetIsOn(connection, true);
			}
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000B32B0 File Offset: 0x000B14B0
		public void Toggle()
		{
			if (this.IsOn)
			{
				this.TurnOff(true);
				return;
			}
			this.TurnOn(true);
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000B32CC File Offset: 0x000B14CC
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

		// Token: 0x06002B57 RID: 11095 RVA: 0x000B331C File Offset: 0x000B151C
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

		// Token: 0x06002B58 RID: 11096 RVA: 0x000B336A File Offset: 0x000B156A
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendIsOn(bool on)
		{
			this.RpcWriter___Server_SendIsOn_1140765316(on);
			this.RpcLogic___SendIsOn_1140765316(on);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000B3380 File Offset: 0x000B1580
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

		// Token: 0x06002B5A RID: 11098 RVA: 0x000B33B6 File Offset: 0x000B15B6
		public override BuildableItemData GetBaseData()
		{
			return new ToggleableItemData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.IsOn);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000B33EC File Offset: 0x000B15EC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendIsOn_1140765316));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOn_214505783));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOn_214505783));
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000B3455 File Offset: 0x000B1655
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.ToggleableItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000B346E File Offset: 0x000B166E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000B347C File Offset: 0x000B167C
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

		// Token: 0x06002B60 RID: 11104 RVA: 0x000B3523 File Offset: 0x000B1723
		private void RpcLogic___SendIsOn_1140765316(bool on)
		{
			base.HasChanged = true;
			this.SetIsOn(null, on);
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000B3534 File Offset: 0x000B1734
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

		// Token: 0x06002B62 RID: 11106 RVA: 0x000B3574 File Offset: 0x000B1774
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

		// Token: 0x06002B63 RID: 11107 RVA: 0x000B362A File Offset: 0x000B182A
		private void RpcLogic___SetIsOn_214505783(NetworkConnection conn, bool on)
		{
			if (on)
			{
				this.TurnOn(false);
				return;
			}
			this.TurnOff(false);
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000B3640 File Offset: 0x000B1840
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

		// Token: 0x06002B65 RID: 11109 RVA: 0x000B367C File Offset: 0x000B187C
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

		// Token: 0x06002B66 RID: 11110 RVA: 0x000B3734 File Offset: 0x000B1934
		private void RpcReader___Target_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(base.LocalConnection, on);
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000B376C File Offset: 0x000B196C
		protected override void dll()
		{
			base.Awake();
			switch (this.StartupAction)
			{
			case ToggleableItem.EStartupAction.TurnOn:
				this.TurnOn(true);
				return;
			case ToggleableItem.EStartupAction.TurnOff:
				this.TurnOff(true);
				return;
			case ToggleableItem.EStartupAction.Toggle:
				this.Toggle();
				return;
			default:
				return;
			}
		}

		// Token: 0x04001F5C RID: 8028
		[Header("Settings")]
		public ToggleableItem.EStartupAction StartupAction;

		// Token: 0x04001F5D RID: 8029
		public UnityEvent onTurnedOn;

		// Token: 0x04001F5E RID: 8030
		public UnityEvent onTurnedOff;

		// Token: 0x04001F5F RID: 8031
		public UnityEvent onTurnOnOrOff;

		// Token: 0x04001F60 RID: 8032
		private bool dll_Excuted;

		// Token: 0x04001F61 RID: 8033
		private bool dll_Excuted;

		// Token: 0x02000674 RID: 1652
		public enum EStartupAction
		{
			// Token: 0x04001F63 RID: 8035
			None,
			// Token: 0x04001F64 RID: 8036
			TurnOn,
			// Token: 0x04001F65 RID: 8037
			TurnOff,
			// Token: 0x04001F66 RID: 8038
			Toggle
		}
	}
}
