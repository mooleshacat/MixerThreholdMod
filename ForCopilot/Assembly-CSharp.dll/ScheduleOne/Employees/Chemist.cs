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
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Employees
{
	// Token: 0x0200067A RID: 1658
	public class Chemist : Employee, IConfigurable
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000B5142 File Offset: 0x000B3342
		public EntityConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002BCA RID: 11210 RVA: 0x000B514A File Offset: 0x000B334A
		// (set) Token: 0x06002BCB RID: 11211 RVA: 0x000B5152 File Offset: 0x000B3352
		protected ChemistConfiguration configuration { get; set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002BCC RID: 11212 RVA: 0x000B515B File Offset: 0x000B335B
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000B5163 File Offset: 0x000B3363
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Chemist;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000B5166 File Offset: 0x000B3366
		// (set) Token: 0x06002BCF RID: 11215 RVA: 0x000B516E File Offset: 0x000B336E
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x000B5177 File Offset: 0x000B3377
		// (set) Token: 0x06002BD1 RID: 11217 RVA: 0x000B517F File Offset: 0x000B337F
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

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000B5189 File Offset: 0x000B3389
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x000B519F File Offset: 0x000B339F
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x000B51A7 File Offset: 0x000B33A7
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x000B3D8F File Offset: 0x000B1F8F
		public Property ParentProperty
		{
			get
			{
				return base.AssignedProperty;
			}
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000B51AF File Offset: 0x000B33AF
		protected override void AssignProperty(Property prop, bool warp)
		{
			base.AssignProperty(prop, warp);
			prop.AddConfigurable(this);
			this.configuration = new ChemistConfiguration(this.configReplicator, this, this);
			this.CreateWorldspaceUI();
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000B432F File Offset: 0x000B252F
		protected override void UnassignProperty()
		{
			base.AssignedProperty.RemoveConfigurable(this);
			base.UnassignProperty();
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000B51DA File Offset: 0x000B33DA
		protected override void ResetConfiguration()
		{
			if (this.configuration != null)
			{
				this.configuration.Reset();
			}
			base.ResetConfiguration();
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000B51F5 File Offset: 0x000B33F5
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

		// Token: 0x06002BDC RID: 11228 RVA: 0x000B5230 File Offset: 0x000B3430
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000B5240 File Offset: 0x000B3440
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Chemist.<>c__DisplayClass44_0 CS$<>8__locals1 = new Chemist.<>c__DisplayClass44_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000B5280 File Offset: 0x000B3480
		protected override void UpdateBehaviour()
		{
			base.UpdateBehaviour();
			if (!InstanceFinder.IsServer)
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
			if (this.configuration.TotalStations == 0)
			{
				base.SubmitNoWorkReason("I haven't been assigned any stations", "You can use your management clipboards to assign stations to me.", 0);
				this.SetIdle(true);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				this.TryStartNewTask();
			}
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000B52F8 File Offset: 0x000B34F8
		private void TryStartNewTask()
		{
			List<LabOven> labOvensReadyToFinish = this.GetLabOvensReadyToFinish();
			if (labOvensReadyToFinish.Count > 0)
			{
				this.FinishLabOven(labOvensReadyToFinish[0]);
				return;
			}
			List<LabOven> labOvensReadyToStart = this.GetLabOvensReadyToStart();
			if (labOvensReadyToStart.Count > 0)
			{
				this.StartLabOven(labOvensReadyToStart[0]);
				return;
			}
			List<ChemistryStation> chemistryStationsReadyToStart = this.GetChemistryStationsReadyToStart();
			if (chemistryStationsReadyToStart.Count > 0)
			{
				this.StartChemistryStation(chemistryStationsReadyToStart[0]);
				return;
			}
			List<Cauldron> cauldronsReadyToStart = this.GetCauldronsReadyToStart();
			if (cauldronsReadyToStart.Count > 0)
			{
				this.StartCauldron(cauldronsReadyToStart[0]);
				return;
			}
			List<MixingStation> mixingStationsReadyToStart = this.GetMixingStationsReadyToStart();
			if (mixingStationsReadyToStart.Count > 0)
			{
				this.StartMixingStation(mixingStationsReadyToStart[0]);
				return;
			}
			List<LabOven> labOvensReadyToMove = this.GetLabOvensReadyToMove();
			if (labOvensReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((labOvensReadyToMove[0].Configuration as LabOvenConfiguration).DestinationRoute, labOvensReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			List<ChemistryStation> chemStationsReadyToMove = this.GetChemStationsReadyToMove();
			if (chemStationsReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((chemStationsReadyToMove[0].Configuration as ChemistryStationConfiguration).DestinationRoute, chemStationsReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			List<Cauldron> cauldronsReadyToMove = this.GetCauldronsReadyToMove();
			if (cauldronsReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((cauldronsReadyToMove[0].Configuration as CauldronConfiguration).DestinationRoute, cauldronsReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			List<MixingStation> mixStationsReadyToMove = this.GetMixStationsReadyToMove();
			if (mixStationsReadyToMove.Count > 0)
			{
				this.MoveItemBehaviour.Initialize((mixStationsReadyToMove[0].Configuration as MixingStationConfiguration).DestinationRoute, mixStationsReadyToMove[0].OutputSlot.ItemInstance, -1, false);
				this.MoveItemBehaviour.Enable_Networked(null);
				return;
			}
			base.SubmitNoWorkReason("There's nothing for me to do right now.", string.Empty, 0);
			this.SetIdle(true);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000B550C File Offset: 0x000B370C
		private bool AnyWorkInProgress()
		{
			return this.StartChemistryStationBehaviour.Active || this.StartLabOvenBehaviour.Active || this.FinishLabOvenBehaviour.Active || this.MoveItemBehaviour.Active || this.StartMixingStationBehaviour.Active;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000B5565 File Offset: 0x000B3765
		protected override bool ShouldIdle()
		{
			return this.configuration.Stations.SelectedObjects.Count == 0 || base.ShouldIdle();
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000B5586 File Offset: 0x000B3786
		private void StartChemistryStation(ChemistryStation station)
		{
			this.StartChemistryStationBehaviour.SetTargetStation(station);
			this.StartChemistryStationBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000B55A0 File Offset: 0x000B37A0
		private void StartCauldron(Cauldron cauldron)
		{
			this.StartCauldronBehaviour.AssignStation(cauldron);
			this.StartCauldronBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000B55BA File Offset: 0x000B37BA
		private void StartLabOven(LabOven oven)
		{
			this.StartLabOvenBehaviour.SetTargetOven(oven);
			this.StartLabOvenBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000B55D4 File Offset: 0x000B37D4
		private void FinishLabOven(LabOven oven)
		{
			this.FinishLabOvenBehaviour.SetTargetOven(oven);
			this.FinishLabOvenBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000B55EE File Offset: 0x000B37EE
		private void StartMixingStation(MixingStation station)
		{
			this.StartMixingStationBehaviour.AssignStation(station);
			this.StartMixingStationBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000B5608 File Offset: 0x000B3808
		public override EmployeeHome GetHome()
		{
			return this.configuration.assignedHome;
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000B5618 File Offset: 0x000B3818
		public List<LabOven> GetLabOvensReadyToFinish()
		{
			List<LabOven> list = new List<LabOven>();
			foreach (LabOven labOven in this.configuration.LabOvens)
			{
				if (!((IUsable)labOven).IsInUse && labOven.CurrentOperation != null && labOven.IsReadyForHarvest() && labOven.CanOutputSpaceFitCurrentOperation())
				{
					list.Add(labOven);
				}
			}
			return list;
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000B5698 File Offset: 0x000B3898
		public List<LabOven> GetLabOvensReadyToStart()
		{
			List<LabOven> list = new List<LabOven>();
			foreach (LabOven labOven in this.configuration.LabOvens)
			{
				if (!((IUsable)labOven).IsInUse && labOven.CurrentOperation == null && labOven.IsReadyToStart())
				{
					list.Add(labOven);
				}
			}
			return list;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000B5710 File Offset: 0x000B3910
		public List<ChemistryStation> GetChemistryStationsReadyToStart()
		{
			List<ChemistryStation> list = new List<ChemistryStation>();
			foreach (ChemistryStation chemistryStation in this.configuration.ChemStations)
			{
				if (!((IUsable)chemistryStation).IsInUse && chemistryStation.CurrentCookOperation == null)
				{
					StationRecipe selectedRecipe = (chemistryStation.Configuration as ChemistryStationConfiguration).Recipe.SelectedRecipe;
					if (!(selectedRecipe == null) && chemistryStation.HasIngredientsForRecipe(selectedRecipe))
					{
						list.Add(chemistryStation);
					}
				}
			}
			return list;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000B57A8 File Offset: 0x000B39A8
		public List<Cauldron> GetCauldronsReadyToStart()
		{
			List<Cauldron> list = new List<Cauldron>();
			foreach (Cauldron cauldron in this.configuration.Cauldrons)
			{
				if (!((IUsable)cauldron).IsInUse && cauldron.RemainingCookTime <= 0 && cauldron.GetState() == Cauldron.EState.Ready)
				{
					list.Add(cauldron);
				}
			}
			return list;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000B5824 File Offset: 0x000B3A24
		public List<MixingStation> GetMixingStationsReadyToStart()
		{
			List<MixingStation> list = new List<MixingStation>();
			foreach (MixingStation mixingStation in this.configuration.MixStations)
			{
				if (!((IUsable)mixingStation).IsInUse && mixingStation.CanStartMix() && mixingStation.CurrentMixOperation == null && (float)mixingStation.GetMixQuantity() >= (mixingStation.Configuration as MixingStationConfiguration).StartThrehold.Value)
				{
					list.Add(mixingStation);
				}
			}
			return list;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000B58BC File Offset: 0x000B3ABC
		protected List<LabOven> GetLabOvensReadyToMove()
		{
			List<LabOven> list = new List<LabOven>();
			foreach (LabOven labOven in this.configuration.LabOvens)
			{
				ItemSlot outputSlot = labOven.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((labOven.Configuration as LabOvenConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(labOven);
				}
			}
			return list;
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000B5954 File Offset: 0x000B3B54
		protected List<ChemistryStation> GetChemStationsReadyToMove()
		{
			List<ChemistryStation> list = new List<ChemistryStation>();
			foreach (ChemistryStation chemistryStation in this.configuration.ChemStations)
			{
				ItemSlot outputSlot = chemistryStation.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((chemistryStation.Configuration as ChemistryStationConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(chemistryStation);
				}
			}
			return list;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000B59EC File Offset: 0x000B3BEC
		protected List<Cauldron> GetCauldronsReadyToMove()
		{
			List<Cauldron> list = new List<Cauldron>();
			foreach (Cauldron cauldron in this.configuration.Cauldrons)
			{
				ItemSlot outputSlot = cauldron.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((cauldron.Configuration as CauldronConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(cauldron);
				}
			}
			return list;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000B5A84 File Offset: 0x000B3C84
		protected List<MixingStation> GetMixStationsReadyToMove()
		{
			List<MixingStation> list = new List<MixingStation>();
			foreach (MixingStation mixingStation in this.configuration.MixStations)
			{
				ItemSlot outputSlot = mixingStation.OutputSlot;
				if (outputSlot.Quantity != 0 && this.MoveItemBehaviour.IsTransitRouteValid((mixingStation.Configuration as MixingStationConfiguration).DestinationRoute, outputSlot.ItemInstance.ID))
				{
					list.Add(mixingStation);
				}
			}
			return list;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000B5B1C File Offset: 0x000B3D1C
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
			ChemistUIElement component = UnityEngine.Object.Instantiate<ChemistUIElement>(this.WorldspaceUIPrefab, assignedProperty.WorldspaceUIContainer).GetComponent<ChemistUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000B5BA7 File Offset: 0x000B3DA7
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000B5BC4 File Offset: 0x000B3DC4
		public override NPCData GetNPCData()
		{
			return new ChemistData(this.ID, base.AssignedProperty.PropertyCode, this.FirstName, this.LastName, base.IsMale, base.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, base.PaidForToday, this.MoveItemBehaviour.GetSaveData());
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000B5C2C File Offset: 0x000B3E2C
		public override DynamicSaveData GetSaveData()
		{
			DynamicSaveData saveData = base.GetSaveData();
			saveData.AddData("Configuration", this.Configuration.GetSaveString());
			return saveData;
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000594B4 File Offset: 0x000576B4
		public override List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000B5C54 File Offset: 0x000B3E54
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentPlayerConfigurer>k__BackingField);
			base.RegisterServerRpc(42U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Chemist));
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000B5CCC File Offset: 0x000B3ECC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.ChemistAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000B5CF0 File Offset: 0x000B3EF0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000B5D00 File Offset: 0x000B3F00
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

		// Token: 0x06002BFB RID: 11259 RVA: 0x000B5DA7 File Offset: 0x000B3FA7
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000B5DB0 File Offset: 0x000B3FB0
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

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002BFD RID: 11261 RVA: 0x000B5DEE File Offset: 0x000B3FEE
		// (set) Token: 0x06002BFE RID: 11262 RVA: 0x000B5DF6 File Offset: 0x000B3FF6
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

		// Token: 0x06002BFF RID: 11263 RVA: 0x000B5E34 File Offset: 0x000B4034
		public override bool Chemist(PooledReader PooledReader0, uint UInt321, bool Boolean2)
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

		// Token: 0x06002C00 RID: 11264 RVA: 0x000B5E86 File Offset: 0x000B4086
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001F94 RID: 8084
		public const int MAX_ASSIGNED_STATIONS = 4;

		// Token: 0x04001F95 RID: 8085
		[Header("References")]
		public Sprite typeIcon;

		// Token: 0x04001F96 RID: 8086
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04001F97 RID: 8087
		[Header("Behaviours")]
		public StartChemistryStationBehaviour StartChemistryStationBehaviour;

		// Token: 0x04001F98 RID: 8088
		public StartLabOvenBehaviour StartLabOvenBehaviour;

		// Token: 0x04001F99 RID: 8089
		public FinishLabOvenBehaviour FinishLabOvenBehaviour;

		// Token: 0x04001F9A RID: 8090
		public StartCauldronBehaviour StartCauldronBehaviour;

		// Token: 0x04001F9B RID: 8091
		public StartMixingStationBehaviour StartMixingStationBehaviour;

		// Token: 0x04001F9C RID: 8092
		[Header("UI")]
		public ChemistUIElement WorldspaceUIPrefab;

		// Token: 0x04001F9D RID: 8093
		public Transform uiPoint;

		// Token: 0x04001FA1 RID: 8097
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04001FA2 RID: 8098
		private bool dll_Excuted;

		// Token: 0x04001FA3 RID: 8099
		private bool dll_Excuted;
	}
}
