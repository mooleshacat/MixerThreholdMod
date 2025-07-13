using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200043A RID: 1082
	[Serializable]
	public class MSGConversationData : SaveData
	{
		// Token: 0x06001670 RID: 5744 RVA: 0x00063E1F File Offset: 0x0006201F
		public MSGConversationData(int conversationIndex, bool read, TextMessageData[] messageHistory, TextResponseData[] activeResponses, bool isHidden)
		{
			this.ConversationIndex = conversationIndex;
			this.Read = read;
			this.MessageHistory = messageHistory;
			this.ActiveResponses = activeResponses;
			this.IsHidden = isHidden;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00063E4C File Offset: 0x0006204C
		public MSGConversationData()
		{
			this.ConversationIndex = 0;
			this.Read = false;
			this.MessageHistory = new TextMessageData[0];
			this.ActiveResponses = new TextResponseData[0];
			this.IsHidden = false;
		}

		// Token: 0x04001450 RID: 5200
		public int ConversationIndex;

		// Token: 0x04001451 RID: 5201
		public bool Read;

		// Token: 0x04001452 RID: 5202
		public TextMessageData[] MessageHistory;

		// Token: 0x04001453 RID: 5203
		public TextResponseData[] ActiveResponses;

		// Token: 0x04001454 RID: 5204
		public bool IsHidden;
	}
}
