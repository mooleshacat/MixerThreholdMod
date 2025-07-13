using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ookii.Dialogs;

namespace SFB
{
	// Token: 0x02000174 RID: 372
	public class StandaloneFileBrowserWindows : IStandaloneFileBrowser
	{
		// Token: 0x06000722 RID: 1826
		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		// Token: 0x06000723 RID: 1827 RVA: 0x000201C8 File Offset: 0x0001E3C8
		public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
		{
			VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog();
			vistaOpenFileDialog.Title = title;
			if (extensions != null)
			{
				vistaOpenFileDialog.Filter = StandaloneFileBrowserWindows.GetFilterFromFileExtensionList(extensions);
				vistaOpenFileDialog.FilterIndex = 1;
			}
			else
			{
				vistaOpenFileDialog.Filter = string.Empty;
			}
			vistaOpenFileDialog.Multiselect = multiselect;
			if (!string.IsNullOrEmpty(directory))
			{
				vistaOpenFileDialog.FileName = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
			}
			string[] result = (vistaOpenFileDialog.ShowDialog(new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) == DialogResult.OK) ? vistaOpenFileDialog.FileNames : new string[0];
			vistaOpenFileDialog.Dispose();
			return result;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00020248 File Offset: 0x0001E448
		public void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
		{
			cb(this.OpenFilePanel(title, directory, extensions, multiselect));
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002025C File Offset: 0x0001E45C
		public string[] OpenFolderPanel(string title, string directory, bool multiselect)
		{
			VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
			vistaFolderBrowserDialog.Description = title;
			if (!string.IsNullOrEmpty(directory))
			{
				vistaFolderBrowserDialog.SelectedPath = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
			}
			string[] result;
			if (vistaFolderBrowserDialog.ShowDialog(new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) != DialogResult.OK)
			{
				result = new string[0];
			}
			else
			{
				(result = new string[1])[0] = vistaFolderBrowserDialog.SelectedPath;
			}
			vistaFolderBrowserDialog.Dispose();
			return result;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000202BA File Offset: 0x0001E4BA
		public void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
		{
			cb(this.OpenFolderPanel(title, directory, multiselect));
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000202CC File Offset: 0x0001E4CC
		public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
		{
			VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog();
			vistaSaveFileDialog.Title = title;
			string text = "";
			if (!string.IsNullOrEmpty(directory))
			{
				text = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
			}
			if (!string.IsNullOrEmpty(defaultName))
			{
				text += defaultName;
			}
			vistaSaveFileDialog.FileName = text;
			if (extensions != null)
			{
				vistaSaveFileDialog.Filter = StandaloneFileBrowserWindows.GetFilterFromFileExtensionList(extensions);
				vistaSaveFileDialog.FilterIndex = 1;
				vistaSaveFileDialog.DefaultExt = extensions[0].Extensions[0];
				vistaSaveFileDialog.AddExtension = true;
			}
			else
			{
				vistaSaveFileDialog.DefaultExt = string.Empty;
				vistaSaveFileDialog.Filter = string.Empty;
				vistaSaveFileDialog.AddExtension = false;
			}
			string result = (vistaSaveFileDialog.ShowDialog(new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) == DialogResult.OK) ? vistaSaveFileDialog.FileName : "";
			vistaSaveFileDialog.Dispose();
			return result;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002038D File Offset: 0x0001E58D
		public void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
		{
			cb(this.SaveFilePanel(title, directory, defaultName, extensions));
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000203A4 File Offset: 0x0001E5A4
		private static string GetFilterFromFileExtensionList(ExtensionFilter[] extensions)
		{
			string text = "";
			foreach (ExtensionFilter extensionFilter in extensions)
			{
				text = text + extensionFilter.Name + "(";
				foreach (string str in extensionFilter.Extensions)
				{
					text = text + "*." + str + ",";
				}
				text = text.Remove(text.Length - 1);
				text += ") |";
				foreach (string str2 in extensionFilter.Extensions)
				{
					text = text + "*." + str2 + "; ";
				}
				text += "|";
			}
			return text.Remove(text.Length - 1);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00020488 File Offset: 0x0001E688
		private static string GetDirectoryPath(string directory)
		{
			string text = Path.GetFullPath(directory);
			if (!text.EndsWith("\\"))
			{
				text += "\\";
			}
			if (Path.GetPathRoot(text) == text)
			{
				return directory;
			}
			return Path.GetDirectoryName(text) + Path.DirectorySeparatorChar.ToString();
		}
	}
}
