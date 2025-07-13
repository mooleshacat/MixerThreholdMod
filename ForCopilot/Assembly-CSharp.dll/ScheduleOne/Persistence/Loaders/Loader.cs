using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A2 RID: 930
	public class Loader
	{
		// Token: 0x0600150D RID: 5389 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Load(string mainPath)
		{
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0005D5A8 File Offset: 0x0005B7A8
		public bool TryLoadFile(string parentPath, string fileName, out string contents)
		{
			return this.TryLoadFile(Path.Combine(parentPath, fileName), out contents, true);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0005D5BC File Offset: 0x0005B7BC
		public bool TryLoadFile(string path, out string contents, bool autoAddExtension = true)
		{
			contents = string.Empty;
			string text = path;
			if (autoAddExtension)
			{
				text += ".json";
			}
			if (!File.Exists(text))
			{
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

		// Token: 0x06001510 RID: 5392 RVA: 0x0005D634 File Offset: 0x0005B834
		protected List<DirectoryInfo> GetDirectories(string parentPath)
		{
			if (!Directory.Exists(parentPath))
			{
				return new List<DirectoryInfo>();
			}
			List<DirectoryInfo> list = new List<DirectoryInfo>();
			string[] directories = Directory.GetDirectories(parentPath);
			for (int i = 0; i < directories.Length; i++)
			{
				list.Add(new DirectoryInfo(directories[i]));
			}
			return list;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0005D67C File Offset: 0x0005B87C
		protected List<FileInfo> GetFiles(string parenPath)
		{
			if (!Directory.Exists(parenPath))
			{
				return new List<FileInfo>();
			}
			List<FileInfo> list = new List<FileInfo>();
			string[] files = Directory.GetFiles(parenPath);
			for (int i = 0; i < files.Length; i++)
			{
				list.Add(new FileInfo(files[i]));
			}
			return list;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0005D6C4 File Offset: 0x0005B8C4
		public static bool TryDeserialize<T>(string json, out T data)
		{
			data = default(T);
			if (string.IsNullOrEmpty(json))
			{
				return false;
			}
			try
			{
				data = JsonUtility.FromJson<T>(json);
			}
			catch (Exception ex)
			{
				string str = "Failed to deserialize JSON: ";
				Exception ex2 = ex;
				Console.LogError(str + ((ex2 != null) ? ex2.ToString() : null), null);
				return false;
			}
			return true;
		}
	}
}
