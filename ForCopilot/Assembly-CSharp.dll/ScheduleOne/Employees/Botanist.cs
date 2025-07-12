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
using ScheduleOne.Dialogue;
using ScheduleOne.EntityFramework;
using ScheduleOne.Growing;
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
	// Token: 0x02000677 RID: 1655
	public class Botanist : Employee, IConfigurable
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x000B3D1D File Offset: 0x000B1F1D
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000B3D25 File Offset: 0x000B1F25
		// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000B3D2D File Offset: 0x000B1F2D
		protected BotanistConfiguration configuration { get; set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000B3D36 File Offset: 0x000B1F36
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x00053CFF File Offset: 0x00051EFF
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Botanist;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000B3D3E File Offset: 0x000B1F3E
		// (set) Token: 0x06002B85 RID: 11141 RVA: 0x000B3D46 File Offset: 0x000B1F46
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000B3D4F File Offset: 0x000B1F4F
		// (set) Token: 0x06002B87 RID: 11143 RVA: 0x000B3D57 File Offset: 0x000B1F57
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

		// Token: 0x06002B88 RID: 11144 RVA: 0x000B3D61 File Offset: 0x000B1F61
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x000B3D77 File Offset: 0x000B1F77
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000B3D87 File Offset: 0x000B1F87
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002B8C RID: 11148 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000B3D8F File Offset: 0x000B1F8F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000B3D97 File Offset: 0x000B1F97
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000B3DA0 File Offset: 0x000B1FA0
		protected override void UpdateBehaviour()
		{
			base.UpdateBehaviour();
			if (this.PotActionBehaviour.Active)
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
			if (this.configuration.AssignedPots.Count + this.configuration.AssignedRacks.Count == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned any pots or drying racks", "You can use your management clipboards to assign pots/drying racks to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Pot potForWatering = this.GetPotForWatering(this.CRITICAL_WATERING_THRESHOLD, true);
			if (potForWatering != null && NavMeshUtility.GetAccessPoint(potForWatering, this) != null)
			{
				this.StartAction(potForWatering, PotActionBehaviour.EActionType.Water);
				return;
			}
			Pot potForSoilSour = this.GetPotForSoilSour();
			if (potForSoilSour != null)
			{
				if (this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potForSoilSour, PotActionBehaviour.EActionType.PourSoil, -1))
				{
					this.StartAction(potForSoilSour, PotActionBehaviour.EActionType.PourSoil);
					return;
				}
				string fix = "Make sure there's soil in my supplies stash.";
				if (this.configuration.Supplies.SelectedObject == null)
				{
					fix = "Use your management clipboards to assign a supplies stash to me. Then make sure there's soil in it.";
				}
				base.SubmitNoWorkReason("There are empty pots, but I don't have any soil to pour.", fix, 0);
			}
			foreach (Pot pot in this.GetPotsReadyForSeed())
			{
				if (!this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, pot, PotActionBehaviour.EActionType.SowSeed, -1))
				{
					string fix2 = "Make sure I have the right seeds in my supplies stash.";
					if (this.configuration.Supplies.SelectedObject == null)
					{
						fix2 = "Use your management clipboards to assign a supplies stash to me, and make sure it contains the right seeds.";
					}
					base.SubmitNoWorkReason("There is a pot ready for sowing, but I don't have any seeds for it.", fix2, 1);
				}
				else if (NavMeshUtility.GetAccessPoint(pot, this))
				{
					this.StartAction(pot, PotActionBehaviour.EActionType.SowSeed);
					return;
				}
			}
			int additiveNumber;
			Pot potForAdditives = this.GetPotForAdditives(out additiveNumber);
			if (potForAdditives != null && this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potForAdditives, PotActionBehaviour.EActionType.ApplyAdditive, additiveNumber))
			{
				this.PotActionBehaviour.AdditiveNumber = additiveNumber;
				this.StartAction(potForAdditives, PotActionBehaviour.EActionType.ApplyAdditive);
				return;
			}
			foreach (Pot pot2 in this.GetPotsForHarvest())
			{
				if (this.IsEntityAccessible(pot2))
				{
					ItemInstance harvestedProduct = pot2.Plant.GetHarvestedProduct(pot2.Plant.ActiveHarvestables.Count);
					if (base.Inventory.CanItemFit(harvestedProduct))
					{
						if (this.PotActionBehaviour.DoesPotHaveValidDestination(pot2))
						{
							this.StartAction(pot2, PotActionBehaviour.EActionType.Harvest);
							return;
						}
						base.SubmitNoWorkReason("There is a plant ready for harvest, but it has no destination or it's destination is full.", "Use your management clipboard to assign a destination for each of my pots, and make sure the destination isn't full.", 0);
					}
				}
			}
			foreach (DryingRack dryingRack in this.GetRacksToStop())
			{
				if (this.IsEntityAccessible(dryingRack))
				{
					this.StopDryingRack(dryingRack);
					return;
				}
			}
			foreach (DryingRack dryingRack2 in this.GetRacksToStart())
			{
				if (this.IsEntityAccessible(dryingRack2))
				{
					this.StartDryingRack(dryingRack2);
					return;
				}
			}
			foreach (DryingRack dryingRack3 in this.GetRacksReadyToMove())
			{
				if (this.IsEntityAccessible(dryingRack3))
				{
					this.MoveItemBehaviour.Initialize((dryingRack3.Configuration as DryingRackConfiguration).DestinationRoute, dryingRack3.OutputSlot.ItemInstance, -1, false);
					this.MoveItemBehaviour.Enable_Networked(null);
					return;
				}
			}
			Pot potForWatering2 = this.GetPotForWatering(this.WATERING_THRESHOLD, false);
			if (potForWatering2 != null)
			{
				this.StartAction(potForWatering2, PotActionBehaviour.EActionType.Water);
				return;
			}
			QualityItemInstance qualityItemInstance;
			DryingRack destination;
			int maxMoveAmount;
			if (this.CanMoveDryableToRack(out qualityItemInstance, out destination, out maxMoveAmount))
			{
				TransitRoute route = new TransitRoute(this.configuration.Supplies.SelectedObject as ITransitEntity, destination);
				if (this.MoveItemBehaviour.IsTransitRouteValid(route, qualityItemInstance.ID))
				{
					this.MoveItemBehaviour.Initialize(route, qualityItemInstance, maxMoveAmount, false);
					this.MoveItemBehaviour.Enable_Networked(null);
					Console.Log(string.Concat(new string[]
					{
						"Moving ",
						maxMoveAmount.ToString(),
						" ",
						qualityItemInstance.ID,
						" to drying rack"
					}), null);
					return;
				}
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", string.Empty, 0);
			this.SetIdle(true);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000B424C File Offset: 0x000B244C
		private bool IsEntityAccessible(ITransitEntity entity)
		{
			return NavMeshUtility.GetAccessPoint(entity, this) != null;
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000B425B File Offset: 0x000B245B
		private void StartAction(Pot pot, PotActionBehaviour.EActionType actionType)
		{
			this.SetIdle(false);
			this.PotActionBehaviour.Initialize(pot, actionType);
			this.PotActionBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000B427D File Offset: 0x000B247D
		private void StartDryingRack(DryingRack rack)
		{
			this.StartDryingRackBehaviour.AssignRack(rack);
			this.StartDryingRackBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000B4297 File Offset: 0x000B2497
		private void StopDryingRack(DryingRack rack)
		{
			this.StopDryingRackBehaviour.AssignRack(rack);
			this.StopDryingRackBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000B42B1 File Offset: 0x000B24B1
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000B42C4 File Offset: 0x000B24C4
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Botanist.<>c__DisplayClass58_0 CS$<>8__locals1 = new Botanist.<>c__DisplayClass58_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000B4304 File Offset: 0x000B2504
		protected override void AssignProperty(Property prop, bool warp)
		{
			base.AssignProperty(prop, warp);
			prop.AddConfigurable(this);
			this.configuration = new BotanistConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000B432F File Offset: 0x000B252F
		protected override void UnassignProperty()
		{
			base.AssignedProperty.RemoveConfigurable(this);
			base.UnassignProperty();
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000B4343 File Offset: 0x000B2543
		protected override void ResetConfiguration()
		{
			if (this.configuration != null)
			{
				this.configuration.Reset();
			}
			base.ResetConfiguration();
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000B435E File Offset: 0x000B255E
		protected override void Fire()
		{
			if (this.configuration != null)
			{
				this.configuration.Destroy();
				this.DestroyWorldspaceUI();
			}
			base.Fire();
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000B4380 File Offset: 0x000B2580
		private bool CanMoveDryableToRack(out QualityItemInstance dryable, out DryingRack destinationRack, out int moveQuantity)
		{
			moveQuantity = 0;
			destinationRack = null;
			dryable = this.GetDryableInSupplies();
			if (dryable == null)
			{
				return false;
			}
			Console.Log("Found dryable in supplies: " + dryable.ID, null);
			int b = 0;
			destinationRack = this.GetAssignedDryingRackFor(dryable, out b);
			if (destinationRack == null)
			{
				return false;
			}
			Console.Log("Found rack with capacity: " + b.ToString(), null);
			moveQuantity = Mathf.Min(dryable.Quantity, b);
			return true;
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000B43FC File Offset: 0x000B25FC
		public QualityItemInstance GetDryableInSupplies()
		{
			if (this.configuration.Supplies.SelectedObject == null)
			{
				return null;
			}
			if (!this.PotActionBehaviour.CanGetToSupplies())
			{
				return null;
			}
			List<ItemSlot> list = new List<ItemSlot>();
			BuildableItem selectedObject = this.configuration.Supplies.SelectedObject;
			if (selectedObject != null)
			{
				list.AddRange((selectedObject as ITransitEntity).OutputSlots);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Quantity > 0 && ItemFilter_Dryable.IsItemDryable(list[i].ItemInstance))
				{
					return list[i].ItemInstance as QualityItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000B44AC File Offset: 0x000B26AC
		private DryingRack GetAssignedDryingRackFor(QualityItemInstance dryable, out int rackInputCapacity)
		{
			rackInputCapacity = 0;
			foreach (DryingRack dryingRack in this.configuration.AssignedRacks)
			{
				if ((dryingRack.Configuration as DryingRackConfiguration).TargetQuality.Value > dryable.Quality)
				{
					int inputCapacityForItem = ((ITransitEntity)dryingRack).GetInputCapacityForItem(dryable, this, true);
					if (inputCapacityForItem > 0)
					{
						rackInputCapacity = inputCapacityForItem;
						return dryingRack;
					}
				}
			}
			return null;
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000B4538 File Offset: 0x000B2738
		public ItemInstance GetItemInSupplies(string id)
		{
			if (this.configuration.Supplies.SelectedObject == null)
			{
				return null;
			}
			if (!this.PotActionBehaviour.CanGetToSupplies())
			{
				return null;
			}
			List<ItemSlot> list = new List<ItemSlot>();
			BuildableItem selectedObject = this.configuration.Supplies.SelectedObject;
			if (selectedObject != null)
			{
				list.AddRange((selectedObject as ITransitEntity).OutputSlots);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Quantity > 0 && list[i].ItemInstance.ID.ToLower() == id.ToLower())
				{
					return list[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000B45F0 File Offset: 0x000B27F0
		public ItemInstance GetSeedInSupplies()
		{
			if (this.configuration.Supplies.SelectedObject == null)
			{
				return null;
			}
			if (!this.PotActionBehaviour.CanGetToSupplies())
			{
				return null;
			}
			List<ItemSlot> list = new List<ItemSlot>();
			BuildableItem selectedObject = this.configuration.Supplies.SelectedObject;
			if (selectedObject != null)
			{
				list.AddRange((selectedObject as ITransitEntity).OutputSlots);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Quantity > 0 && list[i].ItemInstance.Definition is SeedDefinition)
				{
					return list[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000B469D File Offset: 0x000B289D
		protected override bool ShouldIdle()
		{
			return this.configuration.AssignedStations.SelectedObjects.Count == 0 || base.ShouldIdle();
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000B46BE File Offset: 0x000B28BE
		public override EmployeeHome GetHome()
		{
			return this.configuration.assignedHome;
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000B46CC File Offset: 0x000B28CC
		private bool AreThereUnspecifiedPots()
		{
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if ((this.configuration.AssignedPots[i].Configuration as PotConfiguration).Seed.SelectedItem == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000B4724 File Offset: 0x000B2924
		private bool AreThereNullDestinationPots()
		{
			foreach (Pot pot in this.configuration.AssignedPots)
			{
				string text;
				if (pot.IsReadyForHarvest(out text) && (pot.Configuration as PotConfiguration).Destination.SelectedObject == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000B47A4 File Offset: 0x000B29A4
		private bool IsMissingRequiredMaterials()
		{
			Pot potForSoilSour = this.GetPotForSoilSour();
			if (potForSoilSour != null && !this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potForSoilSour, PotActionBehaviour.EActionType.PourSoil, -1))
			{
				return false;
			}
			List<Pot> potsReadyForSeed = this.GetPotsReadyForSeed();
			for (int i = 0; i < potsReadyForSeed.Count; i++)
			{
				if (this.PotActionBehaviour.DoesBotanistHaveMaterialsForTask(this, potsReadyForSeed[i], PotActionBehaviour.EActionType.SowSeed, -1))
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000B4808 File Offset: 0x000B2A08
		private Pot GetPotForWatering(float threshold, bool excludeFullyGrowm)
		{
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotBeWatered(this.configuration.AssignedPots[i], threshold) && (!excludeFullyGrowm || this.configuration.AssignedPots[i].Plant == null || !this.configuration.AssignedPots[i].Plant.IsFullyGrown) && this.IsEntityAccessible(this.configuration.AssignedPots[i]))
				{
					return this.configuration.AssignedPots[i];
				}
			}
			return null;
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000B48C0 File Offset: 0x000B2AC0
		private Pot GetPotForSoilSour()
		{
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotHaveSoilPour(this.configuration.AssignedPots[i]) && this.IsEntityAccessible(this.configuration.AssignedPots[i]))
				{
					return this.configuration.AssignedPots[i];
				}
			}
			return null;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000B4934 File Offset: 0x000B2B34
		private List<Pot> GetPotsReadyForSeed()
		{
			List<Pot> list = new List<Pot>();
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotHaveSeedSown(this.configuration.AssignedPots[i]))
				{
					list.Add(this.configuration.AssignedPots[i]);
				}
			}
			return list;
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000B4998 File Offset: 0x000B2B98
		private T GetAccessableEntity<T>(T entity) where T : ITransitEntity
		{
			if (!(NavMeshUtility.GetAccessPoint(entity, this) != null))
			{
				return default(T);
			}
			return entity;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000B49C4 File Offset: 0x000B2BC4
		private List<T> GetAccessableEntities<T>(List<T> list) where T : ITransitEntity
		{
			return (from item in list
			where NavMeshUtility.GetAccessPoint(item, this) != null
			select item).ToList<T>();
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000B49E0 File Offset: 0x000B2BE0
		private List<Pot> FilterPotsForSpecifiedSeed(List<Pot> pots)
		{
			List<Pot> list = new List<Pot>();
			foreach (Pot pot in pots)
			{
				if ((pot.Configuration as PotConfiguration).Seed.SelectedItem != null)
				{
					list.Add(pot);
				}
			}
			return list;
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000B4A54 File Offset: 0x000B2C54
		private Pot GetPotForAdditives(out int additiveNumber)
		{
			additiveNumber = -1;
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotHaveAdditiveApplied(this.configuration.AssignedPots[i], out additiveNumber) && this.IsEntityAccessible(this.configuration.AssignedPots[i]))
				{
					return this.configuration.AssignedPots[i];
				}
			}
			return null;
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000B4ACC File Offset: 0x000B2CCC
		private List<Pot> GetPotsForHarvest()
		{
			List<Pot> list = new List<Pot>();
			for (int i = 0; i < this.configuration.AssignedPots.Count; i++)
			{
				if (this.PotActionBehaviour.CanPotBeHarvested(this.configuration.AssignedPots[i]))
				{
					list.Add(this.configuration.AssignedPots[i]);
				}
			}
			return list;
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000B4B30 File Offset: 0x000B2D30
		private List<DryingRack> GetRacksToStart()
		{
			List<DryingRack> list = new List<DryingRack>();
			for (int i = 0; i < this.configuration.AssignedRacks.Count; i++)
			{
				if (this.StartDryingRackBehaviour.IsRackReady(this.configuration.AssignedRacks[i]))
				{
					list.Add(this.configuration.AssignedRacks[i]);
				}
			}
			return list;
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000B4B94 File Offset: 0x000B2D94
		private List<DryingRack> GetRacksToStop()
		{
			List<DryingRack> list = new List<DryingRack>();
			for (int i = 0; i < this.configuration.AssignedRacks.Count; i++)
			{
				if (this.StopDryingRackBehaviour.IsRackReady(this.configuration.AssignedRacks[i]))
				{
					list.Add(this.configuration.AssignedRacks[i]);
				}
			}
			return list;
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000B4BF8 File Offset: 0x000B2DF8
		private List<DryingRack> GetRacksReadyToMove()
		{
			List<DryingRack> list = new List<DryingRack>();
			for (int i = 0; i < this.configuration.AssignedRacks.Count; i++)
			{
				ItemSlot outputSlot = this.configuration.AssignedRacks[i].OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((this.configuration.AssignedRacks[i].Configuration as DryingRackConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(this.configuration.AssignedRacks[i]);
				}
			}
			return list;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000B4C98 File Offset: 0x000B2E98
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
			BotanistUIElement component = UnityEngine.Object.Instantiate<BotanistUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<BotanistUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000B4D23 File Offset: 0x000B2F23
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000B4D40 File Offset: 0x000B2F40
		public override NPCData GetNPCData()
		{
			return new BotanistData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData());
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000B4DA8 File Offset: 0x000B2FA8
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			saveData.AddData("Configuration", this.Configuration.GetSaveString());
			return saveData;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000594B4 File Offset: 0x000576B4
		public override List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000B4E5C File Offset: 0x000B305C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(42U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Botanist));
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000B4ED4 File Offset: 0x000B30D4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.BotanistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000B4EF8 File Offset: 0x000B30F8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x000B4F08 File Offset: 0x000B3108
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

		// Token: 0x06002BBA RID: 11194 RVA: 0x000B4FAF File Offset: 0x000B31AF
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000B4FB8 File Offset: 0x000B31B8
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

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x000B4FF6 File Offset: 0x000B31F6
		// (set) Token: 0x06002BBD RID: 11197 RVA: 0x000B4FFE File Offset: 0x000B31FE
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

		// Token: 0x06002BBE RID: 11198 RVA: 0x000B503C File Offset: 0x000B323C
		public override bool Botanist(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x06002BBF RID: 11199 RVA: 0x000B508E File Offset: 0x000B328E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001F73 RID: 8051
		public float CRITICAL_WATERING_THRESHOLD = 0.1f;

		// Token: 0x04001F74 RID: 8052
		public float WATERING_THRESHOLD = 0.3f;

		// Token: 0x04001F75 RID: 8053
		public float TARGET_WATER_LEVEL_MIN = 0.75f;

		// Token: 0x04001F76 RID: 8054
		public float TARGET_WATER_LEVEL_MAX = 1f;

		// Token: 0x04001F77 RID: 8055
		public float SOIL_POUR_TIME = 10f;

		// Token: 0x04001F78 RID: 8056
		public float WATER_POUR_TIME = 10f;

		// Token: 0x04001F79 RID: 8057
		public float ADDITIVE_POUR_TIME = 10f;

		// Token: 0x04001F7A RID: 8058
		public float SEED_SOW_TIME = 15f;

		// Token: 0x04001F7B RID: 8059
		public float HARVEST_TIME = 15f;

		// Token: 0x04001F7C RID: 8060
		[Header("References")]
		public Sprite typeIcon;

		// Token: 0x04001F7D RID: 8061
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001F7E RID: 8062
		public PotActionBehaviour PotActionBehaviour;

		// Token: 0x04001F7F RID: 8063
		public StartDryingRackBehaviour StartDryingRackBehaviour;

		// Token: 0x04001F80 RID: 8064
		public StopDryingRackBehaviour StopDryingRackBehaviour;

		// Token: 0x04001F81 RID: 8065
		[Header("UI")]
		public BotanistUIElement WorldspaceUIPrefab;

		// Token: 0x04001F82 RID: 8066
		public Transform uiPoint;

		// Token: 0x04001F83 RID: 8067
		[Header("Settings")]
		public int MaxAssignedPots = 8;

		// Token: 0x04001F84 RID: 8068
		public DialogueContainer NoAssignedStationsDialogue;

		// Token: 0x04001F85 RID: 8069
		public DialogueContainer UnspecifiedPotsDialogue;

		// Token: 0x04001F86 RID: 8070
		public DialogueContainer NullDestinationPotsDialogue;

		// Token: 0x04001F87 RID: 8071
		public DialogueContainer MissingMaterialsDialogue;

		// Token: 0x04001F88 RID: 8072
		public DialogueContainer NoPotsRequireWorkDialogue;

		// Token: 0x04001F8C RID: 8076
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001F8D RID: 8077
		private bool dll_Excuted;

		// Token: 0x04001F8E RID: 8078
		private bool dll_Excuted;
	}
}
