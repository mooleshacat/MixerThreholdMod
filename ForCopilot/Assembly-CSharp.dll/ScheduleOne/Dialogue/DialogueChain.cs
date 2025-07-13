using System;
using System.Linq;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006E7 RID: 1767
	[Serializable]
	public class DialogueChain
	{
		// Token: 0x06003071 RID: 12401 RVA: 0x000CB35B File Offset: 0x000C955B
		public MessageChain GetMessageChain()
		{
			return new MessageChain
			{
				Messages = this.Lines.ToList<string>()
			};
		}

		// Token: 0x0400220E RID: 8718
		[TextArea(1, 10)]
		public string[] Lines;
	}
}
