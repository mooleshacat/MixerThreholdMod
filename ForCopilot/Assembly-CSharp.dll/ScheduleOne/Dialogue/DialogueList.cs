using System;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006E6 RID: 1766
	[Serializable]
	public class DialogueList
	{
		// Token: 0x0600306F RID: 12399 RVA: 0x000CB328 File Offset: 0x000C9528
		public string GetRandomLine()
		{
			if (this.Lines.Length == 0)
			{
				return string.Empty;
			}
			int num = UnityEngine.Random.Range(0, this.Lines.Length);
			return this.Lines[num];
		}

		// Token: 0x0400220D RID: 8717
		public string[] Lines;
	}
}
