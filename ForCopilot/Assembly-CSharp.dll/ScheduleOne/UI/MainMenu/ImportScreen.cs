using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B60 RID: 2912
	public class ImportScreen : MainMenuScreen
	{
		// Token: 0x06004D7A RID: 19834 RVA: 0x001461C4 File Offset: 0x001443C4
		public void Initialize(int _slotToOverwrite, MainMenuScreen previousScreen)
		{
			this.slotToOverwrite = _slotToOverwrite;
			this.PreviousScreen = previousScreen;
			bool flag = false;
			string tempImportPath = SaveImportButton.TempImportPath;
			string[] directories = Directory.GetDirectories(tempImportPath);
			if (directories.Length != 0)
			{
				string fileName = Path.GetFileName(directories[0]);
				string text = Path.Combine(tempImportPath, fileName);
				SaveInfo saveInfo;
				if (LoadManager.TryLoadSaveInfo(text, -1, out saveInfo, true))
				{
					Console.Log("Loaded save info from: " + text, null);
					this.saveInfo = saveInfo;
					flag = true;
					this.OrganisationNameLabel.text = saveInfo.OrganisationName;
					this.NetworthLabel.text = MoneyManager.FormatAmount(saveInfo.Networth, false, false);
					this.VersionLabel.text = "v" + saveInfo.SaveVersion;
					if (LoadManager.SaveGames[this.slotToOverwrite] != null)
					{
						this.WarningLabel.text = string.Concat(new string[]
						{
							"Warning: This will overwrite the current save in slot ",
							(this.slotToOverwrite + 1).ToString(),
							" (",
							LoadManager.SaveGames[this.slotToOverwrite].OrganisationName,
							")."
						});
						this.WarningLabel.enabled = true;
					}
					else
					{
						this.WarningLabel.enabled = false;
					}
				}
			}
			this.ConfirmButton.interactable = flag;
			this.MainContainer.SetActive(flag);
			this.FailContainer.SetActive(!flag);
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0014631F File Offset: 0x0014451F
		public void Cancel()
		{
			this.Close(true);
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x00146328 File Offset: 0x00144528
		public void Confirm()
		{
			if (this.saveInfo == null)
			{
				Console.LogError("No save info found to import.", null);
				return;
			}
			string text = Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "SaveGame_" + (this.slotToOverwrite + 1).ToString());
			string savePath = this.saveInfo.SavePath;
			if (Directory.Exists(text))
			{
				Directory.Delete(text, true);
			}
			Directory.CreateDirectory(text);
			ImportScreen.CopyFilesRecursively(savePath, text);
			string tempImportPath = SaveImportButton.TempImportPath;
			if (Directory.Exists(tempImportPath))
			{
				Directory.Delete(tempImportPath, true);
			}
			Singleton<LoadManager>.Instance.RefreshSaveInfo();
			this.Close(true);
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x001463C0 File Offset: 0x001445C0
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

		// Token: 0x040039BA RID: 14778
		[Header("References")]
		public GameObject MainContainer;

		// Token: 0x040039BB RID: 14779
		public GameObject FailContainer;

		// Token: 0x040039BC RID: 14780
		public Button ConfirmButton;

		// Token: 0x040039BD RID: 14781
		public TextMeshProUGUI OrganisationNameLabel;

		// Token: 0x040039BE RID: 14782
		public TextMeshProUGUI NetworthLabel;

		// Token: 0x040039BF RID: 14783
		public TextMeshProUGUI VersionLabel;

		// Token: 0x040039C0 RID: 14784
		public TextMeshProUGUI WarningLabel;

		// Token: 0x040039C1 RID: 14785
		private int slotToOverwrite;

		// Token: 0x040039C2 RID: 14786
		private SaveInfo saveInfo;
	}
}
