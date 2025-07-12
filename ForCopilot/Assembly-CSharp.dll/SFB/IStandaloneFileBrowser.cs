using System;

namespace SFB
{
	// Token: 0x02000170 RID: 368
	public interface IStandaloneFileBrowser
	{
		// Token: 0x0600070D RID: 1805
		string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect);

		// Token: 0x0600070E RID: 1806
		string[] OpenFolderPanel(string title, string directory, bool multiselect);

		// Token: 0x0600070F RID: 1807
		string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions);

		// Token: 0x06000710 RID: 1808
		void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb);

		// Token: 0x06000711 RID: 1809
		void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb);

		// Token: 0x06000712 RID: 1810
		void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb);
	}
}
