using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006E8 RID: 1768
	[CreateAssetMenu(fileName = "New Dialogue Database", menuName = "Dialogue/Dialogue Database")]
	[Serializable]
	public class DialogueDatabase : ScriptableObject
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x000CB373 File Offset: 0x000C9573
		private List<DialogueModule> runtimeModules
		{
			get
			{
				return this.handler.runtimeModules;
			}
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000CB380 File Offset: 0x000C9580
		public void Initialize(DialogueHandler _handler)
		{
			this.handler = _handler;
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000CB38C File Offset: 0x000C958C
		public DialogueModule GetModule(EDialogueModule moduleType)
		{
			if (this.runtimeModules == null)
			{
				Console.LogWarning("DialogueDatabase not initialized", null);
				return null;
			}
			DialogueModule dialogueModule = this.runtimeModules.Find((DialogueModule module) => module.ModuleType == moduleType);
			if (dialogueModule != null)
			{
				return dialogueModule;
			}
			return Singleton<DialogueManager>.Instance.Get(moduleType);
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000CB3F0 File Offset: 0x000C95F0
		public DialogueChain GetChain(EDialogueModule moduleType, string key)
		{
			DialogueModule module = this.GetModule(moduleType);
			if (module == null)
			{
				Console.LogWarning("Could not find module: " + moduleType.ToString(), null);
				return null;
			}
			return module.GetChain(key);
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000CB434 File Offset: 0x000C9634
		public bool HasChain(EDialogueModule moduleType, string key)
		{
			DialogueModule module = this.GetModule(moduleType);
			if (module == null)
			{
				Console.LogWarning("Could not find module: " + moduleType.ToString(), null);
				return false;
			}
			return module.HasChain(key);
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000CB478 File Offset: 0x000C9678
		public string GetLine(EDialogueModule moduleType, string key)
		{
			DialogueModule module = this.GetModule(moduleType);
			if (module == null)
			{
				Console.LogWarning("Could not find module: " + moduleType.ToString(), null);
				return string.Empty;
			}
			return module.GetLine(key);
		}

		// Token: 0x0400220F RID: 8719
		public List<DialogueModule> Modules;

		// Token: 0x04002210 RID: 8720
		public List<Entry> GenericEntries = new List<Entry>();

		// Token: 0x04002211 RID: 8721
		private DialogueHandler handler;
	}
}
