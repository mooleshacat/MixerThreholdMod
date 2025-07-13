using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Product;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x020002ED RID: 749
	public class QuestManager : NetworkSingleton<QuestManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x000497E0 File Offset: 0x000479E0
		public string SaveFolderName
		{
			get
			{
				return "Quests";
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x000497E0 File Offset: 0x000479E0
		public string SaveFileName
		{
			get
			{
				return "Quests";
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x000497E7 File Offset: 0x000479E7
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x000497EF File Offset: 0x000479EF
		// (set) Token: 0x0600109E RID: 4254 RVA: 0x000497F7 File Offset: 0x000479F7
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x00049800 File Offset: 0x00047A00
		// (set) Token: 0x060010A0 RID: 4256 RVA: 0x00049808 File Offset: 0x00047A08
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00049811 File Offset: 0x00047A11
		// (set) Token: 0x060010A2 RID: 4258 RVA: 0x00049819 File Offset: 0x00047A19
		public bool HasChanged { get; set; } = true;

		// Token: 0x060010A3 RID: 4259 RVA: 0x00049824 File Offset: 0x00047A24
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Quests.QuestManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00049843 File Offset: 0x00047A43
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00049860 File Offset: 0x00047A60
		public override void OnSpawnServer(NetworkConnection connection)
		{
			QuestManager.<>c__DisplayClass33_0 CS$<>8__locals1 = new QuestManager.<>c__DisplayClass33_0();
			CS$<>8__locals1.connection = connection;
			CS$<>8__locals1.<>4__this = this;
			base.OnSpawnServer(CS$<>8__locals1.connection);
			if (CS$<>8__locals1.connection.IsLocalClient)
			{
				return;
			}
			base.StartCoroutine(CS$<>8__locals1.<OnSpawnServer>g__SendQuestStuff|0());
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x000498A8 File Offset: 0x00047AA8
		private void UpdateVariables()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Active_Contract_Count", Contract.Contracts.Count.ToString(), true);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000498E0 File Offset: 0x00047AE0
		[ServerRpc(RequireOwnership = false)]
		public void SendContractAccepted(NetworkObject customer, ContractInfo contractData, bool track, string guid)
		{
			this.RpcWriter___Server_SendContractAccepted_1030683829(customer, contractData, track, guid);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00049904 File Offset: 0x00047B04
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void CreateContract_Networked(NetworkConnection conn, string guid, bool tracked, NetworkObject customer, ContractInfo contractData, GameDateTime expiry, GameDateTime acceptTime, NetworkObject dealerObj = null)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateContract_Networked_1113640585(conn, guid, tracked, customer, contractData, expiry, acceptTime, dealerObj);
				this.RpcLogic___CreateContract_Networked_1113640585(conn, guid, tracked, customer, contractData, expiry, acceptTime, dealerObj);
			}
			else
			{
				this.RpcWriter___Target_CreateContract_Networked_1113640585(conn, guid, tracked, customer, contractData, expiry, acceptTime, dealerObj);
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00049990 File Offset: 0x00047B90
		public Contract CreateContract_Local(string title, string description, QuestEntryData[] entries, string guid, bool tracked, NetworkObject customer, float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, bool expires, GameDateTime expiry, int pickupScheduleIndex, GameDateTime acceptTime, Dealer dealer = null)
		{
			Contract component = UnityEngine.Object.Instantiate<GameObject>(this.ContractPrefab.gameObject, this.ContractContainer).GetComponent<Contract>();
			component.InitializeContract(title, description, entries, guid, customer, payment, products, deliveryLocationGUID, deliveryWindow, pickupScheduleIndex, acceptTime);
			component.Entries[0].PoILocation = component.DeliveryLocation.CustomerStandPoint;
			component.Entries[0].CreatePoI();
			if (tracked)
			{
				component.SetIsTracked(true);
			}
			if (expires)
			{
				component.ConfigureExpiry(true, expiry);
			}
			if (dealer != null)
			{
				component.SetDealer(dealer);
			}
			component.Begin(false);
			return component;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00049A31 File Offset: 0x00047C31
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendQuestAction(string guid, QuestManager.EQuestAction action)
		{
			this.RpcWriter___Server_SendQuestAction_2848227116(guid, action);
			this.RpcLogic___SendQuestAction_2848227116(guid, action);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00049A50 File Offset: 0x00047C50
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceiveQuestAction(NetworkConnection conn, string guid, QuestManager.EQuestAction action)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveQuestAction_920727549(conn, guid, action);
				this.RpcLogic___ReceiveQuestAction_920727549(conn, guid, action);
			}
			else
			{
				this.RpcWriter___Target_ReceiveQuestAction_920727549(conn, guid, action);
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00049A9D File Offset: 0x00047C9D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendQuestState(string guid, EQuestState state)
		{
			this.RpcWriter___Server_SendQuestState_4117703421(guid, state);
			this.RpcLogic___SendQuestState_4117703421(guid, state);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00049ABC File Offset: 0x00047CBC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceiveQuestState(NetworkConnection conn, string guid, EQuestState state)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveQuestState_3887376304(conn, guid, state);
				this.RpcLogic___ReceiveQuestState_3887376304(conn, guid, state);
			}
			else
			{
				this.RpcWriter___Target_ReceiveQuestState_3887376304(conn, guid, state);
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00049B0C File Offset: 0x00047D0C
		[TargetRpc]
		private void SetQuestTracked(NetworkConnection conn, string guid, bool tracked)
		{
			this.RpcWriter___Target_SetQuestTracked_619441887(conn, guid, tracked);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00049B2B File Offset: 0x00047D2B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendQuestEntryState(string guid, int entryIndex, EQuestState state)
		{
			this.RpcWriter___Server_SendQuestEntryState_375159588(guid, entryIndex, state);
			this.RpcLogic___SendQuestEntryState_375159588(guid, entryIndex, state);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00049B54 File Offset: 0x00047D54
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceiveQuestEntryState(NetworkConnection conn, string guid, int entryIndex, EQuestState state)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveQuestEntryState_311789429(conn, guid, entryIndex, state);
				this.RpcLogic___ReceiveQuestEntryState_311789429(conn, guid, entryIndex, state);
			}
			else
			{
				this.RpcWriter___Target_ReceiveQuestEntryState_311789429(conn, guid, entryIndex, state);
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00049BB0 File Offset: 0x00047DB0
		[Button]
		public void PrintQuestStates()
		{
			for (int i = 0; i < Quest.Quests.Count; i++)
			{
				Console.Log(Quest.Quests[i].GetQuestTitle() + " state: " + Quest.Quests[i].QuestState.ToString(), null);
			}
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00049C10 File Offset: 0x00047E10
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void CreateDeaddropCollectionQuest(NetworkConnection conn, string dropGUID, string guidString = "")
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateDeaddropCollectionQuest_3895153758(conn, dropGUID, guidString);
				this.RpcLogic___CreateDeaddropCollectionQuest_3895153758(conn, dropGUID, guidString);
			}
			else
			{
				this.RpcWriter___Target_CreateDeaddropCollectionQuest_3895153758(conn, dropGUID, guidString);
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00049C60 File Offset: 0x00047E60
		public DeaddropQuest CreateDeaddropCollectionQuest(string dropGUID, string guidString = "")
		{
			Guid guid = (guidString != "") ? new Guid(guidString) : GUIDManager.GenerateUniqueGUID();
			if (GUIDManager.IsGUIDAlreadyRegistered(guid))
			{
				return null;
			}
			DeadDrop @object = GUIDManager.GetObject<DeadDrop>(new Guid(dropGUID));
			if (@object == null)
			{
				Console.LogWarning("Failed to find dead drop with GUID: " + dropGUID, null);
				return null;
			}
			DeaddropQuest component = UnityEngine.Object.Instantiate<GameObject>(this.DeaddropCollectionPrefab.gameObject, this.QuestContainer).GetComponent<DeaddropQuest>();
			component.SetDrop(@object);
			component.Description = "Collect the dead drop " + @object.DeadDropDescription;
			component.SetGUID(guid);
			component.Entries[0].SetEntryTitle(@object.DeadDropName);
			component.Begin(true);
			return component;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00049D17 File Offset: 0x00047F17
		public void PlayCompleteQuestSound()
		{
			if (this.QuestEntryCompleteSound.isPlaying)
			{
				this.QuestEntryCompleteSound.Stop();
			}
			this.QuestCompleteSound.Play();
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00049D3C File Offset: 0x00047F3C
		public void PlayCompleteQuestEntrySound()
		{
			this.QuestEntryCompleteSound.Play();
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00049D4C File Offset: 0x00047F4C
		public virtual string GetSaveString()
		{
			List<QuestData> list = new List<QuestData>();
			List<ContractData> list2 = new List<ContractData>();
			List<DeaddropQuestData> list3 = new List<DeaddropQuestData>();
			for (int i = 0; i < Quest.Quests.Count; i++)
			{
				if (!(Quest.Quests[i] is Contract))
				{
					list.Add(Quest.Quests[i].GetSaveData() as QuestData);
				}
			}
			for (int j = 0; j < Contract.Contracts.Count; j++)
			{
				list2.Add(Contract.Contracts[j].GetSaveData() as ContractData);
			}
			for (int k = 0; k < DeaddropQuest.DeaddropQuests.Count; k++)
			{
				list3.Add(DeaddropQuest.DeaddropQuests[k].GetSaveData() as DeaddropQuestData);
			}
			return new QuestManagerData(list.ToArray(), list2.ToArray(), list3.ToArray()).GetJson(true);
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00049E64 File Offset: 0x00048064
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Quests.QuestManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Quests.QuestManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendContractAccepted_1030683829));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_CreateContract_Networked_1113640585));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_CreateContract_Networked_1113640585));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendQuestAction_2848227116));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveQuestAction_920727549));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveQuestAction_920727549));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SendQuestState_4117703421));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveQuestState_3887376304));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveQuestState_3887376304));
			base.RegisterTargetRpc(9U, new ClientRpcDelegate(this.RpcReader___Target_SetQuestTracked_619441887));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendQuestEntryState_375159588));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveQuestEntryState_311789429));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveQuestEntryState_311789429));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_CreateDeaddropCollectionQuest_3895153758));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_CreateDeaddropCollectionQuest_3895153758));
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00049FE1 File Offset: 0x000481E1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Quests.QuestManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Quests.QuestManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00049FFA File Offset: 0x000481FA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0004A008 File Offset: 0x00048208
		private void RpcWriter___Server_SendContractAccepted_1030683829(NetworkObject customer, ContractInfo contractData, bool track, string guid)
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
			writer.WriteNetworkObject(customer);
			writer.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated(contractData);
			writer.WriteBoolean(track);
			writer.WriteString(guid);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0004A0D8 File Offset: 0x000482D8
		public void RpcLogic___SendContractAccepted_1030683829(NetworkObject customer, ContractInfo contractData, bool track, string guid)
		{
			GameDateTime expiry = default(GameDateTime);
			expiry.time = contractData.DeliveryWindow.WindowEndTime;
			expiry.elapsedDays = NetworkSingleton<TimeManager>.Instance.ElapsedDays;
			if (NetworkSingleton<TimeManager>.Instance.CurrentTime > contractData.DeliveryWindow.WindowEndTime)
			{
				expiry.elapsedDays++;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Accepted_Contract_Count", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Accepted_Contract_Count") + 1f).ToString(), true);
			GameDateTime dateTime = NetworkSingleton<TimeManager>.Instance.GetDateTime();
			this.CreateContract_Networked(null, guid, track, customer, contractData, expiry, dateTime, null);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0004A17C File Offset: 0x0004837C
		private void RpcReader___Server_SendContractAccepted_1030683829(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject customer = PooledReader0.ReadNetworkObject();
			ContractInfo contractData = GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds(PooledReader0);
			bool track = PooledReader0.ReadBoolean();
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendContractAccepted_1030683829(customer, contractData, track, guid);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0004A1E0 File Offset: 0x000483E0
		private void RpcWriter___Observers_CreateContract_Networked_1113640585(NetworkConnection conn, string guid, bool tracked, NetworkObject customer, ContractInfo contractData, GameDateTime expiry, GameDateTime acceptTime, NetworkObject dealerObj = null)
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
			writer.WriteString(guid);
			writer.WriteBoolean(tracked);
			writer.WriteNetworkObject(customer);
			writer.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated(contractData);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(expiry);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(acceptTime);
			writer.WriteNetworkObject(dealerObj);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0004A2E4 File Offset: 0x000484E4
		public void RpcLogic___CreateContract_Networked_1113640585(NetworkConnection conn, string guid, bool tracked, NetworkObject customer, ContractInfo contractData, GameDateTime expiry, GameDateTime acceptTime, NetworkObject dealerObj = null)
		{
			if (GUIDManager.IsGUIDAlreadyRegistered(new Guid(guid)))
			{
				return;
			}
			DeliveryLocation @object = GUIDManager.GetObject<DeliveryLocation>(new Guid(contractData.DeliveryLocationGUID));
			QuestEntryData questEntryData = new QuestEntryData(contractData.Products.GetCommaSeperatedString() + ", " + @object.LocationName, EQuestState.Inactive);
			string nameAddress = customer.GetComponent<Customer>().NPC.GetNameAddress();
			string description = string.Concat(new string[]
			{
				nameAddress,
				" has requested a delivery of ",
				contractData.Products.GetCommaSeperatedString(),
				" ",
				@object.GetDescription(),
				" for ",
				MoneyManager.FormatAmount(contractData.Payment, false, false),
				"."
			});
			Dealer dealer = null;
			if (dealerObj != null)
			{
				dealer = dealerObj.GetComponent<Dealer>();
			}
			this.CreateContract_Local("Deal for " + nameAddress, description, new QuestEntryData[]
			{
				questEntryData
			}, guid, tracked, customer, contractData.Payment, contractData.Products, contractData.DeliveryLocationGUID, contractData.DeliveryWindow, contractData.Expires, expiry, contractData.PickupScheduleIndex, acceptTime, dealer);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0004A408 File Offset: 0x00048608
		private void RpcReader___Observers_CreateContract_Networked_1113640585(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			bool tracked = PooledReader0.ReadBoolean();
			NetworkObject customer = PooledReader0.ReadNetworkObject();
			ContractInfo contractData = GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds(PooledReader0);
			GameDateTime expiry = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(PooledReader0);
			GameDateTime acceptTime = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(PooledReader0);
			NetworkObject dealerObj = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateContract_Networked_1113640585(null, guid, tracked, customer, contractData, expiry, acceptTime, dealerObj);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0004A4AC File Offset: 0x000486AC
		private void RpcWriter___Target_CreateContract_Networked_1113640585(NetworkConnection conn, string guid, bool tracked, NetworkObject customer, ContractInfo contractData, GameDateTime expiry, GameDateTime acceptTime, NetworkObject dealerObj = null)
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
			writer.WriteString(guid);
			writer.WriteBoolean(tracked);
			writer.WriteNetworkObject(customer);
			writer.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated(contractData);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(expiry);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(acceptTime);
			writer.WriteNetworkObject(dealerObj);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0004A5B0 File Offset: 0x000487B0
		private void RpcReader___Target_CreateContract_Networked_1113640585(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			bool tracked = PooledReader0.ReadBoolean();
			NetworkObject customer = PooledReader0.ReadNetworkObject();
			ContractInfo contractData = GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds(PooledReader0);
			GameDateTime expiry = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(PooledReader0);
			GameDateTime acceptTime = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(PooledReader0);
			NetworkObject dealerObj = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateContract_Networked_1113640585(base.LocalConnection, guid, tracked, customer, contractData, expiry, acceptTime, dealerObj);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0004A650 File Offset: 0x00048850
		private void RpcWriter___Server_SendQuestAction_2848227116(string guid, QuestManager.EQuestAction action)
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
			writer.WriteString(guid);
			writer.Write___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generated(action);
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0004A704 File Offset: 0x00048904
		public void RpcLogic___SendQuestAction_2848227116(string guid, QuestManager.EQuestAction action)
		{
			this.ReceiveQuestAction(null, guid, action);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0004A710 File Offset: 0x00048910
		private void RpcReader___Server_SendQuestAction_2848227116(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			QuestManager.EQuestAction action = GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendQuestAction_2848227116(guid, action);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0004A760 File Offset: 0x00048960
		private void RpcWriter___Observers_ReceiveQuestAction_920727549(NetworkConnection conn, string guid, QuestManager.EQuestAction action)
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
			writer.WriteString(guid);
			writer.Write___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generated(action);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0004A824 File Offset: 0x00048A24
		private void RpcLogic___ReceiveQuestAction_920727549(NetworkConnection conn, string guid, QuestManager.EQuestAction action)
		{
			Quest @object = GUIDManager.GetObject<Quest>(new Guid(guid));
			if (@object == null)
			{
				Console.LogWarning("Failed to find quest with GUID: " + guid, null);
				return;
			}
			switch (action)
			{
			case QuestManager.EQuestAction.Begin:
				@object.Begin(false);
				return;
			case QuestManager.EQuestAction.Success:
				@object.Complete(false);
				return;
			case QuestManager.EQuestAction.Fail:
				@object.Fail(false);
				return;
			case QuestManager.EQuestAction.Expire:
				@object.Expire(false);
				return;
			case QuestManager.EQuestAction.Cancel:
				@object.Cancel(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0004A89C File Offset: 0x00048A9C
		private void RpcReader___Observers_ReceiveQuestAction_920727549(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			QuestManager.EQuestAction action = GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveQuestAction_920727549(null, guid, action);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0004A8EC File Offset: 0x00048AEC
		private void RpcWriter___Target_ReceiveQuestAction_920727549(NetworkConnection conn, string guid, QuestManager.EQuestAction action)
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
			writer.WriteString(guid);
			writer.Write___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generated(action);
			base.SendTargetRpc(5U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0004A9B0 File Offset: 0x00048BB0
		private void RpcReader___Target_ReceiveQuestAction_920727549(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			QuestManager.EQuestAction action = GeneratedReaders___Internal.Read___ScheduleOne.Quests.QuestManager/EQuestActionFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveQuestAction_920727549(base.LocalConnection, guid, action);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0004A9F8 File Offset: 0x00048BF8
		private void RpcWriter___Server_SendQuestState_4117703421(string guid, EQuestState state)
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
			writer.WriteString(guid);
			writer.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated(state);
			base.SendServerRpc(6U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0004AAAC File Offset: 0x00048CAC
		public void RpcLogic___SendQuestState_4117703421(string guid, EQuestState state)
		{
			this.ReceiveQuestState(null, guid, state);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0004AAB8 File Offset: 0x00048CB8
		private void RpcReader___Server_SendQuestState_4117703421(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			EQuestState state = GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendQuestState_4117703421(guid, state);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0004AB08 File Offset: 0x00048D08
		private void RpcWriter___Observers_ReceiveQuestState_3887376304(NetworkConnection conn, string guid, EQuestState state)
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
			writer.WriteString(guid);
			writer.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated(state);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0004ABCC File Offset: 0x00048DCC
		private void RpcLogic___ReceiveQuestState_3887376304(NetworkConnection conn, string guid, EQuestState state)
		{
			Quest @object = GUIDManager.GetObject<Quest>(new Guid(guid));
			if (@object == null)
			{
				Console.LogWarning("Failed to find quest with GUID: " + guid, null);
				return;
			}
			@object.SetQuestState(state, false);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0004AC08 File Offset: 0x00048E08
		private void RpcReader___Observers_ReceiveQuestState_3887376304(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			EQuestState state = GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveQuestState_3887376304(null, guid, state);
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0004AC58 File Offset: 0x00048E58
		private void RpcWriter___Target_ReceiveQuestState_3887376304(NetworkConnection conn, string guid, EQuestState state)
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
			writer.WriteString(guid);
			writer.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated(state);
			base.SendTargetRpc(8U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0004AD1C File Offset: 0x00048F1C
		private void RpcReader___Target_ReceiveQuestState_3887376304(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			EQuestState state = GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveQuestState_3887376304(base.LocalConnection, guid, state);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0004AD64 File Offset: 0x00048F64
		private void RpcWriter___Target_SetQuestTracked_619441887(NetworkConnection conn, string guid, bool tracked)
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
			writer.WriteString(guid);
			writer.WriteBoolean(tracked);
			base.SendTargetRpc(9U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0004AE28 File Offset: 0x00049028
		private void RpcLogic___SetQuestTracked_619441887(NetworkConnection conn, string guid, bool tracked)
		{
			Quest @object = GUIDManager.GetObject<Quest>(new Guid(guid));
			if (@object == null)
			{
				Console.LogWarning("Failed to find quest with GUID: " + guid, null);
				return;
			}
			@object.SetIsTracked(tracked);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0004AE64 File Offset: 0x00049064
		private void RpcReader___Target_SetQuestTracked_619441887(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			bool tracked = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetQuestTracked_619441887(base.LocalConnection, guid, tracked);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0004AEAC File Offset: 0x000490AC
		private void RpcWriter___Server_SendQuestEntryState_375159588(string guid, int entryIndex, EQuestState state)
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
			writer.WriteString(guid);
			writer.WriteInt32(entryIndex, 1);
			writer.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated(state);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0004AF72 File Offset: 0x00049172
		public void RpcLogic___SendQuestEntryState_375159588(string guid, int entryIndex, EQuestState state)
		{
			this.ReceiveQuestEntryState(null, guid, entryIndex, state);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0004AF80 File Offset: 0x00049180
		private void RpcReader___Server_SendQuestEntryState_375159588(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			int entryIndex = PooledReader0.ReadInt32(1);
			EQuestState state = GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendQuestEntryState_375159588(guid, entryIndex, state);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0004AFE8 File Offset: 0x000491E8
		private void RpcWriter___Observers_ReceiveQuestEntryState_311789429(NetworkConnection conn, string guid, int entryIndex, EQuestState state)
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
			writer.WriteString(guid);
			writer.WriteInt32(entryIndex, 1);
			writer.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated(state);
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0004B0C0 File Offset: 0x000492C0
		private void RpcLogic___ReceiveQuestEntryState_311789429(NetworkConnection conn, string guid, int entryIndex, EQuestState state)
		{
			Quest @object = GUIDManager.GetObject<Quest>(new Guid(guid));
			if (@object == null)
			{
				Console.LogWarning("Failed to find quest with GUID: " + guid, null);
				return;
			}
			@object.SetQuestEntryState(entryIndex, state, false);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0004B100 File Offset: 0x00049300
		private void RpcReader___Observers_ReceiveQuestEntryState_311789429(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			int entryIndex = PooledReader0.ReadInt32(1);
			EQuestState state = GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveQuestEntryState_311789429(null, guid, entryIndex, state);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0004B164 File Offset: 0x00049364
		private void RpcWriter___Target_ReceiveQuestEntryState_311789429(NetworkConnection conn, string guid, int entryIndex, EQuestState state)
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
			writer.WriteString(guid);
			writer.WriteInt32(entryIndex, 1);
			writer.Write___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generated(state);
			base.SendTargetRpc(12U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0004B238 File Offset: 0x00049438
		private void RpcReader___Target_ReceiveQuestEntryState_311789429(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			int entryIndex = PooledReader0.ReadInt32(1);
			EQuestState state = GeneratedReaders___Internal.Read___ScheduleOne.Quests.EQuestStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveQuestEntryState_311789429(base.LocalConnection, guid, entryIndex, state);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0004B298 File Offset: 0x00049498
		private void RpcWriter___Observers_CreateDeaddropCollectionQuest_3895153758(NetworkConnection conn, string dropGUID, string guidString = "")
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
			writer.WriteString(dropGUID);
			writer.WriteString(guidString);
			base.SendObserversRpc(13U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0004B35B File Offset: 0x0004955B
		public void RpcLogic___CreateDeaddropCollectionQuest_3895153758(NetworkConnection conn, string dropGUID, string guidString = "")
		{
			this.CreateDeaddropCollectionQuest(dropGUID, guidString);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0004B368 File Offset: 0x00049568
		private void RpcReader___Observers_CreateDeaddropCollectionQuest_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string dropGUID = PooledReader0.ReadString();
			string guidString = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateDeaddropCollectionQuest_3895153758(null, dropGUID, guidString);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0004B3B8 File Offset: 0x000495B8
		private void RpcWriter___Target_CreateDeaddropCollectionQuest_3895153758(NetworkConnection conn, string dropGUID, string guidString = "")
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
			writer.WriteString(dropGUID);
			writer.WriteString(guidString);
			base.SendTargetRpc(14U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0004B47C File Offset: 0x0004967C
		private void RpcReader___Target_CreateDeaddropCollectionQuest_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string dropGUID = PooledReader0.ReadString();
			string guidString = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateDeaddropCollectionQuest_3895153758(base.LocalConnection, dropGUID, guidString);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0004B4C4 File Offset: 0x000496C4
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
			foreach (Quest quest in this.QuestContainer.GetComponentsInChildren<Quest>())
			{
				if (!this.DefaultQuests.Contains(quest))
				{
					Console.LogError("Quest " + quest.GetQuestTitle() + " is not in the default quests list!", null);
				}
			}
		}

		// Token: 0x040010E7 RID: 4327
		public const EQuestState DEFAULT_QUEST_STATE = EQuestState.Inactive;

		// Token: 0x040010E8 RID: 4328
		public Quest[] DefaultQuests;

		// Token: 0x040010E9 RID: 4329
		[Header("References")]
		public Transform QuestContainer;

		// Token: 0x040010EA RID: 4330
		public Transform ContractContainer;

		// Token: 0x040010EB RID: 4331
		public AudioSourceController QuestCompleteSound;

		// Token: 0x040010EC RID: 4332
		public AudioSourceController QuestEntryCompleteSound;

		// Token: 0x040010ED RID: 4333
		[Header("Prefabs")]
		public Contract ContractPrefab;

		// Token: 0x040010EE RID: 4334
		public DeaddropQuest DeaddropCollectionPrefab;

		// Token: 0x040010EF RID: 4335
		private QuestsLoader loader = new QuestsLoader();

		// Token: 0x040010F3 RID: 4339
		private bool dll_Excuted;

		// Token: 0x040010F4 RID: 4340
		private bool dll_Excuted;

		// Token: 0x020002EE RID: 750
		public enum EQuestAction
		{
			// Token: 0x040010F6 RID: 4342
			Begin,
			// Token: 0x040010F7 RID: 4343
			Success,
			// Token: 0x040010F8 RID: 4344
			Fail,
			// Token: 0x040010F9 RID: 4345
			Expire,
			// Token: 0x040010FA RID: 4346
			Cancel
		}
	}
}
