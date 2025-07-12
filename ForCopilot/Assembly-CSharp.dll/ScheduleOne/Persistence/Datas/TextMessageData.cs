using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200046C RID: 1132
	[Serializable]
	public class TextMessageData
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x0006473C File Offset: 0x0006293C
		public TextMessageData(int sender, int messageID, string text, bool endOfChain)
		{
			this.Sender = sender;
			this.MessageID = messageID;
			this.Text = text;
			this.EndOfChain = endOfChain;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00064761 File Offset: 0x00062961
		public TextMessageData()
		{
			this.Sender = 0;
			this.MessageID = 0;
			this.Text = "";
			this.EndOfChain = false;
		}

		// Token: 0x040014EE RID: 5358
		public int Sender;

		// Token: 0x040014EF RID: 5359
		public int MessageID;

		// Token: 0x040014F0 RID: 5360
		public string Text;

		// Token: 0x040014F1 RID: 5361
		public bool EndOfChain;
	}
}
