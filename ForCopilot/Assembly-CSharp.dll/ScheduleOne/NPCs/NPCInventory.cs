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
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000494 RID: 1172
	public class NPCInventory : NetworkBehaviour, IItemSlotOwner
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x0006A0FD File Offset: 0x000682FD
		// (set) Token: 0x0600181B RID: 6171 RVA: 0x0006A105 File Offset: 0x00068305
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x0600181C RID: 6172 RVA: 0x0006A110 File Offset: 0x00068310
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCInventory_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0006A12F File Offset: 0x0006832F
		protected virtual void Start()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnSleepStart));
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0006A169 File Offset: 0x00068369
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0006A182 File Offset: 0x00068382
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0006A1A8 File Offset: 0x000683A8
		protected virtual void OnSleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.ClearInventoryEachNight)
			{
				foreach (ItemSlot itemSlot in this.ItemSlots)
				{
					itemSlot.ClearStoredInstance(false);
				}
			}
			if (this.GetTotalItemCount() >= 3)
			{
				return;
			}
			if (this.RandomCash)
			{
				int num = UnityEngine.Random.Range(this.RandomCashMin, this.RandomCashMax);
				if (num > 0)
				{
					CashInstance cashInstance = NetworkSingleton<MoneyManager>.Instance.GetCashInstance((float)num);
					this.InsertItem(cashInstance, true);
				}
			}
			if (this.RandomItems)
			{
				int num2 = UnityEngine.Random.Range(this.RandomItemMin, this.RandomItemMax + 1);
				for (int i = 0; i < num2; i++)
				{
					ItemInstance defaultInstance = this.RandomItemDefinitions[UnityEngine.Random.Range(0, this.RandomItemDefinitions.Length)].GetDefaultInstance(1);
					this.InsertItem(defaultInstance, true);
				}
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0006A298 File Offset: 0x00068498
		public int GetItemCount()
		{
			return ((IItemSlotOwner)this).GetTotalItemCount();
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0006A2A0 File Offset: 0x000684A0
		public int _GetItemAmount(string id)
		{
			return ((IItemSlotOwner)this).GetItemCount(id);
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0006A2AC File Offset: 0x000684AC
		public int GetIdenticalItemAmount(ItemInstance item)
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].Quantity != 0 && this.ItemSlots[i].ItemInstance.CanStackWith(item, false))
				{
					num += this.ItemSlots[i].Quantity;
				}
			}
			return num;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0006A314 File Offset: 0x00068514
		public int GetMaxItemCount(string[] ids)
		{
			int[] array = new int[ids.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((IItemSlotOwner)this).GetItemCount(ids[i]);
			}
			if (array.Length == 0)
			{
				return 0;
			}
			return array.Max();
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0006A350 File Offset: 0x00068550
		public bool CanItemFit(ItemInstance item)
		{
			return this.GetCapacityForItem(item) >= item.Quantity;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0006A364 File Offset: 0x00068564
		public int GetCapacityForItem(ItemInstance item)
		{
			if (item == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i] != null && !this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
				{
					if (this.ItemSlots[i].ItemInstance == null)
					{
						num += item.StackLimit;
					}
					else if (this.ItemSlots[i].ItemInstance.CanStackWith(item, true))
					{
						num += item.StackLimit - this.ItemSlots[i].ItemInstance.Quantity;
					}
				}
			}
			return num;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0006A424 File Offset: 0x00068624
		public void InsertItem(ItemInstance item, bool network = true)
		{
			if (!this.CanItemFit(item))
			{
				Console.LogWarning("StorageEntity InsertItem() called but CanItemFit() returned false", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (!this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
				{
					if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.CanStackWith(item, true))
					{
						int num2 = Mathf.Min(item.StackLimit - this.ItemSlots[i].ItemInstance.Quantity, num);
						num -= num2;
						this.ItemSlots[i].ChangeQuantity(num2, network);
					}
					if (num <= 0)
					{
						return;
					}
				}
			}
			for (int j = 0; j < this.ItemSlots.Count; j++)
			{
				if (!this.ItemSlots[j].IsLocked && !this.ItemSlots[j].IsAddLocked)
				{
					if (this.ItemSlots[j].ItemInstance == null)
					{
						num -= item.StackLimit;
						this.ItemSlots[j].SetStoredItem(item, !network);
						return;
					}
					if (num <= 0)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0006A574 File Offset: 0x00068774
		public ItemInstance GetFirstItem(string id, NPCInventory.ItemFilter filter = null)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.ID == id && (filter == null || filter(this.ItemSlots[i].ItemInstance)))
				{
					return this.ItemSlots[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0006A5F8 File Offset: 0x000687F8
		public ItemInstance GetFirstIdenticalItem(ItemInstance item, NPCInventory.ItemFilter filter = null)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.CanStackWith(item, false) && (filter == null || filter(this.ItemSlots[i].ItemInstance)))
				{
					return this.ItemSlots[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0006A677 File Offset: 0x00068877
		protected virtual void InventoryContentsChanged()
		{
			if (this.onContentsChanged != null)
			{
				this.onContentsChanged.Invoke();
			}
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0006A68C File Offset: 0x0006888C
		public int GetTotalItemCount()
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null)
				{
					num += this.ItemSlots[i].ItemInstance.Quantity;
				}
			}
			return num;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0006A6DE File Offset: 0x000688DE
		public void Hovered()
		{
			if (this.CanPickpocket())
			{
				this.PickpocketIntObj.SetMessage("Pickpocket");
				this.PickpocketIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.PickpocketIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0006A711 File Offset: 0x00068911
		public void Interacted()
		{
			if (this.CanPickpocket())
			{
				this.StartPickpocket();
			}
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0006A721 File Offset: 0x00068921
		private void StartPickpocket()
		{
			Singleton<PickpocketScreen>.Instance.Open(this.npc);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0006A733 File Offset: 0x00068933
		public void ExpirePickpocket()
		{
			this.timeOnLastExpire = Time.time;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0006A740 File Offset: 0x00068940
		private bool CanPickpocket()
		{
			return this.CanBePickpocketed && PlayerSingleton<PlayerMovement>.Instance.isCrouched && Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && Time.time - this.timeOnLastExpire >= 30f && this.npc.IsConscious && !this.npc.behaviour.CallPoliceBehaviour.Active && !this.npc.behaviour.CombatBehaviour.Active && !this.npc.behaviour.FacePlayerBehaviour.Active && !this.npc.behaviour.FleeBehaviour.Active && !this.npc.behaviour.GenericDialogueBehaviour.Active && !this.npc.behaviour.StationaryBehaviour.Active && !this.npc.behaviour.RequestProductBehaviour.Active && !GameManager.IS_TUTORIAL;
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0006A858 File Offset: 0x00068A58
		[Button]
		public void PrintInventoryContents()
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].Quantity != 0)
				{
					Console.Log(string.Concat(new string[]
					{
						"Slot ",
						i.ToString(),
						": ",
						this.ItemSlots[i].ItemInstance.Name,
						" x",
						this.ItemSlots[i].Quantity.ToString()
					}), null);
				}
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0006A8F9 File Offset: 0x00068AF9
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0006A920 File Offset: 0x00068B20
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

		// Token: 0x06001834 RID: 6196 RVA: 0x0006A97F File Offset: 0x00068B7F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0006A99D File Offset: 0x00068B9D
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0006A9BB File Offset: 0x00068BBB
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x0006A9F4 File Offset: 0x00068BF4
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

		// Token: 0x06001838 RID: 6200 RVA: 0x0006AA73 File Offset: 0x00068C73
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x0006AA9C File Offset: 0x00068C9C
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

		// Token: 0x0600183B RID: 6203 RVA: 0x0006AB60 File Offset: 0x00068D60
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0006AC7B File Offset: 0x00068E7B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0006AC8E File Offset: 0x00068E8E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0006AC9C File Offset: 0x00068E9C
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0006AD62 File Offset: 0x00068F62
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0006AD8C File Offset: 0x00068F8C
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

		// Token: 0x06001841 RID: 6209 RVA: 0x0006ADF4 File Offset: 0x00068FF4
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0006AEBC File Offset: 0x000690BC
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0006AEE8 File Offset: 0x000690E8
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

		// Token: 0x06001844 RID: 6212 RVA: 0x0006AF3C File Offset: 0x0006913C
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
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0006B004 File Offset: 0x00069204
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

		// Token: 0x06001846 RID: 6214 RVA: 0x0006B05C File Offset: 0x0006925C
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
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0006B11A File Offset: 0x0006931A
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0006B124 File Offset: 0x00069324
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

		// Token: 0x06001849 RID: 6217 RVA: 0x0006B180 File Offset: 0x00069380
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
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0006B24D File Offset: 0x0006944D
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0006B264 File Offset: 0x00069464
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

		// Token: 0x0600184C RID: 6220 RVA: 0x0006B2BC File Offset: 0x000694BC
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
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0006B39C File Offset: 0x0006959C
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0006B3CC File Offset: 0x000695CC
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

		// Token: 0x0600184F RID: 6223 RVA: 0x0006B454 File Offset: 0x00069654
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
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0006B535 File Offset: 0x00069735
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0006B564 File Offset: 0x00069764
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

		// Token: 0x06001852 RID: 6226 RVA: 0x0006B5E0 File Offset: 0x000697E0
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
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0006B6C4 File Offset: 0x000698C4
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

		// Token: 0x06001854 RID: 6228 RVA: 0x0006B738 File Offset: 0x00069938
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
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0006B7FE File Offset: 0x000699FE
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0006B828 File Offset: 0x00069A28
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

		// Token: 0x06001857 RID: 6231 RVA: 0x0006B890 File Offset: 0x00069A90
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
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0006B958 File Offset: 0x00069B58
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0006B970 File Offset: 0x00069B70
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

		// Token: 0x0600185A RID: 6234 RVA: 0x0006B9C4 File Offset: 0x00069BC4
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
			base.SendTargetRpc(10U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0006BA8C File Offset: 0x00069C8C
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

		// Token: 0x0600185C RID: 6236 RVA: 0x0006BAE4 File Offset: 0x00069CE4
		protected virtual void dll()
		{
			for (int i = 0; i < this.SlotCount; i++)
			{
				ItemSlot itemSlot = new ItemSlot();
				itemSlot.SetSlotOwner(this);
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.InventoryContentsChanged));
			}
			if (Application.isEditor)
			{
				ItemDefinition[] testItems = this.TestItems;
				for (int j = 0; j < testItems.Length; j++)
				{
					ItemInstance defaultInstance = testItems[j].GetDefaultInstance(1);
					this.InsertItem(defaultInstance, true);
				}
			}
			this.npc = base.GetComponent<NPC>();
			this.PickpocketIntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.PickpocketIntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x0400158B RID: 5515
		public InteractableObject PickpocketIntObj;

		// Token: 0x0400158C RID: 5516
		public const float COOLDOWN = 30f;

		// Token: 0x0400158D RID: 5517
		[Header("Settings")]
		public int SlotCount = 5;

		// Token: 0x0400158E RID: 5518
		public bool CanBePickpocketed = true;

		// Token: 0x0400158F RID: 5519
		public bool ClearInventoryEachNight = true;

		// Token: 0x04001590 RID: 5520
		public ItemDefinition[] TestItems;

		// Token: 0x04001591 RID: 5521
		[Header("Random cash")]
		public bool RandomCash = true;

		// Token: 0x04001592 RID: 5522
		public int RandomCashMin;

		// Token: 0x04001593 RID: 5523
		public int RandomCashMax = 100;

		// Token: 0x04001594 RID: 5524
		[Header("Random items")]
		public bool RandomItems = true;

		// Token: 0x04001595 RID: 5525
		public StorableItemDefinition[] RandomItemDefinitions;

		// Token: 0x04001596 RID: 5526
		public int RandomItemMin = -1;

		// Token: 0x04001597 RID: 5527
		public int RandomItemMax = 2;

		// Token: 0x04001598 RID: 5528
		private NPC npc;

		// Token: 0x0400159A RID: 5530
		public UnityEvent onContentsChanged;

		// Token: 0x0400159B RID: 5531
		private float timeOnLastExpire = -100f;

		// Token: 0x0400159C RID: 5532
		private bool dll_Excuted;

		// Token: 0x0400159D RID: 5533
		private bool dll_Excuted;

		// Token: 0x02000495 RID: 1173
		// (Invoke) Token: 0x0600185E RID: 6238
		public delegate bool ItemFilter(ItemInstance item);
	}
}
