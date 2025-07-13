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
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Misc;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.Trash;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C1F RID: 3103
	public class ChemistryStation : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06005469 RID: 21609 RVA: 0x00164B30 File Offset: 0x00162D30
		public bool isOpen
		{
			get
			{
				return this.PlayerUserObject == Player.Local.NetworkObject;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600546A RID: 21610 RVA: 0x00164B47 File Offset: 0x00162D47
		// (set) Token: 0x0600546B RID: 21611 RVA: 0x00164B4F File Offset: 0x00162D4F
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600546C RID: 21612 RVA: 0x00164B58 File Offset: 0x00162D58
		// (set) Token: 0x0600546D RID: 21613 RVA: 0x00164B60 File Offset: 0x00162D60
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

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600546E RID: 21614 RVA: 0x00164B6A File Offset: 0x00162D6A
		// (set) Token: 0x0600546F RID: 21615 RVA: 0x00164B72 File Offset: 0x00162D72
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

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06005470 RID: 21616 RVA: 0x00164B7C File Offset: 0x00162D7C
		// (set) Token: 0x06005471 RID: 21617 RVA: 0x00164B84 File Offset: 0x00162D84
		public ChemistryCookOperation CurrentCookOperation { get; set; }

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06005472 RID: 21618 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06005473 RID: 21619 RVA: 0x00164B8D File Offset: 0x00162D8D
		// (set) Token: 0x06005474 RID: 21620 RVA: 0x00164B95 File Offset: 0x00162D95
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06005475 RID: 21621 RVA: 0x00164B9E File Offset: 0x00162D9E
		// (set) Token: 0x06005476 RID: 21622 RVA: 0x00164BA6 File Offset: 0x00162DA6
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06005477 RID: 21623 RVA: 0x00164BAF File Offset: 0x00162DAF
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06005478 RID: 21624 RVA: 0x00164BB7 File Offset: 0x00162DB7
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06005479 RID: 21625 RVA: 0x00164BBF File Offset: 0x00162DBF
		public bool Selectable { get; } = 1;

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600547A RID: 21626 RVA: 0x00164BC7 File Offset: 0x00162DC7
		// (set) Token: 0x0600547B RID: 21627 RVA: 0x00164BCF File Offset: 0x00162DCF
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600547C RID: 21628 RVA: 0x00164BD8 File Offset: 0x00162DD8
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600547D RID: 21629 RVA: 0x00164BE0 File Offset: 0x00162DE0
		// (set) Token: 0x0600547E RID: 21630 RVA: 0x00164BE8 File Offset: 0x00162DE8
		protected ChemistryStationConfiguration stationConfiguration { get; set; }

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600547F RID: 21631 RVA: 0x00164BF1 File Offset: 0x00162DF1
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x00010F50 File Offset: 0x0000F150
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.ChemistryStation;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06005481 RID: 21633 RVA: 0x00164BF9 File Offset: 0x00162DF9
		// (set) Token: 0x06005482 RID: 21634 RVA: 0x00164C01 File Offset: 0x00162E01
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06005483 RID: 21635 RVA: 0x00164C0A File Offset: 0x00162E0A
		// (set) Token: 0x06005484 RID: 21636 RVA: 0x00164C12 File Offset: 0x00162E12
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

		// Token: 0x06005485 RID: 21637 RVA: 0x00164C1C File Offset: 0x00162E1C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06005486 RID: 21638 RVA: 0x00164C32 File Offset: 0x00162E32
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06005487 RID: 21639 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06005488 RID: 21640 RVA: 0x00164C3A File Offset: 0x00162E3A
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06005489 RID: 21641 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x00164C44 File Offset: 0x00162E44
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.ChemistryStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x00164C64 File Offset: 0x00162E64
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
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
				TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
				instance3.onTimeSkip = (Action<int>)Delegate.Combine(instance3.onTimeSkip, new Action<int>(this.TimeSkipped));
				base.ParentProperty.AddConfigurable(this);
				this.stationConfiguration = new ChemistryStationConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
			}
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x00164D17 File Offset: 0x00162F17
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
			if (this.CurrentCookOperation != null)
			{
				this.SetCookOperation(connection, this.CurrentCookOperation);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x00164D44 File Offset: 0x00162F44
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			ChemistryStation.<>c__DisplayClass101_0 CS$<>8__locals1 = new ChemistryStation.<>c__DisplayClass101_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x00164D84 File Offset: 0x00162F84
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
			if (this.CurrentCookOperation != null)
			{
				reason = "Currently cooking";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x00164DC4 File Offset: 0x00162FC4
		public override void DestroyItem(bool callOnServer = true)
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Remove(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x00164E5C File Offset: 0x0016305C
		protected virtual void MinPass()
		{
			this.Alarm.FlashScreen = false;
			if (this.CurrentCookOperation != null)
			{
				this.CurrentCookOperation.Progress(1);
				base.HasChanged = true;
				float t = Mathf.Clamp01((float)this.CurrentCookOperation.CurrentTime / (float)this.CurrentCookOperation.Recipe.CookTime_Mins);
				this.BoilingFlask.LiquidContainer.SetLiquidColor(Color.Lerp(this.CurrentCookOperation.StartLiquidColor, this.CurrentCookOperation.Recipe.FinalLiquidColor, t), true, true);
				if (InstanceFinder.IsServer && this.CurrentCookOperation.CurrentTime >= this.CurrentCookOperation.Recipe.CookTime_Mins)
				{
					this.FinalizeOperation();
				}
			}
			this.UpdateClock();
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x00164F1C File Offset: 0x0016311C
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

		// Token: 0x06005492 RID: 21650 RVA: 0x00164F44 File Offset: 0x00163144
		private void UpdateClock()
		{
			if (this.CurrentCookOperation == null)
			{
				this.Alarm.SetScreenLit(false);
				this.Alarm.DisplayText(string.Empty);
				return;
			}
			int num = this.CurrentCookOperation.Recipe.CookTime_Mins - this.CurrentCookOperation.CurrentTime;
			num = Mathf.Max(0, num);
			this.Alarm.DisplayMinutes(num);
			if (this.CurrentCookOperation.CurrentTime >= this.CurrentCookOperation.Recipe.CookTime_Mins)
			{
				this.Alarm.FlashScreen = true;
				this.Burner.SetDialPosition(0f);
				return;
			}
			this.Alarm.SetScreenLit(true);
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x00164FED File Offset: 0x001631ED
		protected virtual void Update()
		{
			this.StaticFunnel.gameObject.SetActive(!this.LabStand.Funnel.gameObject.activeSelf);
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x00165018 File Offset: 0x00163218
		public Beaker CreateBeaker()
		{
			Beaker component = UnityEngine.Object.Instantiate<GameObject>(this.BeakerPrefab, this.BeakerAlignmentTransform.position, this.BeakerAlignmentTransform.rotation).GetComponent<Beaker>();
			component.Anchor = this.AnchorRb;
			component.transform.SetParent(this.ItemContainer);
			component.Constraint.Container = this.ItemContainer;
			return component;
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x00165079 File Offset: 0x00163279
		public StirringRod CreateStirringRod()
		{
			StirringRod component = UnityEngine.Object.Instantiate<GameObject>(this.StirringRodPrefab.gameObject, this.BeakerAlignmentTransform).GetComponent<StirringRod>();
			component.transform.localPosition = Vector3.zero;
			component.transform.localRotation = Quaternion.identity;
			return component;
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x001650B6 File Offset: 0x001632B6
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCookOperation(ChemistryCookOperation op)
		{
			this.RpcWriter___Server_SendCookOperation_3552222198(op);
			this.RpcLogic___SendCookOperation_3552222198(op);
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x001650CC File Offset: 0x001632CC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetCookOperation(NetworkConnection conn, ChemistryCookOperation operation)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetCookOperation_1024887225(conn, operation);
				this.RpcLogic___SetCookOperation_1024887225(conn, operation);
			}
			else
			{
				this.RpcWriter___Target_SetCookOperation_1024887225(conn, operation);
			}
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x00165110 File Offset: 0x00163310
		[ObserversRpc]
		public void FinalizeOperation()
		{
			this.RpcWriter___Observers_FinalizeOperation_2166136261();
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x00165124 File Offset: 0x00163324
		public void ResetStation()
		{
			this.BoilingFlask.SetRecipe(null);
			this.BoilingFlask.ResetContents();
			this.BoilingFlask.SetTemperature(0f);
			this.BoilingFlask.LockTemperature = false;
			this.Burner.SetDialPosition(0f);
			this.Burner.LockDial = false;
			this.LabStand.SetPosition(1f);
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x00165190 File Offset: 0x00163390
		public bool DoesOutputHaveSpace(StationRecipe recipe)
		{
			StorableItemInstance productInstance = recipe.GetProductInstance(this.GetIngredients());
			return this.OutputSlot.GetCapacityForItem(productInstance, false) >= recipe.Product.Quantity;
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x001651C8 File Offset: 0x001633C8
		public List<ItemInstance> GetIngredients()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			foreach (ItemSlot itemSlot in this.IngredientSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					list.Add(itemSlot.ItemInstance);
				}
			}
			return list;
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x0016520C File Offset: 0x0016340C
		public bool HasIngredientsForRecipe(StationRecipe recipe)
		{
			List<ItemInstance> ingredients = this.GetIngredients();
			return recipe.DoIngredientsSuffice(ingredients);
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x00165228 File Offset: 0x00163428
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

		// Token: 0x0600549E RID: 21662 RVA: 0x00165358 File Offset: 0x00163558
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

		// Token: 0x0600549F RID: 21663 RVA: 0x001653B2 File Offset: 0x001635B2
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x001653CF File Offset: 0x001635CF
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

		// Token: 0x060054A1 RID: 21665 RVA: 0x001653FC File Offset: 0x001635FC
		public void Open()
		{
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition_Default.position, this.CameraPosition_Default.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<ChemistryStationCanvas>.Instance.Open(this);
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x0016549C File Offset: 0x0016369C
		public void Close()
		{
			Singleton<ChemistryStationCanvas>.Instance.Close(true);
			this.LabStand.SetPosition(1f);
			this.SetPlayerUser(null);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x00165526 File Offset: 0x00163726
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x0016553C File Offset: 0x0016373C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x00165552 File Offset: 0x00163752
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x00165578 File Offset: 0x00163778
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

		// Token: 0x060054A7 RID: 21671 RVA: 0x001655D7 File Offset: 0x001637D7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x001655F5 File Offset: 0x001637F5
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x00165613 File Offset: 0x00163813
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x0016564C File Offset: 0x0016384C
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

		// Token: 0x060054AB RID: 21675 RVA: 0x001656CB File Offset: 0x001638CB
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001656F4 File Offset: 0x001638F4
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

		// Token: 0x060054AD RID: 21677 RVA: 0x00165754 File Offset: 0x00163954
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			if (base.ParentProperty == null)
			{
				Console.LogError(base.gameObject.name + " is not a child of a property!", null);
				return null;
			}
			ChemistryStationUIElement component = UnityEngine.Object.Instantiate<ChemistryStationUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<ChemistryStationUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x001657E0 File Offset: 0x001639E0
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001657FC File Offset: 0x001639FC
		public override BuildableItemData GetBaseData()
		{
			string currentRecipeID = string.Empty;
			EQuality productQuality = EQuality.Standard;
			Color startLiquidColor = Color.clear;
			float liquidLevel = 0f;
			int currentTime = 0;
			if (this.CurrentCookOperation != null)
			{
				currentRecipeID = this.CurrentCookOperation.RecipeID;
				productQuality = this.CurrentCookOperation.ProductQuality;
				startLiquidColor = this.CurrentCookOperation.StartLiquidColor;
				liquidLevel = this.CurrentCookOperation.LiquidLevel;
				currentTime = this.CurrentCookOperation.CurrentTime;
			}
			return new ChemistryStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(this.IngredientSlots), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), currentRecipeID, productQuality, startLiquidColor, liquidLevel, currentTime);
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001658B0 File Offset: 0x00163AB0
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x00165928 File Offset: 0x00163B28
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendCookOperation_3552222198));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_SetCookOperation_1024887225));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetCookOperation_1024887225));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_FinalizeOperation_2166136261));
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
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.ChemistryStation));
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x00165B7D File Offset: 0x00163D7D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.ChemistryStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x00165BB7 File Offset: 0x00163DB7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x00165BC8 File Offset: 0x00163DC8
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

		// Token: 0x060054B8 RID: 21688 RVA: 0x00165C6F File Offset: 0x00163E6F
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x00165C78 File Offset: 0x00163E78
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

		// Token: 0x060054BA RID: 21690 RVA: 0x00165CB8 File Offset: 0x00163EB8
		private void RpcWriter___Server_SendCookOperation_3552222198(ChemistryCookOperation op)
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
			writer.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated(op);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x00165D5F File Offset: 0x00163F5F
		public void RpcLogic___SendCookOperation_3552222198(ChemistryCookOperation op)
		{
			this.SetCookOperation(null, op);
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x00165D6C File Offset: 0x00163F6C
		private void RpcReader___Server_SendCookOperation_3552222198(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ChemistryCookOperation op = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCookOperation_3552222198(op);
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x00165DAC File Offset: 0x00163FAC
		private void RpcWriter___Observers_SetCookOperation_1024887225(NetworkConnection conn, ChemistryCookOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated(operation);
			base.SendObserversRpc(10U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x00165E64 File Offset: 0x00164064
		public void RpcLogic___SetCookOperation_1024887225(NetworkConnection conn, ChemistryCookOperation operation)
		{
			this.CurrentCookOperation = operation;
			this.BoilingFlask.LiquidContainer.SetLiquidLevel(operation.LiquidLevel, false);
			this.BoilingFlask.LiquidContainer.LiquidVolume.liquidColor1 = operation.StartLiquidColor;
			this.BoilingFlask.LiquidContainer.LiquidVolume.liquidColor2 = operation.StartLiquidColor;
			this.BoilingFlask.SetTemperature(operation.Recipe.CookTemperature);
			this.BoilingFlask.LockTemperature = true;
			this.Burner.SetDialPosition(this.CurrentCookOperation.Recipe.CookTemperature / 500f);
			this.Burner.LockDial = true;
			base.HasChanged = true;
			this.UpdateClock();
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x00165F24 File Offset: 0x00164124
		private void RpcReader___Observers_SetCookOperation_1024887225(PooledReader PooledReader0, Channel channel)
		{
			ChemistryCookOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_1024887225(null, operation);
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x00165F60 File Offset: 0x00164160
		private void RpcWriter___Target_SetCookOperation_1024887225(NetworkConnection conn, ChemistryCookOperation operation)
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
			writer.Write___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generated(operation);
			base.SendTargetRpc(11U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x00166018 File Offset: 0x00164218
		private void RpcReader___Target_SetCookOperation_1024887225(PooledReader PooledReader0, Channel channel)
		{
			ChemistryCookOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.ChemistryCookOperationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetCookOperation_1024887225(base.LocalConnection, operation);
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x00166050 File Offset: 0x00164250
		private void RpcWriter___Observers_FinalizeOperation_2166136261()
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

		// Token: 0x060054C3 RID: 21699 RVA: 0x001660FC File Offset: 0x001642FC
		public void RpcLogic___FinalizeOperation_2166136261()
		{
			if (this.CurrentCookOperation == null)
			{
				Console.LogWarning("No cook operation to finalize", null);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				StorableItemInstance productInstance = this.CurrentCookOperation.Recipe.GetProductInstance(this.CurrentCookOperation.ProductQuality);
				this.OutputSlot.AddItem(productInstance, false);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Chemical_Operations_Completed", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Chemical_Operations_Completed") + 1f).ToString(), true);
			}
			this.CurrentCookOperation = null;
			this.ResetStation();
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x00166188 File Offset: 0x00164388
		private void RpcReader___Observers_FinalizeOperation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___FinalizeOperation_2166136261();
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x001661A8 File Offset: 0x001643A8
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

		// Token: 0x060054C6 RID: 21702 RVA: 0x0016624F File Offset: 0x0016444F
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x00166258 File Offset: 0x00164458
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

		// Token: 0x060054C8 RID: 21704 RVA: 0x00166298 File Offset: 0x00164498
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

		// Token: 0x060054C9 RID: 21705 RVA: 0x0016633F File Offset: 0x0016453F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x00166348 File Offset: 0x00164548
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

		// Token: 0x060054CB RID: 21707 RVA: 0x00166388 File Offset: 0x00164588
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

		// Token: 0x060054CC RID: 21708 RVA: 0x0016644E File Offset: 0x0016464E
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x00166478 File Offset: 0x00164678
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

		// Token: 0x060054CE RID: 21710 RVA: 0x001664E0 File Offset: 0x001646E0
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

		// Token: 0x060054CF RID: 21711 RVA: 0x001665A8 File Offset: 0x001647A8
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x001665D4 File Offset: 0x001647D4
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

		// Token: 0x060054D1 RID: 21713 RVA: 0x00166628 File Offset: 0x00164828
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

		// Token: 0x060054D2 RID: 21714 RVA: 0x001666F0 File Offset: 0x001648F0
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

		// Token: 0x060054D3 RID: 21715 RVA: 0x00166748 File Offset: 0x00164948
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

		// Token: 0x060054D4 RID: 21716 RVA: 0x00166806 File Offset: 0x00164A06
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x00166810 File Offset: 0x00164A10
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

		// Token: 0x060054D6 RID: 21718 RVA: 0x0016686C File Offset: 0x00164A6C
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

		// Token: 0x060054D7 RID: 21719 RVA: 0x00166939 File Offset: 0x00164B39
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x00166950 File Offset: 0x00164B50
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

		// Token: 0x060054D9 RID: 21721 RVA: 0x001669A8 File Offset: 0x00164BA8
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

		// Token: 0x060054DA RID: 21722 RVA: 0x00166A88 File Offset: 0x00164C88
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x00166AB8 File Offset: 0x00164CB8
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

		// Token: 0x060054DC RID: 21724 RVA: 0x00166B40 File Offset: 0x00164D40
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

		// Token: 0x060054DD RID: 21725 RVA: 0x00166C21 File Offset: 0x00164E21
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x060054DE RID: 21726 RVA: 0x00166C50 File Offset: 0x00164E50
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

		// Token: 0x060054DF RID: 21727 RVA: 0x00166CCC File Offset: 0x00164ECC
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

		// Token: 0x060054E0 RID: 21728 RVA: 0x00166DB0 File Offset: 0x00164FB0
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

		// Token: 0x060054E1 RID: 21729 RVA: 0x00166E24 File Offset: 0x00165024
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

		// Token: 0x060054E2 RID: 21730 RVA: 0x00166EEA File Offset: 0x001650EA
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x00166F14 File Offset: 0x00165114
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

		// Token: 0x060054E4 RID: 21732 RVA: 0x00166F7C File Offset: 0x0016517C
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

		// Token: 0x060054E5 RID: 21733 RVA: 0x00167044 File Offset: 0x00165244
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x0016705C File Offset: 0x0016525C
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

		// Token: 0x060054E7 RID: 21735 RVA: 0x001670B0 File Offset: 0x001652B0
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

		// Token: 0x060054E8 RID: 21736 RVA: 0x00167178 File Offset: 0x00165378
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

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x001671CF File Offset: 0x001653CF
		// (set) Token: 0x060054EA RID: 21738 RVA: 0x001671D7 File Offset: 0x001653D7
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

		// Token: 0x060054EB RID: 21739 RVA: 0x00167214 File Offset: 0x00165414
		public override bool ChemistryStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060054EC RID: 21740 RVA: 0x001672EE File Offset: 0x001654EE
		// (set) Token: 0x060054ED RID: 21741 RVA: 0x001672F6 File Offset: 0x001654F6
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

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060054EE RID: 21742 RVA: 0x00167332 File Offset: 0x00165532
		// (set) Token: 0x060054EF RID: 21743 RVA: 0x0016733A File Offset: 0x0016553A
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

		// Token: 0x060054F0 RID: 21744 RVA: 0x00167378 File Offset: 0x00165578
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.IngredientSlots = new ItemSlot[3];
				for (int i = 0; i < 3; i++)
				{
					this.IngredientSlots[i] = new ItemSlot(true);
					this.IngredientSlots[i].SetSlotOwner(this);
					this.InputVisuals.AddSlot(this.IngredientSlots[i], false);
					ItemSlot itemSlot = this.IngredientSlots[i];
					itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(delegate()
					{
						base.HasChanged = true;
					}));
				}
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				ItemSlot outputSlot = this.OutputSlot;
				outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.InputSlots.AddRange(this.IngredientSlots);
				this.OutputSlots.Add(this.OutputSlot);
				new ItemSlotSiblingSet(this.InputSlots);
			}
		}

		// Token: 0x04003EE0 RID: 16096
		public const float FOV_OVERRIDE = 65f;

		// Token: 0x04003EE1 RID: 16097
		public const int INPUT_SLOT_COUNT = 3;

		// Token: 0x04003EE6 RID: 16102
		public ItemSlot[] IngredientSlots;

		// Token: 0x04003EE7 RID: 16103
		public ItemSlot OutputSlot;

		// Token: 0x04003EE8 RID: 16104
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04003EE9 RID: 16105
		public Transform CameraPosition_Default;

		// Token: 0x04003EEA RID: 16106
		public Transform CameraPosition_Stirring;

		// Token: 0x04003EEB RID: 16107
		public Transform StaticBeaker;

		// Token: 0x04003EEC RID: 16108
		public Transform StaticFunnel;

		// Token: 0x04003EED RID: 16109
		public Transform StaticStirringRod;

		// Token: 0x04003EEE RID: 16110
		public Transform ItemContainer;

		// Token: 0x04003EEF RID: 16111
		public LabStand LabStand;

		// Token: 0x04003EF0 RID: 16112
		public StorageVisualizer InputVisuals;

		// Token: 0x04003EF1 RID: 16113
		public StorageVisualizer OutputVisuals;

		// Token: 0x04003EF2 RID: 16114
		public Rigidbody AnchorRb;

		// Token: 0x04003EF3 RID: 16115
		public BunsenBurner Burner;

		// Token: 0x04003EF4 RID: 16116
		public BoilingFlask BoilingFlask;

		// Token: 0x04003EF5 RID: 16117
		public DigitalAlarm Alarm;

		// Token: 0x04003EF6 RID: 16118
		public Transform uiPoint;

		// Token: 0x04003EF7 RID: 16119
		public Transform[] accessPoints;

		// Token: 0x04003EF8 RID: 16120
		public ConfigurationReplicator configReplicator;

		// Token: 0x04003EF9 RID: 16121
		public BoxCollider TrashSpawnVolume;

		// Token: 0x04003EFA RID: 16122
		public Transform ExplosionPoint;

		// Token: 0x04003EFB RID: 16123
		[Header("Slot Display Points")]
		public Transform InputSlotsPosition;

		// Token: 0x04003EFC RID: 16124
		public Transform OutputSlotPosition;

		// Token: 0x04003EFD RID: 16125
		[Header("Transforms")]
		public Transform[] IngredientTransforms;

		// Token: 0x04003EFE RID: 16126
		public Transform BeakerAlignmentTransform;

		// Token: 0x04003EFF RID: 16127
		[Header("Prefabs")]
		public GameObject BeakerPrefab;

		// Token: 0x04003F00 RID: 16128
		public StirringRod StirringRodPrefab;

		// Token: 0x04003F01 RID: 16129
		[Header("UI")]
		public ChemistryStationUIElement WorldspaceUIPrefab;

		// Token: 0x04003F02 RID: 16130
		public Sprite typeIcon;

		// Token: 0x04003F0A RID: 16138
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003F0B RID: 16139
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003F0C RID: 16140
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003F0D RID: 16141
		private bool dll_Excuted;

		// Token: 0x04003F0E RID: 16142
		private bool dll_Excuted;

		// Token: 0x02000C20 RID: 3104
		public enum EStep
		{
			// Token: 0x04003F10 RID: 16144
			CombineIngredients,
			// Token: 0x04003F11 RID: 16145
			Stir,
			// Token: 0x04003F12 RID: 16146
			LowerBoilingFlask,
			// Token: 0x04003F13 RID: 16147
			PourIntoBoilingFlask,
			// Token: 0x04003F14 RID: 16148
			RaiseBoilingFlask,
			// Token: 0x04003F15 RID: 16149
			StartHeat,
			// Token: 0x04003F16 RID: 16150
			Cook,
			// Token: 0x04003F17 RID: 16151
			LowerBoilingFlaskAgain,
			// Token: 0x04003F18 RID: 16152
			PourThroughFilter
		}
	}
}
