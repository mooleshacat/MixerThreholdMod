using System;
using System.IO;
using System.IO.Compression;
using ScheduleOne.Persistence;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B6A RID: 2922
	[RequireComponent(typeof(Button))]
	public class SaveExportButton : MonoBehaviour
	{
		// Token: 0x06004DA3 RID: 19875 RVA: 0x00146D18 File Offset: 0x00144F18
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.Clicked));
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x00146D38 File Offset: 0x00144F38
		private void Clicked()
		{
			(new ExtensionFilter[1])[0] = new ExtensionFilter("Zip Files", new string[]
			{
				"zip"
			});
			SaveInfo saveInfo = LoadManager.SaveGames[this.SaveSlotIndex];
			string text = SaveExportButton.ShowSaveFileDialog(SaveManager.MakeFileSafe(saveInfo.OrganisationName));
			if (!string.IsNullOrEmpty(text))
			{
				Console.Log("Exporting save file to: " + text, null);
				SaveExportButton.ZipSaveFolder(saveInfo.SavePath, text);
				Debug.Log("Save exported to: " + text);
			}
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x00146DC0 File Offset: 0x00144FC0
		public static string ShowSaveFileDialog(string fileName)
		{
			ExtensionFilter[] extensions = new ExtensionFilter[]
			{
				new ExtensionFilter("Zip Files", new string[]
				{
					"zip"
				})
			};
			return StandaloneFileBrowser.SaveFilePanel("Export Save File", "", fileName + ".zip", extensions);
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x00146E0E File Offset: 0x0014500E
		public static void ZipSaveFolder(string sourceFolderPath, string destinationZipPath)
		{
			if (File.Exists(destinationZipPath))
			{
				File.Delete(destinationZipPath);
			}
			ZipFile.CreateFromDirectory(sourceFolderPath, destinationZipPath, System.IO.Compression.CompressionLevel.Optimal, true);
		}

		// Token: 0x040039E3 RID: 14819
		public int SaveSlotIndex;
	}
}
