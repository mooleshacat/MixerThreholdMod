using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006FC RID: 1788
	public class DialogueManager : Singleton<DialogueManager>
	{
		// Token: 0x060030DB RID: 12507 RVA: 0x000CC95C File Offset: 0x000CAB5C
		public DialogueModule Get(EDialogueModule moduleType)
		{
			DialogueModule dialogueModule = this.DefaultModules.Find((DialogueModule x) => x.ModuleType == moduleType);
			if (dialogueModule == null)
			{
				Debug.LogError("Generic module not found for: " + moduleType.ToString());
			}
			return dialogueModule;
		}

		// Token: 0x04002245 RID: 8773
		public DialogueDatabase DefaultDatabase;

		// Token: 0x04002246 RID: 8774
		public List<DialogueModule> DefaultModules = new List<DialogueModule>();
	}
}
