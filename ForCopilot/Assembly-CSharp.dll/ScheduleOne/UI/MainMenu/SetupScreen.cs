using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.ExtendedComponents;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B6C RID: 2924
	public class SetupScreen : MainMenuScreen
	{
		// Token: 0x06004DAE RID: 19886 RVA: 0x00146F5D File Offset: 0x0014515D
		protected virtual void Start()
		{
			this.InputField.onSubmit.AddListener(delegate(string <p0>)
			{
				this.StartGame();
			});
			this.SkipIntroContainer.gameObject.SetActive(true);
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x00146F8C File Offset: 0x0014518C
		public void Initialize(int index)
		{
			this.slotIndex = index;
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x00146F98 File Offset: 0x00145198
		private void Update()
		{
			if (base.IsOpen)
			{
				this.StartButton.interactable = (this.IsInputValid() && Singleton<Lobby>.Instance.IsHost);
				this.NotHostWarning.gameObject.SetActive(!Singleton<Lobby>.Instance.IsHost);
			}
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x00146FEC File Offset: 0x001451EC
		public void StartGame()
		{
			if (!this.IsInputValid())
			{
				return;
			}
			if (!Singleton<Lobby>.Instance.IsHost)
			{
				Console.LogWarning("Only the host can start the game.", null);
				return;
			}
			string text = Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "SaveGame_" + (this.slotIndex + 1).ToString());
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			this.ClearFolderContents(text);
			this.CopyDefaultSaveToFolder(text);
			string path = Path.Combine(text, "Game.json");
			int seed = UnityEngine.Random.Range(0, int.MaxValue);
			string json = new GameData(this.InputField.text, seed, new GameSettings()).GetJson(true);
			File.WriteAllText(path, json);
			bool isOn = this.SkipIntroToggle.isOn;
			string path2 = Path.Combine(text, "Metadata.json");
			string json2 = new MetaData(new DateTimeData(DateTime.Now), new DateTimeData(DateTime.Now), Application.version, Application.version, !isOn).GetJson(true);
			File.WriteAllText(path2, json2);
			Singleton<LoadManager>.Instance.RefreshSaveInfo();
			Singleton<LoadManager>.Instance.StartGame(LoadManager.SaveGames[this.slotIndex], false);
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0014710A File Offset: 0x0014530A
		private bool IsInputValid()
		{
			return !string.IsNullOrEmpty(this.InputField.text);
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x00147120 File Offset: 0x00145320
		private void ClearFolderContents(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(true);
			}
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x00147170 File Offset: 0x00145370
		private void CopyDefaultSaveToFolder(string folderPath)
		{
			SetupScreen.CopyFilesRecursively(Path.Combine(Application.streamingAssetsPath, "DefaultSave"), folderPath);
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x00147194 File Offset: 0x00145394
		private static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			string[] array = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < array.Length; i++)
			{
				Directory.CreateDirectory(array[i].Replace(sourcePath, targetPath));
			}
			foreach (string text in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				if (!text.EndsWith(".meta"))
				{
					File.Copy(text, text.Replace(sourcePath, targetPath), true);
				}
			}
		}

		// Token: 0x040039E7 RID: 14823
		public const string DEFAULT_SAVE_PATH = "DefaultSave";

		// Token: 0x040039E8 RID: 14824
		[Header("References")]
		public GameInputField InputField;

		// Token: 0x040039E9 RID: 14825
		public Button StartButton;

		// Token: 0x040039EA RID: 14826
		public RectTransform SkipIntroContainer;

		// Token: 0x040039EB RID: 14827
		public Toggle SkipIntroToggle;

		// Token: 0x040039EC RID: 14828
		public RectTransform NotHostWarning;

		// Token: 0x040039ED RID: 14829
		private int slotIndex;
	}
}
