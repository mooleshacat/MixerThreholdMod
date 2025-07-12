using System;
using System.Collections.Generic;
using System.Linq;
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
using ScheduleOne.Management;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.Trash;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Employees
{
	// Token: 0x0200067D RID: 1661
	public class Cleaner : Employee, IConfigurable
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000B5F3A File Offset: 0x000B413A
		// (set) Token: 0x06002C0B RID: 11275 RVA: 0x000B5F42 File Offset: 0x000B4142
		public TrashGrabberInstance trashGrabberInstance { get; private set; }

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002C0C RID: 11276 RVA: 0x000B5F4B File Offset: 0x000B414B
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002C0D RID: 11277 RVA: 0x000B5F53 File Offset: 0x000B4153
		// (set) Token: 0x06002C0E RID: 11278 RVA: 0x000B5F5B File Offset: 0x000B415B
		protected CleanerConfiguration configuration { get; set; }

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002C0F RID: 11279 RVA: 0x000B5F64 File Offset: 0x000B4164
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002C10 RID: 11280 RVA: 0x000B5F6C File Offset: 0x000B416C
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Cleaner;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002C11 RID: 11281 RVA: 0x000B5F6F File Offset: 0x000B416F
		// (set) Token: 0x06002C12 RID: 11282 RVA: 0x000B5F77 File Offset: 0x000B4177
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000B5F80 File Offset: 0x000B4180
		// (set) Token: 0x06002C14 RID: 11284 RVA: 0x000B5F88 File Offset: 0x000B4188
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

		// Token: 0x06002C15 RID: 11285 RVA: 0x000B5F92 File Offset: 0x000B4192
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002C16 RID: 11286 RVA: 0x000B5FA8 File Offset: 0x000B41A8
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002C17 RID: 11287 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002C18 RID: 11288 RVA: 0x000B5FB0 File Offset: 0x000B41B0
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002C19 RID: 11289 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002C1A RID: 11290 RVA: 0x000B3D8F File Offset: 0x000B1F8F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000B5FB8 File Offset: 0x000B41B8
		protected override void AssignProperty(Property prop, bool warp)
		{
			base.AssignProperty(prop, warp);
			prop.AddConfigurable(this);
			this.configuration = new CleanerConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000B432F File Offset: 0x000B252F
		protected override void UnassignProperty()
		{
			base.AssignedProperty.RemoveConfigurable(this);
			base.UnassignProperty();
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000B5FE3 File Offset: 0x000B41E3
		protected override void ResetConfiguration()
		{
			if (this.configuration != null)
			{
				this.configuration.Reset();
			}
			base.ResetConfiguration();
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000B5FFE File Offset: 0x000B41FE
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

		// Token: 0x06002C1F RID: 11295 RVA: 0x000B6039 File Offset: 0x000B4239
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000B604C File Offset: 0x000B424C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Cleaner.<>c__DisplayClass48_0 CS$<>8__locals1 = new Cleaner.<>c__DisplayClass48_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000B608C File Offset: 0x000B428C
		protected override void MinPass()
		{
			base.MinPass();
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			if (this.AnyWorkInProgress())
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
			if (this.configuration.binItems.Count == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned any trash cans", "You can use your management clipboards to assign trash cans to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.TryStartNewTask();
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000B610C File Offset: 0x000B430C
		private void TryStartNewTask()
		{
			TrashContainerItem[] trashContainersOrderedByDistance = this.GetTrashContainersOrderedByDistance();
			this.EnsureTrashGrabberInInventory();
			foreach (TrashContainerItem trashContainerItem in trashContainersOrderedByDistance)
			{
				if (trashContainerItem.TrashBagsInRadius.Count > 0)
				{
					if (base.AssignedProperty.DisposalArea != null)
					{
						TrashBag targetBag = trashContainerItem.TrashBagsInRadius[0];
						this.DisposeTrashBagBehaviour.SetTargetBag(targetBag);
						this.DisposeTrashBagBehaviour.Enable_Networked(null);
						return;
					}
					Console.LogError("No disposal area assigned to property " + base.AssignedProperty.PropertyCode, null);
				}
			}
			if (this.GetTrashGrabberAmount() < 20)
			{
				foreach (TrashContainerItem trashContainerItem2 in trashContainersOrderedByDistance)
				{
					if (trashContainerItem2.TrashItemsInRadius.Count > 0)
					{
						int num = 0;
						TrashItem trashItem = trashContainerItem2.TrashItemsInRadius[num];
						while (trashItem == null || !this.movement.CanGetTo(trashItem.transform.position, 1f))
						{
							num++;
							if (num >= trashContainerItem2.TrashItemsInRadius.Count)
							{
								trashItem = null;
								break;
							}
							trashItem = trashContainerItem2.TrashItemsInRadius[num];
						}
						if (trashItem != null)
						{
							this.PickUpTrashBehaviour.SetTargetTrash(trashItem);
							this.PickUpTrashBehaviour.Enable_Networked(null);
							return;
						}
					}
				}
			}
			if (this.GetTrashGrabberAmount() >= 20 && this.GetFirstNonFullBin(trashContainersOrderedByDistance) != null)
			{
				this.EmptyTrashGrabberBehaviour.SetTargetTrashCan(this.GetFirstNonFullBin(trashContainersOrderedByDistance));
				this.EmptyTrashGrabberBehaviour.Enable_Networked(null);
				return;
			}
			foreach (TrashContainerItem trashContainerItem3 in trashContainersOrderedByDistance)
			{
				if (trashContainerItem3.Container.NormalizedTrashLevel >= 0.75f)
				{
					this.BagTrashCanBehaviour.SetTargetTrashCan(trashContainerItem3);
					this.BagTrashCanBehaviour.Enable_Networked(null);
					return;
				}
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", string.Empty, 0);
			this.SetIdle(true);
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000B62F6 File Offset: 0x000B44F6
		private TrashContainerItem GetFirstNonFullBin(TrashContainerItem[] bins)
		{
			return bins.FirstOrDefault((TrashContainerItem bin) => bin.Container.NormalizedTrashLevel < 1f);
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000B631D File Offset: 0x000B451D
		public override void SetIdle(bool idle)
		{
			base.SetIdle(idle);
			if (idle && this.Avatar.CurrentEquippable != null)
			{
				base.SetEquippable_Return(string.Empty);
			}
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000B6348 File Offset: 0x000B4548
		private TrashContainerItem[] GetTrashContainersOrderedByDistance()
		{
			TrashContainerItem[] array = this.configuration.binItems.ToArray();
			Array.Sort<TrashContainerItem>(array, delegate(TrashContainerItem x, TrashContainerItem y)
			{
				float num = Vector3.Distance(x.transform.position, base.transform.position);
				float value = Vector3.Distance(y.transform.position, base.transform.position);
				return num.CompareTo(value);
			});
			return array;
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000B636C File Offset: 0x000B456C
		public override EmployeeHome GetHome()
		{
			return this.configuration.assignedHome;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000B637C File Offset: 0x000B457C
		private void EnsureTrashGrabberInInventory()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Inventory._GetItemAmount(this.TrashGrabberDef.ID) == 0)
			{
				base.Inventory.InsertItem(this.TrashGrabberDef.GetDefaultInstance(1), true);
			}
			this.trashGrabberInstance = (base.Inventory.GetFirstItem(this.TrashGrabberDef.ID, null) as TrashGrabberInstance);
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000B63E4 File Offset: 0x000B45E4
		private bool AnyWorkInProgress()
		{
			return this.PickUpTrashBehaviour.Active || this.EmptyTrashGrabberBehaviour.Active || this.BagTrashCanBehaviour.Active || this.DisposeTrashBagBehaviour.Active || this.MoveItemBehaviour.Active;
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000B643D File Offset: 0x000B463D
		private int GetTrashGrabberAmount()
		{
			return this.trashGrabberInstance.GetTotalSize();
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000B644C File Offset: 0x000B464C
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
			CleanerUIElement component = UnityEngine.Object.Instantiate<CleanerUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<CleanerUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000B64D7 File Offset: 0x000B46D7
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000B64F4 File Offset: 0x000B46F4
		public override NPCData GetNPCData()
		{
			return new CleanerData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData());
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000B655C File Offset: 0x000B475C
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			saveData.AddData("Configuration", this.Configuration.GetSaveString());
			return saveData;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000594B4 File Offset: 0x000576B4
		public override List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000B65CC File Offset: 0x000B47CC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(42U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Cleaner));
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x000B6644 File Offset: 0x000B4844
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.CleanerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000B6668 File Offset: 0x000B4868
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000B6678 File Offset: 0x000B4878
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

		// Token: 0x06002C35 RID: 11317 RVA: 0x000B671F File Offset: 0x000B491F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000B6728 File Offset: 0x000B4928
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

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x000B6766 File Offset: 0x000B4966
		// (set) Token: 0x06002C38 RID: 11320 RVA: 0x000B676E File Offset: 0x000B496E
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

		// Token: 0x06002C39 RID: 11321 RVA: 0x000B67AC File Offset: 0x000B49AC
		public override bool Cleaner(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x06002C3A RID: 11322 RVA: 0x000B67FE File Offset: 0x000B49FE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001FA9 RID: 8105
		public const int MAX_ASSIGNED_BINS = 3;

		// Token: 0x04001FAA RID: 8106
		public TrashGrabberDefinition TrashGrabberDef;

		// Token: 0x04001FAB RID: 8107
		[Header("References")]
		public PickUpTrashBehaviour PickUpTrashBehaviour;

		// Token: 0x04001FAC RID: 8108
		public EmptyTrashGrabberBehaviour EmptyTrashGrabberBehaviour;

		// Token: 0x04001FAD RID: 8109
		public BagTrashCanBehaviour BagTrashCanBehaviour;

		// Token: 0x04001FAE RID: 8110
		public DisposeTrashBagBehaviour DisposeTrashBagBehaviour;

		// Token: 0x04001FAF RID: 8111
		public Sprite typeIcon;

		// Token: 0x04001FB0 RID: 8112
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001FB1 RID: 8113
		[Header("UI")]
		public CleanerUIElement WorldspaceUIPrefab;

		// Token: 0x04001FB2 RID: 8114
		public Transform uiPoint;

		// Token: 0x04001FB7 RID: 8119
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001FB8 RID: 8120
		private bool dll_Excuted;

		// Token: 0x04001FB9 RID: 8121
		private bool dll_Excuted;
	}
}
