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
using ScheduleOne.Audio;
using ScheduleOne.Decoration;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C3D RID: 3133
	public class PackagingStation : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x0600579A RID: 22426 RVA: 0x00172B32 File Offset: 0x00170D32
		// (set) Token: 0x0600579B RID: 22427 RVA: 0x00172B3A File Offset: 0x00170D3A
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x0600579C RID: 22428 RVA: 0x00172B43 File Offset: 0x00170D43
		// (set) Token: 0x0600579D RID: 22429 RVA: 0x00172B4B File Offset: 0x00170D4B
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

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x0600579E RID: 22430 RVA: 0x00172B55 File Offset: 0x00170D55
		// (set) Token: 0x0600579F RID: 22431 RVA: 0x00172B5D File Offset: 0x00170D5D
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

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060057A0 RID: 22432 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x00172B67 File Offset: 0x00170D67
		// (set) Token: 0x060057A2 RID: 22434 RVA: 0x00172B6F File Offset: 0x00170D6F
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060057A3 RID: 22435 RVA: 0x00172B78 File Offset: 0x00170D78
		// (set) Token: 0x060057A4 RID: 22436 RVA: 0x00172B80 File Offset: 0x00170D80
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060057A5 RID: 22437 RVA: 0x00172B89 File Offset: 0x00170D89
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060057A6 RID: 22438 RVA: 0x00172B91 File Offset: 0x00170D91
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060057A7 RID: 22439 RVA: 0x00172B99 File Offset: 0x00170D99
		public bool Selectable { get; } = 1;

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060057A8 RID: 22440 RVA: 0x00172BA1 File Offset: 0x00170DA1
		// (set) Token: 0x060057A9 RID: 22441 RVA: 0x00172BA9 File Offset: 0x00170DA9
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060057AA RID: 22442 RVA: 0x00172BB2 File Offset: 0x00170DB2
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060057AB RID: 22443 RVA: 0x00172BBA File Offset: 0x00170DBA
		// (set) Token: 0x060057AC RID: 22444 RVA: 0x00172BC2 File Offset: 0x00170DC2
		protected PackagingStationConfiguration stationConfiguration { get; set; }

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060057AD RID: 22445 RVA: 0x00172BCB File Offset: 0x00170DCB
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x000022C9 File Offset: 0x000004C9
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.PackagingStation;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x00172BD3 File Offset: 0x00170DD3
		// (set) Token: 0x060057B0 RID: 22448 RVA: 0x00172BDB File Offset: 0x00170DDB
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060057B1 RID: 22449 RVA: 0x00172BE4 File Offset: 0x00170DE4
		// (set) Token: 0x060057B2 RID: 22450 RVA: 0x00172BEC File Offset: 0x00170DEC
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

		// Token: 0x060057B3 RID: 22451 RVA: 0x00172BF6 File Offset: 0x00170DF6
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060057B4 RID: 22452 RVA: 0x00172C0C File Offset: 0x00170E0C
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x060057B5 RID: 22453 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x060057B6 RID: 22454 RVA: 0x00172C14 File Offset: 0x00170E14
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x060057B7 RID: 22455 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x00172C1C File Offset: 0x00170E1C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.PackagingStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x00172C3C File Offset: 0x00170E3C
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
				this.stationConfiguration = new PackagingStationConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			}
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x00172C9F File Offset: 0x00170E9F
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x00172CB8 File Offset: 0x00170EB8
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			PackagingStation.<>c__DisplayClass103_0 CS$<>8__locals1 = new PackagingStation.<>c__DisplayClass103_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x00172CF8 File Offset: 0x00170EF8
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!Singleton<PackagingStationCanvas>.Instance.isOpen)
			{
				return;
			}
			if (Singleton<PackagingStationCanvas>.Instance.PackagingStation != this)
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

		// Token: 0x060057BD RID: 22461 RVA: 0x00172D45 File Offset: 0x00170F45
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IUsable)this).IsInUse)
			{
				reason = "Currently in use";
				return false;
			}
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x00172D71 File Offset: 0x00170F71
		public override void DestroyItem(bool callOnServer = true)
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x00172DB0 File Offset: 0x00170FB0
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x00172DD1 File Offset: 0x00170FD1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x00172DE8 File Offset: 0x00170FE8
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

		// Token: 0x060057C2 RID: 22466 RVA: 0x00172E42 File Offset: 0x00171042
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x00172E60 File Offset: 0x00171060
		public void Open()
		{
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
			Singleton<PackagingStationCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x00172F00 File Offset: 0x00171100
		public void Close()
		{
			if (Singleton<PackagingStationCanvas>.InstanceExists)
			{
				Singleton<PackagingStationCanvas>.Instance.SetIsOpen(null, false, true);
			}
			this.SetPlayerUser(null);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			}
			if (Singleton<CompassManager>.InstanceExists)
			{
				Singleton<CompassManager>.Instance.SetVisible(true);
			}
			if (PlayerSingleton<PlayerInventory>.InstanceExists)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			}
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			}
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x00172FA0 File Offset: 0x001711A0
		public PackagingStation.EState GetState(PackagingStation.EMode mode)
		{
			if (mode == PackagingStation.EMode.Package)
			{
				if (this.PackagingSlot.Quantity == 0)
				{
					return PackagingStation.EState.MissingItems;
				}
				if (this.ProductSlot.Quantity == 0)
				{
					return PackagingStation.EState.MissingItems;
				}
				if (this.OutputSlot.IsAtCapacity)
				{
					return PackagingStation.EState.OutputSlotFull;
				}
				if (this.OutputSlot.Quantity > 0 && this.OutputSlot.ItemInstance.ID != this.ProductSlot.ItemInstance.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.Quantity > 0 && (this.OutputSlot.ItemInstance as ProductItemInstance).AppliedPackaging.ID != this.PackagingSlot.ItemInstance.Definition.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.Quantity > 0 && (this.OutputSlot.ItemInstance as ProductItemInstance).Quality != (this.ProductSlot.ItemInstance as ProductItemInstance).Quality)
				{
					return PackagingStation.EState.Mismatch;
				}
				int quantity = (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
				if (this.ProductSlot.Quantity < quantity)
				{
					return PackagingStation.EState.InsufficentProduct;
				}
			}
			else if (mode == PackagingStation.EMode.Unpackage)
			{
				if (this.OutputSlot.Quantity == 0)
				{
					return PackagingStation.EState.MissingItems;
				}
				ProductItemInstance productItemInstance = this.OutputSlot.ItemInstance.GetCopy(1) as ProductItemInstance;
				if (productItemInstance == null)
				{
					return PackagingStation.EState.MissingItems;
				}
				PackagingDefinition appliedPackaging = productItemInstance.AppliedPackaging;
				int quantity2 = appliedPackaging.Quantity;
				if (this.PackagingSlot.GetCapacityForItem(appliedPackaging.GetDefaultInstance(1), false) < 1)
				{
					return PackagingStation.EState.PackageSlotFull;
				}
				productItemInstance.SetPackaging(null);
				if (this.ProductSlot.GetCapacityForItem(productItemInstance, false) < quantity2)
				{
					return PackagingStation.EState.ProductSlotFull;
				}
			}
			return PackagingStation.EState.CanBegin;
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x00173134 File Offset: 0x00171334
		public void Unpack()
		{
			PackagingDefinition appliedPackaging = (this.OutputSlot.ItemInstance as ProductItemInstance).AppliedPackaging;
			int quantity = appliedPackaging.Quantity;
			ProductItemInstance productItemInstance = this.OutputSlot.ItemInstance.GetCopy(quantity) as ProductItemInstance;
			productItemInstance.SetPackaging(null);
			if (appliedPackaging.ID != "brick")
			{
				this.PackagingSlot.AddItem(appliedPackaging.GetDefaultInstance(1), false);
			}
			this.ProductSlot.AddItem(productItemInstance, false);
			this.OutputSlot.ChangeQuantity(-1, false);
		}

		// Token: 0x060057C7 RID: 22471 RVA: 0x001731BC File Offset: 0x001713BC
		public void PackSingleInstance()
		{
			int quantity = (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity;
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("PackagedProductCount");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("PackagedProductCount", (value + 1f).ToString(), true);
			if (this.OutputSlot.ItemInstance == null)
			{
				ItemInstance copy = this.ProductSlot.ItemInstance.GetCopy(1);
				(copy as ProductItemInstance).SetPackaging(this.PackagingSlot.ItemInstance.Definition as PackagingDefinition);
				this.OutputSlot.SetStoredItem(copy, false);
			}
			else
			{
				this.OutputSlot.ChangeQuantity(1, false);
			}
			this.PackagingSlot.ChangeQuantity(-1, false);
			this.ProductSlot.ChangeQuantity(-quantity, false);
		}

		// Token: 0x060057C8 RID: 22472 RVA: 0x0017328C File Offset: 0x0017148C
		public void SetHatchOpen(bool open)
		{
			PackagingStation.<>c__DisplayClass116_0 CS$<>8__locals1 = new PackagingStation.<>c__DisplayClass116_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.open = open;
			if (CS$<>8__locals1.open == this.hatchOpen)
			{
				return;
			}
			this.hatchOpen = CS$<>8__locals1.open;
			if (this.hatchOpen)
			{
				this.HatchOpenSound.Play();
			}
			else
			{
				this.HatchCloseSound.Play();
			}
			if (this.hatchRoutine != null)
			{
				base.StopCoroutine(this.hatchRoutine);
			}
			base.StartCoroutine(CS$<>8__locals1.<SetHatchOpen>g__Routine|0());
		}

		// Token: 0x060057C9 RID: 22473 RVA: 0x00173309 File Offset: 0x00171509
		public void UpdatePackagingVisuals()
		{
			this.UpdatePackagingVisuals(this.PackagingSlot.Quantity);
		}

		// Token: 0x060057CA RID: 22474 RVA: 0x0017331C File Offset: 0x0017151C
		public void SetVisualsLocked(bool locked)
		{
			this.visualsLocked = locked;
		}

		// Token: 0x060057CB RID: 22475 RVA: 0x00173328 File Offset: 0x00171528
		public void UpdatePackagingVisuals(int quantity)
		{
			if (this.PackagingSlot == null)
			{
				return;
			}
			if (this.visualsLocked)
			{
				return;
			}
			string text = string.Empty;
			FunctionalPackaging functionalPackaging = null;
			if (quantity > 0 && this.PackagingSlot.ItemInstance != null)
			{
				text = this.PackagingSlot.ItemInstance.ID;
				if (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition == null)
				{
					string str = "Failed to get packaging definition for item instance: ";
					ItemInstance itemInstance = this.PackagingSlot.ItemInstance;
					Console.LogError(str + ((itemInstance != null) ? itemInstance.ToString() : null), null);
					return;
				}
				functionalPackaging = (this.PackagingSlot.ItemInstance.Definition as PackagingDefinition).FunctionalPackaging;
			}
			for (int i = 0; i < this.PackagingAlignments.Length; i++)
			{
				if ((quantity <= i || this.PackagingSlotModelID[i] != text) && this.PackagingSlotModelID[i] != string.Empty)
				{
					if (this.PackagingAlignments[i].childCount > 0)
					{
						UnityEngine.Object.Destroy(this.PackagingAlignments[i].GetChild(0).gameObject);
					}
					this.PackagingSlotModelID[i] = string.Empty;
				}
				if (!(functionalPackaging == null) && quantity > i && this.PackagingSlotModelID[i] != text)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(functionalPackaging.gameObject, this.PackagingAlignments[i]).gameObject;
					gameObject.GetComponent<FunctionalPackaging>().AlignTo(this.PackagingAlignments[i]);
					this.PackagingSlotModelID[i] = text;
					UnityEngine.Object.Destroy(gameObject.GetComponent<FunctionalPackaging>());
				}
			}
		}

		// Token: 0x060057CC RID: 22476 RVA: 0x001734B6 File Offset: 0x001716B6
		public void UpdateProductVisuals()
		{
			this.UpdateProductVisuals(this.ProductSlot.Quantity);
		}

		// Token: 0x060057CD RID: 22477 RVA: 0x001734CC File Offset: 0x001716CC
		public void UpdateProductVisuals(int quantity)
		{
			if (this.ProductSlot == null)
			{
				return;
			}
			if (this.visualsLocked)
			{
				return;
			}
			string text = string.Empty;
			FunctionalProduct functionalProduct = null;
			if (quantity > 0)
			{
				text = this.ProductSlot.ItemInstance.ID;
				ProductDefinition productDefinition = this.ProductSlot.ItemInstance.Definition as ProductDefinition;
				if (productDefinition == null)
				{
					string str = "Failed to get product definition for item instance: ";
					ItemInstance itemInstance = this.PackagingSlot.ItemInstance;
					Console.LogError(str + ((itemInstance != null) ? itemInstance.ToString() : null), null);
					return;
				}
				functionalProduct = productDefinition.FunctionalProduct;
			}
			for (int i = 0; i < this.ProductAlignments.Length; i++)
			{
				if ((quantity <= i || this.ProductSlotModelID[i] != text) && this.ProductSlotModelID[i] != string.Empty)
				{
					UnityEngine.Object.Destroy(this.ProductAlignments[i].GetChild(0).gameObject);
					this.ProductSlotModelID[i] = string.Empty;
				}
				if (!(functionalProduct == null) && quantity > i && this.ProductSlotModelID[i] != text)
				{
					FunctionalProduct component = UnityEngine.Object.Instantiate<GameObject>(functionalProduct.gameObject, this.ProductAlignments[i]).GetComponent<FunctionalProduct>();
					component.InitializeVisuals(this.ProductSlot.ItemInstance);
					component.AlignTo(this.ProductAlignments[i]);
					if (component.Rb != null)
					{
						component.Rb.isKinematic = true;
					}
					this.ProductSlotModelID[i] = text;
					UnityEngine.Object.Destroy(component);
				}
			}
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x00173657 File Offset: 0x00171857
		public virtual void StartTask()
		{
			new PackageProductTask(this);
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x00173660 File Offset: 0x00171860
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x00173688 File Offset: 0x00171888
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

		// Token: 0x060057D1 RID: 22481 RVA: 0x001736E7 File Offset: 0x001718E7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x00173705 File Offset: 0x00171905
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x00173723 File Offset: 0x00171923
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x0017375C File Offset: 0x0017195C
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

		// Token: 0x060057D5 RID: 22485 RVA: 0x001737DB File Offset: 0x001719DB
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x00173804 File Offset: 0x00171A04
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

		// Token: 0x060057D7 RID: 22487 RVA: 0x00173864 File Offset: 0x00171A64
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
			PackagingStationUIElement component = UnityEngine.Object.Instantiate<PackagingStationUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<PackagingStationUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x001738F7 File Offset: 0x00171AF7
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x00173912 File Offset: 0x00171B12
		public override BuildableItemData GetBaseData()
		{
			return new PackagingStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.ItemSlots));
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x00173944 File Offset: 0x00171B44
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x001739EC File Offset: 0x00171BEC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(13U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(17U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(19U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(20U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(21U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.PackagingStation));
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x00173BE5 File Offset: 0x00171DE5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PackagingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x00173C1F File Offset: 0x00171E1F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x00173C30 File Offset: 0x00171E30
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

		// Token: 0x060057E0 RID: 22496 RVA: 0x00173CD7 File Offset: 0x00171ED7
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x00173CE0 File Offset: 0x00171EE0
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

		// Token: 0x060057E2 RID: 22498 RVA: 0x00173D20 File Offset: 0x00171F20
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
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x00173DC8 File Offset: 0x00171FC8
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
			if (this.OverheadLight != null)
			{
				this.OverheadLight.gameObject.SetActive(this.PlayerUserObject != null);
			}
			if (this.OverheadLightMeshRend != null)
			{
				this.OverheadLightMeshRend.material = ((this.PlayerUserObject != null) ? this.LightMeshOnMat : this.LightMeshOffMat);
			}
			if (this.Switch != null)
			{
				this.Switch.SetIsOn(this.PlayerUserObject != null);
			}
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x00173EA0 File Offset: 0x001720A0
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

		// Token: 0x060057E5 RID: 22501 RVA: 0x00173EE0 File Offset: 0x001720E0
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
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x00173F87 File Offset: 0x00172187
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x00173F90 File Offset: 0x00172190
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

		// Token: 0x060057E8 RID: 22504 RVA: 0x00173FD0 File Offset: 0x001721D0
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
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060057E9 RID: 22505 RVA: 0x00174096 File Offset: 0x00172296
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x060057EA RID: 22506 RVA: 0x001740C0 File Offset: 0x001722C0
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

		// Token: 0x060057EB RID: 22507 RVA: 0x00174128 File Offset: 0x00172328
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
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x001741F0 File Offset: 0x001723F0
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x0017421C File Offset: 0x0017241C
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

		// Token: 0x060057EE RID: 22510 RVA: 0x00174270 File Offset: 0x00172470
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
			base.SendTargetRpc(13U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x00174338 File Offset: 0x00172538
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

		// Token: 0x060057F0 RID: 22512 RVA: 0x00174390 File Offset: 0x00172590
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
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x0017444E File Offset: 0x0017264E
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x00174458 File Offset: 0x00172658
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

		// Token: 0x060057F3 RID: 22515 RVA: 0x001744B4 File Offset: 0x001726B4
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
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x00174581 File Offset: 0x00172781
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x00174598 File Offset: 0x00172798
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

		// Token: 0x060057F6 RID: 22518 RVA: 0x001745F0 File Offset: 0x001727F0
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
			base.SendServerRpc(16U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x001746D0 File Offset: 0x001728D0
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x00174700 File Offset: 0x00172900
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

		// Token: 0x060057F9 RID: 22521 RVA: 0x00174788 File Offset: 0x00172988
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
			base.SendTargetRpc(17U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x00174869 File Offset: 0x00172A69
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x00174898 File Offset: 0x00172A98
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

		// Token: 0x060057FC RID: 22524 RVA: 0x00174914 File Offset: 0x00172B14
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
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x001749F8 File Offset: 0x00172BF8
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

		// Token: 0x060057FE RID: 22526 RVA: 0x00174A6C File Offset: 0x00172C6C
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
			base.SendServerRpc(19U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x00174B32 File Offset: 0x00172D32
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x00174B5C File Offset: 0x00172D5C
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

		// Token: 0x06005801 RID: 22529 RVA: 0x00174BC4 File Offset: 0x00172DC4
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
			base.SendObserversRpc(20U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x00174C8C File Offset: 0x00172E8C
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x00174CA4 File Offset: 0x00172EA4
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

		// Token: 0x06005804 RID: 22532 RVA: 0x00174CF8 File Offset: 0x00172EF8
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
			base.SendTargetRpc(21U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x00174DC0 File Offset: 0x00172FC0
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

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06005806 RID: 22534 RVA: 0x00174E17 File Offset: 0x00173017
		// (set) Token: 0x06005807 RID: 22535 RVA: 0x00174E1F File Offset: 0x0017301F
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

		// Token: 0x06005808 RID: 22536 RVA: 0x00174E5C File Offset: 0x0017305C
		public override bool PackagingStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06005809 RID: 22537 RVA: 0x00174F36 File Offset: 0x00173136
		// (set) Token: 0x0600580A RID: 22538 RVA: 0x00174F3E File Offset: 0x0017313E
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

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x0600580B RID: 22539 RVA: 0x00174F7A File Offset: 0x0017317A
		// (set) Token: 0x0600580C RID: 22540 RVA: 0x00174F82 File Offset: 0x00173182
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

		// Token: 0x0600580D RID: 22541 RVA: 0x00174FC0 File Offset: 0x001731C0
		protected override void dll()
		{
			base.Awake();
			this.OverheadLight.gameObject.SetActive(false);
			this.Switch.SetIsOn(false);
			if (!this.isGhost)
			{
				for (int i = 0; i < this.PackagingAlignments.Length; i++)
				{
					this.PackagingSlotModelID.Add(string.Empty);
				}
				for (int j = 0; j < this.ProductAlignments.Length; j++)
				{
					this.ProductSlotModelID.Add(string.Empty);
				}
				this.PackagingSlot = new ItemSlot(true);
				this.PackagingSlot.SetSlotOwner(this);
				this.ProductSlot = new ItemSlot(true);
				this.ProductSlot.SetSlotOwner(this);
				this.OutputSlot.SetSlotOwner(this);
				ItemSlot packagingSlot = this.PackagingSlot;
				packagingSlot.onItemDataChanged = (Action)Delegate.Combine(packagingSlot.onItemDataChanged, new Action(this.UpdatePackagingVisuals));
				ItemSlot productSlot = this.ProductSlot;
				productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(this.UpdateProductVisuals));
				this.PackagingSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
				{
					EItemCategory.Packaging
				}));
				this.ProductSlot.AddFilter(new ItemFilter_UnpackagedProduct());
				this.OutputSlot.AddFilter(new ItemFilter_PackagedProduct());
				this.InputSlots.Add(this.PackagingSlot);
				this.InputSlots.Add(this.ProductSlot);
				this.OutputSlots.Add(this.OutputSlot);
				new ItemSlotSiblingSet(this.InputSlots);
			}
		}

		// Token: 0x0400406D RID: 16493
		[Header("References")]
		public Light OverheadLight;

		// Token: 0x0400406E RID: 16494
		public MeshRenderer OverheadLightMeshRend;

		// Token: 0x0400406F RID: 16495
		public RockerSwitch Switch;

		// Token: 0x04004070 RID: 16496
		public Transform CameraPosition;

		// Token: 0x04004071 RID: 16497
		public Transform CameraPosition_Task;

		// Token: 0x04004072 RID: 16498
		public InteractableObject IntObj;

		// Token: 0x04004073 RID: 16499
		public Transform ActivePackagingAlignent;

		// Token: 0x04004074 RID: 16500
		public Transform[] ActiveProductAlignments;

		// Token: 0x04004075 RID: 16501
		public Transform Container;

		// Token: 0x04004076 RID: 16502
		public Collider OutputCollider;

		// Token: 0x04004077 RID: 16503
		public Transform Hatch;

		// Token: 0x04004078 RID: 16504
		public Transform[] PackagingAlignments;

		// Token: 0x04004079 RID: 16505
		public Transform[] ProductAlignments;

		// Token: 0x0400407A RID: 16506
		public Transform uiPoint;

		// Token: 0x0400407B RID: 16507
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x0400407C RID: 16508
		public Transform StandPoint;

		// Token: 0x0400407D RID: 16509
		public Transform[] accessPoints;

		// Token: 0x0400407E RID: 16510
		public AudioSourceController HatchOpenSound;

		// Token: 0x0400407F RID: 16511
		public AudioSourceController HatchCloseSound;

		// Token: 0x04004080 RID: 16512
		[Header("UI")]
		public PackagingStationUIElement WorldspaceUIPrefab;

		// Token: 0x04004081 RID: 16513
		public Sprite typeIcon;

		// Token: 0x04004082 RID: 16514
		[Header("Slot Display Points")]
		public Transform PackagingSlotPosition;

		// Token: 0x04004083 RID: 16515
		public Transform ProductSlotPosition;

		// Token: 0x04004084 RID: 16516
		public Transform OutputSlotPosition;

		// Token: 0x04004085 RID: 16517
		[Header("Materials")]
		public Material LightMeshOnMat;

		// Token: 0x04004086 RID: 16518
		public Material LightMeshOffMat;

		// Token: 0x04004087 RID: 16519
		[Header("Settings")]
		public float PackagerEmployeeSpeedMultiplier = 1f;

		// Token: 0x04004088 RID: 16520
		public Vector3 HatchClosedRotation;

		// Token: 0x04004089 RID: 16521
		public Vector3 HatchOpenRotation;

		// Token: 0x0400408A RID: 16522
		public float HatchLerpTime = 0.5f;

		// Token: 0x0400408D RID: 16525
		public ItemSlot PackagingSlot;

		// Token: 0x0400408E RID: 16526
		public ItemSlot ProductSlot;

		// Token: 0x0400408F RID: 16527
		public ItemSlot OutputSlot;

		// Token: 0x04004090 RID: 16528
		private bool hatchOpen;

		// Token: 0x04004091 RID: 16529
		private Coroutine hatchRoutine;

		// Token: 0x04004092 RID: 16530
		private List<string> PackagingSlotModelID = new List<string>();

		// Token: 0x04004093 RID: 16531
		private List<string> ProductSlotModelID = new List<string>();

		// Token: 0x0400409B RID: 16539
		private bool visualsLocked;

		// Token: 0x0400409C RID: 16540
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x0400409D RID: 16541
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x0400409E RID: 16542
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x0400409F RID: 16543
		private bool dll_Excuted;

		// Token: 0x040040A0 RID: 16544
		private bool dll_Excuted;

		// Token: 0x02000C3E RID: 3134
		public enum EMode
		{
			// Token: 0x040040A2 RID: 16546
			Package,
			// Token: 0x040040A3 RID: 16547
			Unpackage
		}

		// Token: 0x02000C3F RID: 3135
		public enum EState
		{
			// Token: 0x040040A5 RID: 16549
			CanBegin,
			// Token: 0x040040A6 RID: 16550
			MissingItems,
			// Token: 0x040040A7 RID: 16551
			InsufficentProduct,
			// Token: 0x040040A8 RID: 16552
			OutputSlotFull,
			// Token: 0x040040A9 RID: 16553
			Mismatch,
			// Token: 0x040040AA RID: 16554
			PackageSlotFull,
			// Token: 0x040040AB RID: 16555
			ProductSlotFull
		}
	}
}
