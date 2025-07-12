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
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Misc;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using TMPro;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C2B RID: 3115
	public class LabOven : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060055B9 RID: 21945 RVA: 0x0016A9BA File Offset: 0x00168BBA
		public bool isOpen
		{
			get
			{
				return Singleton<LabOvenCanvas>.Instance.isOpen && Singleton<LabOvenCanvas>.Instance.Oven == this;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060055BA RID: 21946 RVA: 0x0016A9DA File Offset: 0x00168BDA
		// (set) Token: 0x060055BB RID: 21947 RVA: 0x0016A9E2 File Offset: 0x00168BE2
		public OvenCookOperation CurrentOperation { get; private set; }

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060055BC RID: 21948 RVA: 0x0016A9EB File Offset: 0x00168BEB
		// (set) Token: 0x060055BD RID: 21949 RVA: 0x0016A9F3 File Offset: 0x00168BF3
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060055BE RID: 21950 RVA: 0x0016A9FC File Offset: 0x00168BFC
		// (set) Token: 0x060055BF RID: 21951 RVA: 0x0016AA04 File Offset: 0x00168C04
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

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060055C0 RID: 21952 RVA: 0x0016AA0E File Offset: 0x00168C0E
		// (set) Token: 0x060055C1 RID: 21953 RVA: 0x0016AA16 File Offset: 0x00168C16
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

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060055C2 RID: 21954 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060055C3 RID: 21955 RVA: 0x0016AA20 File Offset: 0x00168C20
		// (set) Token: 0x060055C4 RID: 21956 RVA: 0x0016AA28 File Offset: 0x00168C28
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060055C5 RID: 21957 RVA: 0x0016AA31 File Offset: 0x00168C31
		// (set) Token: 0x060055C6 RID: 21958 RVA: 0x0016AA39 File Offset: 0x00168C39
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060055C7 RID: 21959 RVA: 0x0016AA42 File Offset: 0x00168C42
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060055C8 RID: 21960 RVA: 0x0016AA4A File Offset: 0x00168C4A
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060055C9 RID: 21961 RVA: 0x0016AA52 File Offset: 0x00168C52
		public bool Selectable { get; } = 1;

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060055CA RID: 21962 RVA: 0x0016AA5A File Offset: 0x00168C5A
		// (set) Token: 0x060055CB RID: 21963 RVA: 0x0016AA62 File Offset: 0x00168C62
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060055CC RID: 21964 RVA: 0x0016AA6B File Offset: 0x00168C6B
		public EntityConfiguration Configuration
		{
			get
			{
				return this.ovenConfiguration;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060055CD RID: 21965 RVA: 0x0016AA73 File Offset: 0x00168C73
		// (set) Token: 0x060055CE RID: 21966 RVA: 0x0016AA7B File Offset: 0x00168C7B
		protected LabOvenConfiguration ovenConfiguration { get; set; }

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060055CF RID: 21967 RVA: 0x0016AA84 File Offset: 0x00168C84
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060055D0 RID: 21968 RVA: 0x0001B3D4 File Offset: 0x000195D4
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.LabOven;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x060055D1 RID: 21969 RVA: 0x0016AA8C File Offset: 0x00168C8C
		// (set) Token: 0x060055D2 RID: 21970 RVA: 0x0016AA94 File Offset: 0x00168C94
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x060055D3 RID: 21971 RVA: 0x0016AA9D File Offset: 0x00168C9D
		// (set) Token: 0x060055D4 RID: 21972 RVA: 0x0016AAA5 File Offset: 0x00168CA5
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

		// Token: 0x060055D5 RID: 21973 RVA: 0x0016AAAF File Offset: 0x00168CAF
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060055D6 RID: 21974 RVA: 0x0016AAC5 File Offset: 0x00168CC5
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060055D7 RID: 21975 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x060055D8 RID: 21976 RVA: 0x0016AACD File Offset: 0x00168CCD
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060055D9 RID: 21977 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x0016AAD8 File Offset: 0x00168CD8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.LabOven_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x0016AAF8 File Offset: 0x00168CF8
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
				this.ovenConfiguration = new LabOvenConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
				TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
				instance3.onTimeSkip = (Action<int>)Delegate.Combine(instance3.onTimeSkip, new Action<int>(this.TimeSkipped));
			}
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x0016ABAA File Offset: 0x00168DAA
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
			if (this.CurrentOperation != null)
			{
				this.SetCookOperation(connection, this.CurrentOperation, false);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x0016ABD8 File Offset: 0x00168DD8
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			LabOven.<>c__DisplayClass125_0 CS$<>8__locals1 = new LabOven.<>c__DisplayClass125_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x0016AC18 File Offset: 0x00168E18
		private void Update()
		{
			switch (this.LightMode)
			{
			case LabOven.ELightMode.Off:
				this.Light.isOn = false;
				break;
			case LabOven.ELightMode.On:
				this.Light.isOn = true;
				break;
			case LabOven.ELightMode.Flash:
				this.Light.isOn = (Mathf.Sin(Time.timeSinceLevelLoad * 4f) > 0f);
				break;
			}
			if (this.CurrentOperation != null)
			{
				this.RunLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.RunLoopSound.VolumeMultiplier, 1f, Time.deltaTime);
				if (!this.RunLoopSound.isPlaying)
				{
					this.RunLoopSound.Play();
					return;
				}
			}
			else
			{
				this.RunLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.RunLoopSound.VolumeMultiplier, 0f, Time.deltaTime);
				if (this.RunLoopSound.VolumeMultiplier <= 0f)
				{
					this.RunLoopSound.Stop();
				}
			}
		}

		// Token: 0x060055DF RID: 21983 RVA: 0x0016AD08 File Offset: 0x00168F08
		private void MinPass()
		{
			if (this.CurrentOperation != null)
			{
				bool flag = this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration();
				this.CurrentOperation.UpdateCookProgress(1);
				if (!flag && this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration())
				{
					this.DingSound.Play();
				}
			}
			this.UpdateOvenAppearance();
			this.UpdateLiquid();
		}

		// Token: 0x060055E0 RID: 21984 RVA: 0x0016AD78 File Offset: 0x00168F78
		private void TimeSkipped(int minsSkippped)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < minsSkippped; i++)
			{
				this.MinPass();
			}
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x0016ADA0 File Offset: 0x00168FA0
		private void UpdateOvenAppearance()
		{
			if (this.CurrentOperation != null)
			{
				this.Button.SetPressed(true);
				this.TimerLabel.enabled = true;
				if (this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration())
				{
					this.SetOvenLit(false);
					this.LightMode = LabOven.ELightMode.Flash;
				}
				else
				{
					this.SetOvenLit(true);
					this.LightMode = LabOven.ELightMode.On;
				}
				int num = this.CurrentOperation.GetCookDuration() - this.CurrentOperation.CookProgress;
				num = Mathf.Max(0, num);
				int num2 = num / 60;
				num %= 60;
				this.TimerLabel.text = string.Format("{0:D2}:{1:D2}", num2, num);
				return;
			}
			this.TimerLabel.enabled = false;
			this.Button.SetPressed(false);
			this.SetOvenLit(false);
			this.LightMode = LabOven.ELightMode.Off;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x0016AE78 File Offset: 0x00169078
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

		// Token: 0x060055E3 RID: 21987 RVA: 0x0016AEA3 File Offset: 0x001690A3
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
			if (this.CurrentOperation != null)
			{
				reason = "Currently cooking";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x0016AEE0 File Offset: 0x001690E0
		public override void DestroyItem(bool callOnServer = true)
		{
			base.DestroyItem(callOnServer);
			if (!this.isGhost)
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
			}
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x0016AF81 File Offset: 0x00169181
		public void SetOvenLit(bool lit)
		{
			this.OvenLight.isOn = lit;
			this.Button.SetPressed(lit);
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x0016AF9B File Offset: 0x0016919B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x0016AFB1 File Offset: 0x001691B1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x0016AFC8 File Offset: 0x001691C8
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

		// Token: 0x060055E9 RID: 21993 RVA: 0x0016B022 File Offset: 0x00169222
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060055EA RID: 21994 RVA: 0x0016B040 File Offset: 0x00169240
		public void Open()
		{
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition_Default.position, this.CameraPosition_Default.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<CompassManager>.Instance.SetVisible(false);
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(this, true, true);
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x0016B0E0 File Offset: 0x001692E0
		public void Close()
		{
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(null, false, true);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x0016B15C File Offset: 0x0016935C
		public bool IsIngredientCookable()
		{
			if (this.IngredientSlot.ItemInstance == null)
			{
				return false;
			}
			StorableItemDefinition storableItemDefinition = this.IngredientSlot.ItemInstance.Definition as StorableItemDefinition;
			return !(storableItemDefinition.StationItem == null) && storableItemDefinition.StationItem.HasModule<CookableModule>();
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x0016B1A9 File Offset: 0x001693A9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCookOperation(OvenCookOperation operation)
		{
			this.RpcWriter___Server_SendCookOperation_3708012700(operation);
			this.RpcLogic___SendCookOperation_3708012700(operation);
		}

		// Token: 0x060055EE RID: 21998 RVA: 0x0016B1C0 File Offset: 0x001693C0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetCookOperation(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetCookOperation_2611294368(conn, operation, playButtonPress);
				this.RpcLogic___SetCookOperation_2611294368(conn, operation, playButtonPress);
			}
			else
			{
				this.RpcWriter___Target_SetCookOperation_2611294368(conn, operation, playButtonPress);
			}
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x0016B20D File Offset: 0x0016940D
		public bool IsReadyToStart()
		{
			return this.IngredientSlot.Quantity > 0 && this.IsIngredientCookable() && this.CurrentOperation == null;
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x0016B230 File Offset: 0x00169430
		public bool IsReadyForHarvest()
		{
			return this.CurrentOperation != null && this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration();
		}

		// Token: 0x060055F1 RID: 22001 RVA: 0x0016B257 File Offset: 0x00169457
		public bool CanOutputSpaceFitCurrentOperation()
		{
			return this.CurrentOperation != null && this.OutputSlot.GetCapacityForItem(this.CurrentOperation.GetProductItem(1), false) >= this.CurrentOperation.Cookable.ProductQuantity;
		}

		// Token: 0x060055F2 RID: 22002 RVA: 0x0016B290 File Offset: 0x00169490
		public void SetLiquidColor(Color col)
		{
			this.LiquidMesh.material.color = col;
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x0016B2A4 File Offset: 0x001694A4
		private void UpdateLiquid()
		{
			if (this.CurrentOperation == null)
			{
				return;
			}
			if (this.CurrentOperation.CookProgress >= this.CurrentOperation.GetCookDuration())
			{
				this.LiquidMesh.gameObject.SetActive(false);
				this.CookedLiquidMesh.gameObject.SetActive(true);
				return;
			}
			this.LiquidMesh.gameObject.SetActive(true);
			this.CookedLiquidMesh.gameObject.SetActive(false);
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x0016B318 File Offset: 0x00169518
		public StationItem[] CreateStationItems(int quantity = 1)
		{
			if (this.IngredientSlot.ItemInstance == null)
			{
				return null;
			}
			StorableItemDefinition storableItemDefinition = this.IngredientSlot.ItemInstance.Definition as StorableItemDefinition;
			if (storableItemDefinition.StationItem == null)
			{
				return null;
			}
			StationItem[] array;
			if (storableItemDefinition.StationItem.GetModule<CookableModule>().CookType == CookableModule.ECookableType.Liquid)
			{
				StationItem stationItem = UnityEngine.Object.Instantiate<StationItem>(storableItemDefinition.StationItem, this.PourableContainer);
				stationItem.Initialize(storableItemDefinition);
				array = new StationItem[]
				{
					stationItem
				};
			}
			else
			{
				array = new StationItem[quantity];
				for (int i = 0; i < quantity; i++)
				{
					StationItem stationItem2 = UnityEngine.Object.Instantiate<StationItem>(storableItemDefinition.StationItem, this.ItemContainer);
					stationItem2.Initialize(storableItemDefinition);
					stationItem2.transform.position = this.SolidIngredientSpawnPoints[i].position;
					stationItem2.transform.rotation = this.SolidIngredientSpawnPoints[i].rotation;
					stationItem2.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0f, 360f));
					array[i] = stationItem2;
				}
			}
			return array;
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x0016B41D File Offset: 0x0016961D
		public void ResetPourableContainer()
		{
			this.PourableContainer.localPosition = this.pourableContainerDefaultPos;
			this.PourableContainer.localRotation = this.pourableContainerDefaultRot;
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x0016B441 File Offset: 0x00169641
		public void ResetSquareTray()
		{
			this.SquareTray.SetParent(this.WireTray.transform);
			this.SquareTray.localPosition = this.squareTrayDefaultPos;
			this.SquareTray.localRotation = this.squareTrayDefaultRot;
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x0016B47C File Offset: 0x0016967C
		public LabOvenHammer CreateHammer()
		{
			LabOvenHammer component = UnityEngine.Object.Instantiate<GameObject>(this.HammerPrefab.gameObject, this.HammerSpawnPoint.position, this.HammerSpawnPoint.rotation).GetComponent<LabOvenHammer>();
			component.Rotator.Bitch = this.OafBastard;
			component.Constraint.Container = this.HammerContainer;
			component.transform.SetParent(this.HammerContainer);
			return component;
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x0016B4E8 File Offset: 0x001696E8
		public void CreateImpactEffects(Vector3 point, bool playSound = true)
		{
			Vector3 vector = this.DecalContainer.InverseTransformPoint(point);
			vector.y = 0f;
			vector.x = Mathf.Clamp(vector.x, this.DecalMinBounds.localPosition.x, this.DecalMaxBounds.localPosition.x);
			vector.z = Mathf.Clamp(vector.z, this.DecalMinBounds.localPosition.z, this.DecalMaxBounds.localPosition.z);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SmashDecalPrefab, this.DecalContainer);
			gameObject.transform.localPosition = vector;
			this.decals.Add(gameObject);
			if (playSound)
			{
				this.ImpactSound.transform.position = point;
				this.ImpactSound.Play();
			}
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x0016B5BC File Offset: 0x001697BC
		public void Shatter(int shardQuantity, GameObject shardPrefab)
		{
			this.CookedLiquidMesh.gameObject.SetActive(false);
			this.ShatterParticles.Play();
			this.ShatterSound.Play();
			this.ClearDecals();
			for (int i = 0; i < shardQuantity; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(shardPrefab.gameObject, NetworkSingleton<GameManager>.Instance.Temp);
				gameObject.transform.position = this.ShardSpawnPoints[i].position;
				gameObject.transform.rotation = this.ShardSpawnPoints[i].rotation;
				gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f, 2);
				gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * 2f, 2);
				this.shards.Add(gameObject);
			}
		}

		// Token: 0x060055FA RID: 22010 RVA: 0x0016B690 File Offset: 0x00169890
		public void ClearShards()
		{
			for (int i = 0; i < this.shards.Count; i++)
			{
				UnityEngine.Object.Destroy(this.shards[i].gameObject);
			}
			this.shards.Clear();
		}

		// Token: 0x060055FB RID: 22011 RVA: 0x0016B6D4 File Offset: 0x001698D4
		public void ClearDecals()
		{
			for (int i = 0; i < this.decals.Count; i++)
			{
				UnityEngine.Object.Destroy(this.decals[i]);
			}
			this.decals.Clear();
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x0016B713 File Offset: 0x00169913
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060055FD RID: 22013 RVA: 0x0016B73C File Offset: 0x0016993C
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

		// Token: 0x060055FE RID: 22014 RVA: 0x0016B79B File Offset: 0x0016999B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x0016B7B9 File Offset: 0x001699B9
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x0016B7D7 File Offset: 0x001699D7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x0016B810 File Offset: 0x00169A10
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

		// Token: 0x06005602 RID: 22018 RVA: 0x0016B88F File Offset: 0x00169A8F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005603 RID: 22019 RVA: 0x0016B8B8 File Offset: 0x00169AB8
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

		// Token: 0x06005604 RID: 22020 RVA: 0x0016B918 File Offset: 0x00169B18
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
			LabOvenUIElement component = UnityEngine.Object.Instantiate<LabOvenUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<LabOvenUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x0016B9AB File Offset: 0x00169BAB
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x0016B9C8 File Offset: 0x00169BC8
		public override BuildableItemData GetBaseData()
		{
			string ingredientID = string.Empty;
			int currentIngredientQuantity = 0;
			EQuality ingredientQuality = EQuality.Standard;
			string productID = string.Empty;
			int currentCookProgress = 0;
			if (this.CurrentOperation != null)
			{
				ingredientID = this.CurrentOperation.IngredientID;
				currentIngredientQuantity = this.CurrentOperation.IngredientQuantity;
				ingredientQuality = this.CurrentOperation.IngredientQuality;
				productID = this.CurrentOperation.ProductID;
				currentCookProgress = this.CurrentOperation.CookProgress;
			}
			return new LabOvenData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.IngredientSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), ingredientID, currentIngredientQuantity, ingredientQuality, productID, currentCookProgress);
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x0016BA84 File Offset: 0x00169C84
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x06005609 RID: 22025 RVA: 0x0016BB14 File Offset: 0x00169D14
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_SendCookOperation_3708012700));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetCookOperation_2611294368));
			base.RegisterTargetRpc(13U, new ClientRpcDelegate(this.RpcReader___Target_SetCookOperation_2611294368));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(19U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(23U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(24U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.LabOven));
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x0016BD52 File Offset: 0x00169F52
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.LabOvenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x0016BD8C File Offset: 0x00169F8C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600560C RID: 22028 RVA: 0x0016BD9C File Offset: 0x00169F9C
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

		// Token: 0x0600560D RID: 22029 RVA: 0x0016BE43 File Offset: 0x0016A043
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x0016BE4C File Offset: 0x0016A04C
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

		// Token: 0x0600560F RID: 22031 RVA: 0x0016BE8C File Offset: 0x0016A08C
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

		// Token: 0x06005610 RID: 22032 RVA: 0x0016BF33 File Offset: 0x0016A133
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06005611 RID: 22033 RVA: 0x0016BF3C File Offset: 0x0016A13C
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

		// Token: 0x06005612 RID: 22034 RVA: 0x0016BF7C File Offset: 0x0016A17C
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

		// Token: 0x06005613 RID: 22035 RVA: 0x0016C023 File Offset: 0x0016A223
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06005614 RID: 22036 RVA: 0x0016C02C File Offset: 0x0016A22C
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

		// Token: 0x06005615 RID: 22037 RVA: 0x0016C06C File Offset: 0x0016A26C
		private void RpcWriter___Server_SendCookOperation_3708012700(OvenCookOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated(operation);
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x0016C113 File Offset: 0x0016A313
		public void RpcLogic___SendCookOperation_3708012700(OvenCookOperation operation)
		{
			this.SetCookOperation(null, operation, true);
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x0016C120 File Offset: 0x0016A320
		private void RpcReader___Server_SendCookOperation_3708012700(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			OvenCookOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCookOperation_3708012700(operation);
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x0016C160 File Offset: 0x0016A360
		private void RpcWriter___Observers_SetCookOperation_2611294368(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
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
			writer.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(playButtonPress);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x0016C224 File Offset: 0x0016A424
		public void RpcLogic___SetCookOperation_2611294368(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
		{
			this.CurrentOperation = operation;
			if (this.CurrentOperation == null)
			{
				this.LiquidMesh.gameObject.SetActive(false);
				this.CookedLiquidMesh.gameObject.SetActive(false);
				return;
			}
			CookableModule module = operation.Ingredient.StationItem.GetModule<CookableModule>();
			if (module == null)
			{
				return;
			}
			this.SetLiquidColor(module.LiquidColor);
			this.CookedLiquidMesh.material.color = module.SolidColor;
			this.UpdateLiquid();
			if (playButtonPress)
			{
				this.ButtonSound.Play();
			}
		}

		// Token: 0x0600561A RID: 22042 RVA: 0x0016C2B4 File Offset: 0x0016A4B4
		private void RpcReader___Observers_SetCookOperation_2611294368(PooledReader PooledReader0, Channel channel)
		{
			OvenCookOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds(PooledReader0);
			bool playButtonPress = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_2611294368(null, operation, playButtonPress);
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x0016C304 File Offset: 0x0016A504
		private void RpcWriter___Target_SetCookOperation_2611294368(NetworkConnection conn, OvenCookOperation operation, bool playButtonPress)
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
			writer.Write___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(playButtonPress);
			base.SendTargetRpc(13U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x0016C3C8 File Offset: 0x0016A5C8
		private void RpcReader___Target_SetCookOperation_2611294368(PooledReader PooledReader0, Channel channel)
		{
			OvenCookOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.OvenCookOperationFishNet.Serializing.Generateds(PooledReader0);
			bool playButtonPress = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_2611294368(base.LocalConnection, operation, playButtonPress);
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x0016C410 File Offset: 0x0016A610
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
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x0016C4D6 File Offset: 0x0016A6D6
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600561F RID: 22047 RVA: 0x0016C500 File Offset: 0x0016A700
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

		// Token: 0x06005620 RID: 22048 RVA: 0x0016C568 File Offset: 0x0016A768
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
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x0016C630 File Offset: 0x0016A830
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x0016C65C File Offset: 0x0016A85C
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

		// Token: 0x06005623 RID: 22051 RVA: 0x0016C6B0 File Offset: 0x0016A8B0
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
			base.SendTargetRpc(16U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x0016C778 File Offset: 0x0016A978
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

		// Token: 0x06005625 RID: 22053 RVA: 0x0016C7D0 File Offset: 0x0016A9D0
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
			base.SendServerRpc(17U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x0016C88E File Offset: 0x0016AA8E
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x0016C898 File Offset: 0x0016AA98
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

		// Token: 0x06005628 RID: 22056 RVA: 0x0016C8F4 File Offset: 0x0016AAF4
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
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x0016C9C1 File Offset: 0x0016ABC1
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x0016C9D8 File Offset: 0x0016ABD8
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

		// Token: 0x0600562B RID: 22059 RVA: 0x0016CA30 File Offset: 0x0016AC30
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
			base.SendServerRpc(19U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x0016CB10 File Offset: 0x0016AD10
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x0016CB40 File Offset: 0x0016AD40
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

		// Token: 0x0600562E RID: 22062 RVA: 0x0016CBC8 File Offset: 0x0016ADC8
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
			base.SendTargetRpc(20U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x0016CCA9 File Offset: 0x0016AEA9
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x0016CCD8 File Offset: 0x0016AED8
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

		// Token: 0x06005631 RID: 22065 RVA: 0x0016CD54 File Offset: 0x0016AF54
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
			base.SendObserversRpc(21U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x0016CE38 File Offset: 0x0016B038
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

		// Token: 0x06005633 RID: 22067 RVA: 0x0016CEAC File Offset: 0x0016B0AC
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
			base.SendServerRpc(22U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x0016CF72 File Offset: 0x0016B172
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x0016CF9C File Offset: 0x0016B19C
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

		// Token: 0x06005636 RID: 22070 RVA: 0x0016D004 File Offset: 0x0016B204
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
			base.SendObserversRpc(23U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x0016D0CC File Offset: 0x0016B2CC
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x0016D0E4 File Offset: 0x0016B2E4
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

		// Token: 0x06005639 RID: 22073 RVA: 0x0016D138 File Offset: 0x0016B338
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
			base.SendTargetRpc(24U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x0016D200 File Offset: 0x0016B400
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

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600563B RID: 22075 RVA: 0x0016D257 File Offset: 0x0016B457
		// (set) Token: 0x0600563C RID: 22076 RVA: 0x0016D25F File Offset: 0x0016B45F
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

		// Token: 0x0600563D RID: 22077 RVA: 0x0016D29C File Offset: 0x0016B49C
		public override bool LabOven(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600563E RID: 22078 RVA: 0x0016D376 File Offset: 0x0016B576
		// (set) Token: 0x0600563F RID: 22079 RVA: 0x0016D37E File Offset: 0x0016B57E
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

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06005640 RID: 22080 RVA: 0x0016D3BA File Offset: 0x0016B5BA
		// (set) Token: 0x06005641 RID: 22081 RVA: 0x0016D3C2 File Offset: 0x0016B5C2
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

		// Token: 0x06005642 RID: 22082 RVA: 0x0016D400 File Offset: 0x0016B600
		protected override void dll()
		{
			base.Awake();
			this.pourableContainerDefaultPos = this.PourableContainer.localPosition;
			this.pourableContainerDefaultRot = this.PourableContainer.localRotation;
			this.squareTrayDefaultPos = this.SquareTray.localPosition;
			this.squareTrayDefaultRot = this.SquareTray.localRotation;
			this.TimerLabel.enabled = false;
			if (!this.isGhost)
			{
				this.IngredientSlot = new ItemSlot(true);
				this.IngredientSlot.SetSlotOwner(this);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputSlot.SetIsAddLocked(true);
				this.InputVisuals.AddSlot(this.IngredientSlot, false);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				this.InputSlots.Add(this.IngredientSlot);
				this.OutputSlots.Add(this.OutputSlot);
				new ItemSlotSiblingSet(this.InputSlots);
			}
		}

		// Token: 0x04003F6F RID: 16239
		public const int SOLID_INGREDIENT_COOK_LIMIT = 10;

		// Token: 0x04003F70 RID: 16240
		public const float FOV_OVERRIDE = 70f;

		// Token: 0x04003F73 RID: 16243
		public LabOven.ELightMode LightMode;

		// Token: 0x04003F74 RID: 16244
		[Header("References")]
		public Transform CameraPosition_Default;

		// Token: 0x04003F75 RID: 16245
		public Transform CameraPosition_Pour;

		// Token: 0x04003F76 RID: 16246
		public Transform CameraPosition_PlaceItems;

		// Token: 0x04003F77 RID: 16247
		public Transform CameraPosition_Breaking;

		// Token: 0x04003F78 RID: 16248
		public InteractableObject IntObj;

		// Token: 0x04003F79 RID: 16249
		public LabOvenDoor Door;

		// Token: 0x04003F7A RID: 16250
		public LabOvenWireTray WireTray;

		// Token: 0x04003F7B RID: 16251
		public ToggleableLight OvenLight;

		// Token: 0x04003F7C RID: 16252
		public LabOvenButton Button;

		// Token: 0x04003F7D RID: 16253
		public TextMeshPro TimerLabel;

		// Token: 0x04003F7E RID: 16254
		public ToggleableLight Light;

		// Token: 0x04003F7F RID: 16255
		public Transform PourableContainer;

		// Token: 0x04003F80 RID: 16256
		public Transform ItemContainer;

		// Token: 0x04003F81 RID: 16257
		public Animation PourAnimation;

		// Token: 0x04003F82 RID: 16258
		public SkinnedMeshRenderer LiquidMesh;

		// Token: 0x04003F83 RID: 16259
		public StorageVisualizer InputVisuals;

		// Token: 0x04003F84 RID: 16260
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003F85 RID: 16261
		public MeshRenderer CookedLiquidMesh;

		// Token: 0x04003F86 RID: 16262
		public Animation RemoveTrayAnimation;

		// Token: 0x04003F87 RID: 16263
		public Transform SquareTray;

		// Token: 0x04003F88 RID: 16264
		public Transform HammerSpawnPoint;

		// Token: 0x04003F89 RID: 16265
		public Transform HammerContainer;

		// Token: 0x04003F8A RID: 16266
		public Transform OafBastard;

		// Token: 0x04003F8B RID: 16267
		public Transform DecalContainer;

		// Token: 0x04003F8C RID: 16268
		public Transform DecalMaxBounds;

		// Token: 0x04003F8D RID: 16269
		public Transform DecalMinBounds;

		// Token: 0x04003F8E RID: 16270
		public BoxCollider CookedLiquidCollider;

		// Token: 0x04003F8F RID: 16271
		public Transform[] ShardSpawnPoints;

		// Token: 0x04003F90 RID: 16272
		public ParticleSystem ShatterParticles;

		// Token: 0x04003F91 RID: 16273
		public Transform uiPoint;

		// Token: 0x04003F92 RID: 16274
		public Transform[] accessPoints;

		// Token: 0x04003F93 RID: 16275
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003F94 RID: 16276
		public Transform[] SolidIngredientSpawnPoints;

		// Token: 0x04003F95 RID: 16277
		public BoxCollider TrayDetectionArea;

		// Token: 0x04003F96 RID: 16278
		[Header("Sounds")]
		public AudioSourceController ButtonSound;

		// Token: 0x04003F97 RID: 16279
		public AudioSourceController DingSound;

		// Token: 0x04003F98 RID: 16280
		public AudioSourceController RunLoopSound;

		// Token: 0x04003F99 RID: 16281
		public AudioSourceController ImpactSound;

		// Token: 0x04003F9A RID: 16282
		public AudioSourceController ShatterSound;

		// Token: 0x04003F9B RID: 16283
		[Header("UI")]
		public LabOvenUIElement WorldspaceUIPrefab;

		// Token: 0x04003F9C RID: 16284
		public Sprite typeIcon;

		// Token: 0x04003F9D RID: 16285
		[Header("Prefabs")]
		public LabOvenHammer HammerPrefab;

		// Token: 0x04003F9E RID: 16286
		public GameObject SmashDecalPrefab;

		// Token: 0x04003FA1 RID: 16289
		public ItemSlot IngredientSlot;

		// Token: 0x04003FA2 RID: 16290
		public ItemSlot OutputSlot;

		// Token: 0x04003FAA RID: 16298
		private Vector3 pourableContainerDefaultPos;

		// Token: 0x04003FAB RID: 16299
		private Quaternion pourableContainerDefaultRot;

		// Token: 0x04003FAC RID: 16300
		private Vector3 squareTrayDefaultPos;

		// Token: 0x04003FAD RID: 16301
		private Quaternion squareTrayDefaultRot;

		// Token: 0x04003FAE RID: 16302
		private List<GameObject> decals = new List<GameObject>();

		// Token: 0x04003FAF RID: 16303
		private List<GameObject> shards = new List<GameObject>();

		// Token: 0x04003FB0 RID: 16304
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003FB1 RID: 16305
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003FB2 RID: 16306
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003FB3 RID: 16307
		private bool dll_Excuted;

		// Token: 0x04003FB4 RID: 16308
		private bool dll_Excuted;

		// Token: 0x02000C2C RID: 3116
		public enum ELightMode
		{
			// Token: 0x04003FB6 RID: 16310
			Off,
			// Token: 0x04003FB7 RID: 16311
			On,
			// Token: 0x04003FB8 RID: 16312
			Flash
		}

		// Token: 0x02000C2D RID: 3117
		public enum EState
		{
			// Token: 0x04003FBA RID: 16314
			CanBegin,
			// Token: 0x04003FBB RID: 16315
			MissingItems,
			// Token: 0x04003FBC RID: 16316
			InsufficentProduct,
			// Token: 0x04003FBD RID: 16317
			OutputSlotFull,
			// Token: 0x04003FBE RID: 16318
			Mismatch
		}
	}
}
