using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B6B RID: 2923
	[RequireComponent(typeof(Button))]
	public class SaveImportButton : MonoBehaviour
	{
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x00146E27 File Offset: 0x00145027
		public static string TempImportPath
		{
			get
			{
				return Path.Combine(Singleton<SaveManager>.Instance.IndividualSavesContainerPath, "TempImport");
			}
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x00146E3D File Offset: 0x0014503D
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.Clicked));
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x00146E5C File Offset: 0x0014505C
		private void Clicked()
		{
			(new ExtensionFilter[1])[0] = new ExtensionFilter("Zip Files", new string[]
			{
				"zip"
			});
			string text = SaveImportButton.ShowOpenFileDialog();
			if (!string.IsNullOrEmpty(text))
			{
				string tempImportPath = SaveImportButton.TempImportPath;
				if (Directory.Exists(tempImportPath))
				{
					Directory.Delete(tempImportPath, true);
				}
				SaveImportButton.UnzipSaveFile(text, tempImportPath);
				this.ImportScreen.Initialize(this.SaveSlotIndex, this.ParentScreen);
				this.ImportScreen.Open(true);
			}
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x00146EDC File Offset: 0x001450DC
		public static void UnzipSaveFile(string zipFilePath, string destinationFolderPath)
		{
			Console.Log("Unzipping from " + zipFilePath + " to " + destinationFolderPath, null);
			if (Directory.Exists(destinationFolderPath))
			{
				Directory.Delete(destinationFolderPath, true);
			}
			Directory.CreateDirectory(destinationFolderPath);
			ZipFile.ExtractToDirectory(zipFilePath, destinationFolderPath);
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x00146F14 File Offset: 0x00145114
		public static string ShowOpenFileDialog()
		{
			ExtensionFilter[] extensions = new ExtensionFilter[]
			{
				new ExtensionFilter("Zip Files", new string[]
				{
					"zip"
				})
			};
			return StandaloneFileBrowser.OpenFilePanel("Import Save File", "", extensions, false).FirstOrDefault<string>();
		}

		// Token: 0x040039E4 RID: 14820
		public ImportScreen ImportScreen;

		// Token: 0x040039E5 RID: 14821
		public MainMenuScreen ParentScreen;

		// Token: 0x040039E6 RID: 14822
		public int SaveSlotIndex;
	}
}
