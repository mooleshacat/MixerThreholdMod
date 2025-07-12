using System;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Messaging
{
	// Token: 0x0200057E RID: 1406
	[Serializable]
	public class Message
	{
		// Token: 0x060021D9 RID: 8665 RVA: 0x0008B6F8 File Offset: 0x000898F8
		public Message()
		{
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x0008B707 File Offset: 0x00089907
		public Message(string _text, Message.ESenderType _type, bool _endOfGroup = false, int _messageId = -1)
		{
			this.text = _text;
			this.sender = _type;
			this.endOfGroup = _endOfGroup;
			if (_messageId == -1)
			{
				this.messageId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x0008B745 File Offset: 0x00089945
		public Message(TextMessageData data)
		{
			this.text = data.Text;
			this.sender = (Message.ESenderType)data.Sender;
			this.endOfGroup = data.EndOfChain;
			this.messageId = data.MessageID;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x0008B784 File Offset: 0x00089984
		public TextMessageData GetSaveData()
		{
			return new TextMessageData((int)this.sender, this.messageId, this.text, this.endOfGroup);
		}

		// Token: 0x040019E1 RID: 6625
		public int messageId = -1;

		// Token: 0x040019E2 RID: 6626
		public string text;

		// Token: 0x040019E3 RID: 6627
		public Message.ESenderType sender;

		// Token: 0x040019E4 RID: 6628
		public bool endOfGroup;

		// Token: 0x0200057F RID: 1407
		public enum ESenderType
		{
			// Token: 0x040019E6 RID: 6630
			Player,
			// Token: 0x040019E7 RID: 6631
			Other
		}
	}
}
