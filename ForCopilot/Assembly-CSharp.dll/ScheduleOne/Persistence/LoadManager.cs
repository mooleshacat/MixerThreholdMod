using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using Pathfinding;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Money;
using ScheduleOne.Networking;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.ItemLoaders;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.UI;
using ScheduleOne.UI.MainMenu;
using ScheduleOne.UI.Phone;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037E RID: 894
	public class LoadManager : PersistentSingleton<LoadManager>
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x000598BE File Offset: 0x00057ABE
		public string DefaultTutorialSaveFolder
		{
			get
			{
				return Path.Combine(Application.streamingAssetsPath, "DefaultTutorialSave");
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x000598CF File Offset: 0x00057ACF
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x000598D7 File Offset: 0x00057AD7
		public bool IsGameLoaded { get; protected set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x000598E0 File Offset: 0x00057AE0
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x000598E8 File Offset: 0x00057AE8
		public bool IsLoading { get; protected set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x000598F1 File Offset: 0x00057AF1
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x000598F9 File Offset: 0x00057AF9
		public float TimeSinceGameLoaded { get; protected set; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00059902 File Offset: 0x00057B02
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x0005990A File Offset: 0x00057B0A
		public bool DebugMode { get; protected set; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00059913 File Offset: 0x00057B13
		// (set) Token: 0x06001449 RID: 5193 RVA: 0x0005991B File Offset: 0x00057B1B
		public LoadManager.ELoadStatus LoadStatus { get; protected set; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x00059924 File Offset: 0x00057B24
		// (set) Token: 0x0600144B RID: 5195 RVA: 0x0005992C File Offset: 0x00057B2C
		public string LoadedGameFolderPath { get; protected set; } = string.Empty;

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00059935 File Offset: 0x00057B35
		// (set) Token: 0x0600144D RID: 5197 RVA: 0x0005993D File Offset: 0x00057B3D
		public SaveInfo ActiveSaveInfo { get; private set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00059946 File Offset: 0x00057B46
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x0005994E File Offset: 0x00057B4E
		public SaveInfo StoredSaveInfo { get; private set; }

		// Token: 0x06001450 RID: 5200 RVA: 0x00059957 File Offset: 0x00057B57
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x00059960 File Offset: 0x00057B60
		protected override void Start()
		{
			base.Start();
			if (Singleton<LoadManager>.Instance == null || Singleton<LoadManager>.Instance != this)
			{
				return;
			}
			this.Bananas();
			this.InitializeItemLoaders();
			this.InitializeObjectLoaders();
			this.InitializeNPCLoaders();
			Singleton<SaveManager>.Instance.CheckSaveFolderInitialized();
			this.RefreshSaveInfo();
			if (SceneManager.GetActiveScene().name == "Main" || SceneManager.GetActiveScene().name == "Tutorial")
			{
				this.DebugMode = true;
				this.IsGameLoaded = true;
				this.LoadedGameFolderPath = Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "DevSave");
				if (!Directory.Exists(this.LoadedGameFolderPath))
				{
					Directory.CreateDirectory(this.LoadedGameFolderPath);
				}
			}
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00059A2C File Offset: 0x00057C2C
		private void Bananas()
		{
			string fullName = new DirectoryInfo(Application.dataPath).Parent.FullName;
			Console.Log("Game folder path: " + fullName, null);
			string path = Path.Combine(fullName, "OnlineFix.ini");
			if (!File.Exists(path))
			{
				return;
			}
			string[] array;
			try
			{
				array = File.ReadAllLines(path);
			}
			catch (Exception ex)
			{
				Console.LogWarning("Error reading INI file: " + ex.Message, null);
				return;
			}
			int num = -1;
			int num2 = -1;
			string str = null;
			string str2 = null;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.StartsWith("RealAppId="))
				{
					num = i;
					str = text.Substring("RealAppId=".Length);
				}
				else if (text.StartsWith("FakeAppId="))
				{
					num2 = i;
					str2 = text.Substring("FakeAppId=".Length);
				}
			}
			if (num == -1 || num2 == -1)
			{
				return;
			}
			array[num] = "RealAppId=" + str2;
			array[num2] = "FakeAppId=" + str;
			try
			{
				File.WriteAllLines(path, array);
			}
			catch (Exception ex2)
			{
				Console.LogError("Error writing INI file: " + ex2.Message, null);
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00059B78 File Offset: 0x00057D78
		private void InitializeItemLoaders()
		{
			new ItemLoader();
			new WateringCanLoader();
			new CashLoader();
			new QualityItemLoader();
			new ProductItemLoader();
			new WeedLoader();
			new MethLoader();
			new CocaineLoader();
			new IntegerItemLoader();
			new TrashGrabberLoader();
			new ClothingLoader();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00059BC8 File Offset: 0x00057DC8
		private void InitializeObjectLoaders()
		{
			new BuildableItemLoader();
			new GridItemLoader();
			new ProceduralGridItemLoader();
			new SurfaceItemLoader();
			new ToggleableItemLoader();
			new PotLoader();
			new PackagingStationLoader();
			new StorageRackLoader();
			new ChemistryStationLoader();
			new LabOvenLoader();
			new BrickPressLoader();
			new MixingStationLoader();
			new CauldronLoader();
			new TrashContainerLoader();
			new SoilPourerLoader();
			new DryingRackLoader();
			new JukeboxLoader();
			new ToggleableSurfaceItemLoader();
			new StorageSurfaceItemLoader();
			new LabelledSurfaceItemLoader();
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00059C50 File Offset: 0x00057E50
		private void InitializeNPCLoaders()
		{
			new NPCsLoader();
			new EmployeeLoader();
			new PackagerLoader();
			new BotanistLoader();
			new ChemistLoader();
			new CleanerLoader();
			new LegacyNPCLoader();
			new LegacyEmployeeLoader();
			new LegacyPackagerLoader();
			new LegacyBotanistLoader();
			new LegacyChemistLoader();
			new LegacyCleanerLoader();
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00059CA8 File Offset: 0x00057EA8
		public void Update()
		{
			if (this.IsGameLoaded && this.LoadedGameFolderPath != string.Empty && Input.GetKeyDown(KeyCode.F6) && (Application.isEditor || Debug.isDebugBuild))
			{
				NetworkManager networkManager = UnityEngine.Object.FindObjectOfType<NetworkManager>();
				networkManager.ClientManager.StopConnection();
				networkManager.ServerManager.StopConnection(false);
				this.StartGame(new SaveInfo(this.LoadedGameFolderPath, -1, "Test Org", DateTime.Now, DateTime.Now, 0f, Application.version, new MetaData(null, null, string.Empty, string.Empty, false)), true);
			}
			if (this.IsGameLoaded && this.LoadStatus == LoadManager.ELoadStatus.None)
			{
				this.TimeSinceGameLoaded += Time.deltaTime;
			}
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00059D69 File Offset: 0x00057F69
		public void QueueLoadRequest(LoadRequest request)
		{
			this.loadRequests.Add(request);
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00059D77 File Offset: 0x00057F77
		public void DequeueLoadRequest(LoadRequest request)
		{
			this.loadRequests.Remove(request);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00059D88 File Offset: 0x00057F88
		public ItemLoader GetItemLoader(string itemType)
		{
			ItemLoader itemLoader = this.ItemLoaders.Find((ItemLoader loader) => loader.ItemType == itemType);
			if (itemLoader == null)
			{
				Console.LogError("No item loader found for data type: " + itemType, null);
				return null;
			}
			return itemLoader;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00059DD8 File Offset: 0x00057FD8
		public BuildableItemLoader GetObjectLoader(string objectType)
		{
			BuildableItemLoader buildableItemLoader = this.ObjectLoaders.Find((BuildableItemLoader loader) => loader.ItemType == objectType);
			if (buildableItemLoader == null)
			{
				Console.LogError("No object loader found for data type: " + objectType, null);
				return null;
			}
			return buildableItemLoader;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00059E28 File Offset: 0x00058028
		public LegacyNPCLoader GetLegacyNPCLoader(string npcType)
		{
			LegacyNPCLoader legacyNPCLoader = this.LegacyNPCLoaders.Find((LegacyNPCLoader loader) => loader.NPCType == npcType);
			if (legacyNPCLoader == null)
			{
				Console.LogError("No NPC loader found for NPC type: " + npcType, null);
				return null;
			}
			return legacyNPCLoader;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00059E78 File Offset: 0x00058078
		public NPCLoader GetNPCLoader(string npcType)
		{
			NPCLoader npcloader = this.NPCLoaders.Find((NPCLoader loader) => loader.NPCType == npcType);
			if (npcloader == null)
			{
				Console.LogError("No NPC loader found for NPC type: " + npcType, null);
				return null;
			}
			return npcloader;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00059EC8 File Offset: 0x000580C8
		public string GetLoadStatusText()
		{
			switch (this.LoadStatus)
			{
			case LoadManager.ELoadStatus.LoadingScene:
				return "Loading world...";
			case LoadManager.ELoadStatus.Initializing:
				return "Initializing...";
			case LoadManager.ELoadStatus.LoadingData:
				return "Loading data...";
			case LoadManager.ELoadStatus.SpawningPlayer:
				return "Spawning player...";
			case LoadManager.ELoadStatus.WaitingForHost:
				return "Waiting for host to finish loading...";
			default:
				return string.Empty;
			}
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00059F20 File Offset: 0x00058120
		public void StartGame(SaveInfo info, bool allowLoadStacking = false)
		{
			LoadManager.<>c__DisplayClass65_0 CS$<>8__locals1 = new LoadManager.<>c__DisplayClass65_0();
			CS$<>8__locals1.info = info;
			CS$<>8__locals1.<>4__this = this;
			if (this.IsGameLoaded && !allowLoadStacking)
			{
				Console.LogWarning("Game already loaded, cannot start another", null);
				return;
			}
			if (CS$<>8__locals1.info == null)
			{
				Console.LogWarning("Save info is null, cannot start game", null);
				return;
			}
			string savePath = CS$<>8__locals1.info.SavePath;
			if (!Directory.Exists(savePath))
			{
				Console.LogWarning("Save game does not exist at " + savePath, null);
				return;
			}
			Singleton<MusicPlayer>.Instance.StopAndDisableTracks();
			Console.Log("Starting game!", null);
			this.ActiveSaveInfo = CS$<>8__locals1.info;
			this.IsLoading = true;
			this.TimeSinceGameLoaded = 0f;
			this.LoadedGameFolderPath = CS$<>8__locals1.info.SavePath;
			LoadManager.LoadHistory.Add("Loading game: " + this.ActiveSaveInfo.OrganisationName);
			base.StartCoroutine(CS$<>8__locals1.<StartGame>g__LoadRoutine|0());
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0005A004 File Offset: 0x00058204
		public void LoadTutorialAsClient()
		{
			LoadManager.<>c__DisplayClass66_0 CS$<>8__locals1 = new LoadManager.<>c__DisplayClass66_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.waitForExit = false;
			if (this.IsGameLoaded)
			{
				Console.LogWarning("Game already loaded, exiting", null);
				CS$<>8__locals1.waitForExit = true;
				this.ExitToMenu(null, null, false);
			}
			base.StartCoroutine(CS$<>8__locals1.<LoadTutorialAsClient>g__LoadRoutine|0());
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0005A058 File Offset: 0x00058258
		public void LoadAsClient(string steamId64)
		{
			LoadManager.<>c__DisplayClass67_0 CS$<>8__locals1 = new LoadManager.<>c__DisplayClass67_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.steamId64 = steamId64;
			CS$<>8__locals1.waitForExit = false;
			if (this.IsGameLoaded)
			{
				Console.LogWarning("Game already loaded, exiting", null);
				CS$<>8__locals1.waitForExit = true;
				this.ExitToMenu(null, null, false);
			}
			base.StartCoroutine(CS$<>8__locals1.<LoadAsClient>g__LoadRoutine|0());
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0005A0B0 File Offset: 0x000582B0
		private void StartLoadErrorAutosubmit()
		{
			base.StartCoroutine(this.<StartLoadErrorAutosubmit>g__Wait|68_0());
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0005A0BF File Offset: 0x000582BF
		public void SetWaitingForHostLoad()
		{
			this.IsLoading = true;
			this.LoadStatus = LoadManager.ELoadStatus.WaitingForHost;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0005A0CF File Offset: 0x000582CF
		public void LoadLastSave()
		{
			if (this.ActiveSaveInfo == null)
			{
				Console.LogWarning("No active save info, cannot load last save", null);
				return;
			}
			this.StartGame(this.ActiveSaveInfo, true);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0005A0F4 File Offset: 0x000582F4
		private void CleanUp()
		{
			GUIDManager.Clear();
			Quest.Quests.Clear();
			Quest.ActiveQuests.Clear();
			NodeLink.validNodeLinks.Clear();
			Player.onLocalPlayerSpawned = null;
			Player.PlayerList.Clear();
			SupplierLocation.AllLocations.Clear();
			Phone.ActiveApp = null;
			ATM.WeeklyDepositSum = 0f;
			NavMeshUtility.ClearCache();
			Business.OwnedBusinesses.Clear();
			Business.UnownedBusinesses.Clear();
			Property.OwnedProperties.Clear();
			Property.UnownedProperties.Clear();
			PlayerMovement.StaticMoveSpeedMultiplier = 1f;
			Business.onOperationFinished = null;
			Business.onOperationStarted = null;
			Property.onPropertyAcquired = null;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0005A198 File Offset: 0x00058398
		public void ExitToMenu(SaveInfo autoLoadSave = null, MainMenuPopup.Data mainMenuPopup = null, bool preventLeaveLobby = false)
		{
			LoadManager.<>c__DisplayClass72_0 CS$<>8__locals1 = new LoadManager.<>c__DisplayClass72_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.autoLoadSave = autoLoadSave;
			CS$<>8__locals1.mainMenuPopup = mainMenuPopup;
			if (!this.IsGameLoaded)
			{
				Console.LogWarning("Game not loaded, cannot exit to menu", null);
				return;
			}
			Console.Log("Exiting to menu", null);
			LoadManager.LoadHistory.Add("Exiting to menu");
			if (Player.Local != null && InstanceFinder.IsServer)
			{
				Player.Local.HostExitedGame();
			}
			if (Singleton<Lobby>.InstanceExists && Singleton<Lobby>.Instance.IsInLobby && !preventLeaveLobby)
			{
				Singleton<Lobby>.Instance.LeaveLobby();
			}
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.IsGameLoaded = false;
			this.ActiveSaveInfo = null;
			this.IsLoading = true;
			Time.timeScale = 1f;
			Singleton<MusicPlayer>.Instance.StopAndDisableTracks();
			base.StartCoroutine(CS$<>8__locals1.<ExitToMenu>g__Load|0());
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0005A270 File Offset: 0x00058470
		public static bool TryLoadSaveInfo(string saveFolderPath, int saveSlotIndex, out SaveInfo saveInfo, bool requireGameFile = false)
		{
			saveInfo = null;
			if (Directory.Exists(saveFolderPath))
			{
				string path = Path.Combine(saveFolderPath, "Metadata.json");
				MetaData metaData = null;
				if (File.Exists(path))
				{
					string text = string.Empty;
					try
					{
						text = File.ReadAllText(path);
					}
					catch (Exception ex)
					{
						Console.LogError("Error reading save metadata: " + ex.Message, null);
					}
					if (!string.IsNullOrEmpty(text))
					{
						try
						{
							metaData = JsonUtility.FromJson<MetaData>(text);
							goto IL_8D;
						}
						catch (Exception ex2)
						{
							metaData = null;
							Console.LogError("Error parsing save metadata: " + ex2.Message, null);
							goto IL_8D;
						}
					}
					Console.LogWarning("Metadata is empty", null);
				}
				IL_8D:
				string path2 = Path.Combine(saveFolderPath, "Game.json");
				GameData gameData = null;
				if (File.Exists(path2))
				{
					string text2 = string.Empty;
					try
					{
						text2 = File.ReadAllText(path2);
					}
					catch (Exception ex3)
					{
						Console.LogError("Error reading save game data: " + ex3.Message, null);
					}
					if (!string.IsNullOrEmpty(text2))
					{
						try
						{
							gameData = JsonUtility.FromJson<GameData>(text2);
							goto IL_10D;
						}
						catch (Exception ex4)
						{
							gameData = null;
							Console.LogError("Error parsing save game data: " + ex4.Message, null);
							goto IL_10D;
						}
					}
					Console.LogWarning("Game data is empty", null);
				}
				IL_10D:
				float networth = 0f;
				string path3 = Path.Combine(saveFolderPath, "Money.json");
				MoneyData moneyData = null;
				if (File.Exists(path3))
				{
					string text3 = string.Empty;
					try
					{
						text3 = File.ReadAllText(path3);
					}
					catch (Exception ex5)
					{
						Console.LogError("Error reading save money data: " + ex5.Message, null);
					}
					if (!string.IsNullOrEmpty(text3))
					{
						try
						{
							moneyData = JsonUtility.FromJson<MoneyData>(text3);
							goto IL_197;
						}
						catch (Exception ex6)
						{
							moneyData = null;
							Console.LogError("Error parsing save money data: " + ex6.Message, null);
							goto IL_197;
						}
					}
					Console.LogWarning("Money data is empty", null);
					IL_197:
					if (moneyData != null)
					{
						networth = moneyData.Networth;
					}
				}
				if (metaData == null)
				{
					Console.LogWarning("Failed to load metadata. Setting default", null);
					metaData = new MetaData(new DateTimeData(DateTime.Now), new DateTimeData(DateTime.Now), Application.version, Application.version, false);
					try
					{
						File.WriteAllText(path, metaData.GetJson(true));
					}
					catch (Exception)
					{
					}
				}
				if (gameData == null)
				{
					if (requireGameFile)
					{
						return false;
					}
					Console.LogWarning("Failed to load game data. Setting default", null);
					gameData = new GameData("Unknown", UnityEngine.Random.Range(0, int.MaxValue), new GameSettings());
					try
					{
						File.WriteAllText(path2, gameData.GetJson(true));
					}
					catch (Exception)
					{
					}
				}
				saveInfo = new SaveInfo(saveFolderPath, saveSlotIndex, gameData.OrganisationName, metaData.CreationDate.GetDateTime(), metaData.LastPlayedDate.GetDateTime(), networth, metaData.LastSaveVersion, metaData);
				return true;
			}
			return false;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0005A540 File Offset: 0x00058740
		public void RefreshSaveInfo()
		{
			for (int i = 0; i < 5; i++)
			{
				LoadManager.SaveGames[i] = null;
				SaveInfo saveInfo;
				if (LoadManager.TryLoadSaveInfo(Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "SaveGame_" + (i + 1).ToString()), i + 1, out saveInfo, false))
				{
					LoadManager.SaveGames[i] = saveInfo;
				}
				else
				{
					LoadManager.SaveGames[i] = null;
				}
			}
			LoadManager.LastPlayedGame = null;
			for (int j = 0; j < LoadManager.SaveGames.Length; j++)
			{
				if (LoadManager.SaveGames[j] != null && (LoadManager.LastPlayedGame == null || LoadManager.SaveGames[j].DateLastPlayed > LoadManager.LastPlayedGame.DateLastPlayed))
				{
					LoadManager.LastPlayedGame = LoadManager.SaveGames[j];
				}
			}
			if (this.onSaveInfoLoaded != null)
			{
				this.onSaveInfoLoaded.Invoke();
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0005A67A File Offset: 0x0005887A
		[CompilerGenerated]
		internal static void <LoadAsClient>g__PlayerSpawned|67_5()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(LoadManager.<LoadAsClient>g__PlayerSpawned|67_5));
			Console.Log("Local player spawned", null);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0005A6A7 File Offset: 0x000588A7
		[CompilerGenerated]
		private IEnumerator <StartLoadErrorAutosubmit>g__Wait|68_0()
		{
			for (float t = 0f; t < 90f; t += Time.deltaTime)
			{
				if (this.LoadStatus == LoadManager.ELoadStatus.None)
				{
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			if (Singleton<PauseMenu>.InstanceExists)
			{
				Console.LogError("Load error timeout reached, submitting error report", null);
				Singleton<PauseMenu>.Instance.FeedbackForm.SetFormData("[AUTOREPORT] Load as client error");
				Singleton<PauseMenu>.Instance.FeedbackForm.SetCategory("Bugs - Multiplayer");
				Singleton<PauseMenu>.Instance.FeedbackForm.IncludeScreenshot = false;
				Singleton<PauseMenu>.Instance.FeedbackForm.IncludeSaveFile = false;
				Singleton<PauseMenu>.Instance.FeedbackForm.Submit();
			}
			yield break;
		}

		// Token: 0x04001308 RID: 4872
		public const int LOADS_PER_FRAME = 50;

		// Token: 0x04001309 RID: 4873
		public const bool DEBUG = false;

		// Token: 0x0400130A RID: 4874
		public const float LOAD_ERROR_TIMEOUT = 90f;

		// Token: 0x0400130B RID: 4875
		public const float NETWORK_TIMEOUT = 30f;

		// Token: 0x0400130C RID: 4876
		public static List<string> LoadHistory = new List<string>();

		// Token: 0x0400130D RID: 4877
		public static SaveInfo[] SaveGames = new SaveInfo[5];

		// Token: 0x0400130E RID: 4878
		public static SaveInfo LastPlayedGame = null;

		// Token: 0x04001317 RID: 4887
		private List<LoadRequest> loadRequests = new List<LoadRequest>();

		// Token: 0x04001318 RID: 4888
		public List<ItemLoader> ItemLoaders = new List<ItemLoader>();

		// Token: 0x04001319 RID: 4889
		public List<BuildableItemLoader> ObjectLoaders = new List<BuildableItemLoader>();

		// Token: 0x0400131A RID: 4890
		public List<LegacyNPCLoader> LegacyNPCLoaders = new List<LegacyNPCLoader>();

		// Token: 0x0400131B RID: 4891
		public List<NPCLoader> NPCLoaders = new List<NPCLoader>();

		// Token: 0x0400131C RID: 4892
		public UnityEvent onPreSceneChange;

		// Token: 0x0400131D RID: 4893
		public UnityEvent onPreLoad;

		// Token: 0x0400131E RID: 4894
		public UnityEvent onLoadComplete;

		// Token: 0x0400131F RID: 4895
		public UnityEvent onSaveInfoLoaded;

		// Token: 0x0200037F RID: 895
		public enum ELoadStatus
		{
			// Token: 0x04001321 RID: 4897
			None,
			// Token: 0x04001322 RID: 4898
			LoadingScene,
			// Token: 0x04001323 RID: 4899
			Initializing,
			// Token: 0x04001324 RID: 4900
			LoadingData,
			// Token: 0x04001325 RID: 4901
			SpawningPlayer,
			// Token: 0x04001326 RID: 4902
			WaitingForHost
		}
	}
}
