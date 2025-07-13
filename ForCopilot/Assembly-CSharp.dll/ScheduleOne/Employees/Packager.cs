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
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Employees
{
	// Token: 0x02000688 RID: 1672
	public class Packager : Employee, IConfigurable
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000B8C6C File Offset: 0x000B6E6C
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000B8C74 File Offset: 0x000B6E74
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x000B8C7C File Offset: 0x000B6E7C
		protected PackagerConfiguration configuration { get; set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x000B8C85 File Offset: 0x000B6E85
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002CC7 RID: 11463 RVA: 0x000B8C8D File Offset: 0x000B6E8D
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Packager;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x000B8C90 File Offset: 0x000B6E90
		// (set) Token: 0x06002CC9 RID: 11465 RVA: 0x000B8C98 File Offset: 0x000B6E98
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002CCA RID: 11466 RVA: 0x000B8CA1 File Offset: 0x000B6EA1
		// (set) Token: 0x06002CCB RID: 11467 RVA: 0x000B8CA9 File Offset: 0x000B6EA9
		public NetworkObject CurrentPlayerConfigurer
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentPlayerConfigurer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, true);
			}
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000B8CB3 File Offset: 0x000B6EB3
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002CCD RID: 11469 RVA: 0x000B8CC9 File Offset: 0x000B6EC9
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000B8CD1 File Offset: 0x000B6ED1
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x000B3D8F File Offset: 0x000B1F8F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000B8CD9 File Offset: 0x000B6ED9
		protected override void AssignProperty(Property prop, bool warp)
		{
			base.AssignProperty(prop, warp);
			prop.AddConfigurable(this);
			this.configuration = new PackagerConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000B432F File Offset: 0x000B252F
		protected override void UnassignProperty()
		{
			base.AssignedProperty.RemoveConfigurable(this);
			base.UnassignProperty();
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000B8D04 File Offset: 0x000B6F04
		protected override void ResetConfiguration()
		{
			if (this.configuration != null)
			{
				this.configuration.Reset();
			}
			base.ResetConfiguration();
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000B8D1F File Offset: 0x000B6F1F
		protected override void Fire()
		{
			if (this.configuration != null)
			{
				this.configuration.Destroy();
				this.DestroyWorldspaceUI();
				if (base.AssignedProperty != null)
				{
					base.AssignedProperty.RemoveConfigurable(this);
				}
			}
			base.Fire();
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000B8D5A File Offset: 0x000B6F5A
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000B8D6C File Offset: 0x000B6F6C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Packager.<>c__DisplayClass42_0 CS$<>8__locals1 = new Packager.<>c__DisplayClass42_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000B8DAC File Offset: 0x000B6FAC
		protected override void UpdateBehaviour()
		{
			base.UpdateBehaviour();
			if (this.PackagingBehaviour.Active)
			{
				base.MarkIsWorking();
				return;
			}
			if (this.MoveItemBehaviour.Active)
			{
				base.MarkIsWorking();
				return;
			}
			if (base.Fired)
			{
				base.LeavePropertyAndDespawn();
				return;
			}
			if (!base.CanWork())
			{
				return;
			}
			if (this.configuration.AssignedStationCount + this.configuration.Routes.Routes.Count == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned to any stations or routes.", "You can use your management clipboards to assign stations or routes to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			PackagingStation stationToAttend = this.GetStationToAttend();
			if (stationToAttend != null)
			{
				this.StartPackaging(stationToAttend);
				return;
			}
			BrickPress brickPress = this.GetBrickPress();
			if (brickPress != null)
			{
				this.StartPress(brickPress);
				return;
			}
			PackagingStation stationMoveItems = this.GetStationMoveItems();
			if (stationMoveItems != null)
			{
				this.StartMoveItem(stationMoveItems);
				return;
			}
			BrickPress brickPressMoveItems = this.GetBrickPressMoveItems();
			if (brickPressMoveItems != null)
			{
				this.StartMoveItem(brickPressMoveItems);
				return;
			}
			ItemInstance itemInstance;
			AdvancedTransitRoute transitRouteReady = this.GetTransitRouteReady(out itemInstance);
			if (transitRouteReady != null)
			{
				this.MoveItemBehaviour.Initialize(transitRouteReady, itemInstance, itemInstance.Quantity, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", "I need one of my assigned stations to have enough product and packaging to get to work.", 0);
			this.SetIdle(true);
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000B8EED File Offset: 0x000B70ED
		private void StartPackaging(PackagingStation station)
		{
			Console.Log("Starting packaging at " + station.gameObject.name, null);
			this.PackagingBehaviour.AssignStation(station);
			this.PackagingBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000B8F22 File Offset: 0x000B7122
		private void StartPress(BrickPress press)
		{
			this.BrickPressBehaviour.AssignStation(press);
			this.BrickPressBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000B8F3C File Offset: 0x000B713C
		private void StartMoveItem(PackagingStation station)
		{
			Console.Log("Starting moving items from " + station.gameObject.name, null);
			this.MoveItemBehaviour.Initialize((station.Configuration as PackagingStationConfiguration).DestinationRoute, station.OutputSlot.ItemInstance, -1, false);
			this.MoveItemBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000B8F98 File Offset: 0x000B7198
		private void StartMoveItem(BrickPress press)
		{
			this.MoveItemBehaviour.Initialize((press.Configuration as BrickPressConfiguration).DestinationRoute, press.OutputSlot.ItemInstance, -1, false);
			this.MoveItemBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000B8FD0 File Offset: 0x000B71D0
		protected PackagingStation GetStationToAttend()
		{
			foreach (PackagingStation packagingStation in this.configuration.AssignedStations)
			{
				if (this.PackagingBehaviour.IsStationReady(packagingStation))
				{
					return packagingStation;
				}
			}
			return null;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000B9038 File Offset: 0x000B7238
		protected BrickPress GetBrickPress()
		{
			foreach (BrickPress brickPress in this.configuration.AssignedBrickPresses)
			{
				if (this.BrickPressBehaviour.IsStationReady(brickPress))
				{
					return brickPress;
				}
			}
			return null;
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000B90A0 File Offset: 0x000B72A0
		protected PackagingStation GetStationMoveItems()
		{
			foreach (PackagingStation packagingStation in this.configuration.AssignedStations)
			{
				ItemSlot outputSlot = packagingStation.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((packagingStation.Configuration as PackagingStationConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					return packagingStation;
				}
			}
			return null;
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000B9130 File Offset: 0x000B7330
		protected BrickPress GetBrickPressMoveItems()
		{
			foreach (BrickPress brickPress in this.configuration.AssignedBrickPresses)
			{
				ItemSlot outputSlot = brickPress.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((brickPress.Configuration as BrickPressConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					return brickPress;
				}
			}
			return null;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000B91C0 File Offset: 0x000B73C0
		protected AdvancedTransitRoute GetTransitRouteReady(out ItemInstance item)
		{
			item = null;
			foreach (AdvancedTransitRoute advancedTransitRoute in this.configuration.Routes.Routes)
			{
				item = advancedTransitRoute.GetItemReadyToMove();
				if (item != null && this.movement.CanGetTo(advancedTransitRoute.Source, 1f) && this.movement.CanGetTo(advancedTransitRoute.Destination, 1f) && base.Inventory.GetCapacityForItem(item) > 0)
				{
					return advancedTransitRoute;
				}
			}
			return null;
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000B926C File Offset: 0x000B746C
		protected override bool ShouldIdle()
		{
			return this.configuration.AssignedStationCount == 0 || base.ShouldIdle();
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000B9283 File Offset: 0x000B7483
		public override EmployeeHome GetHome()
		{
			return this.configuration.assignedHome;
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000B9290 File Offset: 0x000B7490
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			Property assignedProperty = base.AssignedProperty;
			if (assignedProperty == null)
			{
				Property property = assignedProperty;
				Console.LogError(((property != null) ? property.ToString() : null) + " is not a child of a property!", null);
				return null;
			}
			PackagerUIElement component = UnityEngine.Object.Instantiate<PackagerUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<PackagerUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000B931B File Offset: 0x000B751B
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000B9338 File Offset: 0x000B7538
		public override NPCData GetNPCData()
		{
			return new PackagerData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData());
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000B93A0 File Offset: 0x000B75A0
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			saveData.AddData("Configuration", this.Configuration.GetSaveString());
			return saveData;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000594B4 File Offset: 0x000576B4
		public override List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000B93D8 File Offset: 0x000B75D8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(42U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Packager));
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000B9450 File Offset: 0x000B7650
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.PackagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000B9474 File Offset: 0x000B7674
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000B9484 File Offset: 0x000B7684
		private void RpcWriter___Server_SetConfigurer_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(42U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000B952B File Offset: 0x000B772B
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000B9534 File Offset: 0x000B7734
		private void RpcReader___Server_SetConfigurer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x000B9572 File Offset: 0x000B7772
		// (set) Token: 0x06002CF1 RID: 11505 RVA: 0x000B957A File Offset: 0x000B777A
		public NetworkObject SyncAccessor_<CurrentPlayerConfigurer>k__BackingField
		{
			get
			{
				return this.<CurrentPlayerConfigurer>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentPlayerConfigurer>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000B95B8 File Offset: 0x000B77B8
		public override bool Packager(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 2U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(this.syncVar___<CurrentPlayerConfigurer>k__BackingField.GetValue(true), true);
				return true;
			}
			NetworkObject value = PooledReader0.ReadNetworkObject();
			this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000B960A File Offset: 0x000B780A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002007 RID: 8199
		[Header("References")]
		public Sprite typeIcon;

		// Token: 0x04002008 RID: 8200
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04002009 RID: 8201
		public PackagingStationBehaviour PackagingBehaviour;

		// Token: 0x0400200A RID: 8202
		public BrickPressBehaviour BrickPressBehaviour;

		// Token: 0x0400200B RID: 8203
		[Header("UI")]
		public PackagerUIElement WorldspaceUIPrefab;

		// Token: 0x0400200C RID: 8204
		public Transform uiPoint;

		// Token: 0x0400200D RID: 8205
		[Header("Settings")]
		public int MaxAssignedStations = 3;

		// Token: 0x0400200E RID: 8206
		[Header("Proficiency Settings")]
		public float PackagingSpeedMultiplier = 1f;

		// Token: 0x04002012 RID: 8210
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04002013 RID: 8211
		private bool dll_Excuted;

		// Token: 0x04002014 RID: 8212
		private bool dll_Excuted;
	}
}
