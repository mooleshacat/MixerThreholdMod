using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C4C RID: 3148
	public class SurfaceStorageEntity : SurfaceItem, IStorageEntity, IUsable
	{
		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060058AE RID: 22702 RVA: 0x0017721A File Offset: 0x0017541A
		public Transform storedItemContainer
		{
			get
			{
				return this._storedItemContainer;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x060058AF RID: 22703 RVA: 0x00177222 File Offset: 0x00175422
		// (set) Token: 0x060058B0 RID: 22704 RVA: 0x0017722A File Offset: 0x0017542A
		public Dictionary<StoredItem, Employee> reservedItems
		{
			get
			{
				return this._reservedItems;
			}
			set
			{
				this._reservedItems = value;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x060058B1 RID: 22705 RVA: 0x00177233 File Offset: 0x00175433
		// (set) Token: 0x060058B2 RID: 22706 RVA: 0x0017723B File Offset: 0x0017543B
		public NetworkObject NPCUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<NPCUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<NPCUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060058B3 RID: 22707 RVA: 0x00177245 File Offset: 0x00175445
		// (set) Token: 0x060058B4 RID: 22708 RVA: 0x0017724D File Offset: 0x0017544D
		public NetworkObject PlayerUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060058B5 RID: 22709 RVA: 0x00177257 File Offset: 0x00175457
		public bool Selectable { get; } = 1;

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060058B6 RID: 22710 RVA: 0x0017725F File Offset: 0x0017545F
		// (set) Token: 0x060058B7 RID: 22711 RVA: 0x00177267 File Offset: 0x00175467
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x060058B8 RID: 22712 RVA: 0x00177270 File Offset: 0x00175470
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x00177278 File Offset: 0x00175478
		public List<StoredItem> GetStoredItems()
		{
			return new List<StoredItem>(this.storedItemContainer.GetComponentsInChildren<StoredItem>());
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x0017728A File Offset: 0x0017548A
		public List<StorageGrid> GetStorageGrids()
		{
			return this.storageGrids;
		}

		// Token: 0x060058BB RID: 22715 RVA: 0x00177294 File Offset: 0x00175494
		[ObserversRpc(RunLocally = true)]
		public void DestroyStoredItem(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			this.RpcWriter___Observers_DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x001772CD File Offset: 0x001754CD
		[ServerRpc(RequireOwnership = false)]
		private void DestroyStoredItem_Server(int gridIndex, Coordinate coord, string jobID)
		{
			this.RpcWriter___Server_DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x001772E1 File Offset: 0x001754E1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x001772F7 File Offset: 0x001754F7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x0017730D File Offset: 0x0017550D
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.StorageEntity.CurrentAccessor != null)
			{
				reason = "In use by other player";
				return false;
			}
			if (this.StorageEntity.ItemCount > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x0017734C File Offset: 0x0017554C
		public override BuildableItemData GetBaseData()
		{
			return new StorageSurfaceItemData(base.GUID, base.ItemInstance, 0, base.ParentSurface.GUID.ToString(), this.RelativePosition, this.RelativeRotation, new ItemSet(this.StorageEntity.ItemSlots));
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x001773D8 File Offset: 0x001755D8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SurfaceStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SurfaceStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyStoredItem_3261517793));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_DestroyStoredItem_Server_3952619116));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.SurfaceStorageEntity));
		}

		// Token: 0x060058C3 RID: 22723 RVA: 0x001774C0 File Offset: 0x001756C0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.SurfaceStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.SurfaceStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x001774EF File Offset: 0x001756EF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x00177500 File Offset: 0x00175700
		private void RpcWriter___Observers_DestroyStoredItem_3261517793(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
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
			writer.WriteInt32(gridIndex, 1);
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(coord);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x001775E4 File Offset: 0x001757E4
		public void RpcLogic___DestroyStoredItem_3261517793(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			if (jobID != "")
			{
				if (this.completedJobs.Contains(jobID))
				{
					return;
				}
			}
			else
			{
				jobID = Guid.NewGuid().ToString();
			}
			this.completedJobs.Add(jobID);
			List<StorageGrid> list = this.GetStorageGrids();
			if (gridIndex > list.Count)
			{
				Console.LogError("DestroyStoredItem: grid index out of range", null);
				return;
			}
			if (list[gridIndex].GetTile(coord) == null)
			{
				Console.LogError("DestroyStoredItem: no tile found at " + ((coord != null) ? coord.ToString() : null), null);
				return;
			}
			list[gridIndex].GetTile(coord).occupant.Destroy_Internal();
			if (network)
			{
				this.DestroyStoredItem_Server(gridIndex, coord, jobID);
			}
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x001776A4 File Offset: 0x001758A4
		private void RpcReader___Observers_DestroyStoredItem_3261517793(PooledReader PooledReader0, Channel channel)
		{
			int gridIndex = PooledReader0.ReadInt32(1);
			Coordinate coord = GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(PooledReader0);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x00177718 File Offset: 0x00175918
		private void RpcWriter___Server_DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
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
			writer.WriteInt32(gridIndex, 1);
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(coord);
			writer.WriteString(jobID);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060058C9 RID: 22729 RVA: 0x001777DE File Offset: 0x001759DE
		private void RpcLogic___DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
		{
			this.DestroyStoredItem(gridIndex, coord, jobID, false);
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x001777EC File Offset: 0x001759EC
		private void RpcReader___Server_DestroyStoredItem_Server_3952619116(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int gridIndex = PooledReader0.ReadInt32(1);
			Coordinate coord = GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(PooledReader0);
			string jobID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x00177844 File Offset: 0x00175A44
		private void RpcWriter___Server_SetPlayerUser_3323014238(NetworkObject playerObject)
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
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x001778EB File Offset: 0x00175AEB
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x001778F4 File Offset: 0x00175AF4
		private void RpcReader___Server_SetPlayerUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x00177934 File Offset: 0x00175B34
		private void RpcWriter___Server_SetNPCUser_3323014238(NetworkObject npcObject)
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
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x001779DB File Offset: 0x00175BDB
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x060058D0 RID: 22736 RVA: 0x001779E4 File Offset: 0x00175BE4
		private void RpcReader___Server_SetNPCUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x060058D1 RID: 22737 RVA: 0x00177A22 File Offset: 0x00175C22
		// (set) Token: 0x060058D2 RID: 22738 RVA: 0x00177A2A File Offset: 0x00175C2A
		public NetworkObject SyncAccessor_<NPCUserObject>k__BackingField
		{
			get
			{
				return this.<NPCUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<NPCUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<NPCUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x00177A68 File Offset: 0x00175C68
		public override bool SurfaceStorageEntity(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerUserObject>k__BackingField(this.syncVar___<PlayerUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060058D4 RID: 22740 RVA: 0x00177AFE File Offset: 0x00175CFE
		// (set) Token: 0x060058D5 RID: 22741 RVA: 0x00177B06 File Offset: 0x00175D06
		public NetworkObject SyncAccessor_<PlayerUserObject>k__BackingField
		{
			get
			{
				return this.<PlayerUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x00177B42 File Offset: 0x00175D42
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040040FD RID: 16637
		[Header("Reference")]
		[SerializeField]
		protected Transform _storedItemContainer;

		// Token: 0x040040FE RID: 16638
		public StorageEntity StorageEntity;

		// Token: 0x040040FF RID: 16639
		[SerializeField]
		protected List<StorageGrid> storageGrids = new List<StorageGrid>();

		// Token: 0x04004100 RID: 16640
		protected Dictionary<StoredItem, Employee> _reservedItems = new Dictionary<StoredItem, Employee>();

		// Token: 0x04004105 RID: 16645
		private List<string> completedJobs = new List<string>();

		// Token: 0x04004106 RID: 16646
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04004107 RID: 16647
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04004108 RID: 16648
		private bool dll_Excuted;

		// Token: 0x04004109 RID: 16649
		private bool dll_Excuted;
	}
}
