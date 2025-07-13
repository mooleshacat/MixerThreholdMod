using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A37 RID: 2615
	public class InputPromptsCanvas : Singleton<InputPromptsCanvas>
	{
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x00127042 File Offset: 0x00125242
		// (set) Token: 0x06004656 RID: 18006 RVA: 0x0012704A File Offset: 0x0012524A
		public string currentModuleLabel { get; protected set; } = string.Empty;

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x00127053 File Offset: 0x00125253
		// (set) Token: 0x06004658 RID: 18008 RVA: 0x0012705B File Offset: 0x0012525B
		public RectTransform currentModule { get; private set; }

		// Token: 0x06004659 RID: 18009 RVA: 0x00127064 File Offset: 0x00125264
		public void LoadModule(string key)
		{
			GameObject module = this.Modules.Find((InputPromptsCanvas.Module x) => x.key.ToLower() == key.ToLower()).module;
			if (module == null)
			{
				Console.LogError("Input prompt module with key '" + key + "' not found!", null);
				return;
			}
			if (this.currentModule != null)
			{
				this.UnloadModule();
			}
			this.currentModuleLabel = key;
			this.currentModule = UnityEngine.Object.Instantiate<GameObject>(module, this.InputPromptsContainer).GetComponent<RectTransform>();
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x001270F7 File Offset: 0x001252F7
		public void UnloadModule()
		{
			this.currentModuleLabel = string.Empty;
			if (this.currentModule != null)
			{
				UnityEngine.Object.Destroy(this.currentModule.gameObject);
			}
		}

		// Token: 0x04003318 RID: 13080
		public RectTransform InputPromptsContainer;

		// Token: 0x04003319 RID: 13081
		[Header("Input prompt modules")]
		public List<InputPromptsCanvas.Module> Modules = new List<InputPromptsCanvas.Module>();

		// Token: 0x02000A38 RID: 2616
		[Serializable]
		public class Module
		{
			// Token: 0x0400331C RID: 13084
			public string key;

			// Token: 0x0400331D RID: 13085
			public GameObject module;
		}
	}
}
