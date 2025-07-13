using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006FE RID: 1790
	public class DialogueModule : MonoBehaviour
	{
		// Token: 0x060030DF RID: 12511 RVA: 0x000CC9DC File Offset: 0x000CABDC
		public Entry GetEntry(string key)
		{
			return this.Entries.Find((Entry x) => x.Key == key);
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000CCA10 File Offset: 0x000CAC10
		public DialogueChain GetChain(string key)
		{
			Entry entry = this.GetEntry(key);
			if (entry.Chains == null || entry.Chains.Length == 0)
			{
				Debug.LogError("DialogueModule.Get: No lines found for key: " + key);
			}
			return entry.GetRandomChain();
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000CCA4D File Offset: 0x000CAC4D
		public bool HasChain(string key)
		{
			return this.GetEntry(key).Chains != null;
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x000CCA60 File Offset: 0x000CAC60
		public string GetLine(string key)
		{
			Entry entry = this.GetEntry(key);
			if (entry.Chains == null || entry.Chains.Length == 0)
			{
				Debug.LogError("DialogueModule.Get: No lines found for key: " + key);
				return string.Empty;
			}
			return entry.GetRandomLine();
		}

		// Token: 0x04002248 RID: 8776
		public EDialogueModule ModuleType;

		// Token: 0x04002249 RID: 8777
		public List<Entry> Entries = new List<Entry>();
	}
}
