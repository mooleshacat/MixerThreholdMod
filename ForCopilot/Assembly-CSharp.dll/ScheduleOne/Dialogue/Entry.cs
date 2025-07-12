using System;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006EA RID: 1770
	[Serializable]
	public struct Entry
	{
		// Token: 0x0600307C RID: 12412 RVA: 0x000CB4E4 File Offset: 0x000C96E4
		public DialogueChain GetRandomChain()
		{
			if (this.Chains.Length == 0)
			{
				return null;
			}
			int num = UnityEngine.Random.Range(0, this.Chains.Length);
			return this.Chains[num];
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000CB513 File Offset: 0x000C9713
		public string GetRandomLine()
		{
			return this.GetRandomChain().Lines[0];
		}

		// Token: 0x04002213 RID: 8723
		public string Key;

		// Token: 0x04002214 RID: 8724
		public DialogueChain[] Chains;
	}
}
