using System;
using System.Collections.Generic;
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
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E5 RID: 2277
	public class StorageEntity : NetworkBehaviour, IItemSlotOwner
	{
		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06003D72 RID: 15730 RVA: 0x00102F50 File Offset: 0x00101150
		public bool IsOpened
		{
			get
			{
				return this.CurrentAccessor != null;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06003D73 RID: 15731 RVA: 0x00102F5E File Offset: 0x0010115E
		// (set) Token: 0x06003D74 RID: 15732 RVA: 0x00102F66 File Offset: 0x00101166
		public Player CurrentAccessor { get; protected set; }

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003D75 RID: 15733 RVA: 0x0006A298 File Offset: 0x00068498
		public int ItemCount
		{
			get
			{
				return ((IItemSlotOwner)this).GetTotalItemCount();
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003D76 RID: 15734 RVA: 0x00102F6F File Offset: 0x0010116F
		// (set) Token: 0x06003D77 RID: 15735 RVA: 0x00102F77 File Offset: 0x00101177
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x06003D78 RID: 15736 RVA: 0x00102F80 File Offset: 0x00101180
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.StorageEntity_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x00102FA0 File Offset: 0x001011A0
		protected virtual void Start()
		{
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			if (this.EmptyOnSleep)
			{
				TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.ClearContents));
			}
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00102FFC File Offset: 0x001011FC
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
				instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
			TimeManager.onSleepStart = (Action)Delegate.Remove(TimeManager.onSleepStart, new Action(this.ClearContents));
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00103058 File Offset: 0x00101258
		private void GetNetworth(MoneyManager.FloatContainer container)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null)
				{
					container.ChangeValue(this.ItemSlots[i].ItemInstance.GetMonetaryValue());
				}
			}
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x001030AA File Offset: 0x001012AA
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemSlotDataToClient(connection);
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x001030BC File Offset: 0x001012BC
		protected virtual void FixedUpdate()
		{
			if (this.IsOpened && this.CurrentAccessor == Player.Local && this.MaxAccessDistance > 0f && Vector3.Distance(PlayerSingleton<PlayerMovement>.Instance.transform.position, base.transform.position) > this.MaxAccessDistance + 1f)
			{
				this.Close();
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x00103124 File Offset: 0x00101324
		public Dictionary<StorableItemInstance, int> GetContentsDictionary()
		{
			Dictionary<StorableItemInstance, int> dictionary = new Dictionary<StorableItemInstance, int>();
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance is StorableItemInstance && this.ItemSlots[i].Quantity > 0 && !dictionary.ContainsKey(this.ItemSlots[i].ItemInstance as StorableItemInstance))
				{
					dictionary.Add(this.ItemSlots[i].ItemInstance as StorableItemInstance, this.ItemSlots[i].Quantity);
				}
			}
			return dictionary;
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x001031DE File Offset: 0x001013DE
		public bool CanItemFit(ItemInstance item, int quantity = 1)
		{
			return this.HowManyCanFit(item) >= quantity;
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x001031F0 File Offset: 0x001013F0
		public int HowManyCanFit(ItemInstance item)
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (!this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
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

		// Token: 0x06003D81 RID: 15745 RVA: 0x0010329C File Offset: 0x0010149C
		public void InsertItem(ItemInstance item, bool network = true)
		{
			if (!this.CanItemFit(item, item.Quantity))
			{
				Console.LogWarning("StorageEntity InsertItem() called but CanItemFit() returned false", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (!this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
				{
					if (this.ItemSlots[i].ItemInstance == null)
					{
						num -= item.StackLimit;
						this.ItemSlots[i].SetStoredItem(item, !network);
						return;
					}
					if (this.ItemSlots[i].ItemInstance.CanStackWith(item, true))
					{
						int num2 = Mathf.Min(item.StackLimit - this.ItemSlots[i].ItemInstance.Quantity, num);
						num -= num2;
						this.ItemSlots[i].ChangeQuantity(-num2, network);
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x001033A0 File Offset: 0x001015A0
		protected virtual void ContentsChanged()
		{
			if (this.onContentsChanged != null)
			{
				this.onContentsChanged.Invoke();
			}
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x001033B8 File Offset: 0x001015B8
		public List<ItemInstance> GetAllItems()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null)
				{
					list.Add(this.ItemSlots[i].ItemInstance);
				}
			}
			return list;
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x0010340C File Offset: 0x0010160C
		public void LoadFromItemSet(ItemInstance[] items)
		{
			int num = 0;
			while (num < items.Length && num < this.ItemSlots.Count)
			{
				this.ItemSlots[num].SetStoredItem(items[num], false);
				num++;
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0010344C File Offset: 0x0010164C
		public void ClearContents()
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				this.ItemSlots[i].ClearStoredInstance(false);
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00103481 File Offset: 0x00101681
		public void Open()
		{
			if (!this.CanBeOpened())
			{
				Console.LogWarning("StorageEntity Open() called but CanBeOpened() returned false", null);
				return;
			}
			Singleton<StorageMenu>.Instance.Open(this);
			this.SendAccessor(Player.Local.NetworkObject);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x001034B2 File Offset: 0x001016B2
		public void Close()
		{
			if (Singleton<StorageMenu>.Instance.OpenedStorageEntity != this)
			{
				Console.LogWarning("StorageEntity Close() called but StorageMenu.Instance.OpenedStorageEntity != this", null);
				return;
			}
			Singleton<StorageMenu>.Instance.CloseMenu();
			this.SendAccessor(null);
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x001034E3 File Offset: 0x001016E3
		protected virtual void OnOpened()
		{
			if (this.onOpened != null)
			{
				this.onOpened.Invoke();
			}
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x001034F8 File Offset: 0x001016F8
		protected virtual void OnClosed()
		{
			if (this.onClosed != null)
			{
				this.onClosed.Invoke();
			}
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x0010350D File Offset: 0x0010170D
		public virtual bool CanBeOpened()
		{
			return !Singleton<ManagementClipboard>.Instance.IsEquipped && this.AccessSettings != StorageEntity.EAccessSettings.Closed && (this.AccessSettings != StorageEntity.EAccessSettings.SinglePlayerOnly || !(this.CurrentAccessor != null));
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00103541 File Offset: 0x00101741
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendAccessor(NetworkObject accessor)
		{
			this.RpcWriter___Server_SendAccessor_3323014238(accessor);
			this.RpcLogic___SendAccessor_3323014238(accessor);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x00103558 File Offset: 0x00101758
		[ObserversRpc(RunLocally = true)]
		private void SetAccessor(NetworkObject accessor)
		{
			this.RpcWriter___Observers_SetAccessor_3323014238(accessor);
			this.RpcLogic___SetAccessor_3323014238(accessor);
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x00103579 File Offset: 0x00101779
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x001035A0 File Offset: 0x001017A0
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

		// Token: 0x06003D8F RID: 15759 RVA: 0x001035FF File Offset: 0x001017FF
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x0010361D File Offset: 0x0010181D
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x0010363B File Offset: 0x0010183B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x00103674 File Offset: 0x00101874
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

		// Token: 0x06003D93 RID: 15763 RVA: 0x001036F3 File Offset: 0x001018F3
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0010371C File Offset: 0x0010191C
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

		// Token: 0x06003D96 RID: 15766 RVA: 0x001037D0 File Offset: 0x001019D0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendAccessor_3323014238));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetAccessor_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00103919 File Offset: 0x00101B19
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x0010392C File Offset: 0x00101B2C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0010393C File Offset: 0x00101B3C
		private void RpcWriter___Server_SendAccessor_3323014238(NetworkObject accessor)
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
			writer.WriteNetworkObject(accessor);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x001039E3 File Offset: 0x00101BE3
		private void RpcLogic___SendAccessor_3323014238(NetworkObject accessor)
		{
			this.SetAccessor(accessor);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x001039EC File Offset: 0x00101BEC
		private void RpcReader___Server_SendAccessor_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject accessor = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAccessor_3323014238(accessor);
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x00103A2C File Offset: 0x00101C2C
		private void RpcWriter___Observers_SetAccessor_3323014238(NetworkObject accessor)
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
			writer.WriteNetworkObject(accessor);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x00103AE4 File Offset: 0x00101CE4
		private void RpcLogic___SetAccessor_3323014238(NetworkObject accessor)
		{
			Player currentAccessor = this.CurrentAccessor;
			if (accessor != null)
			{
				this.CurrentAccessor = accessor.GetComponent<Player>();
			}
			else
			{
				this.CurrentAccessor = null;
			}
			if (this.CurrentAccessor != null && currentAccessor == null)
			{
				this.OnOpened();
			}
			if (this.CurrentAccessor == null && currentAccessor != null)
			{
				this.OnClosed();
			}
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00103B50 File Offset: 0x00101D50
		private void RpcReader___Observers_SetAccessor_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject accessor = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAccessor_3323014238(accessor);
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00103B8C File Offset: 0x00101D8C
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
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00103C52 File Offset: 0x00101E52
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00103C7C File Offset: 0x00101E7C
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

		// Token: 0x06003DA2 RID: 15778 RVA: 0x00103CE4 File Offset: 0x00101EE4
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
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00103DAC File Offset: 0x00101FAC
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00103DD8 File Offset: 0x00101FD8
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

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00103E2C File Offset: 0x0010202C
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
			base.SendTargetRpc(4U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00103EF4 File Offset: 0x001020F4
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

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00103F4C File Offset: 0x0010214C
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
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x0010400A File Offset: 0x0010220A
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00104014 File Offset: 0x00102214
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

		// Token: 0x06003DAA RID: 15786 RVA: 0x00104070 File Offset: 0x00102270
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
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x0010413D File Offset: 0x0010233D
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00104154 File Offset: 0x00102354
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

		// Token: 0x06003DAD RID: 15789 RVA: 0x001041AC File Offset: 0x001023AC
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
			base.SendServerRpc(7U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x0010428C File Offset: 0x0010248C
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x001042BC File Offset: 0x001024BC
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

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00104344 File Offset: 0x00102544
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
			base.SendTargetRpc(8U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00104425 File Offset: 0x00102625
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x00104454 File Offset: 0x00102654
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

		// Token: 0x06003DB3 RID: 15795 RVA: 0x001044D0 File Offset: 0x001026D0
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
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x001045B4 File Offset: 0x001027B4
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

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00104628 File Offset: 0x00102828
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
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x001046EE File Offset: 0x001028EE
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00104718 File Offset: 0x00102918
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

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00104780 File Offset: 0x00102980
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
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00104848 File Offset: 0x00102A48
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x00104860 File Offset: 0x00102A60
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

		// Token: 0x06003DBB RID: 15803 RVA: 0x001048B4 File Offset: 0x00102AB4
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
			base.SendTargetRpc(12U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x0010497C File Offset: 0x00102B7C
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

		// Token: 0x06003DBD RID: 15805 RVA: 0x001049D4 File Offset: 0x00102BD4
		protected virtual void dll()
		{
			for (int i = 0; i < this.SlotCount; i++)
			{
				ItemSlot itemSlot = new ItemSlot(this.SlotsAreFilterable);
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.ContentsChanged));
				itemSlot.SetSlotOwner(this);
			}
			new ItemSlotSiblingSet(this.ItemSlots);
		}

		// Token: 0x04002C00 RID: 11264
		public const int MAX_SLOTS = 20;

		// Token: 0x04002C02 RID: 11266
		[Header("Settings")]
		public string StorageEntityName = "Storage Entity";

		// Token: 0x04002C03 RID: 11267
		public string StorageEntitySubtitle = string.Empty;

		// Token: 0x04002C04 RID: 11268
		[Range(1f, 20f)]
		public int SlotCount = 5;

		// Token: 0x04002C05 RID: 11269
		public bool EmptyOnSleep;

		// Token: 0x04002C06 RID: 11270
		public bool SlotsAreFilterable;

		// Token: 0x04002C07 RID: 11271
		[Header("Display Settings")]
		[Tooltip("How many rows to enforce when display contents in StorageMenu")]
		[Range(1f, 5f)]
		public int DisplayRowCount = 1;

		// Token: 0x04002C08 RID: 11272
		[Header("Access Settings")]
		public StorageEntity.EAccessSettings AccessSettings = StorageEntity.EAccessSettings.Full;

		// Token: 0x04002C09 RID: 11273
		[Tooltip("If the distance between this StorageEntity and the player is greater than this, the StorageMenu will be closed.")]
		[Range(0f, 10f)]
		public float MaxAccessDistance = 6f;

		// Token: 0x04002C0B RID: 11275
		[Header("Events")]
		[Tooltip("Invoked when this StorageEntity is accessed in the StorageMenu")]
		public UnityEvent onOpened;

		// Token: 0x04002C0C RID: 11276
		[Tooltip("Invoked when the StorageMenu is closed.")]
		public UnityEvent onClosed;

		// Token: 0x04002C0D RID: 11277
		[Tooltip("Invoked when the contents change in any way. i.e. an item is added, removed, or the quantity of an item changes.")]
		public UnityEvent onContentsChanged;

		// Token: 0x04002C0E RID: 11278
		private bool dll_Excuted;

		// Token: 0x04002C0F RID: 11279
		private bool dll_Excuted;

		// Token: 0x020008E6 RID: 2278
		public enum EAccessSettings
		{
			// Token: 0x04002C11 RID: 11281
			Closed,
			// Token: 0x04002C12 RID: 11282
			SinglePlayerOnly,
			// Token: 0x04002C13 RID: 11283
			Full
		}
	}
}
