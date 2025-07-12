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
using ScheduleOne.Misc;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.Trash;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C14 RID: 3092
	public class Cauldron : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x001619A4 File Offset: 0x0015FBA4
		public bool isOpen
		{
			get
			{
				return Singleton<CauldronCanvas>.Instance.isOpen && Singleton<CauldronCanvas>.Instance.Cauldron == this;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060053BC RID: 21436 RVA: 0x001619C4 File Offset: 0x0015FBC4
		// (set) Token: 0x060053BD RID: 21437 RVA: 0x001619CC File Offset: 0x0015FBCC
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x060053BE RID: 21438 RVA: 0x001619D5 File Offset: 0x0015FBD5
		// (set) Token: 0x060053BF RID: 21439 RVA: 0x001619DD File Offset: 0x0015FBDD
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

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060053C0 RID: 21440 RVA: 0x001619E7 File Offset: 0x0015FBE7
		// (set) Token: 0x060053C1 RID: 21441 RVA: 0x001619EF File Offset: 0x0015FBEF
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

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060053C3 RID: 21443 RVA: 0x001619F9 File Offset: 0x0015FBF9
		// (set) Token: 0x060053C4 RID: 21444 RVA: 0x00161A01 File Offset: 0x0015FC01
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060053C5 RID: 21445 RVA: 0x00161A0A File Offset: 0x0015FC0A
		// (set) Token: 0x060053C6 RID: 21446 RVA: 0x00161A12 File Offset: 0x0015FC12
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060053C7 RID: 21447 RVA: 0x00161A1B File Offset: 0x0015FC1B
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060053C8 RID: 21448 RVA: 0x00161A23 File Offset: 0x0015FC23
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060053C9 RID: 21449 RVA: 0x00161A2B File Offset: 0x0015FC2B
		public bool Selectable { get; } = 1;

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x00161A33 File Offset: 0x0015FC33
		// (set) Token: 0x060053CB RID: 21451 RVA: 0x00161A3B File Offset: 0x0015FC3B
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060053CC RID: 21452 RVA: 0x00161A44 File Offset: 0x0015FC44
		public EntityConfiguration Configuration
		{
			get
			{
				return this.cauldronConfiguration;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060053CD RID: 21453 RVA: 0x00161A4C File Offset: 0x0015FC4C
		// (set) Token: 0x060053CE RID: 21454 RVA: 0x00161A54 File Offset: 0x0015FC54
		protected CauldronConfiguration cauldronConfiguration { get; set; }

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060053CF RID: 21455 RVA: 0x00161A5D File Offset: 0x0015FC5D
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060053D0 RID: 21456 RVA: 0x00161A65 File Offset: 0x0015FC65
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Cauldron;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x00161A68 File Offset: 0x0015FC68
		// (set) Token: 0x060053D2 RID: 21458 RVA: 0x00161A70 File Offset: 0x0015FC70
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x00161A79 File Offset: 0x0015FC79
		// (set) Token: 0x060053D4 RID: 21460 RVA: 0x00161A81 File Offset: 0x0015FC81
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

		// Token: 0x060053D5 RID: 21461 RVA: 0x00161A8B File Offset: 0x0015FC8B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060053D6 RID: 21462 RVA: 0x00161AA1 File Offset: 0x0015FCA1
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060053D7 RID: 21463 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060053D8 RID: 21464 RVA: 0x00161AA9 File Offset: 0x0015FCA9
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060053D9 RID: 21465 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x00161AB1 File Offset: 0x0015FCB1
		private bool isCooking
		{
			get
			{
				return this.RemainingCookTime > 0;
			}
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x00161ABC File Offset: 0x0015FCBC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Cauldron_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x00161ADC File Offset: 0x0015FCDC
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
				this.cauldronConfiguration = new CauldronConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			}
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x00161B40 File Offset: 0x0015FD40
		protected override void Start()
		{
			base.Start();
			if (!this.isGhost)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onTimeSkip = (Action<int>)Delegate.Combine(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
				this.StartButtonClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ButtonClicked));
			}
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x00161BC3 File Offset: 0x0015FDC3
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.RemainingCookTime > 0)
			{
				this.StartCookOperation(connection, this.RemainingCookTime, this.InputQuality);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x00161BF0 File Offset: 0x0015FDF0
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Cauldron.<>c__DisplayClass108_0 CS$<>8__locals1 = new Cauldron.<>c__DisplayClass108_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x00161C30 File Offset: 0x0015FE30
		public override void DestroyItem(bool callOnServer = true)
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Remove(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x00161CC8 File Offset: 0x0015FEC8
		private void MinPass()
		{
			if (this.RemainingCookTime > 0)
			{
				this.Alarm.SetScreenLit(true);
				this.Alarm.DisplayMinutes(this.RemainingCookTime);
				this.Light.isOn = true;
				this.RemainingCookTime--;
				if (this.RemainingCookTime <= 0 && InstanceFinder.IsServer)
				{
					this.FinishCookOperation();
					return;
				}
			}
			else
			{
				this.Alarm.SetScreenLit(false);
				this.Alarm.DisplayMinutes(0);
				if (this.OutputSlot.Quantity > 0)
				{
					this.Light.isOn = (NetworkSingleton<TimeManager>.Instance.DailyMinTotal % 2 == 0);
					return;
				}
				this.Light.isOn = false;
			}
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x00161D7C File Offset: 0x0015FF7C
		private void TimeSkipped(int minsPassed)
		{
			for (int i = 0; i < minsPassed; i++)
			{
				this.MinPass();
			}
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x00161D9B File Offset: 0x0015FF9B
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

		// Token: 0x060053E4 RID: 21476 RVA: 0x00161DC8 File Offset: 0x0015FFC8
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

		// Token: 0x060053E5 RID: 21477 RVA: 0x00161E22 File Offset: 0x00160022
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00161E40 File Offset: 0x00160040
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
			Singleton<CauldronCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x00161EE0 File Offset: 0x001600E0
		public void Close()
		{
			Singleton<CauldronCanvas>.Instance.SetIsOpen(null, false, true);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x00161F5C File Offset: 0x0016015C
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			if (((IUsable)this).IsInUse)
			{
				reason = "Currently in use";
				return false;
			}
			if (this.isCooking)
			{
				reason = "Currently cooking";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x00161F9C File Offset: 0x0016019C
		private void UpdateIngredientVisuals()
		{
			ItemInstance itemInstance;
			int num;
			ItemInstance itemInstance2;
			int num2;
			this.GetMainInputs(out itemInstance, out num, out itemInstance2, out num2);
			if (itemInstance != null)
			{
				this.PrimaryTub.Configure(CauldronDisplayTub.EContents.CocaLeaf, (float)num / 20f);
			}
			else
			{
				this.PrimaryTub.Configure(CauldronDisplayTub.EContents.None, 0f);
			}
			if (itemInstance2 != null)
			{
				this.SecondaryTub.Configure(CauldronDisplayTub.EContents.CocaLeaf, (float)num2 / 20f);
				return;
			}
			this.SecondaryTub.Configure(CauldronDisplayTub.EContents.None, 0f);
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x0016200C File Offset: 0x0016020C
		public void GetMainInputs(out ItemInstance primaryItem, out int primaryItemQuantity, out ItemInstance secondaryItem, out int secondaryItemQuantity)
		{
			Cauldron.<>c__DisplayClass119_0 CS$<>8__locals1 = new Cauldron.<>c__DisplayClass119_0();
			CS$<>8__locals1.<>4__this = this;
			List<ItemInstance> list = new List<ItemInstance>();
			CS$<>8__locals1.itemQuantities = new Dictionary<ItemInstance, int>();
			int i;
			int k;
			for (i = 0; i < this.IngredientSlots.Length; i = k + 1)
			{
				if (this.IngredientSlots[i].ItemInstance != null)
				{
					ItemInstance itemInstance = list.Find((ItemInstance x) => x.ID == CS$<>8__locals1.<>4__this.IngredientSlots[i].ItemInstance.ID);
					if (itemInstance == null || !itemInstance.CanStackWith(this.IngredientSlots[i].ItemInstance, false))
					{
						itemInstance = this.IngredientSlots[i].ItemInstance;
						list.Add(itemInstance);
						if (!CS$<>8__locals1.itemQuantities.ContainsKey(this.IngredientSlots[i].ItemInstance))
						{
							CS$<>8__locals1.itemQuantities.Add(this.IngredientSlots[i].ItemInstance, 0);
						}
					}
					Dictionary<ItemInstance, int> itemQuantities = CS$<>8__locals1.itemQuantities;
					ItemInstance key = itemInstance;
					itemQuantities[key] += this.IngredientSlots[i].Quantity;
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

		// Token: 0x060053EB RID: 21483 RVA: 0x00162249 File Offset: 0x00160449
		public Cauldron.EState GetState()
		{
			if (this.isCooking)
			{
				return Cauldron.EState.Cooking;
			}
			if (!this.HasIngredients())
			{
				return Cauldron.EState.MissingIngredients;
			}
			if (!this.HasOutputSpace())
			{
				return Cauldron.EState.OutputFull;
			}
			return Cauldron.EState.Ready;
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x0016226C File Offset: 0x0016046C
		public bool HasOutputSpace()
		{
			ItemInstance defaultInstance = this.CocaineBaseDefinition.GetDefaultInstance(1);
			return this.OutputSlot.GetCapacityForItem(defaultInstance, false) >= 10;
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x0016229C File Offset: 0x0016049C
		public EQuality RemoveIngredients()
		{
			this.LiquidSlot.ChangeQuantity(-1, false);
			EQuality equality = EQuality.Heavenly;
			int num = 20;
			int num2 = this.IngredientSlots.Length - 1;
			while (num2 >= 0 && num > 0)
			{
				if (this.IngredientSlots[num2].Quantity > 0)
				{
					EQuality quality = (this.IngredientSlots[num2].ItemInstance as QualityItemInstance).Quality;
					if (quality < equality)
					{
						equality = quality;
					}
					int num3 = Mathf.Min(num, this.IngredientSlots[num2].Quantity);
					this.IngredientSlots[num2].ChangeQuantity(-num3, false);
					num -= num3;
				}
				num2--;
			}
			return equality;
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x00162330 File Offset: 0x00160530
		public bool HasIngredients()
		{
			int num = 0;
			int quantity = this.LiquidSlot.Quantity;
			for (int i = 0; i < this.IngredientSlots.Length; i++)
			{
				if (this.IngredientSlots[i].ItemInstance != null)
				{
					num += this.IngredientSlots[i].Quantity;
				}
			}
			return num >= 20 && quantity > 0;
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x00162388 File Offset: 0x00160588
		[ServerRpc(RequireOwnership = false)]
		public void SendCookOperation(int remainingCookTime, EQuality quality)
		{
			this.RpcWriter___Server_SendCookOperation_3536682170(remainingCookTime, quality);
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x00162398 File Offset: 0x00160598
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void StartCookOperation(NetworkConnection conn, int remainingCookTime, EQuality quality)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_StartCookOperation_4210838825(conn, remainingCookTime, quality);
				this.RpcLogic___StartCookOperation_4210838825(conn, remainingCookTime, quality);
			}
			else
			{
				this.RpcWriter___Target_StartCookOperation_4210838825(conn, remainingCookTime, quality);
			}
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x001623E8 File Offset: 0x001605E8
		[ObserversRpc]
		public void FinishCookOperation()
		{
			this.RpcWriter___Observers_FinishCookOperation_2166136261();
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x001623FB File Offset: 0x001605FB
		private void ButtonClicked(RaycastHit hit)
		{
			if (this.onStartButtonClicked != null)
			{
				this.onStartButtonClicked.Invoke();
			}
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00162410 File Offset: 0x00160610
		public void CreateTrash(List<StationItem> mixerItems)
		{
			for (int i = 0; i < mixerItems.Count; i++)
			{
				if (!(mixerItems[i].TrashPrefab == null))
				{
					Vector3 posiiton = this.TrashSpawnVolume.transform.TransformPoint(new Vector3(UnityEngine.Random.Range(-this.TrashSpawnVolume.size.x / 2f, this.TrashSpawnVolume.size.x / 2f), 0f, UnityEngine.Random.Range(-this.TrashSpawnVolume.size.z / 2f, this.TrashSpawnVolume.size.z / 2f)));
					Vector3 vector = this.TrashSpawnVolume.transform.forward;
					vector = Quaternion.Euler(0f, UnityEngine.Random.Range(-45f, 45f), 0f) * vector;
					float d = UnityEngine.Random.Range(0.25f, 0.4f);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(mixerItems[i].TrashPrefab.ID, posiiton, UnityEngine.Random.rotation, vector * d, "", false);
				}
			}
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x0016253F File Offset: 0x0016073F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00162555 File Offset: 0x00160755
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x0016256B File Offset: 0x0016076B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x00162594 File Offset: 0x00160794
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

		// Token: 0x060053F8 RID: 21496 RVA: 0x001625F3 File Offset: 0x001607F3
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x00162611 File Offset: 0x00160811
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x0016262F File Offset: 0x0016082F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00162668 File Offset: 0x00160868
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

		// Token: 0x060053FC RID: 21500 RVA: 0x001626E7 File Offset: 0x001608E7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x00162710 File Offset: 0x00160910
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

		// Token: 0x060053FE RID: 21502 RVA: 0x00162770 File Offset: 0x00160970
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
			CauldronUIElement component = UnityEngine.Object.Instantiate<CauldronUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<CauldronUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00162803 File Offset: 0x00160A03
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00162820 File Offset: 0x00160A20
		public override BuildableItemData GetBaseData()
		{
			return new CauldronData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>(this.IngredientSlots)), new ItemSet(new List<ItemSlot>
			{
				this.LiquidSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.RemainingCookTime, this.InputQuality);
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x0016289C File Offset: 0x00160A9C
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x00162928 File Offset: 0x00160B28
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendCookOperation_3536682170));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_StartCookOperation_4210838825));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_StartCookOperation_4210838825));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_FinishCookOperation_2166136261));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(17U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(21U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(22U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(23U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(24U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(25U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.Cauldron));
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x00162B7D File Offset: 0x00160D7D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.CauldronAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00162BB7 File Offset: 0x00160DB7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00162BC8 File Offset: 0x00160DC8
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

		// Token: 0x06005407 RID: 21511 RVA: 0x00162C6F File Offset: 0x00160E6F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x00162C78 File Offset: 0x00160E78
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

		// Token: 0x06005409 RID: 21513 RVA: 0x00162CB8 File Offset: 0x00160EB8
		private void RpcWriter___Server_SendCookOperation_3536682170(int remainingCookTime, EQuality quality)
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
			writer.WriteInt32(remainingCookTime, 1);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x00162D71 File Offset: 0x00160F71
		public void RpcLogic___SendCookOperation_3536682170(int remainingCookTime, EQuality quality)
		{
			this.StartCookOperation(null, remainingCookTime, quality);
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x00162D7C File Offset: 0x00160F7C
		private void RpcReader___Server_SendCookOperation_3536682170(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int remainingCookTime = PooledReader0.ReadInt32(1);
			EQuality quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendCookOperation_3536682170(remainingCookTime, quality);
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x00162DC4 File Offset: 0x00160FC4
		private void RpcWriter___Observers_StartCookOperation_4210838825(NetworkConnection conn, int remainingCookTime, EQuality quality)
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
			writer.WriteInt32(remainingCookTime, 1);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendObserversRpc(10U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x00162E8C File Offset: 0x0016108C
		public void RpcLogic___StartCookOperation_4210838825(NetworkConnection conn, int remainingCookTime, EQuality quality)
		{
			this.RemainingCookTime = remainingCookTime;
			this.InputQuality = quality;
			this.CauldronFillable.AddLiquid("gasoline", 1f, Color.white);
			if (this.onCookStart != null)
			{
				this.onCookStart.Invoke();
			}
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x00162ECC File Offset: 0x001610CC
		private void RpcReader___Observers_StartCookOperation_4210838825(PooledReader PooledReader0, Channel channel)
		{
			int remainingCookTime = PooledReader0.ReadInt32(1);
			EQuality quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartCookOperation_4210838825(null, remainingCookTime, quality);
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x00162F20 File Offset: 0x00161120
		private void RpcWriter___Target_StartCookOperation_4210838825(NetworkConnection conn, int remainingCookTime, EQuality quality)
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
			writer.WriteInt32(remainingCookTime, 1);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendTargetRpc(11U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x00162FE8 File Offset: 0x001611E8
		private void RpcReader___Target_StartCookOperation_4210838825(PooledReader PooledReader0, Channel channel)
		{
			int remainingCookTime = PooledReader0.ReadInt32(1);
			EQuality quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartCookOperation_4210838825(base.LocalConnection, remainingCookTime, quality);
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x00163038 File Offset: 0x00161238
		private void RpcWriter___Observers_FinishCookOperation_2166136261()
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
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x001630E4 File Offset: 0x001612E4
		public void RpcLogic___FinishCookOperation_2166136261()
		{
			if (InstanceFinder.IsServer)
			{
				QualityItemInstance qualityItemInstance = this.CocaineBaseDefinition.GetDefaultInstance(10) as QualityItemInstance;
				qualityItemInstance.SetQuality(this.InputQuality);
				this.OutputSlot.InsertItem(qualityItemInstance);
			}
			this.CauldronFillable.ResetContents();
			if (this.onCookEnd != null)
			{
				this.onCookEnd.Invoke();
			}
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x00163144 File Offset: 0x00161344
		private void RpcReader___Observers_FinishCookOperation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___FinishCookOperation_2166136261();
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x00163164 File Offset: 0x00161364
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
			base.SendServerRpc(13U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x0016320B File Offset: 0x0016140B
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x00163214 File Offset: 0x00161414
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

		// Token: 0x06005417 RID: 21527 RVA: 0x00163254 File Offset: 0x00161454
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
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x001632FB File Offset: 0x001614FB
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x00163304 File Offset: 0x00161504
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

		// Token: 0x0600541A RID: 21530 RVA: 0x00163344 File Offset: 0x00161544
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
			base.SendServerRpc(15U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x0016340A File Offset: 0x0016160A
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x00163434 File Offset: 0x00161634
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

		// Token: 0x0600541D RID: 21533 RVA: 0x0016349C File Offset: 0x0016169C
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
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x00163564 File Offset: 0x00161764
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x00163590 File Offset: 0x00161790
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

		// Token: 0x06005420 RID: 21536 RVA: 0x001635E4 File Offset: 0x001617E4
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
			base.SendTargetRpc(17U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x001636AC File Offset: 0x001618AC
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

		// Token: 0x06005422 RID: 21538 RVA: 0x00163704 File Offset: 0x00161904
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
			base.SendServerRpc(18U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x001637C2 File Offset: 0x001619C2
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x001637CC File Offset: 0x001619CC
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

		// Token: 0x06005425 RID: 21541 RVA: 0x00163828 File Offset: 0x00161A28
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
			base.SendObserversRpc(19U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001638F5 File Offset: 0x00161AF5
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x0016390C File Offset: 0x00161B0C
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

		// Token: 0x06005428 RID: 21544 RVA: 0x00163964 File Offset: 0x00161B64
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
			base.SendServerRpc(20U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x00163A44 File Offset: 0x00161C44
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x00163A74 File Offset: 0x00161C74
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

		// Token: 0x0600542B RID: 21547 RVA: 0x00163AFC File Offset: 0x00161CFC
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
			base.SendTargetRpc(21U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x00163BDD File Offset: 0x00161DDD
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x00163C0C File Offset: 0x00161E0C
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

		// Token: 0x0600542E RID: 21550 RVA: 0x00163C88 File Offset: 0x00161E88
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
			base.SendObserversRpc(22U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x00163D6C File Offset: 0x00161F6C
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

		// Token: 0x06005430 RID: 21552 RVA: 0x00163DE0 File Offset: 0x00161FE0
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
			base.SendServerRpc(23U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x00163EA6 File Offset: 0x001620A6
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x00163ED0 File Offset: 0x001620D0
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

		// Token: 0x06005433 RID: 21555 RVA: 0x00163F38 File Offset: 0x00162138
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
			base.SendObserversRpc(24U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x00164000 File Offset: 0x00162200
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x00164018 File Offset: 0x00162218
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

		// Token: 0x06005436 RID: 21558 RVA: 0x0016406C File Offset: 0x0016226C
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
			base.SendTargetRpc(25U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x00164134 File Offset: 0x00162334
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

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06005438 RID: 21560 RVA: 0x0016418B File Offset: 0x0016238B
		// (set) Token: 0x06005439 RID: 21561 RVA: 0x00164193 File Offset: 0x00162393
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

		// Token: 0x0600543A RID: 21562 RVA: 0x001641D0 File Offset: 0x001623D0
		public override bool Cauldron(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600543B RID: 21563 RVA: 0x001642AA File Offset: 0x001624AA
		// (set) Token: 0x0600543C RID: 21564 RVA: 0x001642B2 File Offset: 0x001624B2
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

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600543D RID: 21565 RVA: 0x001642EE File Offset: 0x001624EE
		// (set) Token: 0x0600543E RID: 21566 RVA: 0x001642F6 File Offset: 0x001624F6
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

		// Token: 0x0600543F RID: 21567 RVA: 0x00164334 File Offset: 0x00162534
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.IngredientSlots = new ItemSlot[4];
				for (int i = 0; i < 4; i++)
				{
					this.IngredientSlots[i] = new ItemSlot(true);
					this.IngredientSlots[i].SetSlotOwner(this);
					this.IngredientSlots[i].AddFilter(new ItemFilter_ID(new List<string>
					{
						"cocaleaf"
					}));
					ItemSlot itemSlot = this.IngredientSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.UpdateIngredientVisuals));
				}
				this.LiquidSlot = new ItemSlot(true);
				this.LiquidSlot.SetSlotOwner(this);
				this.LiquidSlot.AddFilter(new ItemFilter_ID(new List<string>
				{
					"gasoline"
				}));
				this.LiquidVisuals.AddSlot(this.LiquidSlot, false);
				this.OutputSlot = new ItemSlot();
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.AddRange(this.IngredientSlots);
				this.InputSlots.Add(this.LiquidSlot);
				this.OutputSlots.Add(this.OutputSlot);
				new ItemSlotSiblingSet(this.InputSlots);
				this.PrimaryTub.gameObject.SetActive(true);
				this.SecondaryTub.gameObject.SetActive(true);
			}
		}

		// Token: 0x04003E6F RID: 15983
		public const int INGREDIENT_SLOT_COUNT = 4;

		// Token: 0x04003E70 RID: 15984
		public const int COCA_LEAF_REQUIRED = 20;

		// Token: 0x04003E71 RID: 15985
		public ItemSlot[] IngredientSlots;

		// Token: 0x04003E72 RID: 15986
		public ItemSlot LiquidSlot;

		// Token: 0x04003E73 RID: 15987
		public ItemSlot OutputSlot;

		// Token: 0x04003E75 RID: 15989
		public int CookTime = 360;

		// Token: 0x04003E76 RID: 15990
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04003E77 RID: 15991
		public Transform CameraPosition_CombineIngredients;

		// Token: 0x04003E78 RID: 15992
		public Transform CameraPosition_StartMachine;

		// Token: 0x04003E79 RID: 15993
		public InteractableObject IntObj;

		// Token: 0x04003E7A RID: 15994
		public Transform[] accessPoints;

		// Token: 0x04003E7B RID: 15995
		public Transform StandPoint;

		// Token: 0x04003E7C RID: 15996
		public Transform uiPoint;

		// Token: 0x04003E7D RID: 15997
		public StorageVisualizer LiquidVisuals;

		// Token: 0x04003E7E RID: 15998
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003E7F RID: 15999
		public CauldronDisplayTub PrimaryTub;

		// Token: 0x04003E80 RID: 16000
		public CauldronDisplayTub SecondaryTub;

		// Token: 0x04003E81 RID: 16001
		public Transform ItemContainer;

		// Token: 0x04003E82 RID: 16002
		public Transform GasolineSpawnPoint;

		// Token: 0x04003E83 RID: 16003
		public Transform TubSpawnPoint;

		// Token: 0x04003E84 RID: 16004
		public Transform[] LeafSpawns;

		// Token: 0x04003E85 RID: 16005
		public Light OverheadLight;

		// Token: 0x04003E86 RID: 16006
		public Fillable CauldronFillable;

		// Token: 0x04003E87 RID: 16007
		public Clickable StartButtonClickable;

		// Token: 0x04003E88 RID: 16008
		public DigitalAlarm Alarm;

		// Token: 0x04003E89 RID: 16009
		public ToggleableLight Light;

		// Token: 0x04003E8A RID: 16010
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003E8B RID: 16011
		public BoxCollider TrashSpawnVolume;

		// Token: 0x04003E8C RID: 16012
		[Header("Prefabs")]
		public StationItem CocaLeafPrefab;

		// Token: 0x04003E8D RID: 16013
		public StationItem GasolinePrefab;

		// Token: 0x04003E8E RID: 16014
		public Draggable TubPrefab;

		// Token: 0x04003E8F RID: 16015
		public QualityItemDefinition CocaineBaseDefinition;

		// Token: 0x04003E90 RID: 16016
		[Header("UI")]
		public CauldronUIElement WorldspaceUIPrefab;

		// Token: 0x04003E91 RID: 16017
		public Sprite typeIcon;

		// Token: 0x04003E92 RID: 16018
		public UnityEvent onStartButtonClicked;

		// Token: 0x04003E93 RID: 16019
		public UnityEvent onCookStart;

		// Token: 0x04003E94 RID: 16020
		public UnityEvent onCookEnd;

		// Token: 0x04003E95 RID: 16021
		public int RemainingCookTime;

		// Token: 0x04003E96 RID: 16022
		public EQuality InputQuality = EQuality.Standard;

		// Token: 0x04003EA0 RID: 16032
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003EA1 RID: 16033
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003EA2 RID: 16034
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003EA3 RID: 16035
		private bool dll_Excuted;

		// Token: 0x04003EA4 RID: 16036
		private bool dll_Excuted;

		// Token: 0x02000C15 RID: 3093
		public enum EState
		{
			// Token: 0x04003EA6 RID: 16038
			MissingIngredients,
			// Token: 0x04003EA7 RID: 16039
			Ready,
			// Token: 0x04003EA8 RID: 16040
			Cooking,
			// Token: 0x04003EA9 RID: 16041
			OutputFull
		}
	}
}
