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
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C44 RID: 3140
	public class PlaceableStorageEntity : GridItem, ITransitEntity, IStorageEntity, IUsable
	{
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600581F RID: 22559 RVA: 0x0017531E File Offset: 0x0017351E
		public Transform storedItemContainer
		{
			get
			{
				return this._storedItemContainer;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06005820 RID: 22560 RVA: 0x00175326 File Offset: 0x00173526
		// (set) Token: 0x06005821 RID: 22561 RVA: 0x0017532E File Offset: 0x0017352E
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

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06005822 RID: 22562 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06005823 RID: 22563 RVA: 0x00175337 File Offset: 0x00173537
		// (set) Token: 0x06005824 RID: 22564 RVA: 0x0017533F File Offset: 0x0017353F
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06005825 RID: 22565 RVA: 0x00175348 File Offset: 0x00173548
		// (set) Token: 0x06005826 RID: 22566 RVA: 0x00175350 File Offset: 0x00173550
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06005827 RID: 22567 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform LinkOrigin
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06005828 RID: 22568 RVA: 0x00175359 File Offset: 0x00173559
		// (set) Token: 0x06005829 RID: 22569 RVA: 0x00175361 File Offset: 0x00173561
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

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x0017536B File Offset: 0x0017356B
		// (set) Token: 0x0600582B RID: 22571 RVA: 0x00175373 File Offset: 0x00173573
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

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x0017537D File Offset: 0x0017357D
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x00175385 File Offset: 0x00173585
		public bool Selectable { get; } = 1;

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x0600582E RID: 22574 RVA: 0x0017538D File Offset: 0x0017358D
		// (set) Token: 0x0600582F RID: 22575 RVA: 0x00175395 File Offset: 0x00173595
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x06005830 RID: 22576 RVA: 0x001753A0 File Offset: 0x001735A0
		protected override void Start()
		{
			base.Start();
			for (int i = 0; i < this.StorageEntity.ItemSlots.Count; i++)
			{
				this.InputSlots.Add(this.StorageEntity.ItemSlots[i]);
				this.OutputSlots.Add(this.StorageEntity.ItemSlots[i]);
			}
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x00175406 File Offset: 0x00173606
		public List<StoredItem> GetStoredItems()
		{
			return new List<StoredItem>(this.storedItemContainer.GetComponentsInChildren<StoredItem>());
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x00175418 File Offset: 0x00173618
		public List<StorageGrid> GetStorageGrids()
		{
			return this.storageGrids;
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x00175420 File Offset: 0x00173620
		[ObserversRpc(RunLocally = true)]
		public void DestroyStoredItem(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			this.RpcWriter___Observers_DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x00175459 File Offset: 0x00173659
		[ServerRpc(RequireOwnership = false)]
		private void DestroyStoredItem_Server(int gridIndex, Coordinate coord, string jobID)
		{
			this.RpcWriter___Server_DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x0017546D File Offset: 0x0017366D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x00175483 File Offset: 0x00173683
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x00175499 File Offset: 0x00173699
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

		// Token: 0x06005838 RID: 22584 RVA: 0x001754D5 File Offset: 0x001736D5
		public override BuildableItemData GetBaseData()
		{
			return new PlaceableStorageData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.StorageEntity.ItemSlots));
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x00175564 File Offset: 0x00173764
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PlaceableStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PlaceableStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyStoredItem_3261517793));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_DestroyStoredItem_Server_3952619116));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.PlaceableStorageEntity));
		}

		// Token: 0x0600583B RID: 22587 RVA: 0x0017564C File Offset: 0x0017384C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PlaceableStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PlaceableStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x0017567B File Offset: 0x0017387B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x0017568C File Offset: 0x0017388C
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

		// Token: 0x0600583E RID: 22590 RVA: 0x00175770 File Offset: 0x00173970
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

		// Token: 0x0600583F RID: 22591 RVA: 0x00175830 File Offset: 0x00173A30
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

		// Token: 0x06005840 RID: 22592 RVA: 0x001758A4 File Offset: 0x00173AA4
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

		// Token: 0x06005841 RID: 22593 RVA: 0x0017596A File Offset: 0x00173B6A
		private void RpcLogic___DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
		{
			this.DestroyStoredItem(gridIndex, coord, jobID, false);
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x00175978 File Offset: 0x00173B78
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

		// Token: 0x06005843 RID: 22595 RVA: 0x001759D0 File Offset: 0x00173BD0
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

		// Token: 0x06005844 RID: 22596 RVA: 0x00175A77 File Offset: 0x00173C77
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x00175A80 File Offset: 0x00173C80
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

		// Token: 0x06005846 RID: 22598 RVA: 0x00175AC0 File Offset: 0x00173CC0
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

		// Token: 0x06005847 RID: 22599 RVA: 0x00175B67 File Offset: 0x00173D67
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x00175B70 File Offset: 0x00173D70
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

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06005849 RID: 22601 RVA: 0x00175BAE File Offset: 0x00173DAE
		// (set) Token: 0x0600584A RID: 22602 RVA: 0x00175BB6 File Offset: 0x00173DB6
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

		// Token: 0x0600584B RID: 22603 RVA: 0x00175BF4 File Offset: 0x00173DF4
		public override bool PlaceableStorageEntity(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600584C RID: 22604 RVA: 0x00175C8A File Offset: 0x00173E8A
		// (set) Token: 0x0600584D RID: 22605 RVA: 0x00175C92 File Offset: 0x00173E92
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

		// Token: 0x0600584E RID: 22606 RVA: 0x00175CCE File Offset: 0x00173ECE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040040B9 RID: 16569
		[Header("Reference")]
		[SerializeField]
		protected Transform _storedItemContainer;

		// Token: 0x040040BA RID: 16570
		public StorageEntity StorageEntity;

		// Token: 0x040040BB RID: 16571
		[SerializeField]
		protected List<StorageGrid> storageGrids = new List<StorageGrid>();

		// Token: 0x040040BC RID: 16572
		public Transform[] accessPoints;

		// Token: 0x040040BD RID: 16573
		protected Dictionary<StoredItem, Employee> _reservedItems = new Dictionary<StoredItem, Employee>();

		// Token: 0x040040C4 RID: 16580
		private List<string> completedJobs = new List<string>();

		// Token: 0x040040C5 RID: 16581
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x040040C6 RID: 16582
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x040040C7 RID: 16583
		private bool dll_Excuted;

		// Token: 0x040040C8 RID: 16584
		private bool dll_Excuted;
	}
}
