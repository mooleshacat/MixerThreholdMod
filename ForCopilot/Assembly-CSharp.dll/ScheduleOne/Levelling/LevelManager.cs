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
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005E9 RID: 1513
	public class LevelManager : NetworkSingleton<LevelManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x00096D0A File Offset: 0x00094F0A
		// (set) Token: 0x06002513 RID: 9491 RVA: 0x00096D12 File Offset: 0x00094F12
		public ERank Rank { get; private set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002514 RID: 9492 RVA: 0x00096D1B File Offset: 0x00094F1B
		// (set) Token: 0x06002515 RID: 9493 RVA: 0x00096D23 File Offset: 0x00094F23
		public int Tier { get; private set; } = 1;

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002516 RID: 9494 RVA: 0x00096D2C File Offset: 0x00094F2C
		// (set) Token: 0x06002517 RID: 9495 RVA: 0x00096D34 File Offset: 0x00094F34
		public int XP { get; private set; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x00096D3D File Offset: 0x00094F3D
		// (set) Token: 0x06002519 RID: 9497 RVA: 0x00096D45 File Offset: 0x00094F45
		public int TotalXP { get; private set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x00096D4E File Offset: 0x00094F4E
		public float XPToNextTier
		{
			get
			{
				return Mathf.Round(Mathf.Lerp(200f, 2500f, (float)this.Rank / (float)this.rankCount) / 25f) * 25f;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x00096D7F File Offset: 0x00094F7F
		public string SaveFolderName
		{
			get
			{
				return "Rank";
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00096D7F File Offset: 0x00094F7F
		public string SaveFileName
		{
			get
			{
				return "Rank";
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x00096D86 File Offset: 0x00094F86
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x00096D8E File Offset: 0x00094F8E
		// (set) Token: 0x06002520 RID: 9504 RVA: 0x00096D96 File Offset: 0x00094F96
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x00096D9F File Offset: 0x00094F9F
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x00096DA7 File Offset: 0x00094FA7
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x00096DB0 File Offset: 0x00094FB0
		// (set) Token: 0x06002524 RID: 9508 RVA: 0x00096DB8 File Offset: 0x00094FB8
		public bool HasChanged { get; set; }

		// Token: 0x06002525 RID: 9509 RVA: 0x00096DC1 File Offset: 0x00094FC1
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Levelling.LevelManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x00096DD5 File Offset: 0x00094FD5
		protected override void Start()
		{
			base.Start();
			this.InitializeSaveable();
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x00096DE3 File Offset: 0x00094FE3
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SetData(connection, this.Rank, this.Tier, this.XP, this.TotalXP);
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x00096E0B File Offset: 0x0009500B
		[ServerRpc(RequireOwnership = false)]
		public void AddXP(int xp)
		{
			this.RpcWriter___Server_AddXP_3316948804(xp);
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x00096E18 File Offset: 0x00095018
		[ObserversRpc]
		private void AddXPLocal(int xp)
		{
			this.RpcWriter___Observers_AddXPLocal_3316948804(xp);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x00096E30 File Offset: 0x00095030
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetData(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetData_20965027(conn, rank, tier, xp, totalXp);
				this.RpcLogic___SetData_20965027(conn, rank, tier, xp, totalXp);
			}
			else
			{
				this.RpcWriter___Target_SetData_20965027(conn, rank, tier, xp, totalXp);
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x00096E98 File Offset: 0x00095098
		[ObserversRpc]
		private void IncreaseTierNetworked(FullRank before, FullRank after)
		{
			this.RpcWriter___Observers_IncreaseTierNetworked_3953286437(before, after);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x00096EB4 File Offset: 0x000950B4
		private void IncreaseTier()
		{
			this.XP -= (int)this.XPToNextTier;
			int tier = this.Tier;
			this.Tier = tier + 1;
			if (this.Tier > 5 && this.Rank != ERank.Kingpin)
			{
				this.Tier = 1;
				ERank rank = this.Rank;
				this.Rank = rank + 1;
			}
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00096F0F File Offset: 0x0009510F
		public virtual string GetSaveString()
		{
			return new RankData((int)this.Rank, this.Tier, this.XP, this.TotalXP).GetJson(true);
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x00096F34 File Offset: 0x00095134
		public FullRank GetFullRank()
		{
			return new FullRank(this.Rank, this.Tier);
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x00096F48 File Offset: 0x00095148
		public void AddUnlockable(Unlockable unlockable)
		{
			if (!this.Unlockables.ContainsKey(unlockable.Rank))
			{
				this.Unlockables.Add(unlockable.Rank, new List<Unlockable>());
			}
			if (this.Unlockables[unlockable.Rank].Find((Unlockable x) => x.Title.ToLower() == unlockable.Title.ToLower() && x.Icon == unlockable.Icon) != null)
			{
				return;
			}
			this.Unlockables[unlockable.Rank].Add(unlockable);
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x00096FE0 File Offset: 0x000951E0
		public int GetTotalXPForRank(FullRank fullrank)
		{
			int num = 0;
			foreach (ERank erank in (ERank[])Enum.GetValues(typeof(ERank)))
			{
				int xpforTier = this.GetXPForTier(erank);
				int num2 = 5;
				if (erank == ERank.Kingpin)
				{
					num2 = 1000;
				}
				for (int j = 1; j <= num2; j++)
				{
					if (erank == fullrank.Rank && j == fullrank.Tier)
					{
						return num;
					}
					num += xpforTier;
				}
			}
			Console.LogError("Rank not found: " + fullrank.ToString(), null);
			return 0;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x00097078 File Offset: 0x00095278
		public FullRank GetFullRank(int totalXp)
		{
			int num = totalXp;
			foreach (ERank erank in (ERank[])Enum.GetValues(typeof(ERank)))
			{
				int xpforTier = this.GetXPForTier(erank);
				if (erank == ERank.Kingpin)
				{
					for (int j = 1; j <= 1000; j++)
					{
						if (num < xpforTier)
						{
							return new FullRank(erank, j);
						}
						num -= xpforTier;
					}
				}
				else
				{
					for (int k = 1; k <= 5; k++)
					{
						if (num < xpforTier)
						{
							return new FullRank(erank, k);
						}
						num -= xpforTier;
					}
				}
			}
			Console.LogError("Rank not found for XP: " + totalXp.ToString(), null);
			return new FullRank(ERank.Street_Rat, 1);
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x00097126 File Offset: 0x00095326
		public int GetXPForTier(ERank rank)
		{
			return Mathf.RoundToInt(Mathf.Round(Mathf.Lerp(200f, 2500f, (float)rank / (float)this.rankCount) / 25f) * 25f);
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x00097158 File Offset: 0x00095358
		public static float GetOrderLimitMultiplier(FullRank rank)
		{
			float rankOrderLimitMultiplier = LevelManager.GetRankOrderLimitMultiplier(rank.Rank);
			if (rank.Rank < ERank.Kingpin)
			{
				float rankOrderLimitMultiplier2 = LevelManager.GetRankOrderLimitMultiplier(rank.Rank + 1);
				float t = (float)(rank.Tier - 1) / 4f;
				return Mathf.Lerp(rankOrderLimitMultiplier, rankOrderLimitMultiplier2, t);
			}
			return Mathf.Clamp(LevelManager.GetRankOrderLimitMultiplier(ERank.Kingpin) + 0.1f * (float)(rank.Tier - 1), 1f, 10f);
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000971C8 File Offset: 0x000953C8
		private static float GetRankOrderLimitMultiplier(ERank rank)
		{
			switch (rank)
			{
			case ERank.Street_Rat:
				return 1f;
			case ERank.Hoodlum:
				return 1.25f;
			case ERank.Peddler:
				return 1.5f;
			case ERank.Hustler:
				return 1.75f;
			case ERank.Bagman:
				return 2f;
			case ERank.Enforcer:
				return 2.25f;
			case ERank.Shot_Caller:
				return 2.5f;
			case ERank.Block_Boss:
				return 2.75f;
			case ERank.Underlord:
				return 3f;
			case ERank.Baron:
				return 3.25f;
			case ERank.Kingpin:
				return 3.5f;
			default:
				return 1f;
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x0009728C File Offset: 0x0009548C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_AddXP_3316948804));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_AddXPLocal_3316948804));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_SetData_20965027));
			base.RegisterTargetRpc(3U, new ClientRpcDelegate(this.RpcReader___Target_SetData_20965027));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_IncreaseTierNetworked_3953286437));
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00097323 File Offset: 0x00095523
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Levelling.LevelManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x0009733C File Offset: 0x0009553C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0009734C File Offset: 0x0009554C
		private void RpcWriter___Server_AddXP_3316948804(int xp)
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
			writer.WriteInt32(xp, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000973F8 File Offset: 0x000955F8
		public void RpcLogic___AddXP_3316948804(int xp)
		{
			this.AddXPLocal(xp);
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x00097404 File Offset: 0x00095604
		private void RpcReader___Server_AddXP_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int xp = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___AddXP_3316948804(xp);
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x0009743C File Offset: 0x0009563C
		private void RpcWriter___Observers_AddXPLocal_3316948804(int xp)
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
			writer.WriteInt32(xp, 1);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000974F8 File Offset: 0x000956F8
		private void RpcLogic___AddXPLocal_3316948804(int xp)
		{
			NetworkSingleton<DailySummary>.Instance.AddXP(xp);
			this.XP += xp;
			this.TotalXP += xp;
			this.HasChanged = true;
			Console.Log(string.Concat(new string[]
			{
				"Rank progress: ",
				this.XP.ToString(),
				"/",
				this.XPToNextTier.ToString(),
				" (Total ",
				this.TotalXP.ToString(),
				")"
			}), null);
			if (InstanceFinder.IsServer)
			{
				FullRank fullRank = this.GetFullRank();
				bool flag = false;
				while ((float)this.XP >= this.XPToNextTier)
				{
					this.IncreaseTier();
					flag = true;
				}
				this.SetData(null, this.Rank, this.Tier, this.XP, this.TotalXP);
				if (flag)
				{
					this.IncreaseTierNetworked(fullRank, this.GetFullRank());
				}
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000975F0 File Offset: 0x000957F0
		private void RpcReader___Observers_AddXPLocal_3316948804(PooledReader PooledReader0, Channel channel)
		{
			int xp = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddXPLocal_3316948804(xp);
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00097628 File Offset: 0x00095828
		private void RpcWriter___Observers_SetData_20965027(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
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
			writer.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated(rank);
			writer.WriteInt32(tier, 1);
			writer.WriteInt32(xp, 1);
			writer.WriteInt32(totalXp, 1);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x00097714 File Offset: 0x00095914
		public void RpcLogic___SetData_20965027(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
		{
			this.Rank = rank;
			this.Tier = tier;
			this.XP = xp;
			this.TotalXP = totalXp;
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x00097734 File Offset: 0x00095934
		private void RpcReader___Observers_SetData_20965027(PooledReader PooledReader0, Channel channel)
		{
			ERank rank = GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds(PooledReader0);
			int tier = PooledReader0.ReadInt32(1);
			int xp = PooledReader0.ReadInt32(1);
			int totalXp = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetData_20965027(null, rank, tier, xp, totalXp);
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000977B4 File Offset: 0x000959B4
		private void RpcWriter___Target_SetData_20965027(NetworkConnection conn, ERank rank, int tier, int xp, int totalXp)
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
			writer.Write___ScheduleOne.Levelling.ERankFishNet.Serializing.Generated(rank);
			writer.WriteInt32(tier, 1);
			writer.WriteInt32(xp, 1);
			writer.WriteInt32(totalXp, 1);
			base.SendTargetRpc(3U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000978A0 File Offset: 0x00095AA0
		private void RpcReader___Target_SetData_20965027(PooledReader PooledReader0, Channel channel)
		{
			ERank rank = GeneratedReaders___Internal.Read___ScheduleOne.Levelling.ERankFishNet.Serializing.Generateds(PooledReader0);
			int tier = PooledReader0.ReadInt32(1);
			int xp = PooledReader0.ReadInt32(1);
			int totalXp = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetData_20965027(base.LocalConnection, rank, tier, xp, totalXp);
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x0009791C File Offset: 0x00095B1C
		private void RpcWriter___Observers_IncreaseTierNetworked_3953286437(FullRank before, FullRank after)
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
			writer.Write___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generated(before);
			writer.Write___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generated(after);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000979E0 File Offset: 0x00095BE0
		private void RpcLogic___IncreaseTierNetworked_3953286437(FullRank before, FullRank after)
		{
			Action<FullRank, FullRank> action = this.onRankUp;
			if (action != null)
			{
				action(before, after);
			}
			this.HasChanged = true;
			Console.Log("Ranked up to " + this.Rank.ToString() + ": " + this.Tier.ToString(), null);
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x00097A40 File Offset: 0x00095C40
		private void RpcReader___Observers_IncreaseTierNetworked_3953286437(PooledReader PooledReader0, Channel channel)
		{
			FullRank before = GeneratedReaders___Internal.Read___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generateds(PooledReader0);
			FullRank after = GeneratedReaders___Internal.Read___ScheduleOne.Levelling.FullRankFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___IncreaseTierNetworked_3953286437(before, after);
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x00097A82 File Offset: 0x00095C82
		protected override void dll()
		{
			base.Awake();
			this.rankCount = Enum.GetValues(typeof(ERank)).Length;
		}

		// Token: 0x04001B78 RID: 7032
		public const int TIERS_PER_RANK = 5;

		// Token: 0x04001B79 RID: 7033
		public const int XP_PER_TIER_MIN = 200;

		// Token: 0x04001B7A RID: 7034
		public const int XP_PER_TIER_MAX = 2500;

		// Token: 0x04001B7C RID: 7036
		private int rankCount;

		// Token: 0x04001B80 RID: 7040
		public Action<FullRank, FullRank> onRankUp;

		// Token: 0x04001B81 RID: 7041
		public Dictionary<FullRank, List<Unlockable>> Unlockables = new Dictionary<FullRank, List<Unlockable>>();

		// Token: 0x04001B82 RID: 7042
		private RankLoader loader = new RankLoader();

		// Token: 0x04001B86 RID: 7046
		private bool dll_Excuted;

		// Token: 0x04001B87 RID: 7047
		private bool dll_Excuted;
	}
}
