using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.GameTime;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Map
{
	// Token: 0x02000C6B RID: 3179
	public class DarkMarket : NetworkSingleton<DarkMarket>
	{
		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06005968 RID: 22888 RVA: 0x00179CDE File Offset: 0x00177EDE
		// (set) Token: 0x06005969 RID: 22889 RVA: 0x00179CE6 File Offset: 0x00177EE6
		public bool IsOpen { get; protected set; } = true;

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x00179CEF File Offset: 0x00177EEF
		// (set) Token: 0x0600596B RID: 22891 RVA: 0x00179CF7 File Offset: 0x00177EF7
		public bool Unlocked { get; protected set; }

		// Token: 0x0600596C RID: 22892 RVA: 0x00179D00 File Offset: 0x00177F00
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.OnLoad));
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x00179D23 File Offset: 0x00177F23
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.Unlocked)
			{
				this.SetUnlocked(connection);
			}
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x00179D3B File Offset: 0x00177F3B
		private void Update()
		{
			this.IsOpen = this.ShouldBeOpen();
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x00179D4C File Offset: 0x00177F4C
		private bool ShouldBeOpen()
		{
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.AccessZone.OpenTime, this.AccessZone.CloseTime))
			{
				return false;
			}
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Player.PlayerList[i].CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005970 RID: 22896 RVA: 0x00179DAC File Offset: 0x00177FAC
		private void OnLoad()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.OnLoad));
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("WarehouseUnlocked"))
			{
				this.SendUnlocked();
				return;
			}
			this.MainDoor.SetKnockingEnabled(true);
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x00179DF8 File Offset: 0x00177FF8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendUnlocked()
		{
			this.RpcWriter___Server_SendUnlocked_2166136261();
			this.RpcLogic___SendUnlocked_2166136261();
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x00179E08 File Offset: 0x00178008
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetUnlocked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetUnlocked_328543758(conn);
				this.RpcLogic___SetUnlocked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_SetUnlocked_328543758(conn);
			}
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x00179E4C File Offset: 0x0017804C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendUnlocked_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetUnlocked_328543758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetUnlocked_328543758));
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x00179EB5 File Offset: 0x001780B5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Map.DarkMarketAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x00179ECE File Offset: 0x001780CE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x00179EDC File Offset: 0x001780DC
		private void RpcWriter___Server_SendUnlocked_2166136261()
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x00179F76 File Offset: 0x00178176
		public void RpcLogic___SendUnlocked_2166136261()
		{
			this.SetUnlocked(null);
		}

		// Token: 0x06005979 RID: 22905 RVA: 0x00179F80 File Offset: 0x00178180
		private void RpcReader___Server_SendUnlocked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendUnlocked_2166136261();
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x00179FB0 File Offset: 0x001781B0
		private void RpcWriter___Observers_SetUnlocked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x0017A05C File Offset: 0x0017825C
		private void RpcLogic___SetUnlocked_328543758(NetworkConnection conn)
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("WarehouseUnlocked", true.ToString(), true);
			this.MainDoor.SetKnockingEnabled(false);
			this.MainDoor.Igor.gameObject.SetActive(false);
			this.Unlocked = true;
			this.Oscar.EnableDeliveries();
			DoorController[] doors = this.AccessZone.Doors;
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].noAccessErrorMessage = "Only open after 6PM";
			}
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x0017A0E0 File Offset: 0x001782E0
		private void RpcReader___Observers_SetUnlocked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetUnlocked_328543758(null);
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x0017A10C File Offset: 0x0017830C
		private void RpcWriter___Target_SetUnlocked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x0017A1B4 File Offset: 0x001783B4
		private void RpcReader___Target_SetUnlocked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetUnlocked_328543758(base.LocalConnection);
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x0017A1DA File Offset: 0x001783DA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04004195 RID: 16789
		public DarkMarketAccessZone AccessZone;

		// Token: 0x04004196 RID: 16790
		public DarkMarketMainDoor MainDoor;

		// Token: 0x04004197 RID: 16791
		public Oscar Oscar;

		// Token: 0x04004198 RID: 16792
		public FullRank UnlockRank;

		// Token: 0x04004199 RID: 16793
		private bool dll_Excuted;

		// Token: 0x0400419A RID: 16794
		private bool dll_Excuted;
	}
}
