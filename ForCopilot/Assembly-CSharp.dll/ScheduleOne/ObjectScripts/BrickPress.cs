using System;
using System.Collections;
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
using ScheduleOne.Audio;
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
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C0C RID: 3084
	public class BrickPress : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x0600531D RID: 21277 RVA: 0x0015F0B8 File Offset: 0x0015D2B8
		public bool isOpen
		{
			get
			{
				return this.PlayerUserObject == Player.Local.NetworkObject;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x0600531E RID: 21278 RVA: 0x0015F0CF File Offset: 0x0015D2CF
		// (set) Token: 0x0600531F RID: 21279 RVA: 0x0015F0D7 File Offset: 0x0015D2D7
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06005320 RID: 21280 RVA: 0x0015F0E0 File Offset: 0x0015D2E0
		// (set) Token: 0x06005321 RID: 21281 RVA: 0x0015F0E8 File Offset: 0x0015D2E8
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

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06005322 RID: 21282 RVA: 0x0015F0F2 File Offset: 0x0015D2F2
		// (set) Token: 0x06005323 RID: 21283 RVA: 0x0015F0FA File Offset: 0x0015D2FA
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

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06005324 RID: 21284 RVA: 0x0015F104 File Offset: 0x0015D304
		// (set) Token: 0x06005325 RID: 21285 RVA: 0x0015F10C File Offset: 0x0015D30C
		public ItemSlot[] ProductSlots { get; private set; }

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06005326 RID: 21286 RVA: 0x0015F115 File Offset: 0x0015D315
		// (set) Token: 0x06005327 RID: 21287 RVA: 0x0015F11D File Offset: 0x0015D31D
		public ItemSlot OutputSlot { get; private set; }

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06005328 RID: 21288 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06005329 RID: 21289 RVA: 0x0015F126 File Offset: 0x0015D326
		// (set) Token: 0x0600532A RID: 21290 RVA: 0x0015F12E File Offset: 0x0015D32E
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600532B RID: 21291 RVA: 0x0015F137 File Offset: 0x0015D337
		// (set) Token: 0x0600532C RID: 21292 RVA: 0x0015F13F File Offset: 0x0015D33F
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600532D RID: 21293 RVA: 0x0015F148 File Offset: 0x0015D348
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600532E RID: 21294 RVA: 0x0015F150 File Offset: 0x0015D350
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600532F RID: 21295 RVA: 0x0015F158 File Offset: 0x0015D358
		public bool Selectable { get; } = 1;

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06005330 RID: 21296 RVA: 0x0015F160 File Offset: 0x0015D360
		// (set) Token: 0x06005331 RID: 21297 RVA: 0x0015F168 File Offset: 0x0015D368
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x0015F171 File Offset: 0x0015D371
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06005333 RID: 21299 RVA: 0x0015F179 File Offset: 0x0015D379
		// (set) Token: 0x06005334 RID: 21300 RVA: 0x0015F181 File Offset: 0x0015D381
		protected BrickPressConfiguration stationConfiguration { get; set; }

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06005335 RID: 21301 RVA: 0x0015F18A File Offset: 0x0015D38A
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x00011B42 File Offset: 0x0000FD42
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.BrickPress;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06005337 RID: 21303 RVA: 0x0015F192 File Offset: 0x0015D392
		// (set) Token: 0x06005338 RID: 21304 RVA: 0x0015F19A File Offset: 0x0015D39A
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06005339 RID: 21305 RVA: 0x0015F1A3 File Offset: 0x0015D3A3
		// (set) Token: 0x0600533A RID: 21306 RVA: 0x0015F1AB File Offset: 0x0015D3AB
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

		// Token: 0x0600533B RID: 21307 RVA: 0x0015F1B5 File Offset: 0x0015D3B5
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600533C RID: 21308 RVA: 0x0015F1CB File Offset: 0x0015D3CB
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600533D RID: 21309 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600533E RID: 21310 RVA: 0x0015F148 File Offset: 0x0015D348
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600533F RID: 21311 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0015F1D4 File Offset: 0x0015D3D4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.BrickPress_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0015F1F4 File Offset: 0x0015D3F4
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
				this.stationConfiguration = new BrickPressConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			}
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x0015F257 File Offset: 0x0015D457
		protected virtual void LateUpdate()
		{
			this.PressTransform.localPosition = Vector3.Lerp(this.PressTransform_Raised.localPosition, this.PressTransform_Lowered.localPosition, this.Handle.CurrentPosition);
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x0015F28A File Offset: 0x0015D48A
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x0015F2A4 File Offset: 0x0015D4A4
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			BrickPress.<>c__DisplayClass98_0 CS$<>8__locals1 = new BrickPress.<>c__DisplayClass98_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x0015F2E4 File Offset: 0x0015D4E4
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.isOpen)
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

		// Token: 0x06005346 RID: 21318 RVA: 0x0015F30F File Offset: 0x0015D50F
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IUsable)this).IsInUse)
			{
				reason = "In use";
				return false;
			}
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x0015F33B File Offset: 0x0015D53B
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

		// Token: 0x06005348 RID: 21320 RVA: 0x0015F37A File Offset: 0x0015D57A
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x0015F390 File Offset: 0x0015D590
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0015F3A8 File Offset: 0x0015D5A8
		public PackagingStation.EState GetState()
		{
			ProductItemInstance productItemInstance;
			if (!this.HasSufficientProduct(out productItemInstance))
			{
				return PackagingStation.EState.InsufficentProduct;
			}
			if (this.OutputSlot.ItemInstance != null)
			{
				if ((this.OutputSlot.ItemInstance as QualityItemInstance).Quality != productItemInstance.Quality)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.ItemInstance.ID != productItemInstance.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.ItemInstance.ID != productItemInstance.ID)
				{
					return PackagingStation.EState.Mismatch;
				}
				if (this.OutputSlot.ItemInstance.Quantity >= this.OutputSlot.ItemInstance.StackLimit)
				{
					return PackagingStation.EState.OutputSlotFull;
				}
			}
			return PackagingStation.EState.CanBegin;
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x0015F454 File Offset: 0x0015D654
		private void UpdateInputVisuals()
		{
			ItemInstance itemInstance;
			int num;
			ItemInstance itemInstance2;
			int num2;
			this.GetMainInputs(out itemInstance, out num, out itemInstance2, out num2);
			if (itemInstance != null)
			{
				this.Container1.SetContents(itemInstance as ProductItemInstance, (float)num / 20f);
			}
			else
			{
				this.Container1.SetContents(null, 0f);
			}
			if (itemInstance2 != null)
			{
				this.Container2.SetContents(itemInstance2 as ProductItemInstance, (float)num2 / 20f);
				return;
			}
			this.Container2.SetContents(null, 0f);
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x0015F4CC File Offset: 0x0015D6CC
		public bool HasSufficientProduct(out ProductItemInstance product)
		{
			ItemInstance itemInstance;
			int num;
			ItemInstance itemInstance2;
			int num2;
			this.GetMainInputs(out itemInstance, out num, out itemInstance2, out num2);
			if (itemInstance == null)
			{
				product = null;
				return false;
			}
			product = (itemInstance as ProductItemInstance);
			return num >= 20;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x0015F500 File Offset: 0x0015D700
		public void GetMainInputs(out ItemInstance primaryItem, out int primaryItemQuantity, out ItemInstance secondaryItem, out int secondaryItemQuantity)
		{
			BrickPress.<>c__DisplayClass107_0 CS$<>8__locals1 = new BrickPress.<>c__DisplayClass107_0();
			CS$<>8__locals1.<>4__this = this;
			List<ItemInstance> list = new List<ItemInstance>();
			CS$<>8__locals1.itemQuantities = new Dictionary<ItemInstance, int>();
			int i;
			int k;
			for (i = 0; i < this.InputSlots.Count; i = k + 1)
			{
				if (this.InputSlots[i].ItemInstance != null)
				{
					ItemInstance itemInstance = list.Find((ItemInstance x) => x.ID == CS$<>8__locals1.<>4__this.InputSlots[i].ItemInstance.ID);
					if (itemInstance == null || !itemInstance.CanStackWith(this.InputSlots[i].ItemInstance, false))
					{
						itemInstance = this.InputSlots[i].ItemInstance;
						list.Add(itemInstance);
						if (!CS$<>8__locals1.itemQuantities.ContainsKey(this.InputSlots[i].ItemInstance))
						{
							CS$<>8__locals1.itemQuantities.Add(this.InputSlots[i].ItemInstance, 0);
						}
					}
					Dictionary<ItemInstance, int> itemQuantities = CS$<>8__locals1.itemQuantities;
					ItemInstance key = itemInstance;
					itemQuantities[key] += this.InputSlots[i].Quantity;
				}
				k = i;
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (CS$<>8__locals1.itemQuantities[list[j]] > 20)
				{
					int num = CS$<>8__locals1.itemQuantities[list[j]] - 20;
					CS$<>8__locals1.itemQuantities[list[j]] = 20;
					ItemInstance copy = list[j].GetCopy(num);
					list.Add(copy);
					CS$<>8__locals1.itemQuantities.Add(copy, num);
				}
			}
			list = (from x in list
			orderby CS$<>8__locals1.itemQuantities[x] descending
			select x).ToList<ItemInstance>();
			if (list.Count > 0)
			{
				primaryItem = list[0];
				primaryItemQuantity = CS$<>8__locals1.itemQuantities[list[0]];
			}
			else
			{
				primaryItem = null;
				primaryItemQuantity = 0;
			}
			if (list.Count > 1)
			{
				secondaryItem = list[1];
				secondaryItemQuantity = CS$<>8__locals1.itemQuantities[list[1]];
				return;
			}
			secondaryItem = null;
			secondaryItemQuantity = 0;
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x0015F758 File Offset: 0x0015D958
		public Draggable CreateFunctionalContainer(ProductItemInstance instance, float productScale, out List<FunctionalProduct> products)
		{
			Draggable draggable = UnityEngine.Object.Instantiate<Draggable>(this.FunctionalContainerPrefab, NetworkSingleton<GameManager>.Instance.Temp);
			draggable.transform.position = this.ContainerSpawnPoint.position;
			draggable.transform.rotation = this.ContainerSpawnPoint.rotation;
			draggable.GetComponent<DraggableConstraint>().SetContainer(base.transform);
			Transform transform = draggable.transform.Find("ProductSpawnPoints");
			ProductDefinition productDefinition = instance.Definition as ProductDefinition;
			products = new List<FunctionalProduct>();
			for (int i = 0; i < 20; i++)
			{
				Transform child = transform.GetChild(i);
				FunctionalProduct functionalProduct = UnityEngine.Object.Instantiate<FunctionalProduct>(productDefinition.FunctionalProduct, NetworkSingleton<GameManager>.Instance.Temp);
				functionalProduct.transform.position = child.position;
				functionalProduct.transform.rotation = child.rotation;
				functionalProduct.transform.localScale = Vector3.one * productScale;
				functionalProduct.Initialize(instance);
				products.Add(functionalProduct);
			}
			return draggable;
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x0015F858 File Offset: 0x0015DA58
		public void PlayPressAnim()
		{
			base.StartCoroutine(this.<PlayPressAnim>g__Routine|109_0());
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x0015F868 File Offset: 0x0015DA68
		public void CompletePress(ProductItemInstance product)
		{
			ProductItemInstance productItemInstance = product.GetCopy(1) as ProductItemInstance;
			productItemInstance.SetPackaging(this.BrickPackaging);
			this.OutputSlot.AddItem(productItemInstance, false);
			int num = 20;
			int num2 = 0;
			while (num2 < this.InputSlots.Count && num > 0)
			{
				if (this.InputSlots[num2].ItemInstance != null && this.InputSlots[num2].ItemInstance.CanStackWith(product, false))
				{
					int num3 = Mathf.Min(num, this.InputSlots[num2].Quantity);
					this.InputSlots[num2].ChangeQuantity(-num3, false);
					num -= num3;
				}
				num2++;
			}
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x0015F918 File Offset: 0x0015DB18
		public List<FunctionalProduct> GetProductInMould()
		{
			Collider[] array = Physics.OverlapBox(this.MouldDetection.bounds.center, this.MouldDetection.bounds.extents, this.MouldDetection.transform.rotation, LayerMask.GetMask(new string[]
			{
				"Task"
			}));
			List<FunctionalProduct> list = new List<FunctionalProduct>();
			for (int i = 0; i < array.Length; i++)
			{
				FunctionalProduct componentInParent = array[i].GetComponentInParent<FunctionalProduct>();
				if (componentInParent != null && !list.Contains(componentInParent))
				{
					list.Add(componentInParent);
				}
			}
			return list;
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x0015F9B0 File Offset: 0x0015DBB0
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
			BrickPressUIElement component = UnityEngine.Object.Instantiate<BrickPressUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<BrickPressUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x0015FA43 File Offset: 0x0015DC43
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x0015FA60 File Offset: 0x0015DC60
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

		// Token: 0x06005355 RID: 21333 RVA: 0x0015FABA File Offset: 0x0015DCBA
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x0015FAD8 File Offset: 0x0015DCD8
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
			Singleton<BrickPressCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x0015FB78 File Offset: 0x0015DD78
		public void Close()
		{
			Singleton<BrickPressCanvas>.Instance.SetIsOpen(null, false, true);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x0015FBF4 File Offset: 0x0015DDF4
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x0015FC1C File Offset: 0x0015DE1C
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

		// Token: 0x0600535A RID: 21338 RVA: 0x0015FC7B File Offset: 0x0015DE7B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x0015FC99 File Offset: 0x0015DE99
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x0015FCB7 File Offset: 0x0015DEB7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x0015FCF0 File Offset: 0x0015DEF0
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

		// Token: 0x0600535E RID: 21342 RVA: 0x0015FD6F File Offset: 0x0015DF6F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x0015FD98 File Offset: 0x0015DF98
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

		// Token: 0x06005360 RID: 21344 RVA: 0x0015FDF7 File Offset: 0x0015DFF7
		public override BuildableItemData GetBaseData()
		{
			return new BrickPressData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.ItemSlots));
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x0015FE28 File Offset: 0x0015E028
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0015FE97 File Offset: 0x0015E097
		[CompilerGenerated]
		private IEnumerator <PlayPressAnim>g__Routine|109_0()
		{
			this.Handle.Locked = true;
			this.Handle.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.SlamSound.Play();
			yield return new WaitForSeconds(0.5f);
			this.Handle.Locked = false;
			yield break;
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x0015FEA8 File Offset: 0x0015E0A8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted = true;
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
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.BrickPress));
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x001600A1 File Offset: 0x0015E2A1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.BrickPressAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x001600DB File Offset: 0x0015E2DB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x001600EC File Offset: 0x0015E2EC
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

		// Token: 0x06005368 RID: 21352 RVA: 0x00160193 File Offset: 0x0015E393
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x0016019C File Offset: 0x0015E39C
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

		// Token: 0x0600536A RID: 21354 RVA: 0x001601DC File Offset: 0x0015E3DC
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

		// Token: 0x0600536B RID: 21355 RVA: 0x00160283 File Offset: 0x0015E483
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x0016028C File Offset: 0x0015E48C
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

		// Token: 0x0600536D RID: 21357 RVA: 0x001602CC File Offset: 0x0015E4CC
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

		// Token: 0x0600536E RID: 21358 RVA: 0x00160373 File Offset: 0x0015E573
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0016037C File Offset: 0x0015E57C
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

		// Token: 0x06005370 RID: 21360 RVA: 0x001603BC File Offset: 0x0015E5BC
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

		// Token: 0x06005371 RID: 21361 RVA: 0x00160482 File Offset: 0x0015E682
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x001604AC File Offset: 0x0015E6AC
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

		// Token: 0x06005373 RID: 21363 RVA: 0x00160514 File Offset: 0x0015E714
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

		// Token: 0x06005374 RID: 21364 RVA: 0x001605DC File Offset: 0x0015E7DC
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x00160608 File Offset: 0x0015E808
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

		// Token: 0x06005376 RID: 21366 RVA: 0x0016065C File Offset: 0x0015E85C
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

		// Token: 0x06005377 RID: 21367 RVA: 0x00160724 File Offset: 0x0015E924
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

		// Token: 0x06005378 RID: 21368 RVA: 0x0016077C File Offset: 0x0015E97C
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

		// Token: 0x06005379 RID: 21369 RVA: 0x0016083A File Offset: 0x0015EA3A
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x00160844 File Offset: 0x0015EA44
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

		// Token: 0x0600537B RID: 21371 RVA: 0x001608A0 File Offset: 0x0015EAA0
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

		// Token: 0x0600537C RID: 21372 RVA: 0x0016096D File Offset: 0x0015EB6D
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x00160984 File Offset: 0x0015EB84
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

		// Token: 0x0600537E RID: 21374 RVA: 0x001609DC File Offset: 0x0015EBDC
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

		// Token: 0x0600537F RID: 21375 RVA: 0x00160ABC File Offset: 0x0015ECBC
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x00160AEC File Offset: 0x0015ECEC
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

		// Token: 0x06005381 RID: 21377 RVA: 0x00160B74 File Offset: 0x0015ED74
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

		// Token: 0x06005382 RID: 21378 RVA: 0x00160C55 File Offset: 0x0015EE55
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x00160C84 File Offset: 0x0015EE84
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

		// Token: 0x06005384 RID: 21380 RVA: 0x00160D00 File Offset: 0x0015EF00
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

		// Token: 0x06005385 RID: 21381 RVA: 0x00160DE4 File Offset: 0x0015EFE4
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

		// Token: 0x06005386 RID: 21382 RVA: 0x00160E58 File Offset: 0x0015F058
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

		// Token: 0x06005387 RID: 21383 RVA: 0x00160F1E File Offset: 0x0015F11E
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x00160F48 File Offset: 0x0015F148
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

		// Token: 0x06005389 RID: 21385 RVA: 0x00160FB0 File Offset: 0x0015F1B0
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

		// Token: 0x0600538A RID: 21386 RVA: 0x00161078 File Offset: 0x0015F278
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x00161090 File Offset: 0x0015F290
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

		// Token: 0x0600538C RID: 21388 RVA: 0x001610E4 File Offset: 0x0015F2E4
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

		// Token: 0x0600538D RID: 21389 RVA: 0x001611AC File Offset: 0x0015F3AC
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

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600538E RID: 21390 RVA: 0x00161203 File Offset: 0x0015F403
		// (set) Token: 0x0600538F RID: 21391 RVA: 0x0016120B File Offset: 0x0015F40B
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

		// Token: 0x06005390 RID: 21392 RVA: 0x00161248 File Offset: 0x0015F448
		public override bool BrickPress(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x00161322 File Offset: 0x0015F522
		// (set) Token: 0x06005392 RID: 21394 RVA: 0x0016132A File Offset: 0x0015F52A
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

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x00161366 File Offset: 0x0015F566
		// (set) Token: 0x06005394 RID: 21396 RVA: 0x0016136E File Offset: 0x0015F56E
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

		// Token: 0x06005395 RID: 21397 RVA: 0x001613AC File Offset: 0x0015F5AC
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.ProductSlots = new ItemSlot[2];
				for (int i = 0; i < 2; i++)
				{
					this.ProductSlots[i] = new ItemSlot(true);
					this.ProductSlots[i].SetSlotOwner(this);
					this.ProductSlots[i].AddFilter(new ItemFilter_UnpackagedProduct());
					ItemSlot itemSlot = this.ProductSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.UpdateInputVisuals));
				}
				new ItemSlotSiblingSet(this.ProductSlots);
				this.OutputSlot = new ItemSlot();
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.AddRange(this.ProductSlots);
				this.OutputSlots.Add(this.OutputSlot);
			}
		}

		// Token: 0x04003E29 RID: 15913
		public const int INPUT_SLOT_COUNT = 2;

		// Token: 0x04003E2D RID: 15917
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04003E2E RID: 15918
		public Transform CameraPosition_Pouring;

		// Token: 0x04003E2F RID: 15919
		public Transform CameraPosition_Raising;

		// Token: 0x04003E30 RID: 15920
		public InteractableObject IntObj;

		// Token: 0x04003E31 RID: 15921
		public Transform uiPoint;

		// Token: 0x04003E32 RID: 15922
		public Transform StandPoint;

		// Token: 0x04003E33 RID: 15923
		public Transform[] accessPoints;

		// Token: 0x04003E34 RID: 15924
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003E35 RID: 15925
		public BrickPressContainer Container1;

		// Token: 0x04003E36 RID: 15926
		public BrickPressContainer Container2;

		// Token: 0x04003E37 RID: 15927
		public Transform ContainerSpawnPoint;

		// Token: 0x04003E38 RID: 15928
		public PackagingDefinition BrickPackaging;

		// Token: 0x04003E39 RID: 15929
		public BoxCollider MouldDetection;

		// Token: 0x04003E3A RID: 15930
		public BrickPressHandle Handle;

		// Token: 0x04003E3B RID: 15931
		public Transform PressTransform;

		// Token: 0x04003E3C RID: 15932
		public Transform PressTransform_Raised;

		// Token: 0x04003E3D RID: 15933
		public Transform PressTransform_Lowered;

		// Token: 0x04003E3E RID: 15934
		public Transform PressTransform_Compressed;

		// Token: 0x04003E3F RID: 15935
		public AudioSourceController SlamSound;

		// Token: 0x04003E40 RID: 15936
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003E41 RID: 15937
		[Header("Prefabs")]
		public Draggable FunctionalContainerPrefab;

		// Token: 0x04003E42 RID: 15938
		[Header("UI")]
		public BrickPressUIElement WorldspaceUIPrefab;

		// Token: 0x04003E43 RID: 15939
		public Sprite typeIcon;

		// Token: 0x04003E4D RID: 15949
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003E4E RID: 15950
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003E4F RID: 15951
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003E50 RID: 15952
		private bool dll_Excuted;

		// Token: 0x04003E51 RID: 15953
		private bool dll_Excuted;
	}
}
