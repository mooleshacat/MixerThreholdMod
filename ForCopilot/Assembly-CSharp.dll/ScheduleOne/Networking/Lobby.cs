using System;
using System.Linq;
using System.Text;
using EasyButtons;
using FishNet.Managing;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using ScheduleOne.UI.MainMenu;
using Steamworks;
using UnityEngine;

namespace ScheduleOne.Networking
{
	// Token: 0x02000576 RID: 1398
	public class Lobby : PersistentSingleton<Lobby>
	{
		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x0008AE33 File Offset: 0x00089033
		public bool IsHost
		{
			get
			{
				return !this.IsInLobby || (this.Players.Length != 0 && this.Players[0] == this.LocalPlayerID);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x0008AE61 File Offset: 0x00089061
		// (set) Token: 0x060021B1 RID: 8625 RVA: 0x0008AE69 File Offset: 0x00089069
		public ulong LobbyID { get; private set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060021B2 RID: 8626 RVA: 0x0008AE72 File Offset: 0x00089072
		public CSteamID LobbySteamID
		{
			get
			{
				return new CSteamID(this.LobbyID);
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x0008AE7F File Offset: 0x0008907F
		public bool IsInLobby
		{
			get
			{
				return this.LobbyID > 0UL;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x0008AE8B File Offset: 0x0008908B
		public int PlayerCount
		{
			get
			{
				if (!this.IsInLobby)
				{
					return 1;
				}
				return this.Players.Count((CSteamID p) => p != CSteamID.Nil);
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x0008AEC1 File Offset: 0x000890C1
		// (set) Token: 0x060021B6 RID: 8630 RVA: 0x0008AEC9 File Offset: 0x000890C9
		public CSteamID LocalPlayerID { get; private set; } = CSteamID.Nil;

		// Token: 0x060021B7 RID: 8631 RVA: 0x0008AED2 File Offset: 0x000890D2
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Lobby>.Instance == null || Singleton<Lobby>.Instance != this)
			{
				return;
			}
			bool destroyed = this.Destroyed;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0008AEFC File Offset: 0x000890FC
		protected override void Start()
		{
			base.Start();
			if (Singleton<Lobby>.Instance == null || Singleton<Lobby>.Instance != this)
			{
				return;
			}
			if (this.Destroyed)
			{
				return;
			}
			if (!SteamManager.Initialized)
			{
				Debug.LogError("Steamworks not initialized");
				return;
			}
			this.LocalPlayerID = SteamUser.GetSteamID();
			this.InitializeCallbacks();
			string launchLobby = this.GetLaunchLobby();
			if (launchLobby != null && launchLobby != string.Empty && SteamManager.Initialized)
			{
				try
				{
					SteamMatchmaking.JoinLobby(new CSteamID(ulong.Parse(launchLobby)));
				}
				catch
				{
					Console.LogWarning("There is an issue with launch commands.", null);
				}
			}
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0008AFA8 File Offset: 0x000891A8
		private void InitializeCallbacks()
		{
			this.LobbyCreatedCallback = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(this.OnLobbyCreated));
			this.LobbyEnteredCallback = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this.OnLobbyEntered));
			this.ChatUpdateCallback = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(this.PlayerEnterOrLeave));
			this.GameLobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.LobbyJoinRequested));
			this.LobbyChatMessageCallback = Callback<LobbyChatMsg_t>.Create(new Callback<LobbyChatMsg_t>.DispatchDelegate(this.OnLobbyChatMessage));
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0008B028 File Offset: 0x00089228
		public void TryOpenInviteInterface()
		{
			if (!this.IsInLobby)
			{
				Console.Log("Not currently in a lobby, creating one...", null);
				this.CreateLobby();
			}
			if (SteamMatchmaking.GetNumLobbyMembers(this.LobbySteamID) >= 4)
			{
				Debug.LogWarning("Lobby already at max capacity!");
				return;
			}
			SteamFriends.ActivateGameOverlayInviteDialog(this.LobbySteamID);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0008B068 File Offset: 0x00089268
		public void LeaveLobby()
		{
			if (this.IsInLobby)
			{
				SteamMatchmaking.LeaveLobby(this.LobbySteamID);
				Console.Log("Leaving lobby: " + this.LobbyID.ToString(), null);
			}
			this.LobbyID = 0UL;
			this.UpdateLobbyMembers();
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0008B0C7 File Offset: 0x000892C7
		private void CreateLobby()
		{
			SteamMatchmaking.CreateLobby(1, 4);
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0008B0D4 File Offset: 0x000892D4
		private string GetLaunchLobby()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].ToLower() == "+connect_lobby" && commandLineArgs.Length > i + 1)
				{
					return commandLineArgs[i + 1];
				}
			}
			return string.Empty;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0008B11C File Offset: 0x0008931C
		private void UpdateLobbyMembers()
		{
			for (int i = 0; i < this.Players.Length; i++)
			{
				this.Players[i] = CSteamID.Nil;
			}
			int num = this.IsInLobby ? SteamMatchmaking.GetNumLobbyMembers(this.LobbySteamID) : 0;
			for (int j = 0; j < num; j++)
			{
				this.Players[j] = SteamMatchmaking.GetLobbyMemberByIndex(this.LobbySteamID, j);
			}
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0008B188 File Offset: 0x00089388
		[Button]
		public void DebugJoin()
		{
			this.JoinAsClient(this.DebugSteamId64);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0008B196 File Offset: 0x00089396
		public void JoinAsClient(string steamId64)
		{
			Singleton<LoadManager>.Instance.LoadAsClient(steamId64);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x0008B1A4 File Offset: 0x000893A4
		public void SendLobbyMessage(string message)
		{
			if (!this.IsInLobby)
			{
				Console.LogWarning("Not in a lobby, cannot send message.", null);
				return;
			}
			byte[] bytes = Encoding.ASCII.GetBytes(message);
			SteamMatchmaking.SendLobbyChatMsg(this.LobbySteamID, bytes, bytes.Length);
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x0008B1E1 File Offset: 0x000893E1
		public void SetLobbyData(string key, string value)
		{
			if (!this.IsInLobby)
			{
				Console.LogWarning("Not in a lobby, cannot set data.", null);
				return;
			}
			SteamMatchmaking.SetLobbyData(this.LobbySteamID, key, value);
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x0008B208 File Offset: 0x00089408
		private void OnLobbyCreated(LobbyCreated_t result)
		{
			if (result.m_eResult == 1)
			{
				Console.Log("Lobby created: " + result.m_ulSteamIDLobby.ToString(), null);
			}
			else
			{
				Console.LogWarning("Lobby creation failed: " + result.m_eResult.ToString(), null);
			}
			this.LobbyID = result.m_ulSteamIDLobby;
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "owner", SteamUser.GetSteamID().ToString());
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "version", Application.version);
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "host_loading", "false");
			SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "ready", "false");
			this.UpdateLobbyMembers();
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0008B300 File Offset: 0x00089500
		private void OnLobbyEntered(LobbyEnter_t result)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData(new CSteamID(result.m_ulSteamIDLobby), "version");
			Console.Log("Lobby version: " + lobbyData + ", client version: " + Application.version, null);
			if (lobbyData != Application.version)
			{
				Console.LogWarning("Lobby version mismatch, cannot join.", null);
				if (Singleton<MainMenuPopup>.InstanceExists)
				{
					Singleton<MainMenuPopup>.Instance.Open("Version Mismatch", "Host version: " + lobbyData + "\nYour version: " + Application.version, true);
				}
				this.LeaveLobby();
				return;
			}
			Console.Log("Entered lobby: " + result.m_ulSteamIDLobby.ToString(), null);
			this.LobbyID = result.m_ulSteamIDLobby;
			this.UpdateLobbyMembers();
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
			string lobbyData2 = SteamMatchmaking.GetLobbyData(this.LobbySteamID, "ready");
			bool flag = SteamMatchmaking.GetLobbyData(this.LobbySteamID, "load_tutorial") == "true";
			bool flag2 = SteamMatchmaking.GetLobbyData(this.LobbySteamID, "host_loading") == "true";
			if (lobbyData2 == "true" && !this.IsHost)
			{
				this.JoinAsClient(SteamMatchmaking.GetLobbyOwner(this.LobbySteamID).m_SteamID.ToString());
				return;
			}
			if (flag && !this.IsHost)
			{
				Singleton<LoadManager>.Instance.LoadTutorialAsClient();
				return;
			}
			if (flag2 && !this.IsHost)
			{
				Singleton<LoadManager>.Instance.SetWaitingForHostLoad();
				Singleton<LoadingScreen>.Instance.Open(false);
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x0008B47C File Offset: 0x0008967C
		private void PlayerEnterOrLeave(LobbyChatUpdate_t result)
		{
			Console.Log("Player join/leave: " + SteamFriends.GetFriendPersonaName(new CSteamID(result.m_ulSteamIDUserChanged)), null);
			this.UpdateLobbyMembers();
			if (result.m_ulSteamIDMakingChange == this.LobbySteamID.m_SteamID && result.m_ulSteamIDUserChanged != this.LocalPlayerID.m_SteamID)
			{
				Console.Log("Lobby owner left, leaving lobby.", null);
				this.LeaveLobby();
			}
			if (this.onLobbyChange != null)
			{
				this.onLobbyChange();
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0008B4FC File Offset: 0x000896FC
		private void LobbyJoinRequested(GameLobbyJoinRequested_t result)
		{
			string str = "Join requested: ";
			CSteamID steamIDLobby = result.m_steamIDLobby;
			Console.Log(str + steamIDLobby.ToString(), null);
			if (this.LobbyID != 0UL)
			{
				this.LeaveLobby();
			}
			SteamMatchmaking.JoinLobby(result.m_steamIDLobby);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0008B548 File Offset: 0x00089748
		private void OnLobbyChatMessage(LobbyChatMsg_t result)
		{
			byte[] array = new byte[128];
			int num = 128;
			CSteamID csteamID;
			EChatEntryType echatEntryType;
			SteamMatchmaking.GetLobbyChatEntry(new CSteamID(this.LobbyID), (int)result.m_iChatID, ref csteamID, array, num, ref echatEntryType);
			string text = Encoding.ASCII.GetString(array);
			text = text.TrimEnd(new char[1]);
			Console.Log("Lobby chat message received: " + text, null);
			if (!this.IsHost && !Singleton<LoadManager>.Instance.IsGameLoaded)
			{
				if (text == "ready")
				{
					this.JoinAsClient(csteamID.m_SteamID.ToString());
					return;
				}
				if (text == "load_tutorial")
				{
					Singleton<LoadManager>.Instance.LoadTutorialAsClient();
					return;
				}
				if (text == "host_loading")
				{
					Singleton<LoadManager>.Instance.SetWaitingForHostLoad();
					Singleton<LoadingScreen>.Instance.Open(false);
				}
			}
		}

		// Token: 0x040019C7 RID: 6599
		public const bool ENABLED = true;

		// Token: 0x040019C8 RID: 6600
		public const int PLAYER_LIMIT = 4;

		// Token: 0x040019C9 RID: 6601
		public const string JOIN_READY = "ready";

		// Token: 0x040019CA RID: 6602
		public const string LOAD_TUTORIAL = "load_tutorial";

		// Token: 0x040019CB RID: 6603
		public const string HOST_LOADING = "host_loading";

		// Token: 0x040019CC RID: 6604
		public NetworkManager NetworkManager;

		// Token: 0x040019CF RID: 6607
		public CSteamID[] Players = new CSteamID[4];

		// Token: 0x040019D0 RID: 6608
		public Action onLobbyChange;

		// Token: 0x040019D1 RID: 6609
		private Callback<LobbyCreated_t> LobbyCreatedCallback;

		// Token: 0x040019D2 RID: 6610
		private Callback<LobbyEnter_t> LobbyEnteredCallback;

		// Token: 0x040019D3 RID: 6611
		private Callback<LobbyChatUpdate_t> ChatUpdateCallback;

		// Token: 0x040019D4 RID: 6612
		private Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequestedCallback;

		// Token: 0x040019D5 RID: 6613
		private Callback<LobbyChatMsg_t> LobbyChatMessageCallback;

		// Token: 0x040019D6 RID: 6614
		public string DebugSteamId64 = string.Empty;
	}
}
