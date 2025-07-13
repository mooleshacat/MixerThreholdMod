using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000395 RID: 917
	public class SaveManager : PersistentSingleton<SaveManager>
	{
		// Token: 0x060014C1 RID: 5313 RVA: 0x0005BFB8 File Offset: 0x0005A1B8
		public static void ReportSaveError()
		{
			SaveManager.SaveError = true;
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0005BFC0 File Offset: 0x0005A1C0
		// (set) Token: 0x060014C3 RID: 5315 RVA: 0x0005BFC8 File Offset: 0x0005A1C8
		public bool AccessPermissionIssueDetected { get; protected set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0005BFD1 File Offset: 0x0005A1D1
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x0005BFD9 File Offset: 0x0005A1D9
		public bool IsSaving { get; protected set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0005BFE2 File Offset: 0x0005A1E2
		// (set) Token: 0x060014C7 RID: 5319 RVA: 0x0005BFEA File Offset: 0x0005A1EA
		public float SecondsSinceLastSave { get; protected set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060014C8 RID: 5320 RVA: 0x0005BFF3 File Offset: 0x0005A1F3
		// (set) Token: 0x060014C9 RID: 5321 RVA: 0x0005BFFB File Offset: 0x0005A1FB
		public string PlayersSavePath { get; protected set; } = string.Empty;

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0005C004 File Offset: 0x0005A204
		// (set) Token: 0x060014CB RID: 5323 RVA: 0x0005C00C File Offset: 0x0005A20C
		public string IndividualSavesContainerPath { get; protected set; } = string.Empty;

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0005C015 File Offset: 0x0005A215
		// (set) Token: 0x060014CD RID: 5325 RVA: 0x0005C01D File Offset: 0x0005A21D
		public string SaveName { get; protected set; } = "DevSave";

		// Token: 0x060014CE RID: 5326 RVA: 0x0005C028 File Offset: 0x0005A228
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<SaveManager>.Instance == null || Singleton<SaveManager>.Instance != this)
			{
				return;
			}
			this.PlayersSavePath = Path.Combine(Application.persistentDataPath, "Saves");
			if (!Directory.Exists(this.PlayersSavePath))
			{
				Directory.CreateDirectory(this.PlayersSavePath);
			}
			if (Directory.GetDirectories(this.PlayersSavePath).Length == 0)
			{
				string path = Path.Combine(this.PlayersSavePath, "TempPlayer");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
			string[] directories = Directory.GetDirectories(this.PlayersSavePath);
			if (directories.Length > 1)
			{
				for (int i = 0; i < directories.Length; i++)
				{
					if (!directories[i].Contains("TempPlayer"))
					{
						this.IndividualSavesContainerPath = directories[i];
						return;
					}
				}
				return;
			}
			this.IndividualSavesContainerPath = directories[0];
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0005C0F5 File Offset: 0x0005A2F5
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			this.CheckSaveFolderInitialized();
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0005C120 File Offset: 0x0005A320
		public void CheckSaveFolderInitialized()
		{
			if (this.saveFolderInitialized)
			{
				return;
			}
			this.saveFolderInitialized = true;
			if (SteamManager.Initialized)
			{
				string path = SteamUser.GetSteamID().ToString();
				string text = Path.Combine(this.PlayersSavePath, path);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				this.IndividualSavesContainerPath = text;
				Console.Log("Initialized individual save folder path: " + this.IndividualSavesContainerPath, null);
			}
			else
			{
				Console.LogError("Steamworks not intialized in time for SaveManager! Using save container path: " + this.IndividualSavesContainerPath, null);
			}
			if (SaveManager.HasWritePermissionOnDir(this.IndividualSavesContainerPath))
			{
				this.AccessPermissionIssueDetected = false;
				Console.Log("Successfully verified write permission on save folder: " + this.IndividualSavesContainerPath, null);
				if (this.WriteIssueDisplay != null)
				{
					this.WriteIssueDisplay.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				this.AccessPermissionIssueDetected = true;
				Console.LogError("No write permission on save folder: " + this.IndividualSavesContainerPath, null);
				if (this.WriteIssueDisplay != null)
				{
					this.WriteIssueDisplay.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0005C230 File Offset: 0x0005A430
		public static bool HasWritePermissionOnDir(string path)
		{
			bool result = false;
			string path2 = Path.Combine(path, "WriteTest.txt");
			if (Directory.Exists(path))
			{
				try
				{
					File.WriteAllText(path2, "If you're reading this, it means Schedule I can write save files properly - Yay!");
					if (File.Exists(path2))
					{
						result = true;
					}
				}
				catch (Exception)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0005C280 File Offset: 0x0005A480
		private void Update()
		{
			if (Singleton<LoadManager>.Instance.IsGameLoaded && Singleton<LoadManager>.Instance.LoadedGameFolderPath != string.Empty && Input.GetKeyDown(KeyCode.F5) && (Application.isEditor || Debug.isDebugBuild))
			{
				this.Save();
			}
			if (Singleton<LoadManager>.Instance.IsGameLoaded)
			{
				this.SecondsSinceLastSave += Time.unscaledDeltaTime;
				return;
			}
			this.SecondsSinceLastSave = 0f;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0005C2F9 File Offset: 0x0005A4F9
		public void DelayedSave()
		{
			base.Invoke("Save", 1f);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0005C30B File Offset: 0x0005A50B
		public void Save()
		{
			this.Save(Singleton<LoadManager>.Instance.LoadedGameFolderPath);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0005C320 File Offset: 0x0005A520
		public void Save(string saveFolderPath)
		{
			SaveManager.<>c__DisplayClass51_0 CS$<>8__locals1 = new SaveManager.<>c__DisplayClass51_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.saveFolderPath = saveFolderPath;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.LoadedGameFolderPath == string.Empty)
			{
				Console.LogWarning("No game loaded to save", null);
				return;
			}
			if (this.IsSaving)
			{
				Console.LogWarning("Save called while saving is already in progress", null);
				return;
			}
			if (NetworkSingleton<GameManager>.Instance.IsTutorial && !Application.isEditor)
			{
				Console.LogWarning("Can't save during tutorial", null);
				return;
			}
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				CS$<>8__locals1.saveFolderPath = Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "DevSave");
			}
			Console.Log("Saving game to " + CS$<>8__locals1.saveFolderPath, null);
			this.IsSaving = true;
			if (this.onSaveStart != null)
			{
				this.onSaveStart.Invoke();
			}
			this.CompletedSaveables.Clear();
			this.ApprovedBaseLevelPaths.Clear();
			SaveManager.SaveError = false;
			base.StartCoroutine(CS$<>8__locals1.<Save>g__SaveRoutine|0());
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0005C420 File Offset: 0x0005A620
		private void ClearBaseLevelOutdatedSaves(string saveFolderPath)
		{
			string[] array = null;
			string[] array2 = null;
			try
			{
				array = Directory.GetFiles(saveFolderPath);
			}
			catch (Exception ex)
			{
				string str = "Failed to get files in folder: ";
				string str2 = "\nException: ";
				Exception ex2 = ex;
				Console.LogError(str + saveFolderPath + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				return;
			}
			try
			{
				array2 = Directory.GetDirectories(saveFolderPath);
			}
			catch (Exception ex3)
			{
				string str3 = "Failed to get folders in folder: ";
				string str4 = "\nException: ";
				Exception ex4 = ex3;
				Console.LogError(str3 + saveFolderPath + str4 + ((ex4 != null) ? ex4.ToString() : null), null);
				return;
			}
			if (array == null || array2 == null)
			{
				Console.LogError("Failed to get files or folders in folder: " + saveFolderPath, null);
				return;
			}
			foreach (string text in array)
			{
				FileInfo fileInfo = new FileInfo(text);
				if (!this.ApprovedBaseLevelPaths.Contains(fileInfo.Name))
				{
					try
					{
						File.Delete(text);
					}
					catch (Exception ex5)
					{
						string str5 = "Failed to delete file: ";
						string str6 = text;
						string str7 = "\nException: ";
						Exception ex6 = ex5;
						Console.LogError(str5 + str6 + str7 + ((ex6 != null) ? ex6.ToString() : null), null);
					}
				}
			}
			foreach (string text2 in array2)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text2);
				if (!this.ApprovedBaseLevelPaths.Contains(directoryInfo.Name))
				{
					try
					{
						Directory.Delete(text2, true);
					}
					catch (Exception ex7)
					{
						string str8 = "Failed to delete folder: ";
						string str9 = text2;
						string str10 = "\nException: ";
						Exception ex8 = ex7;
						Console.LogError(str8 + str9 + str10 + ((ex8 != null) ? ex8.ToString() : null), null);
					}
				}
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0005C5C0 File Offset: 0x0005A7C0
		public void CompleteSaveable(ISaveable saveable)
		{
			if (this.CompletedSaveables.Contains(saveable))
			{
				Console.LogWarning("Saveable already completed", null);
				return;
			}
			this.CompletedSaveables.Add(saveable);
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0005C5E8 File Offset: 0x0005A7E8
		public void ClearCompletedSaveable(ISaveable saveable)
		{
			this.CompletedSaveables.Remove(saveable);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0005C5F7 File Offset: 0x0005A7F7
		public void RegisterSaveable(ISaveable saveable)
		{
			if (this.Saveables.Contains(saveable))
			{
				return;
			}
			this.Saveables.Add(saveable);
			if (saveable is IBaseSaveable)
			{
				this.BaseSaveables.Add(saveable as IBaseSaveable);
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0005C62D File Offset: 0x0005A82D
		public void QueueSaveRequest(SaveRequest request)
		{
			this.QueuedSaveRequests.Add(request);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0005C63B File Offset: 0x0005A83B
		public void DequeueSaveRequest(SaveRequest request)
		{
			this.QueuedSaveRequests.Remove(request);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0005C64A File Offset: 0x0005A84A
		public static string StripExtensions(string filePath)
		{
			return filePath.Replace(".json", string.Empty);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0005C65C File Offset: 0x0005A85C
		public static string MakeFileSafe(string fileName)
		{
			foreach (char oldChar in Path.GetInvalidFileNameChars())
			{
				fileName = fileName.Replace(oldChar, '-');
			}
			return fileName;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0005C690 File Offset: 0x0005A890
		public static float GetVersionNumber(string version)
		{
			version.ToLower().Contains("alternate");
			version = version.Replace(".", string.Empty);
			version = version.Replace("f", ".");
			version = Regex.Replace(version, "[^\\d.]", string.Empty);
			version = version.TrimStart('0');
			float result;
			if (!float.TryParse(version, out result))
			{
				Console.LogError("Failed to parse version number: " + version, null);
				return 0f;
			}
			return result;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0005C710 File Offset: 0x0005A910
		private void Clean()
		{
			this.Saveables.Clear();
			this.BaseSaveables.Clear();
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0005C728 File Offset: 0x0005A928
		public void DisablePlayTutorial(SaveInfo info)
		{
			string path = Path.Combine(info.SavePath, "Metadata.json");
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
					return;
				}
				MetaData metaData = JsonUtility.FromJson<MetaData>(text);
				metaData.PlayTutorial = false;
				try
				{
					File.WriteAllText(path, metaData.GetJson(true));
					Console.Log("Successfully disabled tutorial in metadata file", null);
				}
				catch (Exception ex2)
				{
					string str = "Failed to modify metadata file. Exception: ";
					Exception ex3 = ex2;
					Console.LogError(str + ((ex3 != null) ? ex3.ToString() : null), null);
				}
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0005C7E0 File Offset: 0x0005A9E0
		public static string SanitizeFileName(string fileName)
		{
			foreach (char oldChar in Path.GetInvalidFileNameChars())
			{
				fileName = fileName.Replace(oldChar, '_');
			}
			return fileName;
		}

		// Token: 0x0400136C RID: 4972
		public const string MAIN_SCENE_NAME = "Main";

		// Token: 0x0400136D RID: 4973
		public const string MENU_SCENE_NAME = "Menu";

		// Token: 0x0400136E RID: 4974
		public const string TUTORIAL_SCENE_NAME = "Tutorial";

		// Token: 0x0400136F RID: 4975
		public const int SAVES_PER_FRAME = 15;

		// Token: 0x04001370 RID: 4976
		public const string SAVE_FILE_EXTENSION = ".json";

		// Token: 0x04001371 RID: 4977
		public const int SAVE_SLOT_COUNT = 5;

		// Token: 0x04001372 RID: 4978
		public const string SAVE_GAME_PREFIX = "SaveGame_";

		// Token: 0x04001373 RID: 4979
		public const bool DEBUG = false;

		// Token: 0x04001374 RID: 4980
		public const bool PRETTY_PRINT = true;

		// Token: 0x04001375 RID: 4981
		public static bool SaveError;

		// Token: 0x0400137C RID: 4988
		public List<ISaveable> Saveables = new List<ISaveable>();

		// Token: 0x0400137D RID: 4989
		public List<IBaseSaveable> BaseSaveables = new List<IBaseSaveable>();

		// Token: 0x0400137E RID: 4990
		[HideInInspector]
		public List<string> ApprovedBaseLevelPaths = new List<string>();

		// Token: 0x0400137F RID: 4991
		protected List<ISaveable> CompletedSaveables = new List<ISaveable>();

		// Token: 0x04001380 RID: 4992
		protected List<SaveRequest> QueuedSaveRequests = new List<SaveRequest>();

		// Token: 0x04001381 RID: 4993
		[Header("References")]
		public RectTransform WriteIssueDisplay;

		// Token: 0x04001382 RID: 4994
		[Header("Events")]
		public UnityEvent onSaveStart;

		// Token: 0x04001383 RID: 4995
		public UnityEvent onSaveComplete;

		// Token: 0x04001384 RID: 4996
		private bool saveFolderInitialized;
	}
}
