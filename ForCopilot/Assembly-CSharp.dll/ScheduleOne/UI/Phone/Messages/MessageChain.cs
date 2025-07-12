using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B07 RID: 2823
	[Serializable]
	public class MessageChain
	{
		// Token: 0x06004BB8 RID: 19384 RVA: 0x0013E393 File Offset: 0x0013C593
		public static MessageChain Combine(MessageChain a, MessageChain b)
		{
			MessageChain messageChain = new MessageChain();
			messageChain.Messages.AddRange(a.Messages);
			messageChain.Messages.AddRange(b.Messages);
			return messageChain;
		}

		// Token: 0x04003825 RID: 14373
		[TextArea(2, 10)]
		public List<string> Messages = new List<string>();

		// Token: 0x04003826 RID: 14374
		[HideInInspector]
		public int id = -1;
	}
}
