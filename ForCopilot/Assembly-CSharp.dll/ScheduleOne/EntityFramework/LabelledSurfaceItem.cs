using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x0200066A RID: 1642
	public class LabelledSurfaceItem : SurfaceItem
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000B16EB File Offset: 0x000AF8EB
		// (set) Token: 0x06002ADC RID: 10972 RVA: 0x000B16F3 File Offset: 0x000AF8F3
		public string Message { get; private set; } = "Your Message Here";

		// Token: 0x06002ADD RID: 10973 RVA: 0x000B16FC File Offset: 0x000AF8FC
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			this.SetMessage(connection, this.Message);
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000B171B File Offset: 0x000AF91B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMessageToServer(string message)
		{
			this.RpcWriter___Server_SendMessageToServer_3615296227(message);
			this.RpcLogic___SendMessageToServer_3615296227(message);
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000B1731 File Offset: 0x000AF931
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetMessage(NetworkConnection conn, string message)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetMessage_2971853958(conn, message);
				this.RpcLogic___SetMessage_2971853958(conn, message);
			}
			else
			{
				this.RpcWriter___Target_SetMessage_2971853958(conn, message);
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000B1767 File Offset: 0x000AF967
		public void Interacted()
		{
			Singleton<TextInputScreen>.Instance.Open("Edit Sign Message", this.Message, new TextInputScreen.OnSubmit(this.MessageSubmitted), this.MaxCharacters);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000B1790 File Offset: 0x000AF990
		private void MessageSubmitted(string message)
		{
			this.SendMessageToServer(message);
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000B179C File Offset: 0x000AF99C
		public override BuildableItemData GetBaseData()
		{
			return new LabelledSurfaceItemData(base.GUID, base.ItemInstance, 0, base.ParentSurface.GUID.ToString(), this.RelativePosition, this.RelativeRotation, this.Message);
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000B1804 File Offset: 0x000AFA04
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.LabelledSurfaceItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.LabelledSurfaceItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendMessageToServer_3615296227));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetMessage_2971853958));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetMessage_2971853958));
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000B186D File Offset: 0x000AFA6D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.LabelledSurfaceItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.LabelledSurfaceItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000B1886 File Offset: 0x000AFA86
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x000B1894 File Offset: 0x000AFA94
		private void RpcWriter___Server_SendMessageToServer_3615296227(string message)
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
			writer.WriteString(message);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000B193B File Offset: 0x000AFB3B
		public void RpcLogic___SendMessageToServer_3615296227(string message)
		{
			this.SetMessage(null, message);
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000B1948 File Offset: 0x000AFB48
		private void RpcReader___Server_SendMessageToServer_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMessageToServer_3615296227(message);
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000B1988 File Offset: 0x000AFB88
		private void RpcWriter___Observers_SetMessage_2971853958(NetworkConnection conn, string message)
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
			writer.WriteString(message);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000B1A3E File Offset: 0x000AFC3E
		public void RpcLogic___SetMessage_2971853958(NetworkConnection conn, string message)
		{
			this.Message = message;
			this.Label.text = message;
			base.HasChanged = true;
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000B1A5C File Offset: 0x000AFC5C
		private void RpcReader___Observers_SetMessage_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMessage_2971853958(null, message);
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000B1A98 File Offset: 0x000AFC98
		private void RpcWriter___Target_SetMessage_2971853958(NetworkConnection conn, string message)
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
			writer.WriteString(message);
			base.SendTargetRpc(10U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000B1B50 File Offset: 0x000AFD50
		private void RpcReader___Target_SetMessage_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetMessage_2971853958(base.LocalConnection, message);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000B1B87 File Offset: 0x000AFD87
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001F36 RID: 7990
		public int MaxCharacters = 100;

		// Token: 0x04001F37 RID: 7991
		[Header("References")]
		public TextMeshPro Label;

		// Token: 0x04001F38 RID: 7992
		private bool dll_Excuted;

		// Token: 0x04001F39 RID: 7993
		private bool dll_Excuted;
	}
}
