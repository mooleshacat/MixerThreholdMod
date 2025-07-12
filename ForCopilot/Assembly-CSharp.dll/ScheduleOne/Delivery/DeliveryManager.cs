using System;
using System.Collections.Generic;
using System.Linq;
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
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Property;
using ScheduleOne.UI.Phone.Delivery;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000753 RID: 1875
	public class DeliveryManager : NetworkSingleton<DeliveryManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x000D211D File Offset: 0x000D031D
		public string SaveFolderName
		{
			get
			{
				return "Deliveries";
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x000D211D File Offset: 0x000D031D
		public string SaveFileName
		{
			get
			{
				return "Deliveries";
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x000D2124 File Offset: 0x000D0324
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06003265 RID: 12901 RVA: 0x000D212C File Offset: 0x000D032C
		// (set) Token: 0x06003266 RID: 12902 RVA: 0x000D2134 File Offset: 0x000D0334
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x000D213D File Offset: 0x000D033D
		// (set) Token: 0x06003268 RID: 12904 RVA: 0x000D2145 File Offset: 0x000D0345
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x000D214E File Offset: 0x000D034E
		// (set) Token: 0x0600326A RID: 12906 RVA: 0x000D2156 File Offset: 0x000D0356
		public bool HasChanged { get; set; }

		// Token: 0x0600326B RID: 12907 RVA: 0x000D215F File Offset: 0x000D035F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Delivery.DeliveryManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x000D2173 File Offset: 0x000D0373
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.OnMinPass));
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000D21A4 File Offset: 0x000D03A4
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (DeliveryInstance delivery in this.Deliveries)
			{
				this.SendDelivery(delivery);
			}
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000D2200 File Offset: 0x000D0400
		private void OnMinPass()
		{
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			foreach (DeliveryInstance deliveryInstance in this.Deliveries.ToArray())
			{
				deliveryInstance.OnMinPass();
				if (InstanceFinder.IsServer)
				{
					if (deliveryInstance.TimeUntilArrival == 0 && deliveryInstance.Status != EDeliveryStatus.Arrived)
					{
						if (this.IsLoadingBayFree(deliveryInstance.Destination, deliveryInstance.LoadingDockIndex))
						{
							deliveryInstance.AddItemsToDeliveryVehicle();
							this.SetDeliveryState(deliveryInstance.DeliveryID, EDeliveryStatus.Arrived);
						}
						else if (deliveryInstance.Status != EDeliveryStatus.Waiting)
						{
							this.SetDeliveryState(deliveryInstance.DeliveryID, EDeliveryStatus.Waiting);
						}
					}
					if (deliveryInstance.Status == EDeliveryStatus.Arrived)
					{
						if (!this.minsSinceVehicleEmpty.ContainsKey(deliveryInstance))
						{
							this.minsSinceVehicleEmpty.Add(deliveryInstance, 0);
						}
						if (deliveryInstance.ActiveVehicle.Vehicle.Storage.ItemCount == 0 && deliveryInstance.ActiveVehicle.Vehicle.Storage.CurrentAccessor == null)
						{
							Dictionary<DeliveryInstance, int> dictionary = this.minsSinceVehicleEmpty;
							DeliveryInstance key = deliveryInstance;
							int num = dictionary[key];
							dictionary[key] = num + 1;
							if (this.minsSinceVehicleEmpty[deliveryInstance] >= 3)
							{
								this.SetDeliveryState(deliveryInstance.DeliveryID, EDeliveryStatus.Completed);
							}
						}
						else
						{
							this.minsSinceVehicleEmpty[deliveryInstance] = 0;
						}
					}
				}
			}
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000D233F File Offset: 0x000D053F
		public bool IsLoadingBayFree(Property destination, int loadingDockIndex)
		{
			return !destination.LoadingDocks[loadingDockIndex].IsInUse;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000D2351 File Offset: 0x000D0551
		[ServerRpc(RequireOwnership = false)]
		public void SendDelivery(DeliveryInstance delivery)
		{
			this.RpcWriter___Server_SendDelivery_2813439055(delivery);
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000D2360 File Offset: 0x000D0560
		[ObserversRpc]
		[TargetRpc]
		private void ReceiveDelivery(NetworkConnection conn, DeliveryInstance delivery)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveDelivery_2795369214(conn, delivery);
			}
			else
			{
				this.RpcWriter___Target_ReceiveDelivery_2795369214(conn, delivery);
			}
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000D2394 File Offset: 0x000D0594
		[ObserversRpc(RunLocally = true)]
		private void SetDeliveryState(string deliveryID, EDeliveryStatus status)
		{
			this.RpcWriter___Observers_SetDeliveryState_316609003(deliveryID, status);
			this.RpcLogic___SetDeliveryState_316609003(deliveryID, status);
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000D23C0 File Offset: 0x000D05C0
		private DeliveryInstance GetDelivery(string deliveryID)
		{
			return this.Deliveries.FirstOrDefault((DeliveryInstance d) => d.DeliveryID == deliveryID);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000D23F4 File Offset: 0x000D05F4
		public DeliveryInstance GetDelivery(Property destination)
		{
			return this.Deliveries.FirstOrDefault((DeliveryInstance d) => d.DestinationCode == destination.PropertyCode);
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000D2428 File Offset: 0x000D0628
		public DeliveryInstance GetActiveShopDelivery(DeliveryShop shop)
		{
			return this.Deliveries.FirstOrDefault((DeliveryInstance d) => d.StoreName == shop.MatchingShopInterfaceName);
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000D245C File Offset: 0x000D065C
		public ShopInterface GetShopInterface(string shopName)
		{
			return ShopInterface.AllShops.Find((ShopInterface x) => x.ShopName == shopName);
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000D248C File Offset: 0x000D068C
		public virtual string GetSaveString()
		{
			List<VehicleData> list = new List<VehicleData>();
			foreach (DeliveryInstance deliveryInstance in this.Deliveries)
			{
				if (!(deliveryInstance.ActiveVehicle == null))
				{
					list.Add(deliveryInstance.ActiveVehicle.Vehicle.GetVehicleData());
				}
			}
			return new DeliveriesData(this.Deliveries.ToArray(), list.ToArray()).GetJson(true);
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000D2578 File Offset: 0x000D0778
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendDelivery_2813439055));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveDelivery_2795369214));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveDelivery_2795369214));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetDeliveryState_316609003));
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000D25F8 File Offset: 0x000D07F8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000D2611 File Offset: 0x000D0811
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000D2620 File Offset: 0x000D0820
		private void RpcWriter___Server_SendDelivery_2813439055(DeliveryInstance delivery)
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
			writer.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated(delivery);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x000D26C7 File Offset: 0x000D08C7
		public void RpcLogic___SendDelivery_2813439055(DeliveryInstance delivery)
		{
			this.ReceiveDelivery(null, delivery);
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000D26D4 File Offset: 0x000D08D4
		private void RpcReader___Server_SendDelivery_2813439055(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			DeliveryInstance delivery = GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendDelivery_2813439055(delivery);
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000D2708 File Offset: 0x000D0908
		private void RpcWriter___Observers_ReceiveDelivery_2795369214(NetworkConnection conn, DeliveryInstance delivery)
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
			writer.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated(delivery);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000D27C0 File Offset: 0x000D09C0
		private void RpcLogic___ReceiveDelivery_2795369214(NetworkConnection conn, DeliveryInstance delivery)
		{
			if (this.GetDelivery(delivery.DeliveryID) != null)
			{
				return;
			}
			this.Deliveries.Add(delivery);
			delivery.SetStatus(delivery.Status);
			if (this.onDeliveryCreated != null)
			{
				this.onDeliveryCreated.Invoke(delivery);
			}
			this.HasChanged = true;
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000D2810 File Offset: 0x000D0A10
		private void RpcReader___Observers_ReceiveDelivery_2795369214(PooledReader PooledReader0, Channel channel)
		{
			DeliveryInstance delivery = GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveDelivery_2795369214(null, delivery);
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x000D2844 File Offset: 0x000D0A44
		private void RpcWriter___Target_ReceiveDelivery_2795369214(NetworkConnection conn, DeliveryInstance delivery)
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
			writer.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated(delivery);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000D28FC File Offset: 0x000D0AFC
		private void RpcReader___Target_ReceiveDelivery_2795369214(PooledReader PooledReader0, Channel channel)
		{
			DeliveryInstance delivery = GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveDelivery_2795369214(base.LocalConnection, delivery);
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000D2934 File Offset: 0x000D0B34
		private void RpcWriter___Observers_SetDeliveryState_316609003(string deliveryID, EDeliveryStatus status)
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
			writer.WriteString(deliveryID);
			writer.Write___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generated(status);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000D29F8 File Offset: 0x000D0BF8
		private void RpcLogic___SetDeliveryState_316609003(string deliveryID, EDeliveryStatus status)
		{
			DeliveryInstance delivery = this.GetDelivery(deliveryID);
			if (delivery != null)
			{
				delivery.SetStatus(status);
			}
			if (status == EDeliveryStatus.Completed)
			{
				if (this.onDeliveryCompleted != null)
				{
					this.onDeliveryCompleted.Invoke(delivery);
				}
				this.Deliveries.Remove(delivery);
			}
			this.HasChanged = true;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000D2A44 File Offset: 0x000D0C44
		private void RpcReader___Observers_SetDeliveryState_316609003(PooledReader PooledReader0, Channel channel)
		{
			string deliveryID = PooledReader0.ReadString();
			EDeliveryStatus status = GeneratedReaders___Internal.Read___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetDeliveryState_316609003(deliveryID, status);
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000D2A90 File Offset: 0x000D0C90
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x040023A9 RID: 9129
		public List<DeliveryInstance> Deliveries = new List<DeliveryInstance>();

		// Token: 0x040023AA RID: 9130
		public UnityEvent<DeliveryInstance> onDeliveryCreated;

		// Token: 0x040023AB RID: 9131
		public UnityEvent<DeliveryInstance> onDeliveryCompleted;

		// Token: 0x040023AC RID: 9132
		private DeliveriesLoader loader = new DeliveriesLoader();

		// Token: 0x040023B0 RID: 9136
		private List<string> writtenVehicles = new List<string>();

		// Token: 0x040023B1 RID: 9137
		private Dictionary<DeliveryInstance, int> minsSinceVehicleEmpty = new Dictionary<DeliveryInstance, int>();

		// Token: 0x040023B2 RID: 9138
		private bool dll_Excuted;

		// Token: 0x040023B3 RID: 9139
		private bool dll_Excuted;
	}
}
