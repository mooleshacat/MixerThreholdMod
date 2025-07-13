using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Loaders;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037B RID: 891
	public interface ISaveable
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001424 RID: 5156
		string SaveFolderName { get; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001425 RID: 5157
		string SaveFileName { get; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001426 RID: 5158
		Loader Loader { get; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001427 RID: 5159
		bool ShouldSaveUnderFolder { get; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001428 RID: 5160
		// (set) Token: 0x06001429 RID: 5161
		List<string> LocalExtraFiles { get; set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600142A RID: 5162
		// (set) Token: 0x0600142B RID: 5163
		List<string> LocalExtraFolders { get; set; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600142C RID: 5164
		// (set) Token: 0x0600142D RID: 5165
		bool HasChanged { get; set; }

		// Token: 0x0600142E RID: 5166
		void InitializeSaveable();

		// Token: 0x0600142F RID: 5167
		string GetSaveString();

		// Token: 0x06001430 RID: 5168 RVA: 0x000591A4 File Offset: 0x000573A4
		string Save(string parentFolderPath)
		{
			bool flag;
			string localPath = this.GetLocalPath(out flag);
			if (!flag)
			{
				File.Exists(Path.Combine(parentFolderPath, localPath));
			}
			else
			{
				Directory.Exists(Path.Combine(parentFolderPath, localPath));
			}
			new SaveRequest(this, parentFolderPath);
			return localPath;
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x000591E4 File Offset: 0x000573E4
		void WriteBaseData(string parentFolderPath, string saveString)
		{
			string text = Path.Combine(parentFolderPath, this.SaveFileName + ".json");
			if (this.ShouldSaveUnderFolder)
			{
				text = Path.Combine(this.GetContainerFolder(parentFolderPath), this.SaveFileName + ".json");
			}
			if (!string.IsNullOrEmpty(saveString))
			{
				try
				{
					File.WriteAllText(text, saveString);
					goto IL_A2;
				}
				catch (Exception ex)
				{
					string[] array = new string[6];
					array[0] = "Failed to write save data file. Exception: ";
					int num = 1;
					Exception ex2 = ex;
					array[num] = ((ex2 != null) ? ex2.ToString() : null);
					array[2] = "\nData path: ";
					array[3] = text;
					array[4] = "\nSave string: ";
					array[5] = saveString;
					Console.LogError(string.Concat(array), null);
					goto IL_A2;
				}
			}
			Console.LogError("Failed to write save data file because the save string is empty. Data path: " + text, null);
			IL_A2:
			this.CompleteSave(parentFolderPath, !string.IsNullOrEmpty(saveString));
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000592B4 File Offset: 0x000574B4
		string GetLocalPath(out bool isFolder)
		{
			string result = this.SaveFileName + ".json";
			if (this.ShouldSaveUnderFolder)
			{
				isFolder = true;
				result = this.SaveFolderName;
			}
			else
			{
				isFolder = false;
			}
			return result;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x000592EC File Offset: 0x000574EC
		void CompleteSave(string parentFolderPath, bool writeDataFile)
		{
			List<string> list = new List<string>();
			if (this.LocalExtraFiles != null)
			{
				for (int i = 0; i < this.LocalExtraFiles.Count; i++)
				{
					list.Add(this.LocalExtraFiles[i] + ".json");
				}
			}
			if (this.LocalExtraFolders != null)
			{
				list.AddRange(this.LocalExtraFolders);
			}
			if (writeDataFile)
			{
				bool flag;
				this.GetLocalPath(out flag);
				if (flag)
				{
					string item = Path.Combine(new string[]
					{
						this.SaveFileName + ".json"
					});
					list.Add(item);
				}
			}
			List<string> collection = this.WriteData(parentFolderPath);
			list.AddRange(collection);
			if (this.ShouldSaveUnderFolder)
			{
				string containerFolder = this.GetContainerFolder(parentFolderPath);
				string[] files = Directory.GetFiles(containerFolder);
				string[] directories = Directory.GetDirectories(containerFolder);
				foreach (string text in files)
				{
					FileInfo fileInfo = new FileInfo(text);
					if (!list.Contains(fileInfo.Name))
					{
						try
						{
							File.Delete(text);
						}
						catch (Exception ex)
						{
							string str = "Failed to delete file: ";
							string str2 = text;
							string str3 = "\nException: ";
							Exception ex2 = ex;
							Console.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), null);
						}
					}
				}
				foreach (string text2 in directories)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(text2);
					if (!list.Contains(directoryInfo.Name))
					{
						try
						{
							Directory.Delete(text2, true);
						}
						catch (Exception ex3)
						{
							string str4 = "Failed to delete folder: ";
							string str5 = text2;
							string str6 = "\nException: ";
							Exception ex4 = ex3;
							Console.LogError(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null), null);
						}
					}
				}
				this.DeleteUnapprovedFiles(parentFolderPath);
			}
			Singleton<SaveManager>.Instance.CompleteSaveable(this);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000594B4 File Offset: 0x000576B4
		List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000045B1 File Offset: 0x000027B1
		void DeleteUnapprovedFiles(string parentFolderPath)
		{
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x000594BC File Offset: 0x000576BC
		string GetContainerFolder(string parentFolderPath)
		{
			string text = Path.Combine(parentFolderPath, this.SaveFolderName);
			if (!Directory.Exists(text))
			{
				try
				{
					Directory.CreateDirectory(text);
				}
				catch (Exception ex)
				{
					string str = "Failed to write save folder. Exception: ";
					Exception ex2 = ex;
					Console.LogError(str + ((ex2 != null) ? ex2.ToString() : null) + "\nFolder path: " + text, null);
				}
			}
			return text;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00059520 File Offset: 0x00057720
		string WriteSubfile(string parentPath, string localPath_NoExtensions, string contents)
		{
			bool flag;
			string text = Path.Combine(parentPath, this.GetLocalPath(out flag));
			if (!flag)
			{
				Console.LogError("Failed to write subfile: " + localPath_NoExtensions + " because the saveable is not saved under a folder.", null);
				return string.Empty;
			}
			if (!Directory.Exists(text))
			{
				Console.LogError("Failed to write subfile: " + localPath_NoExtensions + " because the main folder does not exist.", null);
				return string.Empty;
			}
			if (!this.LocalExtraFiles.Contains(localPath_NoExtensions))
			{
				Console.LogWarning("Writing subfile called '" + localPath_NoExtensions + "' that is not in the list of extra saveables. Be sure to include it in the returned files list.", null);
			}
			if (localPath_NoExtensions.Contains(".json"))
			{
				Console.LogError("Failed to write subfile: " + localPath_NoExtensions + " because it contains a data extension.", null);
				return string.Empty;
			}
			string text2 = localPath_NoExtensions + ".json";
			string text3 = Path.Combine(parentPath, text, text2);
			try
			{
				File.WriteAllText(text3, contents);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Failed to write sub file. Exception: ";
				int num = 1;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[2] = "\nData path: ";
				array[3] = text3;
				array[4] = "\nSave string: ";
				array[5] = contents;
				Console.LogError(string.Concat(array), null);
			}
			return text2;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00059644 File Offset: 0x00057844
		string WriteFolder(string parentPath, string localPath_NoExtensions)
		{
			bool flag;
			string text = Path.Combine(parentPath, this.GetLocalPath(out flag));
			if (!flag)
			{
				Console.LogError("Failed to write subfile: " + localPath_NoExtensions + " because the saveable is not saved under a folder.", null);
				return string.Empty;
			}
			if (!Directory.Exists(text))
			{
				Console.LogError(string.Concat(new string[]
				{
					"Failed to write subfile: ",
					localPath_NoExtensions,
					" because the main folder (",
					text,
					") does not exist."
				}), null);
				return string.Empty;
			}
			if (!this.LocalExtraFolders.Contains(localPath_NoExtensions))
			{
				Console.LogWarning("Writing subfile called '" + localPath_NoExtensions + "' that is not in the list of extra saveables. Be sure to include it in the returned files list.", null);
			}
			if (localPath_NoExtensions.Contains(".json"))
			{
				Console.LogError("Failed to write subfile: " + localPath_NoExtensions + " because it contains a data extension.", null);
				return string.Empty;
			}
			string text2 = Path.Combine(parentPath, text, localPath_NoExtensions);
			try
			{
				Directory.CreateDirectory(text2);
			}
			catch (Exception ex)
			{
				string str = "Failed to write sub folder. Exception: ";
				Exception ex2 = ex;
				Console.LogError(str + ((ex2 != null) ? ex2.ToString() : null) + "\nData path: " + text2, null);
			}
			return text2;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00059754 File Offset: 0x00057954
		bool TryLoadFile(string parentPath, string fileName, out string contents)
		{
			return this.TryLoadFile(Path.Combine(parentPath, fileName), out contents, true);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00059768 File Offset: 0x00057968
		bool TryLoadFile(string path, out string contents, bool autoAddExtension = true)
		{
			contents = string.Empty;
			string text = path;
			if (autoAddExtension)
			{
				text += ".json";
			}
			if (!File.Exists(text))
			{
				Console.LogWarning("File not found at: " + text, null);
				return false;
			}
			try
			{
				contents = File.ReadAllText(text);
			}
			catch (Exception ex)
			{
				string str = "Error reading file: ";
				string str2 = text;
				string str3 = "\n";
				Exception ex2 = ex;
				Console.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), null);
				return false;
			}
			return true;
		}
	}
}
