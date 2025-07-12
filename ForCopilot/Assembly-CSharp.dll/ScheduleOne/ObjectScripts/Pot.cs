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
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Growing;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Lighting;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Management;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BFA RID: 3066
	public class Pot : GridItem, IUsable, IConfigurable, ITransitEntity
	{
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x0015B610 File Offset: 0x00159810
		// (set) Token: 0x0600523E RID: 21054 RVA: 0x0015B618 File Offset: 0x00159818
		public float SoilLevel
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<SoilLevel>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<SoilLevel>k__BackingField(value, true);
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x0015B622 File Offset: 0x00159822
		// (set) Token: 0x06005240 RID: 21056 RVA: 0x0015B62A File Offset: 0x0015982A
		public string SoilID
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<SoilID>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<SoilID>k__BackingField(value, true);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06005241 RID: 21057 RVA: 0x0015B634 File Offset: 0x00159834
		// (set) Token: 0x06005242 RID: 21058 RVA: 0x0015B63C File Offset: 0x0015983C
		public int RemainingSoilUses
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<RemainingSoilUses>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<RemainingSoilUses>k__BackingField(value, true);
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06005243 RID: 21059 RVA: 0x0015B646 File Offset: 0x00159846
		// (set) Token: 0x06005244 RID: 21060 RVA: 0x0015B64E File Offset: 0x0015984E
		public float WaterLevel
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<WaterLevel>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<WaterLevel>k__BackingField(value, true);
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06005245 RID: 21061 RVA: 0x0015B658 File Offset: 0x00159858
		public float NormalizedWaterLevel
		{
			get
			{
				return this.WaterLevel / this.WaterCapacity;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06005246 RID: 21062 RVA: 0x0015B667 File Offset: 0x00159867
		public bool IsFilledWithSoil
		{
			get
			{
				return this.SoilLevel >= this.SoilCapacity;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06005247 RID: 21063 RVA: 0x0015B67A File Offset: 0x0015987A
		// (set) Token: 0x06005248 RID: 21064 RVA: 0x0015B682 File Offset: 0x00159882
		public Plant Plant { get; protected set; }

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06005249 RID: 21065 RVA: 0x0015B68B File Offset: 0x0015988B
		// (set) Token: 0x0600524A RID: 21066 RVA: 0x0015B693 File Offset: 0x00159893
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

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x0600524B RID: 21067 RVA: 0x0015B69D File Offset: 0x0015989D
		// (set) Token: 0x0600524C RID: 21068 RVA: 0x0015B6A5 File Offset: 0x001598A5
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

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x0600524D RID: 21069 RVA: 0x0015B6AF File Offset: 0x001598AF
		public EntityConfiguration Configuration
		{
			get
			{
				return this.potConfiguration;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x0600524E RID: 21070 RVA: 0x0015B6B7 File Offset: 0x001598B7
		// (set) Token: 0x0600524F RID: 21071 RVA: 0x0015B6BF File Offset: 0x001598BF
		protected PotConfiguration potConfiguration { get; set; }

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06005250 RID: 21072 RVA: 0x0015B6C8 File Offset: 0x001598C8
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06005251 RID: 21073 RVA: 0x00014B5A File Offset: 0x00012D5A
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Pot;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06005252 RID: 21074 RVA: 0x0015B6D0 File Offset: 0x001598D0
		// (set) Token: 0x06005253 RID: 21075 RVA: 0x0015B6D8 File Offset: 0x001598D8
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06005254 RID: 21076 RVA: 0x0015B6E1 File Offset: 0x001598E1
		// (set) Token: 0x06005255 RID: 21077 RVA: 0x0015B6E9 File Offset: 0x001598E9
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

		// Token: 0x06005256 RID: 21078 RVA: 0x0015B6F3 File Offset: 0x001598F3
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06005257 RID: 21079 RVA: 0x0015B709 File Offset: 0x00159909
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06005258 RID: 21080 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06005259 RID: 21081 RVA: 0x0015B711 File Offset: 0x00159911
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600525A RID: 21082 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600525B RID: 21083 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600525C RID: 21084 RVA: 0x0015B719 File Offset: 0x00159919
		// (set) Token: 0x0600525D RID: 21085 RVA: 0x0015B721 File Offset: 0x00159921
		public List<ItemSlot> InputSlots { get; set; }

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x0600525E RID: 21086 RVA: 0x0015B72A File Offset: 0x0015992A
		// (set) Token: 0x0600525F RID: 21087 RVA: 0x0015B732 File Offset: 0x00159932
		public List<ItemSlot> OutputSlots { get; set; }

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x0015B73B File Offset: 0x0015993B
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x0015B743 File Offset: 0x00159943
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x0015B74B File Offset: 0x0015994B
		public bool Selectable { get; }

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06005263 RID: 21091 RVA: 0x0015B753 File Offset: 0x00159953
		// (set) Token: 0x06005264 RID: 21092 RVA: 0x0015B75B File Offset: 0x0015995B
		public bool IsAcceptingItems { get; set; }

		// Token: 0x06005265 RID: 21093 RVA: 0x0015B764 File Offset: 0x00159964
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Pot_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x0015B778 File Offset: 0x00159978
		protected override void Start()
		{
			base.Start();
			this.WaterLoggedVisuals.gameObject.SetActive(false);
			this.SetSoilState(Pot.ESoilState.Flat);
			this.UpdateSoilScale();
			this.UpdateSoilMaterial();
			this.WaterLevelSlider.value = this.WaterLevel / this.WaterCapacity;
			this.NoWaterIcon.gameObject.SetActive(this.WaterLevel <= 0f);
			this.WaterLevelCanvas.gameObject.SetActive(false);
			this.TaskBounds.gameObject.SetActive(false);
			SoilChunk[] soilChunks = this.SoilChunks;
			for (int i = 0; i < soilChunks.Length; i++)
			{
				soilChunks[i].ClickableEnabled = false;
			}
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x0015B828 File Offset: 0x00159A28
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (Additive additive in this.AppliedAdditives)
			{
				this.ApplyAdditive(connection, additive.AssetPath, false);
			}
			if (this.Plant != null)
			{
				this.PlantSeed(connection, this.Plant.SeedDefinition.ID, this.Plant.NormalizedGrowthProgress, this.Plant.YieldLevel, this.Plant.QualityLevel);
				for (int i = 0; i < this.Plant.ActiveHarvestables.Count; i++)
				{
					this.SetHarvestableActive(connection, this.Plant.ActiveHarvestables[i], true);
				}
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x0015B90C File Offset: 0x00159B0C
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Pot.<>c__DisplayClass143_0 CS$<>8__locals1 = new Pot.<>c__DisplayClass143_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x0015B94C File Offset: 0x00159B4C
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
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.OnMinPass));
				TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
				instance3.onTimeSkip = (Action<int>)Delegate.Combine(instance3.onTimeSkip, new Action<int>(this.TimeSkipped));
				base.ParentProperty.AddConfigurable(this);
				this.potConfiguration = new PotConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				this.outputSlot = new ItemSlot();
				this.OutputSlots.Add(this.outputSlot);
			}
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x0015BA08 File Offset: 0x00159C08
		public override void DestroyItem(bool callOnServer = true)
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.OnMinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Remove(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
			if (this.Plant != null)
			{
				this.Plant.Destroy(false);
			}
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x0015BAA8 File Offset: 0x00159CA8
		protected virtual void LateUpdate()
		{
			if (!this.intObjSetThisFrame)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				if (this.Plant != null && (!Singleton<ManagementClipboard>.InstanceExists || !Singleton<ManagementClipboard>.Instance.IsEquipped))
				{
					if (this.Plant.IsFullyGrown)
					{
						this.IntObj.SetMessage("Use trimmers to harvest");
					}
					else
					{
						this.IntObj.SetMessage(Mathf.FloorToInt(this.Plant.NormalizedGrowthProgress * 100f).ToString() + "% grown");
					}
					this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Label);
				}
			}
			this.intObjSetThisFrame = false;
			if (this.rotationOverridden)
			{
				this.ModelTransform.localRotation = Quaternion.Lerp(this.ModelTransform.localRotation, Quaternion.Euler(0f, this.rotation, 0f), Time.deltaTime * 10f);
			}
			else if (Mathf.Abs(this.ModelTransform.localEulerAngles.y) > 0.1f)
			{
				this.ModelTransform.localRotation = Quaternion.Lerp(this.ModelTransform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 10f);
			}
			this.UpdateCanvas();
			this.rotationOverridden = false;
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x0015BBFC File Offset: 0x00159DFC
		protected void UpdateCanvas()
		{
			if (Player.Local == null)
			{
				return;
			}
			if (Player.Local.CurrentProperty != base.ParentProperty)
			{
				this.WaterLevelCanvas.gameObject.SetActive(false);
				return;
			}
			if (!this.IsFilledWithSoil)
			{
				this.WaterLevelCanvas.gameObject.SetActive(false);
				return;
			}
			float num = Vector3.Distance(this.WaterLevelCanvas.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num > 2.75f)
			{
				this.WaterLevelCanvas.gameObject.SetActive(false);
				return;
			}
			Vector3 normalized = Vector3.ProjectOnPlane(PlayerSingleton<PlayerCamera>.Instance.transform.position - this.WaterCanvasContainer.position, Vector3.up).normalized;
			this.WaterCanvasContainer.forward = normalized;
			this.WaterLevelCanvas.transform.rotation = Quaternion.LookRotation((PlayerSingleton<PlayerCamera>.Instance.transform.position - this.WaterLevelCanvas.transform.position).normalized, PlayerSingleton<PlayerCamera>.Instance.transform.up);
			float num2 = 0.5f;
			float a = 1f - Mathf.Clamp01(Mathf.InverseLerp(2.75f - num2, 2.75f, num));
			float b = Mathf.Clamp01(Mathf.InverseLerp(0.5f, 0.75f, num));
			this.WaterLevelCanvasGroup.alpha = Mathf.Min(a, b);
			this.WaterLevelCanvas.gameObject.SetActive(true);
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x0015BD88 File Offset: 0x00159F88
		private void OnMinPass()
		{
			float num = this.WaterDrainPerHour * this.WaterCapacity / 60f * this.MoistureDrainMultiplier;
			this.WaterLevel = Mathf.Clamp(this.WaterLevel - num, 0f, this.WaterCapacity);
			this.UpdateSoilMaterial();
			if (this.Plant != null)
			{
				this.Plant.MinPass();
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x0015BDF0 File Offset: 0x00159FF0
		private void TimeSkipped(int minsSkippped)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < minsSkippped; i++)
			{
				this.OnMinPass();
			}
			if (this.Plant != null)
			{
				this.SetGrowProgress(this.Plant.NormalizedGrowthProgress);
			}
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x0015BE36 File Offset: 0x0015A036
		public void ConfigureInteraction(string message, InteractableObject.EInteractableState state, bool useHighLabelPos = false)
		{
			this.intObjSetThisFrame = true;
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(state);
			this.IntObj.displayLocationPoint = (useHighLabelPos ? this.IntObjLabel_High : this.IntObjLabel_Low);
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x0015BE74 File Offset: 0x0015A074
		public void PositionCameraContainer()
		{
			if (!this.AutoRotateCameraContainer)
			{
				return;
			}
			Vector3 vector = this.CameraContainer.parent.TransformPoint(new Vector3(0f, 0.75f, 0f));
			Vector3 a = PlayerSingleton<PlayerCamera>.Instance.transform.position - vector;
			a.y = 0f;
			a = a.normalized;
			this.CameraContainer.localPosition = new Vector3(0f, 0.75f, 0f);
			this.CameraContainer.position += a * 0.7f;
			Vector3 normalized = (vector - PlayerSingleton<PlayerCamera>.Instance.transform.position).normalized;
			normalized.y = 0f;
			this.CameraContainer.rotation = Quaternion.LookRotation(normalized, Vector3.up);
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x0015BF5C File Offset: 0x0015A15C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x0015BF7D File Offset: 0x0015A17D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x0015BF94 File Offset: 0x0015A194
		[ObserversRpc(RunLocally = true)]
		public virtual void ResetPot()
		{
			this.RpcWriter___Observers_ResetPot_2166136261();
			this.RpcLogic___ResetPot_2166136261();
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x0015BFB0 File Offset: 0x0015A1B0
		public float GetAverageLightExposure(out float growSpeedMultiplier)
		{
			growSpeedMultiplier = 1f;
			float num = 0f;
			if (this.LightSourceOverride != null)
			{
				return this.LightSourceOverride.GrowSpeedMultiplier;
			}
			for (int i = 0; i < this.CoordinatePairs.Count; i++)
			{
				float num2;
				num += base.OwnerGrid.GetTile(this.CoordinatePairs[i].coord2).LightExposureNode.GetTotalExposure(out num2);
				growSpeedMultiplier += num2;
			}
			growSpeedMultiplier /= (float)this.CoordinatePairs.Count;
			return num / (float)this.CoordinatePairs.Count;
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x0015C04C File Offset: 0x0015A24C
		public bool CanAcceptSeed(out string reason)
		{
			if (this.SoilLevel < this.SoilCapacity)
			{
				reason = "No soil";
				return false;
			}
			if (this.NormalizedWaterLevel >= 1f)
			{
				reason = "Waterlogged";
				return false;
			}
			if (this.Plant != null)
			{
				reason = "Already contains seed";
				return false;
			}
			reason = string.Empty;
			return this.SoilLevel >= this.SoilCapacity;
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x0015C0B8 File Offset: 0x0015A2B8
		public bool IsReadyForHarvest(out string reason)
		{
			if (this.Plant == null)
			{
				reason = "No plant in this pot";
				return false;
			}
			if (!this.Plant.IsFullyGrown)
			{
				reason = Mathf.Floor(this.Plant.NormalizedGrowthProgress * 100f).ToString() + "% grown";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x0015C11D File Offset: 0x0015A31D
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.Plant != null)
			{
				reason = "Contains plant";
				return false;
			}
			if (((IUsable)this).IsInUse)
			{
				reason = "In use by other player";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x0015C14E File Offset: 0x0015A34E
		public void OverrideRotation(float angle)
		{
			this.rotationOverridden = true;
			this.rotation = angle;
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x0015C15E File Offset: 0x0015A35E
		public Transform GetCameraPosition(Pot.ECameraPosition pos)
		{
			switch (pos)
			{
			case Pot.ECameraPosition.Closeup:
				return this.CloseupPosition;
			case Pot.ECameraPosition.Midshot:
				return this.MidshotPosition;
			case Pot.ECameraPosition.Fullshot:
				return this.FullshotPosition;
			case Pot.ECameraPosition.BirdsEye:
				return this.BirdsEyePosition;
			default:
				return null;
			}
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x0015C195 File Offset: 0x0015A395
		public virtual void AddSoil(float amount)
		{
			this.SoilLevel = Mathf.Clamp(this.SoilLevel + amount, 0f, this.SoilCapacity);
			this.UpdateSoilScale();
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x0015C1BB File Offset: 0x0015A3BB
		private void SoilLevelChanged(float _prev, float _new, bool asServer)
		{
			this.UpdateSoilScale();
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x0015C1C4 File Offset: 0x0015A3C4
		protected virtual void UpdateSoilScale()
		{
			Vector3 localScale = Vector3.Lerp(this.DirtMinScale, this.DirtMaxScale, this.SoilLevel / this.SoilCapacity);
			this.Dirt_Flat.localScale = localScale;
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x0015C1FC File Offset: 0x0015A3FC
		public virtual void SetSoilID(string id)
		{
			this.SoilID = id;
			this.appliedSoilDefinition = (Registry.GetItem(this.SoilID) as SoilDefinition);
			this.UpdateSoilMaterial();
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x0015C221 File Offset: 0x0015A421
		public virtual void SetSoilUses(int uses)
		{
			this.RemainingSoilUses = uses;
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x0015C22A File Offset: 0x0015A42A
		public void PushSoilDataToServer()
		{
			this.SendSoilData(this.SoilID, this.SoilLevel, this.RemainingSoilUses);
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x0015C244 File Offset: 0x0015A444
		[ServerRpc(RequireOwnership = false)]
		public void SendSoilData(string soilID, float soilLevel, int soilUses)
		{
			this.RpcWriter___Server_SendSoilData_3104499779(soilID, soilLevel, soilUses);
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x0015C264 File Offset: 0x0015A464
		public void SetSoilState(Pot.ESoilState state)
		{
			if (state == Pot.ESoilState.Flat && this.Plant == null)
			{
				this.Dirt_Parted.gameObject.SetActive(false);
				this.Dirt_Flat.gameObject.SetActive(true);
				return;
			}
			if (state == Pot.ESoilState.Parted || state == Pot.ESoilState.Packed)
			{
				this.Dirt_Parted.gameObject.SetActive(true);
				this.Dirt_Flat.gameObject.SetActive(false);
				if (state == Pot.ESoilState.Packed)
				{
					for (int i = 0; i < this.SoilChunks.Length; i++)
					{
						this.SoilChunks[i].SetLerpedTransform(1f);
					}
					return;
				}
				for (int j = 0; j < this.SoilChunks.Length; j++)
				{
					this.SoilChunks[j].SetLerpedTransform(0f);
				}
			}
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x0015C320 File Offset: 0x0015A520
		protected virtual void UpdateSoilMaterial()
		{
			if (this.SoilID == string.Empty)
			{
				return;
			}
			if (this.appliedSoilDefinition == null)
			{
				this.appliedSoilDefinition = (Registry.GetItem(this.SoilID) as SoilDefinition);
			}
			Material material = this.appliedSoilDefinition.WetSoilMat;
			if (this.NormalizedWaterLevel <= 0f)
			{
				material = this.appliedSoilDefinition.DrySoilMat;
			}
			for (int i = 0; i < this.DirtRenderers.Count; i++)
			{
				if (!(this.DirtRenderers[i] == null))
				{
					this.DirtRenderers[i].material = material;
				}
			}
			this.WaterLoggedVisuals.SetActive(this.NormalizedWaterLevel > 1f);
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x0015C3E0 File Offset: 0x0015A5E0
		public void ChangeWaterAmount(float change)
		{
			this.WaterLevel = Mathf.Clamp(this.WaterLevel + change, 0f, this.WaterCapacity);
			this.UpdateSoilMaterial();
			this.WaterLevelSlider.value = this.WaterLevel / this.WaterCapacity;
			this.NoWaterIcon.gameObject.SetActive(this.WaterLevel <= 0f);
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x0015C449 File Offset: 0x0015A649
		public void PushWaterDataToServer()
		{
			this.SendWaterData(this.WaterLevel);
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x0015C457 File Offset: 0x0015A657
		[ServerRpc(RequireOwnership = false)]
		public void SendWaterData(float waterLevel)
		{
			this.RpcWriter___Server_SendWaterData_431000436(waterLevel);
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x0015C463 File Offset: 0x0015A663
		private void WaterLevelChanged(float _prev, float _new, bool asServer)
		{
			this.UpdateSoilMaterial();
			this.WaterLevelSlider.value = this.WaterLevel / this.WaterCapacity;
			this.NoWaterIcon.gameObject.SetActive(this.WaterLevel <= 0f);
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x0015C4A3 File Offset: 0x0015A6A3
		public void SetTargetActive(bool active)
		{
			this.Target.gameObject.SetActive(active);
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x0015C4B8 File Offset: 0x0015A6B8
		public void RandomizeTarget()
		{
			int num = 0;
			Vector3 vector;
			do
			{
				Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
				insideUnitSphere.y = 0f;
				vector = base.transform.position + insideUnitSphere * (this.PotRadius * 0.85f);
				vector.y = this.Target.position.y;
				num++;
			}
			while (Vector3.Distance(this.Target.position, vector) < 0.15f && num < 100);
			this.Target.position = vector;
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x0015C540 File Offset: 0x0015A740
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendAdditive(string additiveAssetPath, bool initial)
		{
			this.RpcWriter___Server_SendAdditive_310431262(additiveAssetPath, initial);
			this.RpcLogic___SendAdditive_310431262(additiveAssetPath, initial);
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x0015C560 File Offset: 0x0015A760
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ApplyAdditive(NetworkConnection conn, string additiveAssetPath, bool initial)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ApplyAdditive_619441887(conn, additiveAssetPath, initial);
				this.RpcLogic___ApplyAdditive_619441887(conn, additiveAssetPath, initial);
			}
			else
			{
				this.RpcWriter___Target_ApplyAdditive_619441887(conn, additiveAssetPath, initial);
			}
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x0015C5B0 File Offset: 0x0015A7B0
		public float GetAdditiveGrowthMultiplier()
		{
			float num = 1f;
			foreach (Additive additive in this.AppliedAdditives)
			{
				num *= additive.GrowSpeedMultiplier;
			}
			return num;
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x0015C60C File Offset: 0x0015A80C
		public float GetNetYieldChange()
		{
			float num = 0f;
			foreach (Additive additive in this.AppliedAdditives)
			{
				num += additive.YieldChange;
			}
			return num;
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x0015C668 File Offset: 0x0015A868
		public float GetNetQualityChange()
		{
			float num = 0f;
			foreach (Additive additive in this.AppliedAdditives)
			{
				num += additive.QualityChange;
			}
			return num;
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x0015C6C4 File Offset: 0x0015A8C4
		public Additive GetAdditive(string additiveName)
		{
			return this.AppliedAdditives.Find((Additive x) => x.AdditiveName.ToLower() == additiveName.ToLower());
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x0015C6F5 File Offset: 0x0015A8F5
		[ObserversRpc(RunLocally = true)]
		public void FullyGrowPlant()
		{
			this.RpcWriter___Observers_FullyGrowPlant_2166136261();
			this.RpcLogic___FullyGrowPlant_2166136261();
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x0015C703 File Offset: 0x0015A903
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlantSeed(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			this.RpcWriter___Server_SendPlantSeed_2530605204(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
			this.RpcLogic___SendPlantSeed_2530605204(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x0015C734 File Offset: 0x0015A934
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void PlantSeed(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_PlantSeed_709433087(conn, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
				this.RpcLogic___PlantSeed_709433087(conn, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
			}
			else
			{
				this.RpcWriter___Target_PlantSeed_709433087(conn, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
			}
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x0015C799 File Offset: 0x0015A999
		[ObserversRpc]
		private void SetGrowProgress(float progress)
		{
			this.RpcWriter___Observers_SetGrowProgress_431000436(progress);
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x0015C7A8 File Offset: 0x0015A9A8
		private void PlantSeed(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			if (this.Plant != null)
			{
				return;
			}
			if (this.SoilLevel < this.SoilCapacity)
			{
				Console.LogWarning("Pot not full of soil!", null);
				return;
			}
			SeedDefinition seedDefinition = Registry.GetItem(seedID) as SeedDefinition;
			if (seedDefinition == null)
			{
				string str = "PlantSeed: seed not found with ID '";
				SeedDefinition seedDefinition2 = seedDefinition;
				Console.LogWarning(str + ((seedDefinition2 != null) ? seedDefinition2.ToString() : null) + "'", null);
				return;
			}
			this.SetSoilState(Pot.ESoilState.Packed);
			this.Plant = UnityEngine.Object.Instantiate<GameObject>(seedDefinition.PlantPrefab.gameObject, this.PlantContainer).GetComponent<Plant>();
			this.Plant.transform.localEulerAngles = new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f);
			this.Plant.Initialize(base.NetworkObject, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0015C884 File Offset: 0x0015AA84
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetHarvestableActive(NetworkConnection conn, int harvestableIndex, bool active)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetHarvestableActive_338960014(conn, harvestableIndex, active);
				this.RpcLogic___SetHarvestableActive_338960014(conn, harvestableIndex, active);
			}
			else
			{
				this.RpcWriter___Target_SetHarvestableActive_338960014(conn, harvestableIndex, active);
			}
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x0015C8D4 File Offset: 0x0015AAD4
		public void SetHarvestableActive_Local(int harvestableIndex, bool active)
		{
			if (this.Plant == null)
			{
				Console.LogWarning("SetHarvestableActive called but plant is null!", null);
				return;
			}
			if (this.Plant.IsHarvestableActive(harvestableIndex) == active)
			{
				return;
			}
			int count = this.Plant.ActiveHarvestables.Count;
			this.Plant.SetHarvestableActive(harvestableIndex, active);
			if (count > 0 && this.Plant.ActiveHarvestables.Count == 0)
			{
				if (InstanceFinder.IsServer)
				{
					float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("HarvestedPlantCount");
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("HarvestedPlantCount", (value + 1f).ToString(), true);
					NetworkSingleton<LevelManager>.Instance.AddXP(5);
				}
				this.ResetPot();
			}
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0015C984 File Offset: 0x0015AB84
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendHarvestableActive(int harvestableIndex, bool active)
		{
			this.RpcWriter___Server_SendHarvestableActive_3658436649(harvestableIndex, active);
			this.RpcLogic___SendHarvestableActive_3658436649(harvestableIndex, active);
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x0015C9A2 File Offset: 0x0015ABA2
		public void SendHarvestableActive_Local(int harvestableIndex, bool active)
		{
			this.SetHarvestableActive_Local(harvestableIndex, active);
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x0015C9AC File Offset: 0x0015ABAC
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
			PotUIElement component = UnityEngine.Object.Instantiate<PotUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<PotUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x0015CA3F File Offset: 0x0015AC3F
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x0015CA5C File Offset: 0x0015AC5C
		public override BuildableItemData GetBaseData()
		{
			PlantData plantData = null;
			if (this.Plant != null)
			{
				plantData = this.Plant.GetPlantData();
			}
			return new PotData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.SoilID, this.SoilLevel, this.RemainingSoilUses, this.WaterLevel, this.AppliedAdditives.ConvertAll<string>((Additive x) => x.AssetPath).ToArray(), plantData);
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x0015CAF4 File Offset: 0x0015ACF4
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			if (this.Configuration.ShouldSave())
			{
				saveData.AddData("Configuration", this.Configuration.GetSaveString());
			}
			return saveData;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x0015CB2C File Offset: 0x0015AD2C
		public virtual void LoadPlant(PlantData data)
		{
			Pot.<>c__DisplayClass196_0 CS$<>8__locals1 = new Pot.<>c__DisplayClass196_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.data = data;
			if (string.IsNullOrEmpty(CS$<>8__locals1.data.SeedID))
			{
				return;
			}
			base.StartCoroutine(CS$<>8__locals1.<LoadPlant>g__Wait|0());
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x0015CB70 File Offset: 0x0015AD70
		public Pot()
		{
			this.<SoilID>k__BackingField = string.Empty;
			this.AppliedAdditives = new List<Additive>();
			this.InputSlots = new List<ItemSlot>();
			this.OutputSlots = new List<ItemSlot>();
			this.IsAcceptingItems = true;
			base..ctor();
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x0015CC28 File Offset: 0x0015AE28
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 6U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 5U, 1, 0, -1f, 0, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 4U, 1, 0, -1f, 0, this.<NPCUserObject>k__BackingField);
			this.syncVar___<WaterLevel>k__BackingField = new SyncVar<float>(this, 3U, 1, 0, -1f, 0, this.<WaterLevel>k__BackingField);
			this.syncVar___<WaterLevel>k__BackingField.OnChange += this.WaterLevelChanged;
			this.syncVar___<RemainingSoilUses>k__BackingField = new SyncVar<int>(this, 2U, 1, 0, -1f, 0, this.<RemainingSoilUses>k__BackingField);
			this.syncVar___<SoilID>k__BackingField = new SyncVar<string>(this, 1U, 1, 0, -1f, 0, this.<SoilID>k__BackingField);
			this.syncVar___<SoilLevel>k__BackingField = new SyncVar<float>(this, 0U, 1, 0, -1f, 0, this.<SoilLevel>k__BackingField);
			this.syncVar___<SoilLevel>k__BackingField.OnChange += this.SoilLevelChanged;
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ResetPot_2166136261));
			base.RegisterServerRpc(12U, new ServerRpcDelegate(this.RpcReader___Server_SendSoilData_3104499779));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_SendWaterData_431000436));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SendAdditive_310431262));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_ApplyAdditive_619441887));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_ApplyAdditive_619441887));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_FullyGrowPlant_2166136261));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SendPlantSeed_2530605204));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_PlantSeed_709433087));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_PlantSeed_709433087));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetGrowProgress_431000436));
			base.RegisterObserversRpc(22U, new ClientRpcDelegate(this.RpcReader___Observers_SetHarvestableActive_338960014));
			base.RegisterTargetRpc(23U, new ClientRpcDelegate(this.RpcReader___Target_SetHarvestableActive_338960014));
			base.RegisterServerRpc(24U, new ServerRpcDelegate(this.RpcReader___Server_SendHarvestableActive_3658436649));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.Pot));
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x0015CF40 File Offset: 0x0015B140
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
			this.syncVar___<WaterLevel>k__BackingField.SetRegistered();
			this.syncVar___<RemainingSoilUses>k__BackingField.SetRegistered();
			this.syncVar___<SoilID>k__BackingField.SetRegistered();
			this.syncVar___<SoilLevel>k__BackingField.SetRegistered();
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x0015CFB1 File Offset: 0x0015B1B1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x0015CFC0 File Offset: 0x0015B1C0
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

		// Token: 0x060052A2 RID: 21154 RVA: 0x0015D067 File Offset: 0x0015B267
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x0015D070 File Offset: 0x0015B270
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

		// Token: 0x060052A4 RID: 21156 RVA: 0x0015D0B0 File Offset: 0x0015B2B0
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

		// Token: 0x060052A5 RID: 21157 RVA: 0x0015D158 File Offset: 0x0015B358
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x0015D1AC File Offset: 0x0015B3AC
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

		// Token: 0x060052A7 RID: 21159 RVA: 0x0015D1EC File Offset: 0x0015B3EC
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

		// Token: 0x060052A8 RID: 21160 RVA: 0x0015D293 File Offset: 0x0015B493
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x0015D29C File Offset: 0x0015B49C
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

		// Token: 0x060052AA RID: 21162 RVA: 0x0015D2DC File Offset: 0x0015B4DC
		private void RpcWriter___Observers_ResetPot_2166136261()
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
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x0015D388 File Offset: 0x0015B588
		public virtual void RpcLogic___ResetPot_2166136261()
		{
			if (this.Plant != null)
			{
				this.Plant.Destroy(true);
			}
			this.Plant = null;
			if (InstanceFinder.IsServer)
			{
				int remainingSoilUses = this.RemainingSoilUses;
				this.RemainingSoilUses = remainingSoilUses - 1;
			}
			if (this.RemainingSoilUses <= 0)
			{
				this.WaterLevel = 0f;
				this.appliedSoilDefinition = null;
				this.SoilID = string.Empty;
				this.SoilLevel = 0f;
			}
			foreach (Additive additive in this.AppliedAdditives)
			{
				UnityEngine.Object.Destroy(additive.gameObject);
			}
			this.AppliedAdditives.Clear();
			this.SetSoilState(Pot.ESoilState.Flat);
			this.UpdateSoilScale();
			this.UpdateSoilMaterial();
			base.HasChanged = true;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x0015D46C File Offset: 0x0015B66C
		private void RpcReader___Observers_ResetPot_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ResetPot_2166136261();
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x0015D498 File Offset: 0x0015B698
		private void RpcWriter___Server_SendSoilData_3104499779(string soilID, float soilLevel, int soilUses)
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
			writer.WriteString(soilID);
			writer.WriteSingle(soilLevel, 0);
			writer.WriteInt32(soilUses, 1);
			base.SendServerRpc(12U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x0015D564 File Offset: 0x0015B764
		public void RpcLogic___SendSoilData_3104499779(string soilID, float soilLevel, int soilUses)
		{
			this.SoilID = soilID;
			if (soilID != string.Empty)
			{
				this.appliedSoilDefinition = (Registry.GetItem(this.SoilID) as SoilDefinition);
			}
			else
			{
				this.appliedSoilDefinition = null;
			}
			this.SoilLevel = soilLevel;
			this.RemainingSoilUses = soilUses;
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x0015D5B4 File Offset: 0x0015B7B4
		private void RpcReader___Server_SendSoilData_3104499779(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string soilID = PooledReader0.ReadString();
			float soilLevel = PooledReader0.ReadSingle(0);
			int soilUses = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendSoilData_3104499779(soilID, soilLevel, soilUses);
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x0015D614 File Offset: 0x0015B814
		private void RpcWriter___Server_SendWaterData_431000436(float waterLevel)
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
			writer.WriteSingle(waterLevel, 0);
			base.SendServerRpc(13U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x0015D6C0 File Offset: 0x0015B8C0
		public void RpcLogic___SendWaterData_431000436(float waterLevel)
		{
			this.WaterLevel = waterLevel;
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x0015D6CC File Offset: 0x0015B8CC
		private void RpcReader___Server_SendWaterData_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float waterLevel = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendWaterData_431000436(waterLevel);
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x0015D704 File Offset: 0x0015B904
		private void RpcWriter___Server_SendAdditive_310431262(string additiveAssetPath, bool initial)
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
			writer.WriteString(additiveAssetPath);
			writer.WriteBoolean(initial);
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x0015D7B8 File Offset: 0x0015B9B8
		public void RpcLogic___SendAdditive_310431262(string additiveAssetPath, bool initial)
		{
			this.ApplyAdditive(null, additiveAssetPath, initial);
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x0015D7C4 File Offset: 0x0015B9C4
		private void RpcReader___Server_SendAdditive_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string additiveAssetPath = PooledReader0.ReadString();
			bool initial = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAdditive_310431262(additiveAssetPath, initial);
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x0015D814 File Offset: 0x0015BA14
		private void RpcWriter___Observers_ApplyAdditive_619441887(NetworkConnection conn, string additiveAssetPath, bool initial)
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
			writer.WriteString(additiveAssetPath);
			writer.WriteBoolean(initial);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x0015D8D8 File Offset: 0x0015BAD8
		public void RpcLogic___ApplyAdditive_619441887(NetworkConnection conn, string additiveAssetPath, bool initial)
		{
			if (this.AppliedAdditives.Find((Additive x) => x.AssetPath == additiveAssetPath))
			{
				Console.Log("Pot already contains additive at " + additiveAssetPath, null);
				return;
			}
			GameObject gameObject = Resources.Load(additiveAssetPath) as GameObject;
			if (gameObject == null)
			{
				Console.LogWarning("Failed to load additive at path: " + additiveAssetPath, null);
				return;
			}
			Additive component = UnityEngine.Object.Instantiate<GameObject>(gameObject, this.AdditivesContainer).GetComponent<Additive>();
			component.transform.localPosition = Vector3.zero;
			component.transform.localRotation = Quaternion.identity;
			if (this.Plant != null)
			{
				this.Plant.QualityLevel += component.QualityChange;
				this.Plant.YieldLevel += component.YieldChange;
				if (initial)
				{
					this.Plant.SetNormalizedGrowthProgress(this.Plant.NormalizedGrowthProgress + component.InstantGrowth);
					if (component.InstantGrowth > 0f)
					{
						this.PoofParticles.Play();
						this.PoofSound.Play();
					}
				}
			}
			this.AppliedAdditives.Add(component);
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x0015DA18 File Offset: 0x0015BC18
		private void RpcReader___Observers_ApplyAdditive_619441887(PooledReader PooledReader0, Channel channel)
		{
			string additiveAssetPath = PooledReader0.ReadString();
			bool initial = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ApplyAdditive_619441887(null, additiveAssetPath, initial);
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x0015DA68 File Offset: 0x0015BC68
		private void RpcWriter___Target_ApplyAdditive_619441887(NetworkConnection conn, string additiveAssetPath, bool initial)
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
			writer.WriteString(additiveAssetPath);
			writer.WriteBoolean(initial);
			base.SendTargetRpc(16U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x0015DB2C File Offset: 0x0015BD2C
		private void RpcReader___Target_ApplyAdditive_619441887(PooledReader PooledReader0, Channel channel)
		{
			string additiveAssetPath = PooledReader0.ReadString();
			bool initial = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ApplyAdditive_619441887(base.LocalConnection, additiveAssetPath, initial);
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x0015DB74 File Offset: 0x0015BD74
		private void RpcWriter___Observers_FullyGrowPlant_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x0015DC1D File Offset: 0x0015BE1D
		public void RpcLogic___FullyGrowPlant_2166136261()
		{
			if (this.Plant == null)
			{
				Console.LogWarning("FullyGrowPlant called but plant is null!", null);
				return;
			}
			this.Plant.SetNormalizedGrowthProgress(1f);
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x0015DC4C File Offset: 0x0015BE4C
		private void RpcReader___Observers_FullyGrowPlant_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___FullyGrowPlant_2166136261();
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x0015DC78 File Offset: 0x0015BE78
		private void RpcWriter___Server_SendPlantSeed_2530605204(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
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
			writer.WriteString(seedID);
			writer.WriteSingle(normalizedSeedProgress, 0);
			writer.WriteSingle(yieldLevel, 0);
			writer.WriteSingle(qualityLevel, 0);
			base.SendServerRpc(18U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x0015DD55 File Offset: 0x0015BF55
		public void RpcLogic___SendPlantSeed_2530605204(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			this.PlantSeed(null, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x0015DD64 File Offset: 0x0015BF64
		private void RpcReader___Server_SendPlantSeed_2530605204(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string seedID = PooledReader0.ReadString();
			float normalizedSeedProgress = PooledReader0.ReadSingle(0);
			float yieldLevel = PooledReader0.ReadSingle(0);
			float qualityLevel = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlantSeed_2530605204(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x0015DDE4 File Offset: 0x0015BFE4
		private void RpcWriter___Observers_PlantSeed_709433087(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
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
			writer.WriteString(seedID);
			writer.WriteSingle(normalizedSeedProgress, 0);
			writer.WriteSingle(yieldLevel, 0);
			writer.WriteSingle(qualityLevel, 0);
			base.SendObserversRpc(19U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x0015DED0 File Offset: 0x0015C0D0
		public void RpcLogic___PlantSeed_709433087(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			this.PlantSeed(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x0015DEE0 File Offset: 0x0015C0E0
		private void RpcReader___Observers_PlantSeed_709433087(PooledReader PooledReader0, Channel channel)
		{
			string seedID = PooledReader0.ReadString();
			float normalizedSeedProgress = PooledReader0.ReadSingle(0);
			float yieldLevel = PooledReader0.ReadSingle(0);
			float qualityLevel = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PlantSeed_709433087(null, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x0015DF60 File Offset: 0x0015C160
		private void RpcWriter___Target_PlantSeed_709433087(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
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
			writer.WriteString(seedID);
			writer.WriteSingle(normalizedSeedProgress, 0);
			writer.WriteSingle(yieldLevel, 0);
			writer.WriteSingle(qualityLevel, 0);
			base.SendTargetRpc(20U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x0015E04C File Offset: 0x0015C24C
		private void RpcReader___Target_PlantSeed_709433087(PooledReader PooledReader0, Channel channel)
		{
			string seedID = PooledReader0.ReadString();
			float normalizedSeedProgress = PooledReader0.ReadSingle(0);
			float yieldLevel = PooledReader0.ReadSingle(0);
			float qualityLevel = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PlantSeed_709433087(base.LocalConnection, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x0015E0C8 File Offset: 0x0015C2C8
		private void RpcWriter___Observers_SetGrowProgress_431000436(float progress)
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
			writer.WriteSingle(progress, 0);
			base.SendObserversRpc(21U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x0015E183 File Offset: 0x0015C383
		private void RpcLogic___SetGrowProgress_431000436(float progress)
		{
			if (this.Plant == null)
			{
				Console.LogWarning("SetGrowProgress called but plant is null!", null);
				return;
			}
			this.Plant.SetNormalizedGrowthProgress(progress);
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x0015E1AC File Offset: 0x0015C3AC
		private void RpcReader___Observers_SetGrowProgress_431000436(PooledReader PooledReader0, Channel channel)
		{
			float progress = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGrowProgress_431000436(progress);
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x0015E1E4 File Offset: 0x0015C3E4
		private void RpcWriter___Observers_SetHarvestableActive_338960014(NetworkConnection conn, int harvestableIndex, bool active)
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
			writer.WriteInt32(harvestableIndex, 1);
			writer.WriteBoolean(active);
			base.SendObserversRpc(22U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x0015E2AC File Offset: 0x0015C4AC
		public void RpcLogic___SetHarvestableActive_338960014(NetworkConnection conn, int harvestableIndex, bool active)
		{
			this.SetHarvestableActive_Local(harvestableIndex, active);
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x0015E2B8 File Offset: 0x0015C4B8
		private void RpcReader___Observers_SetHarvestableActive_338960014(PooledReader PooledReader0, Channel channel)
		{
			int harvestableIndex = PooledReader0.ReadInt32(1);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetHarvestableActive_338960014(null, harvestableIndex, active);
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x0015E30C File Offset: 0x0015C50C
		private void RpcWriter___Target_SetHarvestableActive_338960014(NetworkConnection conn, int harvestableIndex, bool active)
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
			writer.WriteInt32(harvestableIndex, 1);
			writer.WriteBoolean(active);
			base.SendTargetRpc(23U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x0015E3D4 File Offset: 0x0015C5D4
		private void RpcReader___Target_SetHarvestableActive_338960014(PooledReader PooledReader0, Channel channel)
		{
			int harvestableIndex = PooledReader0.ReadInt32(1);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetHarvestableActive_338960014(base.LocalConnection, harvestableIndex, active);
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x0015E424 File Offset: 0x0015C624
		private void RpcWriter___Server_SendHarvestableActive_3658436649(int harvestableIndex, bool active)
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
			writer.WriteInt32(harvestableIndex, 1);
			writer.WriteBoolean(active);
			base.SendServerRpc(24U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x0015E4DD File Offset: 0x0015C6DD
		public void RpcLogic___SendHarvestableActive_3658436649(int harvestableIndex, bool active)
		{
			this.SetHarvestableActive(null, harvestableIndex, active);
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x0015E4E8 File Offset: 0x0015C6E8
		private void RpcReader___Server_SendHarvestableActive_3658436649(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int harvestableIndex = PooledReader0.ReadInt32(1);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendHarvestableActive_3658436649(harvestableIndex, active);
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060052D1 RID: 21201 RVA: 0x0015E53C File Offset: 0x0015C73C
		// (set) Token: 0x060052D2 RID: 21202 RVA: 0x0015E544 File Offset: 0x0015C744
		public float SyncAccessor_<SoilLevel>k__BackingField
		{
			get
			{
				return this.<SoilLevel>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<SoilLevel>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<SoilLevel>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x0015E580 File Offset: 0x0015C780
		public override bool Pot(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 6U)
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
			else if (UInt321 == 5U)
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
			else if (UInt321 == 4U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value3 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value3, Boolean2);
				return true;
			}
			else if (UInt321 == 3U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<WaterLevel>k__BackingField(this.syncVar___<WaterLevel>k__BackingField.GetValue(true), true);
					return true;
				}
				float value4 = PooledReader0.ReadSingle(0);
				this.sync___set_value_<WaterLevel>k__BackingField(value4, Boolean2);
				return true;
			}
			else if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<RemainingSoilUses>k__BackingField(this.syncVar___<RemainingSoilUses>k__BackingField.GetValue(true), true);
					return true;
				}
				int value5 = PooledReader0.ReadInt32(1);
				this.sync___set_value_<RemainingSoilUses>k__BackingField(value5, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<SoilID>k__BackingField(this.syncVar___<SoilID>k__BackingField.GetValue(true), true);
					return true;
				}
				string value6 = PooledReader0.ReadString();
				this.sync___set_value_<SoilID>k__BackingField(value6, Boolean2);
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
					this.sync___set_value_<SoilLevel>k__BackingField(this.syncVar___<SoilLevel>k__BackingField.GetValue(true), true);
					return true;
				}
				float value7 = PooledReader0.ReadSingle(0);
				this.sync___set_value_<SoilLevel>k__BackingField(value7, Boolean2);
				return true;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060052D4 RID: 21204 RVA: 0x0015E779 File Offset: 0x0015C979
		// (set) Token: 0x060052D5 RID: 21205 RVA: 0x0015E781 File Offset: 0x0015C981
		public string SyncAccessor_<SoilID>k__BackingField
		{
			get
			{
				return this.<SoilID>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<SoilID>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<SoilID>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060052D6 RID: 21206 RVA: 0x0015E7BD File Offset: 0x0015C9BD
		// (set) Token: 0x060052D7 RID: 21207 RVA: 0x0015E7C5 File Offset: 0x0015C9C5
		public int SyncAccessor_<RemainingSoilUses>k__BackingField
		{
			get
			{
				return this.<RemainingSoilUses>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<RemainingSoilUses>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<RemainingSoilUses>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060052D8 RID: 21208 RVA: 0x0015E801 File Offset: 0x0015CA01
		// (set) Token: 0x060052D9 RID: 21209 RVA: 0x0015E809 File Offset: 0x0015CA09
		public float SyncAccessor_<WaterLevel>k__BackingField
		{
			get
			{
				return this.<WaterLevel>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<WaterLevel>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<WaterLevel>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x060052DA RID: 21210 RVA: 0x0015E845 File Offset: 0x0015CA45
		// (set) Token: 0x060052DB RID: 21211 RVA: 0x0015E84D File Offset: 0x0015CA4D
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

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060052DC RID: 21212 RVA: 0x0015E889 File Offset: 0x0015CA89
		// (set) Token: 0x060052DD RID: 21213 RVA: 0x0015E891 File Offset: 0x0015CA91
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

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060052DE RID: 21214 RVA: 0x0015E8CD File Offset: 0x0015CACD
		// (set) Token: 0x060052DF RID: 21215 RVA: 0x0015E8D5 File Offset: 0x0015CAD5
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

		// Token: 0x060052E0 RID: 21216 RVA: 0x0015E911 File Offset: 0x0015CB11
		protected override void dll()
		{
			base.Awake();
			this.SoilCover.gameObject.SetActive(false);
			this.SetTargetActive(false);
		}

		// Token: 0x04003DA1 RID: 15777
		public const float DryThreshold = 0f;

		// Token: 0x04003DA2 RID: 15778
		public const float WaterloggedThreshold = 1f;

		// Token: 0x04003DA3 RID: 15779
		public const float ROTATION_SPEED = 10f;

		// Token: 0x04003DA4 RID: 15780
		public const float MAX_CAMERA_DISTANCE = 2.75f;

		// Token: 0x04003DA5 RID: 15781
		public const float MIN_CAMERA_DISTANCE = 0.5f;

		// Token: 0x04003DA6 RID: 15782
		[Header("References")]
		public Transform ModelTransform;

		// Token: 0x04003DA7 RID: 15783
		public InteractableObject IntObj;

		// Token: 0x04003DA8 RID: 15784
		public Transform PourableStartPoint;

		// Token: 0x04003DA9 RID: 15785
		public Transform SeedStartPoint;

		// Token: 0x04003DAA RID: 15786
		public Transform SeedRestingPoint;

		// Token: 0x04003DAB RID: 15787
		public GameObject WaterLoggedVisuals;

		// Token: 0x04003DAC RID: 15788
		public Transform LookAtPoint;

		// Token: 0x04003DAD RID: 15789
		public Transform AdditivesContainer;

		// Token: 0x04003DAE RID: 15790
		public Transform PlantContainer;

		// Token: 0x04003DAF RID: 15791
		public Transform IntObjLabel_Low;

		// Token: 0x04003DB0 RID: 15792
		public Transform IntObjLabel_High;

		// Token: 0x04003DB1 RID: 15793
		public Transform uiPoint;

		// Token: 0x04003DB2 RID: 15794
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04003DB3 RID: 15795
		public Transform[] accessPoints;

		// Token: 0x04003DB4 RID: 15796
		public Transform TaskBounds;

		// Token: 0x04003DB5 RID: 15797
		public PotSoilCover SoilCover;

		// Token: 0x04003DB6 RID: 15798
		public Transform LeafDropPoint;

		// Token: 0x04003DB7 RID: 15799
		public ParticleSystem PoofParticles;

		// Token: 0x04003DB8 RID: 15800
		public AudioSourceController PoofSound;

		// Token: 0x04003DB9 RID: 15801
		[Header("UI")]
		public Transform WaterCanvasContainer;

		// Token: 0x04003DBA RID: 15802
		public Canvas WaterLevelCanvas;

		// Token: 0x04003DBB RID: 15803
		public CanvasGroup WaterLevelCanvasGroup;

		// Token: 0x04003DBC RID: 15804
		public Slider WaterLevelSlider;

		// Token: 0x04003DBD RID: 15805
		public GameObject NoWaterIcon;

		// Token: 0x04003DBE RID: 15806
		public PotUIElement WorldspaceUIPrefab;

		// Token: 0x04003DBF RID: 15807
		public Sprite typeIcon;

		// Token: 0x04003DC0 RID: 15808
		[Header("Camera References")]
		public Transform CameraContainer;

		// Token: 0x04003DC1 RID: 15809
		public Transform MidshotPosition;

		// Token: 0x04003DC2 RID: 15810
		public Transform CloseupPosition;

		// Token: 0x04003DC3 RID: 15811
		public Transform FullshotPosition;

		// Token: 0x04003DC4 RID: 15812
		public Transform BirdsEyePosition;

		// Token: 0x04003DC5 RID: 15813
		public bool AutoRotateCameraContainer = true;

		// Token: 0x04003DC6 RID: 15814
		[Header("Dirt references")]
		public Transform Dirt_Flat;

		// Token: 0x04003DC7 RID: 15815
		public Transform Dirt_Parted;

		// Token: 0x04003DC8 RID: 15816
		public SoilChunk[] SoilChunks;

		// Token: 0x04003DC9 RID: 15817
		public List<MeshRenderer> DirtRenderers = new List<MeshRenderer>();

		// Token: 0x04003DCA RID: 15818
		[Header("Pot Settings")]
		public float PotRadius = 0.2f;

		// Token: 0x04003DCB RID: 15819
		[Range(0.2f, 2f)]
		public float YieldMultiplier = 1f;

		// Token: 0x04003DCC RID: 15820
		[Range(0.2f, 2f)]
		public float GrowSpeedMultiplier = 1f;

		// Token: 0x04003DCD RID: 15821
		[Range(0.2f, 2f)]
		public float MoistureDrainMultiplier = 1f;

		// Token: 0x04003DCE RID: 15822
		public bool AlignLeafDropToPlayer = true;

		// Token: 0x04003DCF RID: 15823
		[Header("Capacity Settings")]
		public float SoilCapacity = 20f;

		// Token: 0x04003DD0 RID: 15824
		public float WaterCapacity = 5f;

		// Token: 0x04003DD1 RID: 15825
		public float WaterDrainPerHour = 2f;

		// Token: 0x04003DD2 RID: 15826
		[Header("Dirt Settings")]
		[SerializeField]
		protected Vector3 DirtMinScale;

		// Token: 0x04003DD3 RID: 15827
		[SerializeField]
		protected Vector3 DirtMaxScale = Vector3.one;

		// Token: 0x04003DD4 RID: 15828
		[Header("Pour Target")]
		public Transform Target;

		// Token: 0x04003DD5 RID: 15829
		[Header("Lighting")]
		public UsableLightSource LightSourceOverride;

		// Token: 0x04003DDB RID: 15835
		public List<Additive> AppliedAdditives;

		// Token: 0x04003DE5 RID: 15845
		private bool intObjSetThisFrame;

		// Token: 0x04003DE6 RID: 15846
		private ItemSlot outputSlot;

		// Token: 0x04003DE7 RID: 15847
		private float rotation;

		// Token: 0x04003DE8 RID: 15848
		private bool rotationOverridden;

		// Token: 0x04003DE9 RID: 15849
		private SoilDefinition appliedSoilDefinition;

		// Token: 0x04003DEA RID: 15850
		public SyncVar<float> syncVar___<SoilLevel>k__BackingField;

		// Token: 0x04003DEB RID: 15851
		public SyncVar<string> syncVar___<SoilID>k__BackingField;

		// Token: 0x04003DEC RID: 15852
		public SyncVar<int> syncVar___<RemainingSoilUses>k__BackingField;

		// Token: 0x04003DED RID: 15853
		public SyncVar<float> syncVar___<WaterLevel>k__BackingField;

		// Token: 0x04003DEE RID: 15854
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003DEF RID: 15855
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003DF0 RID: 15856
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003DF1 RID: 15857
		private bool dll_Excuted;

		// Token: 0x04003DF2 RID: 15858
		private bool dll_Excuted;

		// Token: 0x02000BFB RID: 3067
		public enum ECameraPosition
		{
			// Token: 0x04003DF4 RID: 15860
			Closeup,
			// Token: 0x04003DF5 RID: 15861
			Midshot,
			// Token: 0x04003DF6 RID: 15862
			Fullshot,
			// Token: 0x04003DF7 RID: 15863
			BirdsEye
		}

		// Token: 0x02000BFC RID: 3068
		public enum ESoilState
		{
			// Token: 0x04003DF9 RID: 15865
			Flat,
			// Token: 0x04003DFA RID: 15866
			Parted,
			// Token: 0x04003DFB RID: 15867
			Packed
		}
	}
}
