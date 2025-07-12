using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.CrashReportHandler;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200071A RID: 1818
	public class GameManager : NetworkSingleton<GameManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x000CDAD8 File Offset: 0x000CBCD8
		public static bool IS_TUTORIAL
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Tutorial";
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06003134 RID: 12596 RVA: 0x000CDAFC File Offset: 0x000CBCFC
		public static int Seed
		{
			get
			{
				if (NetworkSingleton<GameManager>.Instance != null)
				{
					return NetworkSingleton<GameManager>.Instance.seed;
				}
				return 0;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x000CDB17 File Offset: 0x000CBD17
		// (set) Token: 0x06003136 RID: 12598 RVA: 0x000CDB1F File Offset: 0x000CBD1F
		public Sprite OrganisationLogo { get; protected set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06003137 RID: 12599 RVA: 0x000CDB28 File Offset: 0x000CBD28
		public bool IsTutorial
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Tutorial";
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06003138 RID: 12600 RVA: 0x000CDB4C File Offset: 0x000CBD4C
		public string SaveFolderName
		{
			get
			{
				return "Game";
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000CDB4C File Offset: 0x000CBD4C
		public string SaveFileName
		{
			get
			{
				return "Game";
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x000CDB53 File Offset: 0x000CBD53
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x000CDB5B File Offset: 0x000CBD5B
		// (set) Token: 0x0600313D RID: 12605 RVA: 0x000CDB63 File Offset: 0x000CBD63
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Logo.png"
		};

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x000CDB6C File Offset: 0x000CBD6C
		// (set) Token: 0x0600313F RID: 12607 RVA: 0x000CDB74 File Offset: 0x000CBD74
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x000CDB7D File Offset: 0x000CBD7D
		// (set) Token: 0x06003141 RID: 12609 RVA: 0x000CDB85 File Offset: 0x000CBD85
		public bool HasChanged { get; set; }

		// Token: 0x06003142 RID: 12610 RVA: 0x000CDB8E File Offset: 0x000CBD8E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.DevUtilities.GameManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000CDBA2 File Offset: 0x000CBDA2
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000CDBAA File Offset: 0x000CBDAA
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!connection.IsHost)
			{
				this.SetGameData(connection, new GameData(this.OrganisationName, this.seed, this.Settings));
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000CDBD9 File Offset: 0x000CBDD9
		[TargetRpc]
		public void SetGameData(NetworkConnection conn, GameData data)
		{
			this.RpcWriter___Target_SetGameData_3076874643(conn, data);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000CDBE9 File Offset: 0x000CBDE9
		public virtual string GetSaveString()
		{
			return new GameData(this.OrganisationName, this.seed, this.Settings).GetJson(true);
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000CDC08 File Offset: 0x000CBE08
		public void Load(GameData data, string path)
		{
			this.OrganisationName = data.OrganisationName;
			this.seed = data.Seed;
			this.Settings = data.Settings;
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
			this.HasChanged = true;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000CDC48 File Offset: 0x000CBE48
		[Button]
		public void EndTutorial(bool natural)
		{
			if (!this.IsTutorial)
			{
				return;
			}
			Console.Log("Ending tutorial...", null);
			if (Singleton<LoadManager>.Instance.StoredSaveInfo != null && (!Singleton<Lobby>.Instance.IsInLobby || Singleton<Lobby>.Instance.IsHost))
			{
				Singleton<SaveManager>.Instance.DisablePlayTutorial(Singleton<LoadManager>.Instance.StoredSaveInfo);
				Singleton<LoadManager>.Instance.StoredSaveInfo.MetaData.PlayTutorial = false;
			}
			if (natural)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.COMPLETE_PROLOGUE);
			}
			Singleton<LoadManager>.Instance.ExitToMenu(Singleton<LoadManager>.Instance.StoredSaveInfo, null, true);
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000CDD3C File Offset: 0x000CBF3C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterTargetRpc(0U, new ClientRpcDelegate(this.RpcReader___Target_SetGameData_3076874643));
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000CDD6C File Offset: 0x000CBF6C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.DevUtilities.GameManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000CDD85 File Offset: 0x000CBF85
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000CDD94 File Offset: 0x000CBF94
		private void RpcWriter___Target_SetGameData_3076874643(NetworkConnection conn, GameData data)
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
			writer.Write___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(0U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000CDE49 File Offset: 0x000CC049
		public void RpcLogic___SetGameData_3076874643(NetworkConnection conn, GameData data)
		{
			this.OrganisationName = data.OrganisationName;
			this.seed = data.Seed;
			this.Settings = data.Settings;
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000CDE84 File Offset: 0x000CC084
		private void RpcReader___Target_SetGameData_3076874643(PooledReader PooledReader0, Channel channel)
		{
			GameData data = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.GameDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGameData_3076874643(base.LocalConnection, data);
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000CDEBB File Offset: 0x000CC0BB
		protected override void dll()
		{
			base.Awake();
			CrashReportHandler.logBufferSize = 50U;
			this.InitializeSaveable();
		}

		// Token: 0x040022A8 RID: 8872
		public const bool IS_DEMO = false;

		// Token: 0x040022A9 RID: 8873
		public static bool IS_BETA;

		// Token: 0x040022AA RID: 8874
		[SerializeField]
		private int seed;

		// Token: 0x040022AB RID: 8875
		public string OrganisationName = "Organisation";

		// Token: 0x040022AD RID: 8877
		public GameSettings Settings = new GameSettings();

		// Token: 0x040022AE RID: 8878
		public Transform SpawnPoint;

		// Token: 0x040022AF RID: 8879
		public Transform NoHomeRespawnPoint;

		// Token: 0x040022B0 RID: 8880
		public Transform Temp;

		// Token: 0x040022B1 RID: 8881
		public UnityEvent onSettingsLoaded = new UnityEvent();

		// Token: 0x040022B2 RID: 8882
		private GameDataLoader loader = new GameDataLoader();

		// Token: 0x040022B6 RID: 8886
		private bool dll_Excuted;

		// Token: 0x040022B7 RID: 8887
		private bool dll_Excuted;
	}
}
