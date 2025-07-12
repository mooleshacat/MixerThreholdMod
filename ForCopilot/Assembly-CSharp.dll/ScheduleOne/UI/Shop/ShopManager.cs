using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000B97 RID: 2967
	public class ShopManager : NetworkSingleton<ShopManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x0014C6A3 File Offset: 0x0014A8A3
		public string SaveFolderName
		{
			get
			{
				return "Shops";
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06004EAA RID: 20138 RVA: 0x0014C6A3 File Offset: 0x0014A8A3
		public string SaveFileName
		{
			get
			{
				return "Shops";
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0014C6AA File Offset: 0x0014A8AA
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06004EAC RID: 20140 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06004EAD RID: 20141 RVA: 0x0014C6B2 File Offset: 0x0014A8B2
		// (set) Token: 0x06004EAE RID: 20142 RVA: 0x0014C6BA File Offset: 0x0014A8BA
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06004EAF RID: 20143 RVA: 0x0014C6C3 File Offset: 0x0014A8C3
		// (set) Token: 0x06004EB0 RID: 20144 RVA: 0x0014C6CB File Offset: 0x0014A8CB
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06004EB1 RID: 20145 RVA: 0x0014C6D4 File Offset: 0x0014A8D4
		// (set) Token: 0x06004EB2 RID: 20146 RVA: 0x0014C6DC File Offset: 0x0014A8DC
		public bool HasChanged { get; set; } = true;

		// Token: 0x06004EB3 RID: 20147 RVA: 0x0014C6E5 File Offset: 0x0014A8E5
		protected override void Start()
		{
			base.Start();
			this.InitializeSaveable();
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x0014C6F4 File Offset: 0x0014A8F4
		public virtual string GetSaveString()
		{
			List<ShopData> list = new List<ShopData>();
			for (int i = 0; i < ShopInterface.AllShops.Count; i++)
			{
				if (!(ShopInterface.AllShops[i] == null) && ShopInterface.AllShops[i].ShouldSave())
				{
					list.Add(ShopInterface.AllShops[i].GetSaveData());
				}
			}
			return new ShopManagerData(list.ToArray()).GetJson(true);
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x0014C768 File Offset: 0x0014A968
		[ServerRpc(RequireOwnership = false)]
		public void SendStock(string shopCode, string itemID, int stock)
		{
			this.RpcWriter___Server_SendStock_15643032(shopCode, itemID, stock);
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x0014C77C File Offset: 0x0014A97C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetStock(NetworkConnection conn, string shopCode, string itemID, int stock)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStock_3509965635(conn, shopCode, itemID, stock);
				this.RpcLogic___SetStock_3509965635(conn, shopCode, itemID, stock);
			}
			else
			{
				this.RpcWriter___Target_SetStock_3509965635(conn, shopCode, itemID, stock);
			}
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x0014C808 File Offset: 0x0014AA08
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.UI.Shop.ShopManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.UI.Shop.ShopManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendStock_15643032));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetStock_3509965635));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetStock_3509965635));
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x0014C871 File Offset: 0x0014AA71
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.UI.Shop.ShopManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.UI.Shop.ShopManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x0014C88A File Offset: 0x0014AA8A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x0014C898 File Offset: 0x0014AA98
		private void RpcWriter___Server_SendStock_15643032(string shopCode, string itemID, int stock)
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
			writer.WriteString(shopCode);
			writer.WriteString(itemID);
			writer.WriteInt32(stock, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x0014C95E File Offset: 0x0014AB5E
		public void RpcLogic___SendStock_15643032(string shopCode, string itemID, int stock)
		{
			this.SetStock(null, shopCode, itemID, stock);
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x0014C96C File Offset: 0x0014AB6C
		private void RpcReader___Server_SendStock_15643032(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string shopCode = PooledReader0.ReadString();
			string itemID = PooledReader0.ReadString();
			int stock = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendStock_15643032(shopCode, itemID, stock);
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x0014C9C4 File Offset: 0x0014ABC4
		private void RpcWriter___Observers_SetStock_3509965635(NetworkConnection conn, string shopCode, string itemID, int stock)
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
			writer.WriteString(shopCode);
			writer.WriteString(itemID);
			writer.WriteInt32(stock, 1);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x0014CA9C File Offset: 0x0014AC9C
		public void RpcLogic___SetStock_3509965635(NetworkConnection conn, string shopCode, string itemID, int stock)
		{
			ShopInterface shopInterface = ShopInterface.AllShops.Find((ShopInterface x) => x.ShopCode == shopCode);
			if (shopInterface == null)
			{
				Debug.LogError("Failed to set stock: Shop not found: " + shopCode);
				return;
			}
			ShopListing listing = shopInterface.GetListing(itemID);
			if (listing == null)
			{
				Debug.LogError("Failed to set stock: Listing not found: " + itemID);
				return;
			}
			listing.SetStock(stock, false);
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x0014CB14 File Offset: 0x0014AD14
		private void RpcReader___Observers_SetStock_3509965635(PooledReader PooledReader0, Channel channel)
		{
			string shopCode = PooledReader0.ReadString();
			string itemID = PooledReader0.ReadString();
			int stock = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStock_3509965635(null, shopCode, itemID, stock);
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x0014CB78 File Offset: 0x0014AD78
		private void RpcWriter___Target_SetStock_3509965635(NetworkConnection conn, string shopCode, string itemID, int stock)
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
			writer.WriteString(shopCode);
			writer.WriteString(itemID);
			writer.WriteInt32(stock, 1);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x0014CC4C File Offset: 0x0014AE4C
		private void RpcReader___Target_SetStock_3509965635(PooledReader PooledReader0, Channel channel)
		{
			string shopCode = PooledReader0.ReadString();
			string itemID = PooledReader0.ReadString();
			int stock = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetStock_3509965635(base.LocalConnection, shopCode, itemID, stock);
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x0014CCAA File Offset: 0x0014AEAA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003AF5 RID: 15093
		private ShopManagerLoader loader = new ShopManagerLoader();

		// Token: 0x04003AF9 RID: 15097
		private bool dll_Excuted;

		// Token: 0x04003AFA RID: 15098
		private bool dll_Excuted;
	}
}
