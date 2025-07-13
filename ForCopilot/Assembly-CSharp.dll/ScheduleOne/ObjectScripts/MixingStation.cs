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
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C37 RID: 3127
	public class MixingStation : GridItem, IUsable, IItemSlotOwner, ITransitEntity, IConfigurable
	{
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600568F RID: 22159 RVA: 0x0016DF96 File Offset: 0x0016C196
		// (set) Token: 0x06005690 RID: 22160 RVA: 0x0016DF9E File Offset: 0x0016C19E
		public bool IsOpen { get; private set; }

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06005691 RID: 22161 RVA: 0x0016DFA7 File Offset: 0x0016C1A7
		// (set) Token: 0x06005692 RID: 22162 RVA: 0x0016DFAF File Offset: 0x0016C1AF
		public MixOperation CurrentMixOperation { get; set; }

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06005693 RID: 22163 RVA: 0x0016DFB8 File Offset: 0x0016C1B8
		public bool IsMixingDone
		{
			get
			{
				return this.CurrentMixOperation != null && this.CurrentMixTime >= this.GetMixTimeForCurrentOperation();
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06005694 RID: 22164 RVA: 0x0016DFD5 File Offset: 0x0016C1D5
		// (set) Token: 0x06005695 RID: 22165 RVA: 0x0016DFDD File Offset: 0x0016C1DD
		public int CurrentMixTime { get; protected set; }

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06005696 RID: 22166 RVA: 0x0016DFE6 File Offset: 0x0016C1E6
		// (set) Token: 0x06005697 RID: 22167 RVA: 0x0016DFEE File Offset: 0x0016C1EE
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06005698 RID: 22168 RVA: 0x0016DFF7 File Offset: 0x0016C1F7
		// (set) Token: 0x06005699 RID: 22169 RVA: 0x0016DFFF File Offset: 0x0016C1FF
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

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600569A RID: 22170 RVA: 0x0016E009 File Offset: 0x0016C209
		// (set) Token: 0x0600569B RID: 22171 RVA: 0x0016E011 File Offset: 0x0016C211
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

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600569C RID: 22172 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x0600569D RID: 22173 RVA: 0x0016E01B File Offset: 0x0016C21B
		// (set) Token: 0x0600569E RID: 22174 RVA: 0x0016E023 File Offset: 0x0016C223
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600569F RID: 22175 RVA: 0x0016E02C File Offset: 0x0016C22C
		// (set) Token: 0x060056A0 RID: 22176 RVA: 0x0016E034 File Offset: 0x0016C234
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x0016E03D File Offset: 0x0016C23D
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x060056A2 RID: 22178 RVA: 0x0016E045 File Offset: 0x0016C245
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x0016E04D File Offset: 0x0016C24D
		public bool Selectable { get; } = 1;

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x060056A4 RID: 22180 RVA: 0x0016E055 File Offset: 0x0016C255
		// (set) Token: 0x060056A5 RID: 22181 RVA: 0x0016E05D File Offset: 0x0016C25D
		public bool IsAcceptingItems { get; set; } = true;

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x060056A6 RID: 22182 RVA: 0x0016E066 File Offset: 0x0016C266
		public EntityConfiguration Configuration
		{
			get
			{
				return this.stationConfiguration;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x0016E06E File Offset: 0x0016C26E
		// (set) Token: 0x060056A8 RID: 22184 RVA: 0x0016E076 File Offset: 0x0016C276
		protected MixingStationConfiguration stationConfiguration { get; set; }

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x0016E07F File Offset: 0x0016C27F
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x060056AA RID: 22186 RVA: 0x0016E087 File Offset: 0x0016C287
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.MixingStation;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0016E08B File Offset: 0x0016C28B
		// (set) Token: 0x060056AC RID: 22188 RVA: 0x0016E093 File Offset: 0x0016C293
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x060056AD RID: 22189 RVA: 0x0016E09C File Offset: 0x0016C29C
		// (set) Token: 0x060056AE RID: 22190 RVA: 0x0016E0A4 File Offset: 0x0016C2A4
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

		// Token: 0x060056AF RID: 22191 RVA: 0x0016E0AE File Offset: 0x0016C2AE
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x060056B0 RID: 22192 RVA: 0x0016E0C4 File Offset: 0x0016C2C4
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x060056B1 RID: 22193 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x060056B2 RID: 22194 RVA: 0x0016E03D File Offset: 0x0016C23D
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x060056B3 RID: 22195 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x060056B4 RID: 22196 RVA: 0x0016E0CC File Offset: 0x0016C2CC
		// (set) Token: 0x060056B5 RID: 22197 RVA: 0x0016E0D4 File Offset: 0x0016C2D4
		public Vector3 DiscoveryBoxOffset { get; private set; }

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x060056B6 RID: 22198 RVA: 0x0016E0DD File Offset: 0x0016C2DD
		// (set) Token: 0x060056B7 RID: 22199 RVA: 0x0016E0E5 File Offset: 0x0016C2E5
		public Quaternion DiscoveryBoxRotation { get; private set; }

		// Token: 0x060056B8 RID: 22200 RVA: 0x0016E0F0 File Offset: 0x0016C2F0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.MixingStation_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0016E110 File Offset: 0x0016C310
		protected override void Start()
		{
			base.Start();
			if (!this.isGhost)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onTimeSkip = (Action<int>)Delegate.Combine(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
				if (this.StartButton != null)
				{
					this.StartButton.onClickStart.AddListener(new UnityAction<RaycastHit>(this.StartButtonClicked));
				}
			}
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x0016E1A4 File Offset: 0x0016C3A4
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
				this.stationConfiguration = new MixingStationConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
			}
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x0016E1F5 File Offset: 0x0016C3F5
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
			if (this.CurrentMixOperation != null)
			{
				this.SetMixOperation(connection, this.CurrentMixOperation, this.CurrentMixTime);
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x0016E228 File Offset: 0x0016C428
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			MixingStation.<>c__DisplayClass121_0 CS$<>8__locals1 = new MixingStation.<>c__DisplayClass121_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x0016E268 File Offset: 0x0016C468
		public override bool CanBeDestroyed(out string reason)
		{
			if (((IItemSlotOwner)this).GetTotalItemCount() > 0)
			{
				reason = "Contains items";
				return false;
			}
			if (this.CurrentMixOperation != null && this.IsMixingDone)
			{
				reason = "Contains items";
				return false;
			}
			if (this.CurrentMixOperation != null)
			{
				reason = "Mixing in progress";
				return false;
			}
			if (((IUsable)this).IsInUse)
			{
				reason = "Currently in use";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x0016E2CC File Offset: 0x0016C4CC
		public override void DestroyItem(bool callOnServer = true)
		{
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

		// Token: 0x060056BF RID: 22207 RVA: 0x0016E354 File Offset: 0x0016C554
		private void TimeSkipped(int minsPassed)
		{
			for (int i = 0; i < minsPassed; i++)
			{
				this.MinPass();
			}
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x0016E374 File Offset: 0x0016C574
		protected virtual void MinPass()
		{
			if (this.CurrentMixOperation != null || this.OutputSlot.Quantity > 0)
			{
				int num = 0;
				if (this.CurrentMixOperation != null)
				{
					int currentMixTime = this.CurrentMixTime;
					int currentMixTime2 = this.CurrentMixTime;
					this.CurrentMixTime = currentMixTime2 + 1;
					num = this.GetMixTimeForCurrentOperation();
					if (this.CurrentMixTime >= num && currentMixTime < num && InstanceFinder.IsServer)
					{
						NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Mixing_Operations_Completed", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Mixing_Operations_Completed") + 1f).ToString(), true);
						this.MixingDone_Networked();
					}
				}
				if (this.Clock != null)
				{
					this.Clock.SetScreenLit(true);
					this.Clock.DisplayMinutes(Mathf.Max(num - this.CurrentMixTime, 0));
				}
				if (this.Light != null)
				{
					if (this.IsMixingDone)
					{
						this.Light.isOn = (NetworkSingleton<TimeManager>.Instance.DailyMinTotal % 2 == 0);
						return;
					}
					this.Light.isOn = true;
					return;
				}
			}
			else
			{
				if (this.Clock != null)
				{
					this.Clock.SetScreenLit(false);
					this.Clock.DisplayText(string.Empty);
				}
				if (this.Light != null && this.IsMixingDone)
				{
					this.Light.isOn = false;
				}
			}
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x0016E4C6 File Offset: 0x0016C6C6
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMixingOperation(MixOperation operation, int mixTime)
		{
			this.RpcWriter___Server_SendMixingOperation_2669582547(operation, mixTime);
			this.RpcLogic___SendMixingOperation_2669582547(operation, mixTime);
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x0016E4E4 File Offset: 0x0016C6E4
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void SetMixOperation(NetworkConnection conn, MixOperation operation, int mixTIme)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetMixOperation_1073078804(conn, operation, mixTIme);
				this.RpcLogic___SetMixOperation_1073078804(conn, operation, mixTIme);
			}
			else
			{
				this.RpcWriter___Target_SetMixOperation_1073078804(conn, operation, mixTIme);
			}
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x0016E531 File Offset: 0x0016C731
		public virtual void MixingStart()
		{
			this.StartSound.Play();
			this.MachineSound.StartAudio();
			if (this.onMixStart != null)
			{
				this.onMixStart.Invoke();
			}
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x0016E55C File Offset: 0x0016C75C
		[ObserversRpc]
		public void MixingDone_Networked()
		{
			this.RpcWriter___Observers_MixingDone_Networked_2166136261();
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x0016E564 File Offset: 0x0016C764
		public virtual void MixingDone()
		{
			this.MachineSound.StopAudio();
			this.StopSound.Play();
			this.TryCreateOutputItems();
			if (this.onMixDone != null)
			{
				this.onMixDone.Invoke();
			}
		}

		// Token: 0x060056C6 RID: 22214 RVA: 0x0016E598 File Offset: 0x0016C798
		public bool DoesOutputHaveSpace(StationRecipe recipe)
		{
			StorableItemInstance productInstance = recipe.GetProductInstance(this.GetIngredients());
			return this.OutputSlot.GetCapacityForItem(productInstance, false) >= 1;
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x0016E5C8 File Offset: 0x0016C7C8
		public List<ItemInstance> GetIngredients()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			if (this.ProductSlot.ItemInstance != null)
			{
				list.Add(this.ProductSlot.ItemInstance);
			}
			if (this.MixerSlot.ItemInstance != null)
			{
				list.Add(this.MixerSlot.ItemInstance);
			}
			return list;
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x0016E618 File Offset: 0x0016C818
		public int GetMixQuantity()
		{
			if (this.GetProduct() == null || this.GetMixer() == null)
			{
				return 0;
			}
			return Mathf.Min(Mathf.Min(this.ProductSlot.Quantity, this.MixerSlot.Quantity), this.MaxMixQuantity);
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x0016E669 File Offset: 0x0016C869
		public bool CanStartMix()
		{
			return this.GetMixQuantity() > 0 && this.OutputSlot.Quantity == 0;
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x0016E684 File Offset: 0x0016C884
		public ProductDefinition GetProduct()
		{
			if (this.ProductSlot.ItemInstance != null)
			{
				return this.ProductSlot.ItemInstance.Definition as ProductDefinition;
			}
			return null;
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x0016E6AC File Offset: 0x0016C8AC
		public PropertyItemDefinition GetMixer()
		{
			if (this.MixerSlot.ItemInstance != null)
			{
				PropertyItemDefinition propertyItemDefinition = this.MixerSlot.ItemInstance.Definition as PropertyItemDefinition;
				if (propertyItemDefinition != null && NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(propertyItemDefinition))
				{
					return propertyItemDefinition;
				}
			}
			return null;
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x0016E6FA File Offset: 0x0016C8FA
		public int GetMixTimeForCurrentOperation()
		{
			if (this.CurrentMixOperation == null)
			{
				return 0;
			}
			return this.MixTimePerItem * this.CurrentMixOperation.Quantity;
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x0016E718 File Offset: 0x0016C918
		[ServerRpc(RequireOwnership = false)]
		public void TryCreateOutputItems()
		{
			this.RpcWriter___Server_TryCreateOutputItems_2166136261();
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x0016E72B File Offset: 0x0016C92B
		public void SetStartButtonClickable(bool clickable)
		{
			this.StartButton.ClickableEnabled = clickable;
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x0016E73C File Offset: 0x0016C93C
		private void OutputChanged()
		{
			if (this.OutputSlot.Quantity == 0)
			{
				if (this.onOutputCollected != null)
				{
					this.onOutputCollected.Invoke();
				}
				if (InstanceFinder.IsServer)
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Mixing_Operations_Collected", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Mixing_Operations_Collected") + 1f).ToString(), true);
				}
			}
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x0016E79D File Offset: 0x0016C99D
		private void StartButtonClicked(RaycastHit hit)
		{
			this.SetStartButtonClickable(false);
			if (this.onStartButtonClicked != null)
			{
				this.onStartButtonClicked.Invoke();
			}
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x0016E7BC File Offset: 0x0016C9BC
		public void Open()
		{
			this.IsOpen = true;
			if (this.CurrentMixOperation != null && this.IsMixingDone)
			{
				this.TryCreateOutputItems();
			}
			this.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<MixingStationCanvas>.Instance.Open(this);
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x0016E884 File Offset: 0x0016CA84
		public void Close()
		{
			this.IsOpen = false;
			this.SetPlayerUser(null);
			if (this.DiscoveryBox != null)
			{
				this.DiscoveryBox.transform.SetParent(this.CameraPosition.transform);
				this.DiscoveryBox.gameObject.SetActive(false);
			}
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				Singleton<CompassManager>.Instance.SetVisible(true);
			}
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x0016E93C File Offset: 0x0016CB3C
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

		// Token: 0x060056D4 RID: 22228 RVA: 0x0016E996 File Offset: 0x0016CB96
		public void Interacted()
		{
			if (((IUsable)this).IsInUse || Singleton<ManagementClipboard>.Instance.IsEquipped)
			{
				return;
			}
			this.Open();
		}

		// Token: 0x060056D5 RID: 22229 RVA: 0x0016E9B4 File Offset: 0x0016CBB4
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
			MixingStationUIElement component = UnityEngine.Object.Instantiate<MixingStationUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<MixingStationUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x0016EA47 File Offset: 0x0016CC47
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x0016EA62 File Offset: 0x0016CC62
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x060056D8 RID: 22232 RVA: 0x0016EA88 File Offset: 0x0016CC88
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

		// Token: 0x060056D9 RID: 22233 RVA: 0x0016EAE7 File Offset: 0x0016CCE7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x0016EB05 File Offset: 0x0016CD05
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x0016EB23 File Offset: 0x0016CD23
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x0016EB5C File Offset: 0x0016CD5C
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

		// Token: 0x060056DD RID: 22237 RVA: 0x0016EBDB File Offset: 0x0016CDDB
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x060056DE RID: 22238 RVA: 0x0016EC04 File Offset: 0x0016CE04
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

		// Token: 0x060056DF RID: 22239 RVA: 0x0016EC64 File Offset: 0x0016CE64
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x0016EC85 File Offset: 0x0016CE85
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x0016EC9C File Offset: 0x0016CE9C
		public override BuildableItemData GetBaseData()
		{
			return new MixingStationData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, new ItemSet(new List<ItemSlot>
			{
				this.ProductSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.MixerSlot
			}), new ItemSet(new List<ItemSlot>
			{
				this.OutputSlot
			}), this.CurrentMixOperation, this.CurrentMixTime);
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x0016ED1C File Offset: 0x0016CF1C
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x060056E7 RID: 22247 RVA: 0x0016EDB0 File Offset: 0x0016CFB0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 0U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendMixingOperation_2669582547));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_SetMixOperation_1073078804));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetMixOperation_1073078804));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_MixingDone_Networked_2166136261));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_TryCreateOutputItems_2166136261));
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
			base.RegisterServerRpc(25U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(26U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.MixingStation));
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x0016F01C File Offset: 0x0016D21C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x0016F056 File Offset: 0x0016D256
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x0016F064 File Offset: 0x0016D264
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

		// Token: 0x060056EB RID: 22251 RVA: 0x0016F10B File Offset: 0x0016D30B
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x0016F114 File Offset: 0x0016D314
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

		// Token: 0x060056ED RID: 22253 RVA: 0x0016F154 File Offset: 0x0016D354
		private void RpcWriter___Server_SendMixingOperation_2669582547(MixOperation operation, int mixTime)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			writer.WriteInt32(mixTime, 1);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x0016F20D File Offset: 0x0016D40D
		public void RpcLogic___SendMixingOperation_2669582547(MixOperation operation, int mixTime)
		{
			this.SetMixOperation(null, operation, mixTime);
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x0016F218 File Offset: 0x0016D418
		private void RpcReader___Server_SendMixingOperation_2669582547(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			MixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			int mixTime = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMixingOperation_2669582547(operation, mixTime);
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x0016F26C File Offset: 0x0016D46C
		private void RpcWriter___Observers_SetMixOperation_1073078804(NetworkConnection conn, MixOperation operation, int mixTIme)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			writer.WriteInt32(mixTIme, 1);
			base.SendObserversRpc(10U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x0016F334 File Offset: 0x0016D534
		public virtual void RpcLogic___SetMixOperation_1073078804(NetworkConnection conn, MixOperation operation, int mixTIme)
		{
			if (operation != null && string.IsNullOrEmpty(operation.ProductID))
			{
				operation = null;
			}
			MixOperation currentMixOperation = this.CurrentMixOperation;
			this.CurrentMixOperation = operation;
			this.CurrentMixTime = mixTIme;
			if (operation != null)
			{
				if (currentMixOperation == null)
				{
					this.MixingStart();
					return;
				}
			}
			else if (currentMixOperation != null && this.onMixDone != null)
			{
				this.onMixDone.Invoke();
			}
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x0016F38C File Offset: 0x0016D58C
		private void RpcReader___Observers_SetMixOperation_1073078804(PooledReader PooledReader0, Channel channel)
		{
			MixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			int mixTIme = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_1073078804(null, operation, mixTIme);
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x0016F3E0 File Offset: 0x0016D5E0
		private void RpcWriter___Target_SetMixOperation_1073078804(NetworkConnection conn, MixOperation operation, int mixTIme)
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
			writer.Write___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generated(operation);
			writer.WriteInt32(mixTIme, 1);
			base.SendTargetRpc(11U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x0016F4A8 File Offset: 0x0016D6A8
		private void RpcReader___Target_SetMixOperation_1073078804(PooledReader PooledReader0, Channel channel)
		{
			MixOperation operation = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.MixOperationFishNet.Serializing.Generateds(PooledReader0);
			int mixTIme = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_1073078804(base.LocalConnection, operation, mixTIme);
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x0016F4F8 File Offset: 0x0016D6F8
		private void RpcWriter___Observers_MixingDone_Networked_2166136261()
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

		// Token: 0x060056F6 RID: 22262 RVA: 0x0016F5A1 File Offset: 0x0016D7A1
		public void RpcLogic___MixingDone_Networked_2166136261()
		{
			this.MixingDone();
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x0016F5AC File Offset: 0x0016D7AC
		private void RpcReader___Observers_MixingDone_Networked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___MixingDone_Networked_2166136261();
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x0016F5CC File Offset: 0x0016D7CC
		private void RpcWriter___Server_TryCreateOutputItems_2166136261()
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
			base.SendServerRpc(13U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x0016F668 File Offset: 0x0016D868
		public void RpcLogic___TryCreateOutputItems_2166136261()
		{
			if (this.CurrentMixOperation == null)
			{
				return;
			}
			ProductDefinition productDefinition;
			if (this.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				QualityItemInstance qualityItemInstance = productDefinition.GetDefaultInstance(this.CurrentMixOperation.Quantity) as QualityItemInstance;
				qualityItemInstance.SetQuality(this.CurrentMixOperation.ProductQuality);
				this.OutputSlot.AddItem(qualityItemInstance, false);
				if (NetworkSingleton<ProductManager>.Instance.GetRecipe(this.CurrentMixOperation.ProductID, this.CurrentMixOperation.IngredientID) == null)
				{
					NetworkSingleton<ProductManager>.Instance.SendMixRecipe(this.CurrentMixOperation.ProductID, this.CurrentMixOperation.IngredientID, qualityItemInstance.ID);
				}
				this.SetMixOperation(null, null, 0);
			}
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x0016F71C File Offset: 0x0016D91C
		private void RpcReader___Server_TryCreateOutputItems_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___TryCreateOutputItems_2166136261();
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x0016F73C File Offset: 0x0016D93C
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

		// Token: 0x060056FC RID: 22268 RVA: 0x0016F802 File Offset: 0x0016DA02
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x0016F82C File Offset: 0x0016DA2C
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

		// Token: 0x060056FE RID: 22270 RVA: 0x0016F894 File Offset: 0x0016DA94
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

		// Token: 0x060056FF RID: 22271 RVA: 0x0016F95C File Offset: 0x0016DB5C
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x0016F988 File Offset: 0x0016DB88
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

		// Token: 0x06005701 RID: 22273 RVA: 0x0016F9DC File Offset: 0x0016DBDC
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

		// Token: 0x06005702 RID: 22274 RVA: 0x0016FAA4 File Offset: 0x0016DCA4
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

		// Token: 0x06005703 RID: 22275 RVA: 0x0016FAFC File Offset: 0x0016DCFC
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

		// Token: 0x06005704 RID: 22276 RVA: 0x0016FBBA File Offset: 0x0016DDBA
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x0016FBC4 File Offset: 0x0016DDC4
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

		// Token: 0x06005706 RID: 22278 RVA: 0x0016FC20 File Offset: 0x0016DE20
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

		// Token: 0x06005707 RID: 22279 RVA: 0x0016FCED File Offset: 0x0016DEED
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x0016FD04 File Offset: 0x0016DF04
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

		// Token: 0x06005709 RID: 22281 RVA: 0x0016FD5C File Offset: 0x0016DF5C
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

		// Token: 0x0600570A RID: 22282 RVA: 0x0016FE3C File Offset: 0x0016E03C
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x0016FE6C File Offset: 0x0016E06C
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

		// Token: 0x0600570C RID: 22284 RVA: 0x0016FEF4 File Offset: 0x0016E0F4
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

		// Token: 0x0600570D RID: 22285 RVA: 0x0016FFD5 File Offset: 0x0016E1D5
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x00170004 File Offset: 0x0016E204
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

		// Token: 0x0600570F RID: 22287 RVA: 0x00170080 File Offset: 0x0016E280
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

		// Token: 0x06005710 RID: 22288 RVA: 0x00170164 File Offset: 0x0016E364
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

		// Token: 0x06005711 RID: 22289 RVA: 0x001701D8 File Offset: 0x0016E3D8
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

		// Token: 0x06005712 RID: 22290 RVA: 0x0017029E File Offset: 0x0016E49E
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x001702C8 File Offset: 0x0016E4C8
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

		// Token: 0x06005714 RID: 22292 RVA: 0x00170330 File Offset: 0x0016E530
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

		// Token: 0x06005715 RID: 22293 RVA: 0x001703F8 File Offset: 0x0016E5F8
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x00170410 File Offset: 0x0016E610
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

		// Token: 0x06005717 RID: 22295 RVA: 0x00170464 File Offset: 0x0016E664
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

		// Token: 0x06005718 RID: 22296 RVA: 0x0017052C File Offset: 0x0016E72C
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

		// Token: 0x06005719 RID: 22297 RVA: 0x00170584 File Offset: 0x0016E784
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
			base.SendServerRpc(25U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x0017062C File Offset: 0x0016E82C
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x00170680 File Offset: 0x0016E880
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

		// Token: 0x0600571C RID: 22300 RVA: 0x001706C0 File Offset: 0x0016E8C0
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
			base.SendServerRpc(26U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x00170767 File Offset: 0x0016E967
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x00170770 File Offset: 0x0016E970
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

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x001707AE File Offset: 0x0016E9AE
		// (set) Token: 0x06005720 RID: 22304 RVA: 0x001707B6 File Offset: 0x0016E9B6
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

		// Token: 0x06005721 RID: 22305 RVA: 0x001707F4 File Offset: 0x0016E9F4
		public override bool MixingStation(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06005722 RID: 22306 RVA: 0x001708CE File Offset: 0x0016EACE
		// (set) Token: 0x06005723 RID: 22307 RVA: 0x001708D6 File Offset: 0x0016EAD6
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

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06005724 RID: 22308 RVA: 0x00170912 File Offset: 0x0016EB12
		// (set) Token: 0x06005725 RID: 22309 RVA: 0x0017091A File Offset: 0x0016EB1A
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

		// Token: 0x06005726 RID: 22310 RVA: 0x00170958 File Offset: 0x0016EB58
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this.ProductSlot = new ItemSlot(true);
				this.ProductSlot.AddFilter(new ItemFilter_UnpackagedProduct());
				this.ProductSlot.SetSlotOwner(this);
				this.InputVisuals.AddSlot(this.ProductSlot, false);
				ItemSlot productSlot = this.ProductSlot;
				productSlot.onItemDataChanged = (Action)Delegate.Combine(productSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.MixerSlot = new ItemSlot(true);
				this.MixerSlot.AddFilter(new ItemFilter_MixingIngredient());
				this.MixerSlot.SetSlotOwner(this);
				this.InputVisuals.AddSlot(this.MixerSlot, false);
				ItemSlot mixerSlot = this.MixerSlot;
				mixerSlot.onItemDataChanged = (Action)Delegate.Combine(mixerSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				this.OutputSlot.SetIsAddLocked(true);
				this.OutputSlot.SetSlotOwner(this);
				this.OutputVisuals.AddSlot(this.OutputSlot, false);
				ItemSlot outputSlot = this.OutputSlot;
				outputSlot.onItemDataChanged = (Action)Delegate.Combine(outputSlot.onItemDataChanged, new Action(delegate()
				{
					base.HasChanged = true;
				}));
				ItemSlot outputSlot2 = this.OutputSlot;
				outputSlot2.onItemDataChanged = (Action)Delegate.Combine(outputSlot2.onItemDataChanged, new Action(this.OutputChanged));
				this.DiscoveryBoxOffset = this.DiscoveryBox.transform.localPosition;
				this.DiscoveryBoxRotation = this.DiscoveryBox.transform.localRotation;
				this.InputSlots.AddRange(new List<ItemSlot>
				{
					this.ProductSlot,
					this.MixerSlot
				});
				this.OutputSlots.Add(this.OutputSlot);
				new ItemSlotSiblingSet(this.InputSlots);
			}
		}

		// Token: 0x0400400D RID: 16397
		public ItemSlot ProductSlot;

		// Token: 0x0400400E RID: 16398
		public ItemSlot MixerSlot;

		// Token: 0x0400400F RID: 16399
		public ItemSlot OutputSlot;

		// Token: 0x04004013 RID: 16403
		public bool RequiresIngredientInsertion = true;

		// Token: 0x0400401B RID: 16411
		[Header("Settings")]
		public int MixTimePerItem = 15;

		// Token: 0x0400401C RID: 16412
		public int MaxMixQuantity = 10;

		// Token: 0x0400401D RID: 16413
		[Header("Prefabs")]
		public GameObject JugPrefab;

		// Token: 0x0400401E RID: 16414
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x0400401F RID: 16415
		public Transform CameraPosition;

		// Token: 0x04004020 RID: 16416
		public Transform CameraPosition_CombineIngredients;

		// Token: 0x04004021 RID: 16417
		public Transform CameraPosition_StartMachine;

		// Token: 0x04004022 RID: 16418
		public StorageVisualizer InputVisuals;

		// Token: 0x04004023 RID: 16419
		public StorageVisualizer OutputVisuals;

		// Token: 0x04004024 RID: 16420
		public DigitalAlarm Clock;

		// Token: 0x04004025 RID: 16421
		public ToggleableLight Light;

		// Token: 0x04004026 RID: 16422
		public NewMixDiscoveryBox DiscoveryBox;

		// Token: 0x04004027 RID: 16423
		public Transform ItemContainer;

		// Token: 0x04004028 RID: 16424
		public Transform[] IngredientTransforms;

		// Token: 0x04004029 RID: 16425
		public Fillable BowlFillable;

		// Token: 0x0400402A RID: 16426
		public Clickable StartButton;

		// Token: 0x0400402B RID: 16427
		public Transform JugAlignment;

		// Token: 0x0400402C RID: 16428
		public Rigidbody Anchor;

		// Token: 0x0400402D RID: 16429
		public BoxCollider TrashSpawnVolume;

		// Token: 0x0400402E RID: 16430
		public Transform uiPoint;

		// Token: 0x0400402F RID: 16431
		public Transform[] accessPoints;

		// Token: 0x04004030 RID: 16432
		public ConfigurationReplicator configReplicator;

		// Token: 0x04004031 RID: 16433
		[Header("Sounds")]
		public StartLoopStopAudio MachineSound;

		// Token: 0x04004032 RID: 16434
		public AudioSourceController StartSound;

		// Token: 0x04004033 RID: 16435
		public AudioSourceController StopSound;

		// Token: 0x04004034 RID: 16436
		[Header("Mix Timing")]
		[Header("UI")]
		public MixingStationUIElement WorldspaceUIPrefab;

		// Token: 0x04004035 RID: 16437
		public Sprite typeIcon;

		// Token: 0x04004036 RID: 16438
		public UnityEvent onMixStart;

		// Token: 0x04004037 RID: 16439
		public UnityEvent onMixDone;

		// Token: 0x04004038 RID: 16440
		public UnityEvent onOutputCollected;

		// Token: 0x04004039 RID: 16441
		public UnityEvent onStartButtonClicked;

		// Token: 0x0400403C RID: 16444
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x0400403D RID: 16445
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x0400403E RID: 16446
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x0400403F RID: 16447
		private bool dll_Excuted;

		// Token: 0x04004040 RID: 16448
		private bool dll_Excuted;
	}
}
