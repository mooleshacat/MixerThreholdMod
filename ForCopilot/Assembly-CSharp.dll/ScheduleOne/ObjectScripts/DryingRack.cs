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
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C26 RID: 3110
	public class DryingRack : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600551A RID: 21786 RVA: 0x00167C70 File Offset: 0x00165E70
		// (set) Token: 0x0600551B RID: 21787 RVA: 0x00167C78 File Offset: 0x00165E78
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

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600551C RID: 21788 RVA: 0x00167C82 File Offset: 0x00165E82
		// (set) Token: 0x0600551D RID: 21789 RVA: 0x00167C8A File Offset: 0x00165E8A
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

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600551E RID: 21790 RVA: 0x00167C94 File Offset: 0x00165E94
		// (set) Token: 0x0600551F RID: 21791 RVA: 0x00167C9C File Offset: 0x00165E9C
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06005520 RID: 21792 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06005521 RID: 21793 RVA: 0x00167CA5 File Offset: 0x00165EA5
		// (set) Token: 0x06005522 RID: 21794 RVA: 0x00167CAD File Offset: 0x00165EAD
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06005523 RID: 21795 RVA: 0x00167CB6 File Offset: 0x00165EB6
		// (set) Token: 0x06005524 RID: 21796 RVA: 0x00167CBE File Offset: 0x00165EBE
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06005525 RID: 21797 RVA: 0x00167CC7 File Offset: 0x00165EC7
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06005526 RID: 21798 RVA: 0x00167CCF File Offset: 0x00165ECF
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06005527 RID: 21799 RVA: 0x00167CD7 File Offset: 0x00165ED7
		public bool Selectable { get; } = 1;

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06005528 RID: 21800 RVA: 0x00167CDF File Offset: 0x00165EDF
		// (set) Token: 0x06005529 RID: 21801 RVA: 0x00167CE7 File Offset: 0x00165EE7
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600552A RID: 21802 RVA: 0x00167CF0 File Offset: 0x00165EF0
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600552B RID: 21803 RVA: 0x00167CF8 File Offset: 0x00165EF8
		// (set) Token: 0x0600552C RID: 21804 RVA: 0x00167D00 File Offset: 0x00165F00
		protected DryingRackConfiguration stationConfiguration { get; set; }

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600552D RID: 21805 RVA: 0x00167D09 File Offset: 0x00165F09
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600552E RID: 21806 RVA: 0x00167D11 File Offset: 0x00165F11
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.DryingRack;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600552F RID: 21807 RVA: 0x00167D15 File Offset: 0x00165F15
		// (set) Token: 0x06005530 RID: 21808 RVA: 0x00167D1D File Offset: 0x00165F1D
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06005531 RID: 21809 RVA: 0x00167D26 File Offset: 0x00165F26
		// (set) Token: 0x06005532 RID: 21810 RVA: 0x00167D2E File Offset: 0x00165F2E
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

		// Token: 0x06005533 RID: 21811 RVA: 0x00167D38 File Offset: 0x00165F38
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06005534 RID: 21812 RVA: 0x00167D4E File Offset: 0x00165F4E
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06005535 RID: 21813 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06005536 RID: 21814 RVA: 0x00167CC7 File Offset: 0x00165EC7
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06005537 RID: 21815 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06005538 RID: 21816 RVA: 0x00167D56 File Offset: 0x00165F56
		// (set) Token: 0x06005539 RID: 21817 RVA: 0x00167D5E File Offset: 0x00165F5E
		public ItemSlot InputSlot { get; private set; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600553A RID: 21818 RVA: 0x00167D67 File Offset: 0x00165F67
		// (set) Token: 0x0600553B RID: 21819 RVA: 0x00167D6F File Offset: 0x00165F6F
		public ItemSlot OutputSlot { get; private set; }

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600553C RID: 21820 RVA: 0x00167D78 File Offset: 0x00165F78
		// (set) Token: 0x0600553D RID: 21821 RVA: 0x00167D80 File Offset: 0x00165F80
		public bool IsOpen { get; private set; }

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600553E RID: 21822 RVA: 0x00167D89 File Offset: 0x00165F89
		// (set) Token: 0x0600553F RID: 21823 RVA: 0x00167D91 File Offset: 0x00165F91
		public List<DryingOperation> DryingOperations { get; set; } = new List<DryingOperation>();

		// Token: 0x06005540 RID: 21824 RVA: 0x00167D9C File Offset: 0x00165F9C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.DryingRack_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x00167DBC File Offset: 0x00165FBC
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			bool initialized = base.Initialized;
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (initialized)
			{
				return;
			}
			if (!this.isGhost)
			{
				base.ParentProperty.AddConfigurable(this);
				this.stationConfiguration = new DryingRackConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x00167E48 File Offset: 0x00166048
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
			foreach (DryingOperation op in this.DryingOperations)
			{
				this.PleaseReceiveOp(connection, op);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x00167EB4 File Offset: 0x001660B4
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			DryingRack.<>c__DisplayClass97_0 CS$<>8__locals1 = new DryingRack.<>c__DisplayClass97_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x00167EF4 File Offset: 0x001660F4
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			action.Used = true;
			this.Close();
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00167F1F File Offset: 0x0016611F
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IUsable)this).IsInUse)
			{
				reason = "In use";
				return false;
			}
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0 || this.DryingOperations.Count > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00167F5C File Offset: 0x0016615C
		public override void DestroyItem(bool callOnServer = true)
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x00167FCC File Offset: 0x001661CC
		private void MinPass()
		{
			foreach (DryingOperation dryingOperation in this.DryingOperations.ToArray())
			{
				dryingOperation.Time++;
				if (dryingOperation.Time >= 720)
				{
					if (dryingOperation.StartQuality >= EQuality.Premium)
					{
						if (InstanceFinder.IsServer && this.GetOutputCapacityForOperation(dryingOperation, EQuality.Heavenly) >= dryingOperation.Quantity)
						{
							this.TryEndOperation(this.DryingOperations.IndexOf(dryingOperation), false, EQuality.Heavenly, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
						}
					}
					else
					{
						dryingOperation.IncreaseQuality();
					}
				}
			}
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x0016805D File Offset: 0x0016625D
		public bool CanStartOperation()
		{
			return this.GetTotalDryingItems() < this.ItemCapacity && this.InputSlot.Quantity != 0 && !this.InputSlot.IsLocked && !this.InputSlot.IsRemovalLocked;
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x0016809C File Offset: 0x0016629C
		public void StartOperation()
		{
			int num = Mathf.Min(this.InputSlot.Quantity, this.ItemCapacity - this.GetTotalDryingItems());
			EQuality quality = (this.InputSlot.ItemInstance as QualityItemInstance).Quality;
			DryingOperation op = new DryingOperation(this.InputSlot.ItemInstance.ID, num, quality, 0);
			this.SendOperation(op);
			this.InputSlot.ChangeQuantity(-num, false);
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x0016810C File Offset: 0x0016630C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void TryEndOperation(int operationIndex, bool allowSplitting, EQuality quality, int requestID)
		{
			this.RpcWriter___Server_TryEndOperation_4146970406(operationIndex, allowSplitting, quality, requestID);
			this.RpcLogic___TryEndOperation_4146970406(operationIndex, allowSplitting, quality, requestID);
		}

		// Token: 0x0600554B RID: 21835 RVA: 0x00168148 File Offset: 0x00166348
		public List<DryingOperation> GetOperationsAtTargetQuality()
		{
			EQuality targetQuality = (this.Configuration as DryingRackConfiguration).TargetQuality.Value;
			return (from x in this.DryingOperations
			where x.StartQuality >= targetQuality
			select x).ToList<DryingOperation>();
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x00168194 File Offset: 0x00166394
		public int GetOutputCapacityForOperation(DryingOperation operation, EQuality quality)
		{
			QualityItemInstance qualityItemInstance = Registry.GetItem(operation.ItemID).GetDefaultInstance(1) as QualityItemInstance;
			qualityItemInstance.SetQuality(quality);
			return this.OutputSlot.GetCapacityForItem(qualityItemInstance, false);
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x001681CC File Offset: 0x001663CC
		[ServerRpc(RequireOwnership = false)]
		private void SendOperation(DryingOperation op)
		{
			this.RpcWriter___Server_SendOperation_1307702229(op);
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x001681D8 File Offset: 0x001663D8
		[TargetRpc]
		[ObserversRpc]
		private void PleaseReceiveOp(NetworkConnection conn, DryingOperation op)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_PleaseReceiveOp_1575047616(conn, op);
			}
			else
			{
				this.RpcWriter___Target_PleaseReceiveOp_1575047616(conn, op);
			}
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x0016820C File Offset: 0x0016640C
		[ObserversRpc(RunLocally = true, ExcludeServer = true)]
		private void RemoveOperation(int opIndex)
		{
			this.RpcWriter___Observers_RemoveOperation_3316948804(opIndex);
			this.RpcLogic___RemoveOperation_3316948804(opIndex);
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x00168230 File Offset: 0x00166430
		[ObserversRpc]
		private void SetOperationQuantity(int opIndex, int quantity)
		{
			this.RpcWriter___Observers_SetOperationQuantity_1692629761(opIndex, quantity);
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x0016824B File Offset: 0x0016644B
		public int GetTotalDryingItems()
		{
			return this.DryingOperations.Sum((DryingOperation x) => x.Quantity);
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x00168278 File Offset: 0x00166478
		public void RefreshHangingVisuals()
		{
			for (int i = 0; i < this.hangSlots.Length; i++)
			{
				if (this.DryingOperations.Count > i)
				{
					QualityItemInstance qualityItemInstance = this.DryingOperations[i].GetQualityItemInstance();
					this.hangSlots[i].SetStoredItem(qualityItemInstance, false);
				}
				else
				{
					this.hangSlots[i].ClearStoredInstance(false);
				}
			}
			this.HangingVisuals.RefreshVisuals();
			StoredItem[] array = (from x in this.HangingVisuals.ItemContainer.GetComponentsInChildren<StoredItem>()
			where !x.Destroyed
			select x).ToArray<StoredItem>();
			int num = 0;
			while (num < array.Length && num < this.HangAlignments.Length)
			{
				Transform transform = array[num].GetComponentsInChildren<Transform>().FirstOrDefault((Transform x) => x.name == "HangingAlignment");
				if (transform == null)
				{
					Console.LogError("Missing alignment transform on stored item: " + array[num].name, null);
				}
				else
				{
					Transform transform2 = this.HangAlignments[num];
					Quaternion lhs = transform2.rotation * Quaternion.Inverse(transform.rotation);
					array[num].transform.rotation = lhs * array[num].transform.rotation;
					Vector3 b = transform2.position - transform.position;
					array[num].transform.position += b;
				}
				num++;
			}
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x00168400 File Offset: 0x00166600
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			if (base.ParentProperty == null)
			{
				Property parentProperty = base.ParentProperty;
				Console.LogError(((parentProperty != null) ? parentProperty.ToString() : null) + " is not a child of a property!", null);
				return null;
			}
			DryingRackUIElement component = UnityEngine.Object.Instantiate<DryingRackUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<DryingRackUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005554 RID: 21844 RVA: 0x00168493 File Offset: 0x00166693
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06005555 RID: 21845 RVA: 0x001684AE File Offset: 0x001666AE
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x001684C4 File Offset: 0x001666C4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x001684DC File Offset: 0x001666DC
		public void Hovered()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage("Use " + base.ItemInstance.Name);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06005558 RID: 21848 RVA: 0x00168536 File Offset: 0x00166736
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x00168554 File Offset: 0x00166754
		public void Open()
		{
			this.IsOpen = true;
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Transform transform = this.CameraPositions[0];
			if (Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.CameraPositions[1].position) < Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.CameraPositions[0].position))
			{
				transform = this.CameraPositions[1];
			}
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(transform.position, transform.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
			Singleton<DryingRackCanvas>.Instance.SetIsOpen(this, true);
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x00168648 File Offset: 0x00166848
		public void Close()
		{
			this.IsOpen = false;
			Singleton<DryingRackCanvas>.Instance.SetIsOpen(null, false);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x001686CA File Offset: 0x001668CA
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x001686F0 File Offset: 0x001668F0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetStoredInstance_Internal(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
			else
			{
				this.RpcWriter___Target_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x0016874F File Offset: 0x0016694F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x0016876D File Offset: 0x0016696D
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x0016878B File Offset: 0x0016698B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x001687C4 File Offset: 0x001669C4
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetSlotLocked_Internal(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
			else
			{
				this.RpcWriter___Target_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x00168843 File Offset: 0x00166A43
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x0016886C File Offset: 0x00166A6C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetSlotFilter_Internal(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
				this.RpcLogic___SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
			}
			else
			{
				this.RpcWriter___Target_SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
				this.RpcLogic___SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
			}
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x001688CC File Offset: 0x00166ACC
		public override BuildableItemData GetBaseData()
		{
			return new DryingRackData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.InputSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.DryingOperations.ToArray());
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x00168934 File Offset: 0x00166B34
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x001689CC File Offset: 0x00166BCC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_TryEndOperation_4146970406));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendOperation_1307702229));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_PleaseReceiveOp_1575047616));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_PleaseReceiveOp_1575047616));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_RemoveOperation_3316948804));
			base.RegisterObserversRpc(14U, new ClientRpcDelegate(this.RpcReader___Observers_SetOperationQuantity_1692629761));
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(19U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(23U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(24U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(25U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(26U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(27U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.DryingRack));
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x00168C4F File Offset: 0x00166E4F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.DryingRackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x00168C89 File Offset: 0x00166E89
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x00168C98 File Offset: 0x00166E98
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
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x00168D3F File Offset: 0x00166F3F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x0600556B RID: 21867 RVA: 0x00168D48 File Offset: 0x00166F48
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

		// Token: 0x0600556C RID: 21868 RVA: 0x00168D88 File Offset: 0x00166F88
		private void RpcWriter___Server_TryEndOperation_4146970406(int operationIndex, bool allowSplitting, EQuality quality, int requestID)
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
			writer.WriteInt32(operationIndex, 1);
			writer.WriteBoolean(allowSplitting);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			writer.WriteInt32(requestID, 1);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x00168E60 File Offset: 0x00167060
		public void RpcLogic___TryEndOperation_4146970406(int operationIndex, bool allowSplitting, EQuality quality, int requestID)
		{
			if (this.requestIDs.Contains(requestID))
			{
				return;
			}
			this.requestIDs.Add(requestID);
			if (operationIndex >= this.DryingOperations.Count)
			{
				Console.LogError("Invalid operation index: " + operationIndex.ToString(), null);
				return;
			}
			DryingOperation dryingOperation = this.DryingOperations[operationIndex];
			int outputCapacityForOperation = this.GetOutputCapacityForOperation(dryingOperation, quality);
			int num = Mathf.Min(dryingOperation.Quantity, outputCapacityForOperation);
			if (num == 0)
			{
				Console.LogWarning("No space in output slot for operation: " + operationIndex.ToString(), null);
				return;
			}
			if (!allowSplitting && num < dryingOperation.Quantity)
			{
				Console.LogWarning("Operation would be split, but splitting is not allowed", null);
				return;
			}
			QualityItemInstance qualityItemInstance = Registry.GetItem(dryingOperation.ItemID).GetDefaultInstance(num) as QualityItemInstance;
			qualityItemInstance.SetQuality(quality);
			this.OutputSlot.InsertItem(qualityItemInstance);
			if (num == dryingOperation.Quantity)
			{
				this.RemoveOperation(this.DryingOperations.IndexOf(dryingOperation));
				return;
			}
			this.SetOperationQuantity(this.DryingOperations.IndexOf(dryingOperation), dryingOperation.Quantity - num);
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x00168F68 File Offset: 0x00167168
		private void RpcReader___Server_TryEndOperation_4146970406(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int operationIndex = PooledReader0.ReadInt32(1);
			bool allowSplitting = PooledReader0.ReadBoolean();
			EQuality quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			int requestID = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___TryEndOperation_4146970406(operationIndex, allowSplitting, quality, requestID);
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x00168FE4 File Offset: 0x001671E4
		private void RpcWriter___Server_SendOperation_1307702229(DryingOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated(op);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x0016908B File Offset: 0x0016728B
		private void RpcLogic___SendOperation_1307702229(DryingOperation op)
		{
			this.PleaseReceiveOp(null, op);
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x00169098 File Offset: 0x00167298
		private void RpcReader___Server_SendOperation_1307702229(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			DryingOperation op = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendOperation_1307702229(op);
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x001690CC File Offset: 0x001672CC
		private void RpcWriter___Target_PleaseReceiveOp_1575047616(NetworkConnection conn, DryingOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated(op);
			base.SendTargetRpc(11U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005573 RID: 21875 RVA: 0x00169184 File Offset: 0x00167384
		private void RpcLogic___PleaseReceiveOp_1575047616(NetworkConnection conn, DryingOperation op)
		{
			if (op.Quantity == 0)
			{
				Console.LogWarning("Operation quantity is 0. Ignoring", null);
				return;
			}
			this.DryingOperations.Add(op);
			if (this.onOperationStart != null)
			{
				this.onOperationStart(op);
			}
			if (this.onOperationsChanged != null)
			{
				this.onOperationsChanged();
			}
			this.RefreshHangingVisuals();
		}

		// Token: 0x06005574 RID: 21876 RVA: 0x001691E0 File Offset: 0x001673E0
		private void RpcReader___Target_PleaseReceiveOp_1575047616(PooledReader PooledReader0, Channel channel)
		{
			DryingOperation op = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PleaseReceiveOp_1575047616(base.LocalConnection, op);
		}

		// Token: 0x06005575 RID: 21877 RVA: 0x00169218 File Offset: 0x00167418
		private void RpcWriter___Observers_PleaseReceiveOp_1575047616(NetworkConnection conn, DryingOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generated(op);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x001692D0 File Offset: 0x001674D0
		private void RpcReader___Observers_PleaseReceiveOp_1575047616(PooledReader PooledReader0, Channel channel)
		{
			DryingOperation op = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.DryingOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PleaseReceiveOp_1575047616(null, op);
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x00169304 File Offset: 0x00167504
		private void RpcWriter___Observers_RemoveOperation_3316948804(int opIndex)
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
			writer.WriteInt32(opIndex, 1);
			base.SendObserversRpc(13U, writer, channel, 0, false, true, false);
			writer.Store();
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x001693C0 File Offset: 0x001675C0
		private void RpcLogic___RemoveOperation_3316948804(int opIndex)
		{
			if (opIndex < this.DryingOperations.Count)
			{
				DryingOperation dryingOperation = this.DryingOperations[opIndex];
				this.DryingOperations.Remove(dryingOperation);
				if (this.onOperationComplete != null)
				{
					this.onOperationComplete(dryingOperation);
				}
				if (this.onOperationsChanged != null)
				{
					this.onOperationsChanged();
				}
				this.RefreshHangingVisuals();
				return;
			}
			Console.LogError("Invalid operation index: " + opIndex.ToString(), null);
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x0016943C File Offset: 0x0016763C
		private void RpcReader___Observers_RemoveOperation_3316948804(PooledReader PooledReader0, Channel channel)
		{
			int opIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RemoveOperation_3316948804(opIndex);
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x0016947C File Offset: 0x0016767C
		private void RpcWriter___Observers_SetOperationQuantity_1692629761(int opIndex, int quantity)
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
			writer.WriteInt32(opIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendObserversRpc(14U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x0016954C File Offset: 0x0016774C
		private void RpcLogic___SetOperationQuantity_1692629761(int opIndex, int quantity)
		{
			if (opIndex < this.DryingOperations.Count)
			{
				this.DryingOperations[opIndex].Quantity = quantity;
				if (this.onOperationsChanged != null)
				{
					this.onOperationsChanged();
				}
				this.RefreshHangingVisuals();
				return;
			}
			Console.LogError("Invalid operation index: " + opIndex.ToString(), null);
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x001695AC File Offset: 0x001677AC
		private void RpcReader___Observers_SetOperationQuantity_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int opIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetOperationQuantity_1692629761(opIndex, quantity);
		}

		// Token: 0x0600557D RID: 21885 RVA: 0x001695F8 File Offset: 0x001677F8
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
			base.SendServerRpc(15U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x0016969F File Offset: 0x0016789F
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x001696A8 File Offset: 0x001678A8
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

		// Token: 0x06005580 RID: 21888 RVA: 0x001696E8 File Offset: 0x001678E8
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
			base.SendServerRpc(16U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x0016978F File Offset: 0x0016798F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x00169798 File Offset: 0x00167998
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

		// Token: 0x06005583 RID: 21891 RVA: 0x001697D8 File Offset: 0x001679D8
		private void RpcWriter___Server_SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendServerRpc(17U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x0016989E File Offset: 0x00167A9E
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x001698C8 File Offset: 0x00167AC8
		private void RpcReader___Server_SetStoredInstance_2652194801(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_2652194801(conn2, itemSlotIndex, instance);
		}

		// Token: 0x06005586 RID: 21894 RVA: 0x00169930 File Offset: 0x00167B30
		private void RpcWriter___Observers_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005587 RID: 21895 RVA: 0x001699F8 File Offset: 0x00167BF8
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005588 RID: 21896 RVA: 0x00169A24 File Offset: 0x00167C24
		private void RpcReader___Observers_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(null, itemSlotIndex, instance);
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x00169A78 File Offset: 0x00167C78
		private void RpcWriter___Target_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendTargetRpc(19U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600558A RID: 21898 RVA: 0x00169B40 File Offset: 0x00167D40
		private void RpcReader___Target_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(base.LocalConnection, itemSlotIndex, instance);
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x00169B98 File Offset: 0x00167D98
		private void RpcWriter___Server_SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendServerRpc(20U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600558C RID: 21900 RVA: 0x00169C56 File Offset: 0x00167E56
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x00169C60 File Offset: 0x00167E60
		private void RpcReader___Server_SetItemSlotQuantity_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x00169CBC File Offset: 0x00167EBC
		private void RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendObserversRpc(21U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x00169D89 File Offset: 0x00167F89
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x00169DA0 File Offset: 0x00167FA0
		private void RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x00169DF8 File Offset: 0x00167FF8
		private void RpcWriter___Server_SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendServerRpc(22U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x00169ED8 File Offset: 0x001680D8
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x00169F08 File Offset: 0x00168108
		private void RpcReader___Server_SetSlotLocked_3170825843(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_3170825843(conn2, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x00169F90 File Offset: 0x00168190
		private void RpcWriter___Target_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendTargetRpc(23U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x0016A071 File Offset: 0x00168271
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x0016A0A0 File Offset: 0x001682A0
		private void RpcReader___Target_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(base.LocalConnection, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x0016A11C File Offset: 0x0016831C
		private void RpcWriter___Observers_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendObserversRpc(24U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x0016A200 File Offset: 0x00168400
		private void RpcReader___Observers_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(null, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x0016A274 File Offset: 0x00168474
		private void RpcWriter___Server_SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendServerRpc(25U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x0016A33A File Offset: 0x0016853A
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x0016A364 File Offset: 0x00168564
		private void RpcReader___Server_SetSlotFilter_527532783(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_527532783(conn2, itemSlotIndex, filter);
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x0016A3CC File Offset: 0x001685CC
		private void RpcWriter___Observers_SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendObserversRpc(26U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x0016A494 File Offset: 0x00168694
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x0016A4AC File Offset: 0x001686AC
		private void RpcReader___Observers_SetSlotFilter_Internal_527532783(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_Internal_527532783(null, itemSlotIndex, filter);
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x0016A500 File Offset: 0x00168700
		private void RpcWriter___Target_SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendTargetRpc(27U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x0016A5C8 File Offset: 0x001687C8
		private void RpcReader___Target_SetSlotFilter_Internal_527532783(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_Internal_527532783(base.LocalConnection, itemSlotIndex, filter);
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x060055A1 RID: 21921 RVA: 0x0016A61F File Offset: 0x0016881F
		// (set) Token: 0x060055A2 RID: 21922 RVA: 0x0016A627 File Offset: 0x00168827
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

		// Token: 0x060055A3 RID: 21923 RVA: 0x0016A664 File Offset: 0x00168864
		public override bool DryingRack(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(this.syncVar___<CurrentPlayerConfigurer>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerUserObject>k__BackingField(this.syncVar___<PlayerUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<PlayerUserObject>k__BackingField(value2, Boolean2);
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
				NetworkObject value3 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x060055A4 RID: 21924 RVA: 0x0016A73E File Offset: 0x0016893E
		// (set) Token: 0x060055A5 RID: 21925 RVA: 0x0016A746 File Offset: 0x00168946
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

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x060055A6 RID: 21926 RVA: 0x0016A782 File Offset: 0x00168982
		// (set) Token: 0x060055A7 RID: 21927 RVA: 0x0016A78A File Offset: 0x0016898A
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

		// Token: 0x060055A8 RID: 21928 RVA: 0x0016A7C8 File Offset: 0x001689C8
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.InputSlot = new ItemSlot(true);
				this.InputSlot.SetSlotOwner(this);
				this.InputSlot.AddFilter(new ItemFilter_Dryable());
				this.InputVisuals.AddSlot(this.InputSlot, false);
				this.OutputSlot = new ItemSlot();
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.Add(this.InputSlot);
				this.OutputSlots.Add(this.OutputSlot);
				new ItemSlotSiblingSet(this.InputSlots);
				this.HangingVisuals.BlockRefreshes = true;
				this.hangSlots = new ItemSlot[this.HangAlignments.Length];
				for (int i = 0; i < this.HangAlignments.Length; i++)
				{
					this.hangSlots[i] = new ItemSlot();
					this.HangingVisuals.AddSlot(this.hangSlots[i], false);
				}
			}
		}

		// Token: 0x04003F40 RID: 16192
		public const int DRY_MINS_PER_TIER = 720;

		// Token: 0x04003F41 RID: 16193
		[Header("Settings")]
		public int ItemCapacity = 20;

		// Token: 0x04003F42 RID: 16194
		[Header("References")]
		public Transform[] CameraPositions;

		// Token: 0x04003F43 RID: 16195
		public InteractableObject IntObj;

		// Token: 0x04003F44 RID: 16196
		public Transform uiPoint;

		// Token: 0x04003F45 RID: 16197
		public Transform[] accessPoints;

		// Token: 0x04003F46 RID: 16198
		public StorageVisualizer InputVisuals;

		// Token: 0x04003F47 RID: 16199
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003F48 RID: 16200
		public StorageVisualizer HangingVisuals;

		// Token: 0x04003F49 RID: 16201
		public Transform[] HangAlignments;

		// Token: 0x04003F4A RID: 16202
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003F4B RID: 16203
		[Header("UI")]
		public DryingRackUIElement WorldspaceUIPrefab;

		// Token: 0x04003F4C RID: 16204
		public Sprite typeIcon;

		// Token: 0x04003F5B RID: 16219
		public Action<DryingOperation> onOperationStart;

		// Token: 0x04003F5C RID: 16220
		public Action<DryingOperation> onOperationComplete;

		// Token: 0x04003F5D RID: 16221
		public Action onOperationsChanged;

		// Token: 0x04003F5E RID: 16222
		private ItemSlot[] hangSlots;

		// Token: 0x04003F5F RID: 16223
		private List<int> requestIDs = new List<int>();

		// Token: 0x04003F60 RID: 16224
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003F61 RID: 16225
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003F62 RID: 16226
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003F63 RID: 16227
		private bool dll_Excuted;

		// Token: 0x04003F64 RID: 16228
		private bool dll_Excuted;
	}
}
