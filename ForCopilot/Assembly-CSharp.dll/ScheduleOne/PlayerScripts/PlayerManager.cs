using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using Steamworks;
using Unity.AI.Navigation;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000632 RID: 1586
	public class PlayerManager : Singleton<PlayerManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060028E2 RID: 10466 RVA: 0x000A8438 File Offset: 0x000A6638
		public string SaveFolderName
		{
			get
			{
				return "Players";
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000A8438 File Offset: 0x000A6638
		public string SaveFileName
		{
			get
			{
				return "Players";
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x000A843F File Offset: 0x000A663F
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000A8447 File Offset: 0x000A6647
		// (set) Token: 0x060028E7 RID: 10471 RVA: 0x000A844F File Offset: 0x000A664F
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x000A8458 File Offset: 0x000A6658
		// (set) Token: 0x060028E9 RID: 10473 RVA: 0x000A8460 File Offset: 0x000A6660
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x000A8469 File Offset: 0x000A6669
		// (set) Token: 0x060028EB RID: 10475 RVA: 0x000A8471 File Offset: 0x000A6671
		public bool HasChanged { get; set; }

		// Token: 0x060028EC RID: 10476 RVA: 0x000A847A File Offset: 0x000A667A
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x00092542 File Offset: 0x00090742
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000A8488 File Offset: 0x000A6688
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			int i;
			int j;
			for (i = 0; i < Player.PlayerList.Count; i = j + 1)
			{
				new SaveRequest(Player.PlayerList[i], containerFolder);
				list.Add(Player.PlayerList[i].SaveFolderName);
				if (!this.loadedPlayerData.Exists((PlayerData PlayerData) => PlayerData.PlayerCode == Player.PlayerList[i].PlayerCode))
				{
					this.loadedPlayerData.Add(Player.PlayerList[i].GetPlayerData());
					this.loadedPlayerDataPaths.Add(Path.Combine(containerFolder, Player.PlayerList[i].SaveFolderName));
					this.loadedPlayerFileNames.Add(Player.PlayerList[i].SaveFolderName);
				}
				j = i;
			}
			string[] collection = Directory.GetDirectories(containerFolder).Select(new Func<string, string>(Path.GetFileName)).ToArray<string>();
			list.AddRange(collection);
			list.AddRange(this.loadedPlayerFileNames);
			return list;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000A85C0 File Offset: 0x000A67C0
		public void SavePlayer(Player player)
		{
			Console.Log("Saving player: " + player.PlayerCode, null);
			string text = Path.Combine(Singleton<LoadManager>.Instance.LoadedGameFolderPath, this.SaveFolderName);
			Singleton<SaveManager>.Instance.ClearCompletedSaveable(player);
			string saveString = player.GetSaveString();
			((ISaveable)player).WriteBaseData(text, saveString);
			player.WriteData(text);
			PlayerData playerData = this.loadedPlayerData.FirstOrDefault((PlayerData PlayerData) => PlayerData.PlayerCode == player.PlayerCode);
			if (playerData != null)
			{
				int index = this.loadedPlayerData.IndexOf(playerData);
				this.loadedPlayerData[index] = player.GetPlayerData();
				return;
			}
			this.loadedPlayerData.Add(player.GetPlayerData());
			this.loadedPlayerDataPaths.Add(Path.Combine(text, player.SaveFolderName));
			this.loadedPlayerFileNames.Add(player.SaveFolderName);
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000A86CC File Offset: 0x000A68CC
		public void LoadPlayer(PlayerData data, string containerPath)
		{
			this.loadedPlayerData.Add(data);
			this.loadedPlayerDataPaths.Add(containerPath);
			this.loadedPlayerFileNames.Add(Path.GetFileName(containerPath));
			Player player = Player.PlayerList.FirstOrDefault((Player Player) => Player.PlayerCode == data.PlayerCode);
			if (player == null && InstanceFinder.IsServer)
			{
				string fileName = Path.GetFileName(containerPath);
				if (fileName == "Player_Local" || fileName == "Player_0")
				{
					player = Player.Local;
				}
			}
			if (player != null)
			{
				player.Load(data, containerPath);
			}
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000A8778 File Offset: 0x000A6978
		public void AllPlayerFilesLoaded()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			string text = string.Empty;
			if (SteamManager.Initialized)
			{
				text = SteamUser.GetSteamID().ToString();
			}
			if (this.loadedPlayerFileNames.Contains("Player_0"))
			{
				int index = this.loadedPlayerFileNames.IndexOf("Player_0");
				Player.Local.Load(this.loadedPlayerData[index], this.loadedPlayerDataPaths[index]);
				return;
			}
			if (text != string.Empty && this.loadedPlayerFileNames.Contains("Player_" + text))
			{
				int index2 = this.loadedPlayerFileNames.IndexOf("Player_" + text);
				Player.Local.Load(this.loadedPlayerData[index2], this.loadedPlayerDataPaths[index2]);
				return;
			}
			if (this.loadedPlayerFileNames.Contains("Player_Local"))
			{
				int index3 = this.loadedPlayerFileNames.IndexOf("Player_Local");
				Player.Local.Load(this.loadedPlayerData[index3], this.loadedPlayerDataPaths[index3]);
				return;
			}
			if (this.loadedPlayerData.Count > 0)
			{
				Player.Local.Load(this.loadedPlayerData[0], this.loadedPlayerDataPaths[0]);
				return;
			}
			Console.LogWarning("Couldn't find any data for host player. This is fine if this is a new game, but not if this is a loaded game.", null);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000A88D8 File Offset: 0x000A6AD8
		public bool TryGetPlayerData(string playerCode, out PlayerData data, out string inventoryString, out string appearanceString, out string clothingString, out VariableData[] variables)
		{
			data = this.loadedPlayerData.FirstOrDefault((PlayerData PlayerData) => PlayerData.PlayerCode == playerCode);
			inventoryString = string.Empty;
			appearanceString = string.Empty;
			clothingString = string.Empty;
			variables = null;
			List<VariableData> list = new List<VariableData>();
			if (data != null)
			{
				string text = this.loadedPlayerDataPaths[this.loadedPlayerData.IndexOf(data)];
				PlayerLoader playerLoader = new PlayerLoader();
				string text2;
				if (playerLoader.TryLoadFile(text, "Inventory", out text2))
				{
					inventoryString = text2;
				}
				else
				{
					Console.LogWarning("Failed to load player inventory under " + text, null);
				}
				string text3;
				if (playerLoader.TryLoadFile(text, "Appearance", out text3))
				{
					appearanceString = text3;
				}
				else
				{
					Console.LogWarning("Failed to load player appearance under " + text, null);
				}
				string text4;
				if (playerLoader.TryLoadFile(text, "Clothing", out text4))
				{
					clothingString = text4;
				}
				else
				{
					Console.LogWarning("Failed to load player clothing under " + text, null);
				}
				bool flag = false;
				string text5;
				if (playerLoader.TryLoadFile(text, "Variables", out text5))
				{
					VariableCollectionData variableCollectionData = null;
					try
					{
						variableCollectionData = JsonUtility.FromJson<VariableCollectionData>(text5);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error loading player variable data: " + ex.Message);
					}
					if (data != null)
					{
						flag = true;
						foreach (VariableData variableData in variableCollectionData.Variables)
						{
							if (variableData != null)
							{
								list.Add(variableData);
							}
						}
					}
				}
				if (!flag)
				{
					string text6 = Path.Combine(text, "Variables");
					if (Directory.Exists(text6))
					{
						Console.Log("Loading legacy player variables from " + text6, null);
						string[] files = Directory.GetFiles(text6);
						VariablesLoader variablesLoader = new VariablesLoader();
						for (int j = 0; j < files.Length; j++)
						{
							string text7;
							if (variablesLoader.TryLoadFile(files[j], out text7, false))
							{
								VariableData item = null;
								try
								{
									item = JsonUtility.FromJson<VariableData>(text7);
								}
								catch (Exception ex2)
								{
									Debug.LogError("Error loading player variable data: " + ex2.Message);
								}
								if (data != null)
								{
									list.Add(item);
								}
							}
						}
					}
					else
					{
						Console.LogWarning("Failed to load player variables under " + text, null);
					}
				}
			}
			if (list.Count > 0)
			{
				variables = list.ToArray();
			}
			return data != null;
		}

		// Token: 0x04001D6C RID: 7532
		private PlayersLoader loader = new PlayersLoader();

		// Token: 0x04001D70 RID: 7536
		[SerializeField]
		protected List<PlayerData> loadedPlayerData = new List<PlayerData>();

		// Token: 0x04001D71 RID: 7537
		protected List<string> loadedPlayerDataPaths = new List<string>();

		// Token: 0x04001D72 RID: 7538
		protected List<string> loadedPlayerFileNames = new List<string>();

		// Token: 0x04001D73 RID: 7539
		public NavMeshSurface PlayerRecoverySurface;
	}
}
