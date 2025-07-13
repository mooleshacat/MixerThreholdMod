using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000867 RID: 2151
	public class TrashContainer : NetworkBehaviour
	{
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06003A99 RID: 15001 RVA: 0x000F82C0 File Offset: 0x000F64C0
		// (set) Token: 0x06003A9A RID: 15002 RVA: 0x000F82C8 File Offset: 0x000F64C8
		public TrashContent Content { get; protected set; } = new TrashContent();

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x000F82D1 File Offset: 0x000F64D1
		public int TrashLevel
		{
			get
			{
				return this.Content.GetTotalSize();
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003A9C RID: 15004 RVA: 0x000F82DE File Offset: 0x000F64DE
		public float NormalizedTrashLevel
		{
			get
			{
				return (float)this.Content.GetTotalSize() / (float)this.TrashCapacity;
			}
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x000F82F4 File Offset: 0x000F64F4
		public virtual void AddTrash(TrashItem item)
		{
			this.SendTrash(item.ID, 1);
			item.DestroyTrash();
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ContainedTrashItems", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("ContainedTrashItems") + 1f).ToString(), true);
			}
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x000F8348 File Offset: 0x000F6548
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.Content.GetTotalSize() > 0)
			{
				this.LoadContent(connection, this.Content.GetData());
			}
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x000F8371 File Offset: 0x000F6571
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendTrash(string trashID, int quantity)
		{
			this.RpcWriter___Server_SendTrash_3643459082(trashID, quantity);
			this.RpcLogic___SendTrash_3643459082(trashID, quantity);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x000F8390 File Offset: 0x000F6590
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void AddTrash(NetworkConnection conn, string trashID, int quantity)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_AddTrash_3905681115(conn, trashID, quantity);
				this.RpcLogic___AddTrash_3905681115(conn, trashID, quantity);
			}
			else
			{
				this.RpcWriter___Target_AddTrash_3905681115(conn, trashID, quantity);
			}
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x000F83DD File Offset: 0x000F65DD
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendClear()
		{
			this.RpcWriter___Server_SendClear_2166136261();
			this.RpcLogic___SendClear_2166136261();
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x000F83EB File Offset: 0x000F65EB
		[ObserversRpc(RunLocally = true)]
		private void Clear()
		{
			this.RpcWriter___Observers_Clear_2166136261();
			this.RpcLogic___Clear_2166136261();
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x000F83F9 File Offset: 0x000F65F9
		[TargetRpc]
		private void LoadContent(NetworkConnection conn, TrashContentData data)
		{
			this.RpcWriter___Target_LoadContent_189522235(conn, data);
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x000F840C File Offset: 0x000F660C
		public void TriggerEnter(Collider other)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.TrashLevel >= this.TrashCapacity)
			{
				return;
			}
			TrashItem componentInParent = other.GetComponentInParent<TrashItem>();
			if (componentInParent == null)
			{
				return;
			}
			if (!componentInParent.CanGoInContainer)
			{
				return;
			}
			this.AddTrash(componentInParent);
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x000F8451 File Offset: 0x000F6651
		public bool CanBeBagged()
		{
			return this.TrashLevel > 0;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x000F845C File Offset: 0x000F665C
		public void BagTrash()
		{
			NetworkSingleton<TrashManager>.Instance.CreateTrashBag(NetworkSingleton<TrashManager>.Instance.TrashBagPrefab.ID, this.TrashBagDropLocation.position, this.TrashBagDropLocation.rotation, this.Content.GetData(), this.TrashBagDropLocation.forward * 3f, "", false);
			this.SendClear();
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("TrashContainersBagged", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("TrashContainersBagged") + 1f).ToString(), true);
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x000F8510 File Offset: 0x000F6710
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendTrash_3643459082));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_AddTrash_3905681115));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_AddTrash_3905681115));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendClear_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Clear_2166136261));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_LoadContent_189522235));
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x000F85B8 File Offset: 0x000F67B8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x000F85CB File Offset: 0x000F67CB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x000F85DC File Offset: 0x000F67DC
		private void RpcWriter___Server_SendTrash_3643459082(string trashID, int quantity)
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
			writer.WriteString(trashID);
			writer.WriteInt32(quantity, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x000F8695 File Offset: 0x000F6895
		private void RpcLogic___SendTrash_3643459082(string trashID, int quantity)
		{
			this.AddTrash(null, trashID, quantity);
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x000F86A0 File Offset: 0x000F68A0
		private void RpcReader___Server_SendTrash_3643459082(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string trashID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendTrash_3643459082(trashID, quantity);
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x000F86F4 File Offset: 0x000F68F4
		private void RpcWriter___Observers_AddTrash_3905681115(NetworkConnection conn, string trashID, int quantity)
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
			writer.WriteString(trashID);
			writer.WriteInt32(quantity, 1);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x000F87BC File Offset: 0x000F69BC
		private void RpcLogic___AddTrash_3905681115(NetworkConnection conn, string trashID, int quantity)
		{
			this.Content.AddTrash(trashID, quantity);
			if (this.onTrashAdded != null)
			{
				this.onTrashAdded.Invoke(trashID);
			}
			if (this.onTrashLevelChanged != null)
			{
				this.onTrashLevelChanged.Invoke();
			}
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x000F87F4 File Offset: 0x000F69F4
		private void RpcReader___Observers_AddTrash_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string trashID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddTrash_3905681115(null, trashID, quantity);
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x000F8848 File Offset: 0x000F6A48
		private void RpcWriter___Target_AddTrash_3905681115(NetworkConnection conn, string trashID, int quantity)
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
			writer.WriteString(trashID);
			writer.WriteInt32(quantity, 1);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x000F8910 File Offset: 0x000F6B10
		private void RpcReader___Target_AddTrash_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string trashID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddTrash_3905681115(base.LocalConnection, trashID, quantity);
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x000F8960 File Offset: 0x000F6B60
		private void RpcWriter___Server_SendClear_2166136261()
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
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x000F89FA File Offset: 0x000F6BFA
		private void RpcLogic___SendClear_2166136261()
		{
			this.Clear();
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x000F8A04 File Offset: 0x000F6C04
		private void RpcReader___Server_SendClear_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendClear_2166136261();
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x000F8A34 File Offset: 0x000F6C34
		private void RpcWriter___Observers_Clear_2166136261()
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
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x000F8ADD File Offset: 0x000F6CDD
		private void RpcLogic___Clear_2166136261()
		{
			this.Content.Clear();
			if (this.onTrashLevelChanged != null)
			{
				this.onTrashLevelChanged.Invoke();
			}
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x000F8B00 File Offset: 0x000F6D00
		private void RpcReader___Observers_Clear_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Clear_2166136261();
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x000F8B2C File Offset: 0x000F6D2C
		private void RpcWriter___Target_LoadContent_189522235(NetworkConnection conn, TrashContentData data)
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
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(5U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x000F8BE1 File Offset: 0x000F6DE1
		private void RpcLogic___LoadContent_189522235(NetworkConnection conn, TrashContentData data)
		{
			this.Content.LoadFromData(data);
			if (this.onTrashLevelChanged != null)
			{
				this.onTrashLevelChanged.Invoke();
			}
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x000F8C04 File Offset: 0x000F6E04
		private void RpcReader___Target_LoadContent_189522235(PooledReader PooledReader0, Channel channel)
		{
			TrashContentData data = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___LoadContent_189522235(base.LocalConnection, data);
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x000F85CB File Offset: 0x000F67CB
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002A19 RID: 10777
		[Header("Settings")]
		[Range(1f, 50f)]
		public int TrashCapacity = 10;

		// Token: 0x04002A1A RID: 10778
		[Header("Settings")]
		public Transform TrashBagDropLocation;

		// Token: 0x04002A1B RID: 10779
		public UnityEvent<string> onTrashAdded;

		// Token: 0x04002A1C RID: 10780
		public UnityEvent onTrashLevelChanged;

		// Token: 0x04002A1D RID: 10781
		private bool dll_Excuted;

		// Token: 0x04002A1E RID: 10782
		private bool dll_Excuted;
	}
}
